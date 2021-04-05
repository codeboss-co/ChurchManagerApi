using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.Persistence.Models.People;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ChurchManager.Api.Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {
        private readonly IPersonDbRepository _dbRepository;
        private readonly IGenericRepositoryAsync<OnlineUser> _onlineUserRepository;
        private readonly ILogger<NotificationHub> _logger;

        private static readonly ConcurrentBag<NotificationConnection> NotificationConnections = new();


        public NotificationHub(
            IPersonDbRepository dbRepository,
            IGenericRepositoryAsync<OnlineUser> onlineUserRepository,
            ILogger<NotificationHub> logger)
        {
            _dbRepository = dbRepository;
            _onlineUserRepository = onlineUserRepository;
            _logger = logger;
        }

        /// <summary>
        /// This will get called whenever authentication user connects to this hub
        /// We respond with a welcome message
        /// </summary>
        public override async Task OnConnectedAsync()
        {
            var person = await _dbRepository.ProfileByUserLoginId(Context.UserIdentifier);

            _logger.LogDebug("[√] NotificationHub Connected for {user}", Context.UserIdentifier);

            var notification = new
            {
                Type = "info",
                Title = $"Welcome back, {person?.FullName.FirstName}",
                Payload = Context.UserIdentifier
            };

            await Clients.User(Context.UserIdentifier).SendAsync("DirectMessage", notification);
            await base.OnConnectedAsync();

            // Extra
            var connectionId = Context.ConnectionId;
            var userId = Context.UserIdentifier;
            var onlineUser = await _onlineUserRepository.Queryable().FirstOrDefaultAsync(p => p.UserLoginId == userId);
            if (onlineUser == null)
            {
                await _onlineUserRepository.AddAsync(new OnlineUser {UserLoginId = userId});
            }
            else
            {
                onlineUser.GoOnline();
            }
            await _onlineUserRepository.SaveChangesAsync();


            var onlineUserVm = new
            {
                Id = userId,
                Name = $"{person.FullName.FirstName} {person.FullName.LastName}",
                Avatar = person.PhotoUrl,
                Status = "online",
                Unread = 2
            };

            await Clients.All.SendAsync("OnlineUser", onlineUserVm);
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            var userId = Context.UserIdentifier;
            var onlineUser = await _onlineUserRepository.Queryable().FirstOrDefaultAsync(p => p.UserLoginId == userId);
            if(onlineUser != null)
            {
                _logger.LogDebug("[x] NotificationHub Disconnected for {user} with Connection: {connection}", Context.UserIdentifier, Context.ConnectionId);

                onlineUser.GoOffline();
                await _onlineUserRepository.SaveChangesAsync();

                var person = await _dbRepository.ProfileByUserLoginId(Context.UserIdentifier);
                var onlineUserVm = new
                {
                    Id = userId,
                    Name = $"{person.FullName.FirstName} {person.FullName.LastName}",
                    Avatar = person.PhotoUrl,
                    Status = "offline",
                    Unread = 2
                };

                await Clients.All.SendAsync("OnlineUser", onlineUserVm);
            }

            await base.OnDisconnectedAsync(ex);
        }

    }

    public class NotificationConnection
    {
        public string ConnectionId { get; set; }
        public string UserId { get; set; }
    }
}

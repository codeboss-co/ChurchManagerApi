using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using ChurchManager.Domain.Features.People.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace ChurchManager.Api.Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {
        private readonly IPersonDbRepository _dbRepository;
        private readonly ILogger<NotificationHub> _logger;

        private static readonly ConcurrentBag<NotificationConnection> NotificationConnections = new();


        public NotificationHub(
            IPersonDbRepository dbRepository,
            ILogger<NotificationHub> logger)
        {
            _dbRepository = dbRepository;
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
            if (NotificationConnections.All(p => p.UserId != userId))
            {
                NotificationConnections.Add(new NotificationConnection { UserId = userId, ConnectionId = connectionId });
            }

            var onlineUser = new
            {
                Id = userId,
                Name = $"{person.FullName.FirstName} {person.FullName.LastName}",
                Avatar = person.PhotoUrl,
                Status = "online",
                Unread = 2
            };

            await Clients.All.SendAsync("OnlineUser", onlineUser);
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            var userId = Context.UserIdentifier;
            var item = NotificationConnections.FirstOrDefault(p => p.UserId == userId);
            if(item != null && NotificationConnections.TryTake(out item))
            {
                _logger.LogDebug("[x] NotificationHub Disconnected for {user} with Connection: {connection}", Context.UserIdentifier, Context.ConnectionId);
                
                var person = await _dbRepository.ProfileByUserLoginId(Context.UserIdentifier);
                var onlineUser = new
                {
                    Id = userId,
                    Name = $"{person.FullName.FirstName} {person.FullName.LastName}",
                    Avatar = person.PhotoUrl,
                    Status = "offline",
                    Unread = 2
                };

                await Clients.All.SendAsync("OnlineUser", onlineUser);
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

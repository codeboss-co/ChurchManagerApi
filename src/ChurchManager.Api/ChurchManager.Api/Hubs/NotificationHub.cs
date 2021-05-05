using System;
using System.Linq;
using System.Threading.Tasks;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Domain.Model.People;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using Codeboss.Types;
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
        private readonly IDateTimeProvider _dateTime;
        private readonly ILogger<NotificationHub> _logger;

        public NotificationHub(
            IPersonDbRepository dbRepository,
            IGenericRepositoryAsync<OnlineUser> onlineUserRepository,
            IDateTimeProvider dateTime,
            ILogger<NotificationHub> logger)
        {
            _dbRepository = dbRepository;
            _onlineUserRepository = onlineUserRepository;
            _dateTime = dateTime;
            _logger = logger;
        }

        /// <summary>
        /// This will get called whenever authentication user connects to this hub
        /// We respond with a welcome message
        /// </summary>
        public override async Task OnConnectedAsync()
        {
            _logger.LogDebug("[√] NotificationHub Connected for {user}", Context.UserIdentifier);

            var person = await _dbRepository.ProfileByUserLoginId(Context.UserIdentifier) 
                         ?? throw new ArgumentNullException("person", "Person not found with UserLoginId");
            var notification = new
            {
                Type = "info",
                Title = $"Welcome back, {person.FullName.FirstName}",
                Payload = Context.UserIdentifier
            };

            // Send welcome message to user
            await Clients.User(Context.UserIdentifier).SendAsync("DirectMessage", notification);

            // Extra
            var connectionId = Context.ConnectionId;
            var userId = Context.UserIdentifier;
            var onlineUser = await _onlineUserRepository
                .Queryable()
                .Include(x => x.Person)
                .FirstOrDefaultAsync(p => p.Person.UserLoginId == userId);

            // Create new online user
            if (onlineUser is null)
            {
                await _onlineUserRepository.AddAsync(new OnlineUser
                {
                    PersonId = person.PersonId,
                    ConnectionId = connectionId
                });
            }
            else
            {
                onlineUser.GoOnline();
            }
            await _onlineUserRepository.SaveChangesAsync();

            var onlineUsers = await _onlineUserRepository
                .Queryable()
                .AsNoTracking()
                .Include(x => x.Person)
                //.Where(x => x.Status == "online")
                .Select(x => new
                {
                    Id = userId,
                    Name = $"{x.Person.FullName.FirstName} {x.Person.FullName.LastName}",
                    Avatar = x.Person.PhotoUrl,
                    x.Status,
                    Unread = 2,
                    LastOnline = _dateTime.ConvertFromUtc(x.LastOnlineDateTime.UtcDateTime)
                })
                .ToListAsync();
            
            await Clients.All.SendAsync("OnlineUsers", onlineUsers);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            var userId = Context.UserIdentifier;
            var onlineUser = await _onlineUserRepository
                .Queryable()
                .Include(x => x.Person)
                .FirstOrDefaultAsync(p => p.Person.UserLoginId == userId);

            if(onlineUser is not null && onlineUser.IsOnline)
            {
                _logger.LogDebug("[x] NotificationHub Disconnected for {user} with Connection: {connection}", Context.UserIdentifier, Context.ConnectionId);

                onlineUser.GoOffline();
                await _onlineUserRepository.SaveChangesAsync();

                var onlineUsers = await _onlineUserRepository
                    .Queryable()
                    .AsNoTracking()
                    .Include(x => x.Person)
                    //.Where(x => x.Status == "online")
                    .Select(x => new
                    {
                        Id = userId,
                        Name = $"{x.Person.FullName.FirstName} {x.Person.FullName.LastName}",
                        Avatar = x.Person.PhotoUrl,
                        x.Status,
                        Unread = 2,
                        LastOnline = _dateTime.ConvertFromUtc(x.LastOnlineDateTime.UtcDateTime)
                    })
                    .ToListAsync();

                await Clients.All.SendAsync("OnlineUsers", onlineUsers);
            }

            await base.OnDisconnectedAsync(ex);
        }

    }
}

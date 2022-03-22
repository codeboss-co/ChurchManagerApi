using System;
using System.Threading.Tasks;
using ChurchManager.Domain.Features.People;
using ChurchManager.Domain.Features.People.Specifications;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using Codeboss.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ChurchManager.Infrastructure.Shared.SignalR.Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {
        private readonly IGenericDbRepository<OnlineUser> _onlineUserRepository;
        private readonly IDateTimeProvider _dateTime;
        private readonly ILogger<NotificationHub> _logger;

        public NotificationHub(
            IGenericDbRepository<OnlineUser> onlineUserRepository,
            IDateTimeProvider dateTime,
            ILogger<NotificationHub> logger)
        {
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

            // var person = await _service.ProfileByUserLoginId(Context.UserIdentifier) ?? throw new ArgumentNullException("person", "Person not found with UserLoginId");
            var notification = new
            {
                Type = "info",
                Title = $"Welcome back",
                Payload = Context.UserIdentifier
            };

            // Send welcome message to user
            await Clients.User(Context.UserIdentifier).SendAsync("DirectMessage", notification);

            // Extra
            var connectionId = Context.ConnectionId;
            var userId = Context.UserIdentifier;
            var spec = new OnlineUserSpecification(userId);
            var onlineUser = await _onlineUserRepository.GetBySpecAsync(spec);

            // Create new online user
            /*if (onlineUser is null)
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

            var onlineUsersSpec = new OnlineUsersSpecification(_dateTime);
            var onlineUsers = await _onlineUserRepository.ListAsync(onlineUsersSpec);

            await Clients.All.SendAsync("OnlineUsers", onlineUsers);*/

            // Add user connections to a group for that user
            await Groups.AddToGroupAsync(connectionId, userId);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            var connectionId = Context.ConnectionId;
            var userId = Context.UserIdentifier;
            
            /*var onlineUser = await _onlineUserRepository
               .Queryable()
               .Include(x => x.Person)
               .FirstOrDefaultAsync(p => p.Person.UserLoginId == userId);

           if(onlineUser is not null && onlineUser.IsOnline)
           {
               _logger.LogDebug("[x] NotificationHub Disconnected for {user} with Connection: {connection}", Context.UserIdentifier, Context.ConnectionId);

               onlineUser.GoOffline();
               await _onlineUserRepository.SaveChangesAsync();

               var onlineUsersSpec = new OnlineUsersSpecification(_dateTime);
               var onlineUsers = await _onlineUserRepository.ListAsync(onlineUsersSpec);

               await Clients.All.SendAsync("OnlineUsers", onlineUsers);
           }*/

            // Remove user connection to a group for that user
            await Groups.RemoveFromGroupAsync(connectionId, userId);

            await base.OnDisconnectedAsync(ex);
        }

    }
}

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

            _logger.LogDebug("UserIdentifier: {userloginId}", Context.UserIdentifier);

            var notification = new
            {
                Type = "info",
                Title = $"Welcome, {person?.FullName.FirstName}",
                Payload = Context.UserIdentifier
            };

            await Task.Delay(2000);
            await Clients.User(Context.UserIdentifier).SendAsync("DirectMessage", notification);
            await base.OnConnectedAsync();
        }
    }
}

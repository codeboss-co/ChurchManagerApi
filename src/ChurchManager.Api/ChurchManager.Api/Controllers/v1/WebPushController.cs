using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Application.Common;
using ChurchManager.Application.Features.Communication.Commands;
using ChurchManager.Infrastructure.Shared.WebPush;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Wangkanai.Detection.Services;

namespace ChurchManager.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize]
    public class WebPushController : BaseApiController                                       
    {
        private readonly IDetectionService _device;
        private readonly ICognitoCurrentUser _currentUser;
        private readonly PushNotificationsOptions _options;

        public WebPushController(
            IOptions<PushNotificationsOptions> options,
            IDetectionService device,
            ICognitoCurrentUser currentUser)
        {
            _device = device;
            _currentUser = currentUser;
            _options = options.Value;
        }

        [HttpPost("subscribe")]
        public async Task<IActionResult> Subscribe([FromBody] SubscribeToWebPushCommand command, CancellationToken token)
        {
            command.Device = _device.Device.Type.ToString(); // Desktop, Mobile
            command.UniqueIdentification = _device.Browser.Name.ToString(); // _device.Browser?.Version.Major
            await Mediator.Send(command, token);
            return Accepted();
        }

        [HttpPost("unsubscribe")]
        public async Task<IActionResult> Unsubscribe([FromBody] UnsubscribeToWebPushCommand command, CancellationToken token)
        {
            await Mediator.Send(command, token);
            return Accepted();
        }

        [HttpPost("remove-subscription")]
        public async Task<IActionResult> Remove(CancellationToken token)
        {
            var command = new RemoveSubscriptionsToWebPushCommand(
                _device.Device.Type.ToString(),
                _device.Browser.Name.ToString(),
                _currentUser.PersonId);
            await Mediator.Send(command, token);
            return Accepted();
        }

        [HttpPost("send")]
        [AllowAnonymous]
        public async Task<IActionResult> Send([FromBody] SendWebPushNotificationCommand command, CancellationToken token)
        {
            await Mediator.Send(command, token);
            return Accepted();
        }

        [HttpGet("public-key")]
        public IActionResult PublicKey()
        {
            return Content(_options.PublicKey, "text/plain");
        }
    }
}

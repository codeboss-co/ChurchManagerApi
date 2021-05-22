using System.Threading;
using System.Threading.Tasks;
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
        private readonly PushNotificationsOptions _options;

        public WebPushController(IOptions<PushNotificationsOptions> options, IDetectionService device)
        {
            _device = device;
            _options = options.Value;
        }

        [HttpPost("subscribe")]
        public async Task<IActionResult> Subscribe([FromBody] SubscribeToWebPushCommand command, CancellationToken token)
        {
            command.Device = _device.Device.Type.ToString();
            await Mediator.Send(command, token);
            return Accepted();
        }

        [HttpPost("unsubscribe")]
        public async Task<IActionResult> Unsubscribe([FromBody] UnsubscribeToWebPushCommand command, CancellationToken token)
        {
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

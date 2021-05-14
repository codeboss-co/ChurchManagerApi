using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Application.Abstractions.Services;
using MediatR;

namespace ChurchManager.Application.Features.Communication.Commands
{
    public record SendWebPushNotificationCommand(int PersonId, string Payload) : IRequest
    {
    }

    public class SendWebPushHandler : IRequestHandler<SendWebPushNotificationCommand>
    {
        private readonly IPushNotificationService _push;

        public SendWebPushHandler(IPushNotificationService push)
        {
            _push = push;
        }

        public async Task<Unit> Handle(SendWebPushNotificationCommand command, CancellationToken ct)
        {
            await _push.SendNotificationToPersonAsync(command.PersonId, "test message", ct);

            return Unit.Value;
        }
    }
}

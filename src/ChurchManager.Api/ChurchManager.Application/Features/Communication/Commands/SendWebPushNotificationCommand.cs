using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Domain.Common;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Application.Features.Communication.Commands
{
    public record SendWebPushNotificationCommand(int PersonId, string Payload) : IRequest
    {
    }

    public class SendWebPushHandler : IRequestHandler<SendWebPushNotificationCommand>
    {
        private readonly IPushNotificationService _push;
        private readonly IGenericDbRepository<PushDevice> _dbRepository;

        public SendWebPushHandler(IPushNotificationService push, IGenericDbRepository<PushDevice> dbRepository)
        {
            _push = push;
            _dbRepository = dbRepository;
        }

        public async Task<Unit> Handle(SendWebPushNotificationCommand command, CancellationToken ct)
        {
            var device = await _dbRepository.Queryable().FirstAsync(x => x.PersonId == command.PersonId, ct);

            await _push.SendNotificationAsync(device, "test message", ct);

            return Unit.Value;
        }
    }
}

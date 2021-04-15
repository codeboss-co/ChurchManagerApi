using System;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Domain.Features.People.Repositories;
using MediatR;

namespace ChurchManager.Application.Features.People.Commands.UpdatePerson
{
    public record UpdateConnectionInfoCommand : IRequest
    {
        public int PersonId { get; set; }
        public int ChurchId { get; set; }
        public string ConnectionStatus { get; set; }
        public DateTime? FirstVisitDate { get; set; }
        public string Source { get; set; }
    }

    public class UpdateConnectionInfoHandler : IRequestHandler<UpdateConnectionInfoCommand>
    {
        private readonly IPersonDbRepository _dbRepository;

        public UpdateConnectionInfoHandler(IPersonDbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }

        public async Task<Unit> Handle(UpdateConnectionInfoCommand command, CancellationToken cancellationToken)
        {
            var person = await _dbRepository.GetByIdAsync(command.PersonId);
            person.ChurchId = command.ChurchId;
            person.ConnectionStatus = command.ConnectionStatus;
            person.FirstVisitDate = command.FirstVisitDate;
            person.Source = command.Source;

            await _dbRepository.SaveChangesAsync();

            return new Unit();
        }
    }
}

using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Domain.Features.People.Repositories;
using MediatR;

namespace ChurchManager.Application.Features.People.Commands.UpdatePerson
{
    public record UpdatePersonalInfoCommand : IRequest
    {
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string AgeClassification { get; set; }
    }

    public class UpdatePersonalInfoCommandHandler : IRequestHandler<UpdatePersonalInfoCommand>
    {
        private readonly IPersonDbRepository _dbRepository;

        public UpdatePersonalInfoCommandHandler(IPersonDbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }

        public async Task<Unit> Handle(UpdatePersonalInfoCommand command, CancellationToken cancellationToken)
        {
            var person = await _dbRepository.GetByIdAsync(command.PersonId);
            person.FullName.FirstName = command.FirstName;
            person.FullName.MiddleName = command.MiddleName;
            person.FullName.LastName = command.LastName;
            person.Gender = command.Gender;
            person.AgeClassification = command.AgeClassification;


            await _dbRepository.SaveChangesAsync();

            return new Unit();
        }
    }
}

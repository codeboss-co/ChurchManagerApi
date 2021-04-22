using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Application.Features.People.Commands.AddNewFamily;
using ChurchManager.Domain.Features.People.Repositories;
using MediatR;

namespace ChurchManager.Application.Features.People.Commands.UpdatePerson
{
    public record UpdateGeneralInfoCommand : IRequest
    {
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string AgeClassification { get; set; }
        public string MaritalStatus { get; set; }
        public BirthDate BirthDate { get; set; }
    }

    public class UpdateGeneralInfoCommandHandler : IRequestHandler<UpdateGeneralInfoCommand>
    {
        private readonly IPersonDbRepository _dbRepository;

        public UpdateGeneralInfoCommandHandler(IPersonDbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }

        public async Task<Unit> Handle(UpdateGeneralInfoCommand command, CancellationToken cancellationToken)
        {
            var person = await _dbRepository.GetByIdAsync(command.PersonId);
            person.FullName.FirstName = command.FirstName;
            person.FullName.MiddleName = command.MiddleName;
            person.FullName.LastName = command.LastName;
            person.Gender = command.Gender;
            person.AgeClassification = command.AgeClassification;
            person.MaritalStatus = command.MaritalStatus;
            person.BirthDate.BirthDay = command.BirthDate.Day;
            person.BirthDate.BirthMonth = command.BirthDate.Month;
            person.BirthDate.BirthYear = command.BirthDate.Year;

            await _dbRepository.SaveChangesAsync();

            return new Unit();
        }
    }
}

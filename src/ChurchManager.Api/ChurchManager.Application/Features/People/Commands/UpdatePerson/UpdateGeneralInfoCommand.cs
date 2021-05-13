using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Domain.Features.People;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Domain.Features.People.Specifications;
using CodeBoss.Extensions;
using MediatR;
using BirthDate = ChurchManager.Application.Features.People.Commands.AddNewFamily.BirthDate;

namespace ChurchManager.Application.Features.People.Commands.UpdatePerson
{
    public record UpdateGeneralInfoCommand : IRequest
    {
        public int PersonId { get; set; }
        public string Occupation { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
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

        public async Task<Unit> Handle(UpdateGeneralInfoCommand command, CancellationToken ct)
        {
            var spec = new PersonWithPhonesSpecification(command.PersonId);

            var person = await _dbRepository.GetBySpecAsync(spec, ct) ?? throw new ArgumentNullException(nameof(Person));
            
            if (person.PhoneNumbers.Any())
            {
                var phoneNumber = person.PhoneNumbers.First();
                phoneNumber.CountryCode = "+27";
                phoneNumber.Number = command.PhoneNumber;
            }
            else
            {
                person.PhoneNumbers = new List<PhoneNumber>
                {
                    new() {CountryCode = "+27", Number = command.PhoneNumber}
                };
            }

            if (person.BirthDate is null)
            {
                person.BirthDate = new Domain.Features.People.BirthDate
                {
                    BirthDay = command.BirthDate.Day,
                    BirthMonth = command.BirthDate.Month,
                    BirthYear = command.BirthDate.Year
                };
            }
            else
            {
                person.BirthDate.BirthDay = command.BirthDate.Day;
                person.BirthDate.BirthMonth = command.BirthDate.Month;
                person.BirthDate.BirthYear = command.BirthDate.Year;
            }

            person.Occupation = command.Occupation;
            person.Email = new Email {Address = command.Email, IsActive = !command.Email.IsNullOrEmpty()};
            person.MaritalStatus = command.MaritalStatus;

            await _dbRepository.SaveChangesAsync(ct);

            return new Unit();
        }
    }
}

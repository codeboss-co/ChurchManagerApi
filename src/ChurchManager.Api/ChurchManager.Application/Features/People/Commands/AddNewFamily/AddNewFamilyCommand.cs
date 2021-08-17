using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Domain.Features.People;
using ChurchManager.Domain.Features.People.Repositories;
using CodeBoss.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChurchManager.Application.Features.People.Commands.AddNewFamily
{
    public record AddNewFamilyCommand : IRequest
    {
        public string FamilyName { get; set; }
        public IEnumerable<FamilyMember> Members { get; set; }
        public Address Address { get; set; }
    }

    public class AddNewFamilyHandler : IRequestHandler<AddNewFamilyCommand>
    {
        private readonly IPersonDbRepository _dbRepository;
        private readonly ILogger<AddNewFamilyHandler> _logger;

        public AddNewFamilyHandler(
            IPersonDbRepository dbRepository, 
            ILogger<AddNewFamilyHandler> logger)
        {
            _dbRepository = dbRepository;
            _logger = logger;
        }

        public async Task<Unit> Handle(AddNewFamilyCommand command, CancellationToken ct)
        {
            try
            {
                var family = new Family
                {
                    Name = $"{command.FamilyName} Family",
                    Address = new Domain.Features.People.Address
                    {
                        Street = command.Address.Street,
                        City = command.Address.City,
                        Province = command.Address.Province,
                        Country = command.Address.Country,
                        PostalCode = command.Address.PostalCode
                    }
                };

                var members = command.Members.Select(x => new Person
                {
                    FullName = new FullName
                    {
                        FirstName = x.Person.FirstName,
                        MiddleName = x.Person.MiddleName,
                        LastName = x.Person.LastName
                    },
                    FirstVisitDate = x.FirstVisitDate,
                    ConnectionStatus = x.ConnectionStatus,
                    Gender = x.Person.Gender,
                    AgeClassification = x.Person.AgeClassification,
                    ReceivedHolySpirit = x.Person.ReceivedHolySpirit,
                    BirthDate = new Domain.Features.People.BirthDate
                    {
                        BirthDay = x.Person.BirthDate?.Day,
                        BirthMonth = x.Person.BirthDate?.Month,
                        BirthYear = x.Person.BirthDate?.Year
                    },
                    ChurchId = x.ChurchId,
                    Email = !x.Person.EmailAddress.IsNullOrEmpty()
                        ? new Email { Address = x.Person.EmailAddress, IsActive = true }
                        : null,
                    PhoneNumbers = !x.Person.PhoneNumber.IsNullOrEmpty()
                        ? new List<PhoneNumber> { new() { CountryCode = "+27", Number = x.Person.PhoneNumber } }
                        : null,
                    Source = x.Source,
                    Family = family
                });

                await _dbRepository.AddRangeAsync(members, ct);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error adding new family command: {@command}", command);
                throw;
            }

            return new Unit();
        }
    }
}
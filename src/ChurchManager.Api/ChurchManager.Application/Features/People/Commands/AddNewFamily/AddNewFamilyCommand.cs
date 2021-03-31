﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Persistence.Models.People;
using CodeBoss.Extensions;
using MediatR;

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

        public AddNewFamilyHandler(IPersonDbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }

        public async Task<Unit> Handle(AddNewFamilyCommand command, CancellationToken cancellationToken)
        {
            var family = new Family
            {
                Name = $"{command.FamilyName} Family",
                Address = new Persistence.Models.People.Address
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
                    LastName = x.Person.LastName,
                },
                ConnectionStatus = x.ConnectionStatus,
                Gender = x.Person.Gender,
                AgeClassification = x.Person.AgeClassification,
                ReceivedHolySpirit = x.Person.ReceivedHolySpirit,
                BirthDate = new Persistence.Models.People.BirthDate
                {
                    BirthDay = x.Person.BirthDate.Day,
                    BirthMonth = x.Person.BirthDate.Month,
                    BirthYear = x.Person.BirthDate.Year
                },
                ChurchId = x.ChurchId,
                Email = !x.EmailAddress.IsNullOrEmpty() ? new Email {Address = x.EmailAddress, IsActive = true} : null,
                PhoneNumbers = !x.PhoneNumber.IsNullOrEmpty() 
                    ? new List<PhoneNumber>() {new() {CountryCode = "+27", Number = x.PhoneNumber}}
                    : null,
                Source = x.Source,
                Family = family
            });

            await _dbRepository.AddRangeAsync(members);
            await _dbRepository.SaveChangesAsync();

            return new Unit();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bogus;
using Bogus.DataSets;
using ChurchManager.Domain;
using ChurchManager.Domain.Features.People;
using ChurchManager.Domain.Model.People;
using ChurchManager.Infrastructure.Persistence.Contexts;
using CodeBoss.AspNetCore.Startup;
using CodeBoss.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Person = ChurchManager.Domain.Model.People.Person;

namespace ChurchManager.Infrastructure.Persistence.Seeding.Development
{
    /// <summary>
    /// Seeds the database with some dummy data
    /// </summary>
    public class PeopleFakeDbSeedInitializer : IInitializer
    {
        public int OrderNumber { get; } = 1;
        private readonly IServiceScopeFactory _scopeFactory;
        private ChurchManagerDbContext _dbContext;

        public PeopleFakeDbSeedInitializer(IServiceScopeFactory scopeFactory) => _scopeFactory = scopeFactory;

        public async Task InitializeAsync()
        {
            using var scope = _scopeFactory.CreateScope();
            _dbContext = scope.ServiceProvider.GetRequiredService<ChurchManagerDbContext>();

            if (!await _dbContext.Person.AnyAsync())
            {
                await SeedMyDetails();

                var tasks = new List<Task>();
                // Seed 200 singles
                foreach(var batch in GetData().Batch(100))
                {
                    tasks.Add(_dbContext.AddRangeAsync(batch));
                }

                // Seed 50 families
                var random = new Random();
                var familyMembersBatch = new List<Person>();
                for(int i = 0; i < 50; i++)
                {
                    var members = GenerateFamilyData(howManyChildren: random.Next(0, 4));
                    familyMembersBatch.AddRange(members);
                }
                tasks.Add(_dbContext.AddRangeAsync(familyMembersBatch));

                // Will complete each first
                await Task.WhenAll(tasks);

                // Save them
                await _dbContext.SaveChangesAsync();
            }
        }

        private async Task SeedMyDetails()
       {
            var cagnettaFamily = new Family {Name = "Cagnetta Family", Language = "English"};
            await _dbContext.SaveChangesAsync();

            // Add me as the first Person i.e. with Id 1
            var dillan = new Person
            {
                Family = cagnettaFamily,
                AgeClassification = AgeClassification.Adult,
                RecordStatus = RecordStatus.Active,
                Gender = Gender.Male,
                PhotoUrl = "https://secure.gravatar.com/avatar/6fdc48b6ec4d95f2fd682fc2982eb01b",
                ConnectionStatus = ConnectionStatus.Member,
                BaptismStatus = new Baptism {IsBaptised = true},
                ChurchId = 1,
                Email = new Email {Address = "dillancagnetta@yahoo.com", IsActive = true},
                FullName = new FullName {FirstName = "Dillan", LastName = "Cagnetta"},
                MaritalStatus = "Married",
                AnniversaryDate = new DateTime(2013, 01, 22),
                UserLoginId = "08925ade-9249-476b-8787-b3dd8f5dbc13",
                BirthDate = new BirthDate {BirthDay = 6, BirthMonth = 11, BirthYear = 1981},
                ReceivedHolySpirit = true,
                Occupation = "Software developer",
                PhoneNumbers = new List<PhoneNumber>(1) {PhoneNumbersFaker()}
            };

            var danielle = new Person
            {
                Family = cagnettaFamily,
                AgeClassification = AgeClassification.Adult,
                RecordStatus = RecordStatus.Active,
                Gender = Gender.Female,
                PhotoUrl = "https://samanthabernhardi.com/site/wp-content/uploads/2019/10/danielle-cagnetta-1.jpg",
                ConnectionStatus = ConnectionStatus.Member,
                BaptismStatus = new Baptism { IsBaptised = true },
                ChurchId = 1,
                Email = new Email { Address = "danielle@yahoo.com", IsActive = true },
                FullName = new FullName { FirstName = "Danielle", LastName = "Cagnetta" },
                MaritalStatus = "Married",
                AnniversaryDate = new DateTime(2013, 01, 22),
                BirthDate = new BirthDate { BirthDay = 13, BirthMonth = 03, BirthYear = 1980 },
                ReceivedHolySpirit = true,
                Occupation = "Church Right hand",
                PhoneNumbers = new List<PhoneNumber>(1) { PhoneNumbersFaker() }
            };

            var david = new Person
            {
                Family = cagnettaFamily,
                AgeClassification = AgeClassification.Child,
                RecordStatus = RecordStatus.Active,
                Gender = Gender.Male,
                PhotoUrl = null,
                ConnectionStatus = ConnectionStatus.Member,
                BaptismStatus = new Baptism { IsBaptised = false },
                ChurchId = 1,
                FullName = new FullName { FirstName = "David", LastName = "Cagnetta" },
                BirthDate = new BirthDate { BirthDay = 06, BirthMonth = 07, BirthYear = 2017 },
                ReceivedHolySpirit = false,
            };

            var daniel = new Person
            {
                Family = cagnettaFamily,
                AgeClassification = AgeClassification.Child,
                RecordStatus = RecordStatus.Active,
                Gender = Gender.Male,
                PhotoUrl = null,
                ConnectionStatus = ConnectionStatus.Member,
                BaptismStatus = new Baptism { IsBaptised = true },
                ChurchId = 1,
                FullName = new FullName { FirstName = "Daniel", LastName = "Cagnetta" },
                BirthDate = new BirthDate { BirthDay = 28, BirthMonth = 06, BirthYear = 2013 },
                ReceivedHolySpirit = true,
            };

            await _dbContext.Person.AddAsync(dillan);
            await _dbContext.Person.AddAsync(danielle);
            await _dbContext.Person.AddAsync(david);
            await _dbContext.Person.AddAsync(daniel);

            await _dbContext.SaveChangesAsync();
        }

        private IEnumerable<Person> GetData()
        {
            var faker = new Faker("en");
            string lastName = faker.Name.LastName();

            var fullName = new Faker<FullName>()
                .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName())
                .RuleFor(u => u.LastName, (f, u) => lastName)
                .RuleFor(u => u.Suffix, (f, u) => f.Name.Suffix())
                .RuleFor(p => p.NickName, (f, u) => f.Lorem.Word());

            var testPeople = new Faker<Person>()
                .RuleFor(p => p.FullName, f => fullName)
                .RuleFor(p => p.PhotoUrl, f => f.Internet.Avatar())
                .RuleFor(p => p.Occupation, f => f.Commerce.Department())
                .RuleFor(p => p.ReceivedHolySpirit, f => f.PickRandom(true, false))
                .RuleFor(p => p.ConnectionStatus, f => f.PickRandom(ConnectionStatuses).Value)
                .RuleFor(p => p.Gender, f => f.PickRandom(Genders).Value)
                .RuleFor(p => p.AgeClassification, f => f.PickRandom(AgeClassifications).Value)
                .RuleFor(p => p.Email, f => EmailFaker())
                .RuleFor(p => p.ChurchId, f => f.PickRandom(Churches))
                .RuleFor(p => p.PhoneNumbers, f => PhoneNumbersFaker().Generate(f.Random.Number(0, 3)))
                .RuleFor(p => p.FirstVisitDate, f => f.Date.Past(yearsToGoBack: 2));
            ;

            // Generate singles and add to family
            var people = testPeople.Generate(200);
            people.ForEach(x =>
            {
                var familyFaker = new Faker<Family>()
                    .RuleFor(u => u.Name, f => $"{x.FullName.LastName} Family")
                    .RuleFor(u => u.Language, f => faker.PickRandom(Languages));

                x.Family = familyFaker.Generate();
            });

            return people;
        }


        private IEnumerable<Person> GenerateFamilyData(int howManyChildren)
        {
            var faker = new Faker("en");
            string lastName = faker.Name.LastName(Name.Gender.Male);

            var church = faker.PickRandom(Churches);

            var momFullName = new Faker<FullName>()
                .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName(Name.Gender.Female))
                .RuleFor(u => u.LastName, (f, u) => lastName);

            var dadFullName = new Faker<FullName>()
                .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName(Name.Gender.Male))
                .RuleFor(u => u.LastName, (f, u) => lastName);

            var familyFaker = new Faker<Family>()
                .RuleFor(u => u.Name, f => $"{lastName} Family")
                .RuleFor(u => u.Language, f => faker.PickRandom(Languages));

            var family = familyFaker.Generate();

            var momFaker = new Faker<Person>()
                    .RuleFor(u => u.ChurchId, f => church)
                    .RuleFor(u => u.Family, f => family)
                    .RuleFor(p => p.FullName, f => momFullName)
                    .RuleFor(p => p.PhotoUrl, f => f.Internet.Avatar())
                    .RuleFor(p => p.ConnectionStatus, f => f.PickRandom(ConnectionStatuses).Value)
                    .RuleFor(p => p.Gender, f => Gender.Female.Value)
                    .RuleFor(p => p.AgeClassification, f => AgeClassification.Adult.Value)
                    .RuleFor(p => p.Email, f => EmailFaker())
                    .RuleFor(p => p.PhoneNumbers, f => PhoneNumbersFaker().Generate(f.Random.Number(0, 3)))
                    .RuleFor(p => p.FirstVisitDate, f => f.Date.Past(yearsToGoBack: 2))
                ;

            var dadFaker = new Faker<Person>()
                    .RuleFor(u => u.ChurchId, f => church )
                    .RuleFor(u => u.Family, f => family )
                    .RuleFor(p => p.FullName, f => dadFullName)
                    .RuleFor(p => p.PhotoUrl, f => f.Internet.Avatar())
                    .RuleFor(p => p.ConnectionStatus, f => f.PickRandom(ConnectionStatuses).Value)
                    .RuleFor(p => p.Gender, f => Gender.Male.Value)
                    .RuleFor(p => p.AgeClassification, f => AgeClassification.Adult.Value)
                    .RuleFor(p => p.Email, f => EmailFaker())
                    .RuleFor(p => p.PhoneNumbers, f => PhoneNumbersFaker().Generate(f.Random.Number(0, 3)))
                    .RuleFor(p => p.FirstVisitDate, f => f.Date.Past(yearsToGoBack: 2))
                ;

            // Generate the family and add each person document to the family
            var mom = momFaker.Generate();
            var dad = dadFaker.Generate();
            var children = GenerateChildrenData(lastName, family, howManyChildren, church);

            var familyMembers = new List<Person>(children);
            familyMembers.Add(mom);
            familyMembers.Add(dad);

            return familyMembers;
        }

        private IEnumerable<Person> GenerateChildrenData(string lastName, Family family, int howManyChildren, int churchId)
        {
            var fullName = new Faker<FullName>()
                .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName())
                .RuleFor(u => u.LastName, (f, u) => lastName);

            var childFaker = new Faker<Person>()
                .RuleFor(u => u.ChurchId, f => churchId)
                .RuleFor(u => u.Family, f => family)
                .RuleFor(p => p.AgeClassification, f => AgeClassification.Child.Value)
                .RuleFor(p => p.ConnectionStatus, f => f.PickRandom(ConnectionStatuses).Value)
                .RuleFor(p => p.Gender, f => f.PickRandom(Genders).Value)
                .RuleFor(p => p.FullName, f => fullName);

            var children = childFaker.Generate(howManyChildren);

            return children;
        }

        private Faker<PhoneNumber> PhoneNumbersFaker()
        {
            var phoneNumbers = new Faker<PhoneNumber>()
                .RuleFor(u => u.IsMessagingEnabled, f => f.Random.Bool())
                .RuleFor(u => u.IsUnlisted, f => f.Random.Bool())
                .RuleFor(p => p.CountryCode, f => "+27")
                .RuleFor(p => p.Number, f => f.Phone.PhoneNumber("#########"));

            return phoneNumbers;

        }

        private Faker<Email> EmailFaker()
        {
            var email = new Faker<Email>()
                .RuleFor(u => u.IsActive, f => f.Random.Bool())
                .RuleFor(u => u.Address, f => f.Internet.Email());

            return email;
        }

        // Churches Ids
        private int[] Churches => new[] { 1, 2 };

        private string[] Languages => new[] { "English", "Afrikaans", "Xhosa", "IsiZulu" };

        private Gender[] Genders => new[] { Gender.Male, Gender.Female, Gender.Unknown };

        private ConnectionStatus[] ConnectionStatuses => new[] { ConnectionStatus.Member, ConnectionStatus.FirstTimer, ConnectionStatus.NewConvert };

        private AgeClassification[] AgeClassifications => new[] { AgeClassification.Adult, AgeClassification.Child, AgeClassification.Unknown };
    }
}

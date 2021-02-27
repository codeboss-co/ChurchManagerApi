using System;
using System.Threading.Tasks;
using Bogus;
using Churches.Persistence.Models;
using CodeBoss.AspNetCore.Startup;
using DbMigrations.DbContext;

namespace DbMigrations.Seeding
{
    /// <summary>
    /// Seeds the database with some dummy data
    /// </summary>
    public class ChurchesDbSeedInitializer : IInitializer
    {
        public int OrderNumber { get; } = 0;

        private readonly ChurchManagerDbContext _dbContext;

        public ChurchesDbSeedInitializer(ChurchManagerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task InitializeAsync()
        {
            var faker = new Faker();

            var group1 = new ChurchGroup {Name = faker.Address.City() + " Group", Description = faker.Company.Bs()};
            var group2 = new ChurchGroup {Name = faker.Address.City() + " Group", Description = faker.Company.Bs()};
            var churchGroups = new[] { group1, group2 };

            for (int i = 0; i < 10; i++)
            {
                _dbContext.Church.Add(new Church
                {
                    Name = faker.Address.City() + " Church",
                    Description = faker.Company.Bs(),
                    ShortCode = faker.Address.ZipCode(),
                    PhoneNumber = faker.Phone.PhoneNumber(),
                    ChurchGroup = faker.PickRandom(churchGroups),
                });
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}

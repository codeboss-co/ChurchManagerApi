using System.Threading.Tasks;
using Bogus;
using Churches.Persistence.Models;
using CodeBoss.AspNetCore.Startup;
using DbMigrations.DbContext;
using Microsoft.EntityFrameworkCore;

namespace DbMigrations.Seeding
{
    /// <summary>
    /// Seeds the database with some dummy data
    /// </summary>
    public class ChurchesDbSeedInitializer : IInitializer
    {
        public int OrderNumber { get; } = 0;

        private readonly ChurchManagerDbContext _dbContext;

        public ChurchesDbSeedInitializer()
        {
            var connectionstring = "Server=localhost;Port=5432;Database=churchmanager_db;User Id=admin;password=P455word1;";

            var optionsBuilder = new DbContextOptionsBuilder<ChurchManagerDbContext>();
            optionsBuilder.UseNpgsql(connectionstring);

            _dbContext = new ChurchManagerDbContext(optionsBuilder.Options);
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

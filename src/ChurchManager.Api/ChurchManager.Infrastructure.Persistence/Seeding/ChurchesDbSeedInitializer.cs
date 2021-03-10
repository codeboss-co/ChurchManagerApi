using System.Threading.Tasks;
using Bogus;
using ChurchManager.Infrastructure.Persistence.Contexts;
using ChurchManager.Persistence.Models.Churches;
using CodeBoss.AspNetCore.Startup;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Infrastructure.Persistence.Seeding
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
            var connectionString = "Server=localhost;Port=5432;Database=churchmanager_db;User Id=admin;password=P455word1;";

            var optionsBuilder = new DbContextOptionsBuilder<ChurchManagerDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            _dbContext = new ChurchManagerDbContext(optionsBuilder.Options);
        }

        public async Task InitializeAsync()
        {
            if (!await _dbContext.Church.AnyAsync())
            {
                var faker = new Faker();

                var group1 = new ChurchGroup { Name = faker.Address.City() + " Group", Description = faker.Company.Bs() };
                var group2 = new ChurchGroup { Name = faker.Address.City() + " Group", Description = faker.Company.Bs() };
                var churchGroups = new[] { group1, group2 };

                for(int i = 0; i < 10; i++)
                {
                    _dbContext.Church.Add(new Church
                    {
                        Name = faker.Address.City() + " Church",
                        Description = faker.Address.City() + " Church",
                        ShortCode = faker.Address.ZipCode(),
                        PhoneNumber = faker.Phone.PhoneNumber(),
                        ChurchGroup = faker.PickRandom(churchGroups),
                    });
                }

                await _dbContext.SaveChangesAsync();
            }
        }
    }
}

using System.Threading.Tasks;
using Bogus;
using ChurchManager.Domain.Features.Churches;
using ChurchManager.Infrastructure.Persistence.Contexts;
using CodeBoss.AspNetCore.Startup;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ChurchManager.Infrastructure.Persistence.Seeding.Development
{
    /// <summary>
    /// Seeds the database with some dummy data
    /// </summary>
    public class ChurchesFakeDbSeedInitializer : IInitializer
    {
        public int OrderNumber { get; } = 0;
        private readonly IServiceScopeFactory _scopeFactory;

        public ChurchesFakeDbSeedInitializer(IServiceScopeFactory scopeFactory) => _scopeFactory = scopeFactory;

        public async Task InitializeAsync()
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ChurchManagerDbContext>();

            if(!await dbContext.Church.AnyAsync())
            {
                var faker = new Faker();

                var group1 = new ChurchGroup { Name = faker.Address.City() + " Group", Description = faker.Company.Bs() };
                var group2 = new ChurchGroup { Name = faker.Address.City() + " Group", Description = faker.Company.Bs() };
                var churchGroups = new[] { group1, group2 };

                for(int i = 0; i < 10; i++)
                {
                    dbContext.Church.Add(new Church
                    {
                        Name = faker.Address.City() + " Church",
                        Description = faker.Address.City() + " Church",
                        ShortCode = faker.Address.ZipCode(),
                        PhoneNumber = faker.Phone.PhoneNumber(),
                        ChurchGroup = faker.PickRandom(churchGroups),
                    });
                }

                await dbContext.SaveChangesAsync();
            }
        }
    }
}

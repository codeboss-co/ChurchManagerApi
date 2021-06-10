using System.Threading.Tasks;
using ChurchManager.Domain.Features.Churches;
using ChurchManager.Infrastructure.Persistence.Contexts;
using CodeBoss.AspNetCore.Startup;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ChurchManager.Infrastructure.Persistence.Seeding.Production
{
    /// <summary>
    /// Seeds the database with some dummy data
    /// </summary>
    public class ChurchesDbSeedInitializer : IInitializer
    {
        public int OrderNumber { get; } = 0;
        private readonly IServiceScopeFactory _scopeFactory;

        public ChurchesDbSeedInitializer(IServiceScopeFactory scopeFactory) => _scopeFactory = scopeFactory;

        public async Task InitializeAsync()
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ChurchManagerDbContext>();

            if(!await dbContext.Church.AnyAsync())
            {
                var group1 = new ChurchGroup { Name = "Cape Town Group", Description = "Cape Town Group" };
                var group2 = new ChurchGroup { Name = "North Group", Description = "North Group" };

                await dbContext.Church.AddAsync(new Church
                {
                    Name = "Waterfront",
                    Description = "CE Waterfront",
                    ShortCode = "CEWF",
                    PhoneNumber = "0214472005",
                    Address = "7 W Quay Rd, Victoria & Alfred Waterfront, Cape Town, 8002",
                    ChurchGroup = group1,
                });

                await dbContext.Church.AddAsync(new Church
                {
                    Name = "North",
                    Description = "CE Cape Town North Church",
                    ShortCode = "CECN",
                    PhoneNumber = "0745359808",
                    Address = "6 Viro Cres, Stikland Industrial, Cape Town, 7530",
                    ChurchGroup = group2,
                });

                await dbContext.SaveChangesAsync();
            }
        }
    }
}

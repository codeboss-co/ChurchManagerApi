using System.Threading.Tasks;
using ChurchManager.Infrastructure.Persistence.Contexts;
using ChurchManager.Persistence.Models.Churches;
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
                var group1 = new ChurchGroup { Name = "Cape Town Group", Description = "CE Cape Town Group" };
                var group2 = new ChurchGroup { Name = "North Group", Description = "CE North Group" };

                await dbContext.Church.AddAsync(new Church
                {
                    Name = "CE Cape Town",
                    Description = "CE Cape Town Church",
                    ShortCode = "CECT",
                    PhoneNumber = "021 447 2005",
                    Address = "7 W Quay Rd, Victoria & Alfred Waterfront, Cape Town, 8002",
                    ChurchGroup = group1,
                });

                await dbContext.Church.AddAsync(new Church
                {
                    Name = "CE North",
                    Description = "CE Cape Town North Church",
                    ShortCode = "CECT",
                    PhoneNumber = "074 535 9808",
                    Address = "6 Viro Cres, Stikland Industrial, Cape Town, 7530",
                    ChurchGroup = group2,
                });

                await dbContext.SaveChangesAsync();
            }
        }
    }
}

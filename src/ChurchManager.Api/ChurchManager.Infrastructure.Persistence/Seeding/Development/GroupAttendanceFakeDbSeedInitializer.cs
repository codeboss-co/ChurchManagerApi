using System;
using System.Threading.Tasks;
using ChurchManager.Infrastructure.Persistence.Contexts;
using ChurchManager.Persistence.Models.Groups;
using CodeBoss.AspNetCore.Startup;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ChurchManager.Infrastructure.Persistence.Seeding.Development
{
    public class GroupAttendanceFakeDbSeedInitializer : IInitializer
    {
        public int OrderNumber => 99;

        private readonly IServiceScopeFactory _scopeFactory;

        public GroupAttendanceFakeDbSeedInitializer(IServiceScopeFactory scopeFactory) => _scopeFactory = scopeFactory;

        public async Task InitializeAsync()
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ChurchManagerDbContext>();

            if (!await dbContext.GroupAttendance.AnyAsync())
            {
                var groups = dbContext.Group.AsQueryable().AsNoTracking();

                foreach (var group in groups)
                {
                    for (int i = 0; i < 20; i++)
                    {
                        var attendance = new GroupAttendance
                        {

                        };
                    }
                }
            }
        }
    }
}

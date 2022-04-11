using System.Threading.Tasks;
using ChurchManager.Domain.Features.Churches;
using ChurchManager.Infrastructure.Persistence.Contexts;
using CodeBoss.AspNetCore.Startup;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ChurchManager.Infrastructure.Persistence.Seeding.Production
{
    public class ChurchAttendanceTypeDbInitializer : IInitializer
    {
        public int OrderNumber => 4;

        private readonly IServiceScopeFactory _scopeFactory;

        public ChurchAttendanceTypeDbInitializer(IServiceScopeFactory scopeFactory) => _scopeFactory = scopeFactory;

        public async Task InitializeAsync()
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ChurchManagerDbContext>();

            if (!await dbContext.ChurchAttendanceType.AnyAsync())
            {
                var sundayService = new ChurchAttendanceType { Name = "Sunday", Description = "Sunday Church Service" };
                var wednesdayService = new ChurchAttendanceType { Name = "Midweek", Description = "Midweek Church Service" };

                await dbContext.AddAsync(sundayService);
                await dbContext.AddAsync(wednesdayService);

                await dbContext.SaveChangesAsync();
            }
        }
    }
}

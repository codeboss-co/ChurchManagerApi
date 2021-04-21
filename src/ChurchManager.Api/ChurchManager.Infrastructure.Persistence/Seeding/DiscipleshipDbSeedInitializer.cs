using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChurchManager.Infrastructure.Persistence.Contexts;
using ChurchManager.Persistence.Models.Discipleship;
using CodeBoss.AspNetCore.Startup;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ChurchManager.Infrastructure.Persistence.Seeding
{
    public class DiscipleshipDbSeedInitializer : IInitializer
    {
        public int OrderNumber { get; } = 5;

        private readonly IServiceScopeFactory _scopeFactory;

        public DiscipleshipDbSeedInitializer(IServiceScopeFactory scopeFactory) => _scopeFactory = scopeFactory;

        public async Task InitializeAsync()
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ChurchManagerDbContext>();

            if (!await dbContext.DiscipleshipType.AnyAsync())
            {
                var newConvertsDiscipleshipType = new DiscipleshipType { Name = "New Converts" };
                var foundationSchoolStepDefinition = new DiscipleshipStepDefinition
                {
                    Name = "Foundation School",
                    Description = "Basics of our Faith and Doctrine",
                    DiscipleshipType = newConvertsDiscipleshipType
                };
               

                await dbContext.AddAsync(foundationSchoolStepDefinition);

                await AddFoundationSchoolStepsToPeopleAsync(dbContext, foundationSchoolStepDefinition);

                await dbContext.SaveChangesAsync();
            }
        }

        private async Task AddFoundationSchoolStepsToPeopleAsync(ChurchManagerDbContext dbContext, DiscipleshipStepDefinition definition)
        {
            var dillan = await dbContext.Person.FirstOrDefaultAsync(x => x.Id == 1);
            var personFoundationSchoolStep = new DiscipleshipStep
            {
                Definition = definition,
                CompletionDate = DateTime.Today.AddYears(-15),
                Person = dillan
            };

            var newConvertsDiscipleshipProgram = new DiscipleshipProgram
            {
                Name = "New Converts Program",
                Description = "Discipleship for New Converts",
                DiscipleshipSteps = new List<DiscipleshipStep>(1) { personFoundationSchoolStep }
            };

            await dbContext.AddAsync(personFoundationSchoolStep);
            await dbContext.AddAsync(newConvertsDiscipleshipProgram);
        }
    }
}

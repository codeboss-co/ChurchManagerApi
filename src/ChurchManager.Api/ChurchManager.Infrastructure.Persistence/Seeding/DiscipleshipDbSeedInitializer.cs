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

            if (!await dbContext.DiscipleshipProgram.AnyAsync())
            {
                var foundationSchoolStepDefinition = new DiscipleshipStepDefinition
                {
                    Name = "Foundation School",
                    Description = "Basics of our Faith and Doctrine",
                    IconCssClass = "heroicons-solid:academic-cap",
                    Order = 0
                };
                var baptismClassStepDefinition = new DiscipleshipStepDefinition
                {
                    Name = "Baptism Class",
                    Description = "Understanding Baptism",
                    AllowMultiple = false,
                    Order = 1
                };

                var newConvertsDiscipleshipProgram = new DiscipleshipProgram
                {
                    Name = "New Christians Program",
                    Description = "Discipleship for New Converts",
                    Category = "New Christians",
                    StepDefinitions = new List<DiscipleshipStepDefinition>(1) { foundationSchoolStepDefinition, baptismClassStepDefinition }
                };

                await dbContext.AddAsync(newConvertsDiscipleshipProgram);

                await AddFoundationSchoolStepsToPeopleAsync(dbContext, foundationSchoolStepDefinition, baptismClassStepDefinition);

                await dbContext.SaveChangesAsync();
            }
        }

        private async Task AddFoundationSchoolStepsToPeopleAsync(
            ChurchManagerDbContext dbContext, 
            DiscipleshipStepDefinition foundationSchool,
            DiscipleshipStepDefinition baptismClass
            )
        {
            var dillan = await dbContext.Person.FirstOrDefaultAsync(x => x.Id == 1);
            var personFoundationSchoolStep = new DiscipleshipStep
            {
                Definition = foundationSchool,
                CompletionDate = DateTime.Today.AddYears(-15),
                Status = "Completed",
                Person = dillan
            };
            var baptismClassStep = new DiscipleshipStep
            {
                Definition = baptismClass,
                StartDateTime = DateTime.Today.AddYears(-15),
                Status = "In Progress",
                Person = dillan
            };
            
            await dbContext.AddAsync(personFoundationSchoolStep);
            await dbContext.AddAsync(baptismClassStep);
        }
    }
}

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
                var baptismClassStepDefinition = new DiscipleshipStepDefinition
                {
                    Name = "Baptism Class",
                    Description = "Understanding Baptism",
                    DiscipleshipType = newConvertsDiscipleshipType
                };


                await dbContext.AddAsync(foundationSchoolStepDefinition);
                await dbContext.AddAsync(baptismClassStepDefinition);

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
                CommencementDate = DateTime.Today.AddYears(-15),
                Status = "In Progress",
                Person = dillan
            };

            var newConvertsDiscipleshipProgram = new DiscipleshipProgram
            {
                Name = "New Converts Program",
                Description = "Discipleship for New Converts",
                DiscipleshipSteps = new List<DiscipleshipStep>(1) { personFoundationSchoolStep, baptismClassStep }
            };

            dillan.DiscipleshipPrograms.Add(newConvertsDiscipleshipProgram);

            await dbContext.AddAsync(personFoundationSchoolStep);
            await dbContext.AddAsync(newConvertsDiscipleshipProgram);
        }
    }
}

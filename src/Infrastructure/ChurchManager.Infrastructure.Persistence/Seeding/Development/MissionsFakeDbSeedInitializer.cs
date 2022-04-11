using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using ChurchManager.Domain.Features.Groups;
using ChurchManager.Domain.Features.Missions;
using ChurchManager.Infrastructure.Persistence.Contexts;
using CodeBoss.AspNetCore.Startup;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ChurchManager.Infrastructure.Persistence.Seeding.Development
{
    public class MissionsFakeDbSeedInitializer : IInitializer
    {
        public int OrderNumber => 99;

        private readonly IServiceScopeFactory _scopeFactory;

        private string[] Streams => new[] { "Person", "Group", "Church" };
        private string[] Categories => new[] { "ROSA", "Healing Streams" };
        private string[] Types => new[] { "InReach", "OutReach" };

        public MissionsFakeDbSeedInitializer(IServiceScopeFactory scopeFactory) => _scopeFactory = scopeFactory;

        public async Task InitializeAsync()
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ChurchManagerDbContext>();

            if (!await dbContext.Mission.AnyAsync())
            {
                var groups = dbContext.Group.AsQueryable().AsNoTracking().Take(4);
                var people = dbContext.Person.AsQueryable().AsNoTracking().Take(4);

                var faker = new Faker();
                var random = new Random();

                var attendances = new List<Mission>();

                foreach(var group in groups)
                {
                    var count = random.Next(1, 10);
                    for (int i = 0; i < count; i++)
                    {
                        attendances.AddRange(MissionsForGroups(group, faker, random));
                    }
                }

                foreach(var person in people)
                {
                    var count = random.Next(1, 4);
                    for(int i = 0; i < count; i++)
                    {
                        attendances.AddRange(MissionsForPerson(person, faker, random));
                    }
                }

                await dbContext.Mission.AddRangeAsync(attendances);
                await dbContext.SaveChangesAsync();
            }
        }

        public IEnumerable<Mission> MissionsForGroups(Group group, Faker faker, Random random)
        {
            var attendance = new
            {
                attendancedate = faker.Date.Between(new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 01), DateTime.UtcNow),
                members = random.Next(1, 100),
                firsttimers = random.Next(1, 50),
            };

            yield return new Mission
            {
                Name = $"{faker.Commerce.Department(1)} Mission Event",
                Type = faker.PickRandom(Types),
                Category = faker.PickRandom(Categories),
                Stream = "Group",
                GroupId = group.Id,
                ChurchId = group.ChurchId,
                StartDateTime = attendance.attendancedate,
                Attendance = new Attendance
                {
                    AttendanceCount = attendance.members + attendance.firsttimers,
                    FirstTimerCount = attendance.firsttimers,
                    // Max half the first timers will be born again
                    NewConvertCount = random.Next(0, (int)Math.Round((decimal)attendance.firsttimers / 2)),
                    ReceivedHolySpiritCount = random.Next(0, (int)Math.Round((decimal)attendance.firsttimers / 2))
                }
            };
        }

        public IEnumerable<Mission> MissionsForPerson(Domain.Features.People.Person person, Faker faker, Random random)
        {
            var attendance = new
            {
                attendancedate = faker.Date.Between(new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 01), DateTime.UtcNow),
                members = random.Next(1, 100),
                firsttimers = random.Next(1, 50),
            };

            yield return new Mission
            {
                Name = $"{faker.Commerce.Department(1)} Mission Event",
                Type = faker.PickRandom(Types),
                Category = faker.PickRandom(Categories),
                Stream = "Person",
                PersonId = person.Id,
                StartDateTime = attendance.attendancedate,
                Attendance = new Attendance
                {
                    AttendanceCount = attendance.members + attendance.firsttimers,
                    FirstTimerCount = attendance.firsttimers,
                    // Max half the first timers will be born again
                    NewConvertCount = random.Next(0, (int)Math.Round((decimal)attendance.firsttimers / 2)),
                    ReceivedHolySpiritCount = random.Next(0, (int)Math.Round((decimal)attendance.firsttimers / 2))
                }
            };
        }
    }
}

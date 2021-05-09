using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bogus;
using ChurchManager.Domain.Features.Groups;
using ChurchManager.Infrastructure.Persistence.Contexts;
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

                var faker = new Faker();
                var random = new Random();

                var attendances = new List<GroupAttendance>();

                foreach(var group in groups)
                {
                    var count = random.Next(5, 21);
                    for (int i = 0; i < count; i++)
                    {
                        var attendance = new
                        {
                            attendancedate = faker.Date.Between(new DateTime(2020, 01, 01), DateTime.Now),
                            members = random.Next(0, 10),
                            firsttimers = random.Next(1, 5),
                        };

                        attendances.Add(new GroupAttendance
                            {
                                GroupId = group.Id,
                                AttendanceDate = attendance.attendancedate,
                                AttendanceCount = attendance.members + attendance.firsttimers,
                                FirstTimerCount = attendance.firsttimers,
                                // Max half the first timers will be born again
                                NewConvertCount = random.Next(0, (int)Math.Round((decimal)attendance.firsttimers / 2)),
                                ReceivedHolySpiritCount = random.Next(0, (int)Math.Round((decimal)attendance.firsttimers / 2))
                            });
                    }
                }

                await dbContext.GroupAttendance.AddRangeAsync(attendances);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}

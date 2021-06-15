using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using ChurchManager.Domain.Features.Groups;
using ChurchManager.Infrastructure.Persistence.Contexts;
using CodeBoss.AspNetCore.Startup;
using Ical.Net.Serialization;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ChurchManager.Infrastructure.Persistence.Seeding.Development
{
    /// <summary>
    /// Seeds the database with some dummy data
    /// </summary>
    public class GroupsFakeDbSeedInitializer : IInitializer
    {
        public int OrderNumber { get; } = 2;
        private readonly IServiceScopeFactory _scopeFactory;
        private ChurchManagerDbContext _dbContext;

        int personId = 2;

        // Cell Group Type
        private readonly GroupType _sectionGroupType = new() { Name = "Section", Description = "Group Section", IconCssClass = "heroicons_outline:collection" };
        private readonly GroupType _cellGroupType = new() { Name = "Cell", Description = "Cell Ministry", IconCssClass = "heroicons_outline:share" };

        private static readonly CalendarSerializer CalendarSerializer = new();

        public GroupsFakeDbSeedInitializer(IServiceScopeFactory scopeFactory) => _scopeFactory = scopeFactory;

        public async Task InitializeAsync()
        {
            using var scope = _scopeFactory.CreateScope();
            _dbContext = scope.ServiceProvider.GetRequiredService<ChurchManagerDbContext>();

            if (!_dbContext.GroupType.Any())
            {
                await _dbContext.GroupType.AddAsync(_sectionGroupType);
                await _dbContext.GroupType.AddAsync(_cellGroupType);
                await _dbContext.SaveChangesAsync();
            }

            if (!await _dbContext.GroupTypeRole.AnyAsync())
            {
                var cellLeaderRole = new GroupTypeRole
                {
                    Name = "Leader", Description = "Cell Leader", IsLeader = true,
                    CanView = true, CanEdit = true, CanManageMembers = true,
                    GroupType = _cellGroupType
                };
                var cellAssistantRole = new GroupTypeRole
                    { Name = "Assistant", Description = "Assistant Leader", GroupType = _cellGroupType, IsLeader = true };
                var cellMemberRole = new GroupTypeRole
                    { Name = "Member", Description = "Group Member", GroupType = _cellGroupType };

                await _dbContext.GroupTypeRole.AddAsync(cellLeaderRole);
                await _dbContext.GroupTypeRole.AddAsync(cellAssistantRole);
                await _dbContext.GroupTypeRole.AddAsync(cellMemberRole);

                await _dbContext.SaveChangesAsync();
            }

            if (!await _dbContext.Group.AnyAsync())
            {
                // Cell Groups Section
                var cellSectionParentGroup = new Group
                {
                    GroupType = _sectionGroupType,
                    Name = "Cell Groups",
                    Description = "Grouping section for cell groups",
                    CreatedDate = DateTime.UtcNow,
                    ChurchId = 1
                };

                // await SeedMyGroups();
                await _dbContext.Group.AddRangeAsync(GenerateGroups(2, cellSectionParentGroup, groupLeaderPersonId: 1));
                await _dbContext.Group.AddRangeAsync(GenerateGroups(2, cellSectionParentGroup));
                await _dbContext.Group.AddRangeAsync(GenerateGroups(2, cellSectionParentGroup));
                await _dbContext.Group.AddRangeAsync(GenerateGroups(2, cellSectionParentGroup));
                await _dbContext.Group.AddRangeAsync(GenerateGroups(2, cellSectionParentGroup));
                await _dbContext.SaveChangesAsync();
            }
        }

        private async Task SeedMyGroups()
        {
            if(!await _dbContext.Group.AnyAsync())
            {
                var faker = new Faker();
                var random = new Random();

                var cellLeader = new Faker<GroupMember>()
                    .RuleFor(u => u.PersonId, f => 1)
                    .RuleFor(u => u.GroupRoleId, f => 1);

                var cellMember = new Faker<GroupMember>()
                .RuleFor(u => u.PersonId, f => personId++)
                .RuleFor(u => u.GroupRoleId, f => 2);
                
                for(int i = 0; i < 4; i++)
                {
                    var cellGroupMembers = cellMember.Generate(random.Next(1, 30));
                    cellGroupMembers.Add(cellLeader);

                    _dbContext.Group.Add(new Group
                    {
                        Name = faker.Address.City() + " Cell Group",
                        Description = faker.Address.City() + " Cell Group",
                        GroupType = _cellGroupType,
                        ChurchId = 1,
                        Members = cellGroupMembers,
                        StartDate = DateTimeOffset.UtcNow,
                        IsOnline = i % 2 == 0,
                        Address = faker.Address.FullAddress(),
                    });
                }

                await _dbContext.SaveChangesAsync();
            }
        }

        private IList<Group> GenerateGroups(int count, Group parentGroup, int? groupLeaderPersonId = null, bool generateChildren = true, int level = 0)
        {
            var faker = new Faker();
            var random = new Random();

            IList<Group> groups = new List<Group>(count);
            for(int i = 0; i < count; i++)
            {
                var fakeName =  $"{ faker.Address.City()} Cell";
                var group = new Group
                {
                    Name = parentGroup.Name == "Cell Groups" ? fakeName : $"{ parentGroup.Name} - {level + i}",
                    Description = fakeName,
                    GroupType = _cellGroupType,
                    ChurchId = 1,
                    Members = GenerateGroupMembers(groupLeaderPersonId),
                    StartDate = DateTimeOffset.UtcNow,
                    IsOnline = i % 2 == 0,
                    Address = faker.Address.FullAddress(),
                    Schedule = GenerateSchedule(),
                    ParentGroup = parentGroup
                };

                // Generate sub groups
                if (generateChildren)
                {
                    level++;
                    var children = GenerateGroups(random.Next(0, 4), group, null, false, level);
                    group.Groups = children;
                }

                groups.Add(group);
            }

            return groups;
        }

        private Schedule GenerateSchedule()
        {
            var calendar = InetCalendarHelper.CalendarWithWeeklyRecurrence(
                DateTime.UtcNow, null,
                new TimeSpan(18, 0, 0), new [] {DayOfWeek.Thursday});

            return new Schedule
            {
                WeeklyDayOfWeek = DayOfWeek.Thursday,
                StartDate = DateTime.UtcNow.Date,
                iCalendarContent = CalendarSerializer.SerializeToString(calendar),
            };
        }

        private IList<GroupMember> GenerateGroupMembers(int? groupLeaderPersonId = null)
        {
            var random = new Random();

            var totalPeopleInDb = _dbContext.Person.Count();

            var cellLeader = new Faker<GroupMember>()
                .RuleFor(u => u.PersonId, f => groupLeaderPersonId ?? random.Next(2, totalPeopleInDb / 2)) // The personIds generated
                .RuleFor(u => u.GroupRoleId, f => 1);

            var cellMember = new Faker<GroupMember>()
                .RuleFor(u => u.PersonId, f => random.Next((totalPeopleInDb / 2)+1, totalPeopleInDb))
                .RuleFor(u => u.GroupRoleId, f => random.Next(2, 4));

            var cellGroupMembers = cellMember.Generate(random.Next(1, 16));

            cellGroupMembers.Add(cellLeader);

            return cellGroupMembers;
        }
    }
}

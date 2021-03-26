using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bogus;
using ChurchManager.Infrastructure.Persistence.Contexts;
using ChurchManager.Persistence.Models.Groups;
using CodeBoss.AspNetCore.Startup;
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
        private readonly GroupType _cellGroupType  = new() { Name = "Cell", Description = "Cell Ministry" };

        public GroupsFakeDbSeedInitializer(IServiceScopeFactory scopeFactory) => _scopeFactory = scopeFactory;

        public async Task InitializeAsync()
        {
            using var scope = _scopeFactory.CreateScope();
            _dbContext = scope.ServiceProvider.GetRequiredService<ChurchManagerDbContext>();

            if (!await _dbContext.GroupMemberRole.AnyAsync())
            {
                var cellLeaderRole = new GroupMemberRole { Name = "Leader", Description = "Cell Leader", IsLeader = true };
                var cellMemberRole = new GroupMemberRole { Name = "Member", Description = "Cell Member" };

                await _dbContext.GroupMemberRole.AddAsync(cellLeaderRole);
                await _dbContext.GroupMemberRole.AddAsync(cellMemberRole);

                await _dbContext.SaveChangesAsync();
            }

            if (!await _dbContext.Group.AnyAsync())
            { 
                await SeedMyGroups();
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
                    .RuleFor(u => u.GroupMemberRoleId, f => 1);

                var cellMember = new Faker<GroupMember>()
                .RuleFor(u => u.PersonId, f => personId++)
                .RuleFor(u => u.GroupMemberRoleId, f => 2);
                
                for(int i = 0; i < 20; i++)
                {
                    var cellGroupMembers = cellMember.Generate(random.Next(1, 30));
                    cellGroupMembers.Add(cellLeader);

                    _dbContext.Group.Add(new Group
                    {
                        Name = faker.Address.City() + " Cell Group",
                        Description = faker.Address.City() + " Cell Group",
                        GroupType = _cellGroupType,
                        ChurchId = 1,
                        Members = cellGroupMembers
                    });
                }

                await _dbContext.SaveChangesAsync();
            }
        }

        private IList<GroupMember> GenerateGroupMembers()
        {
            var random = new Random();

            var cellLeader = new Faker<GroupMember>()
                .RuleFor(u => u.PersonId, f => random.Next(2, 200)) // The personIds generated
                .RuleFor(u => u.GroupMemberRoleId, f => 1);

            var cellMember = new Faker<GroupMember>()
                .RuleFor(u => u.PersonId, f => random.Next(201, 378))
                .RuleFor(u => u.GroupMemberRoleId, f => 2);

            var cellGroupMembers = cellMember.Generate(random.Next(1, 30));

            cellGroupMembers.Add(cellLeader);

            return cellGroupMembers;
        }
    }
}

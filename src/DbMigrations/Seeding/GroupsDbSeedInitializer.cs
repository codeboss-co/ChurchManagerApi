using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bogus;
using CodeBoss.AspNetCore.Startup;
using DbMigrations.DbContext;
using Groups.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using People.Domain;

namespace DbMigrations.Seeding
{
    /// <summary>
    /// Seeds the database with some dummy data
    /// </summary>
    public class GroupsDbSeedInitializer : IInitializer
    {
        public int OrderNumber { get; } = 2;

        private readonly ChurchManagerDbContext _dbContext;


        public GroupsDbSeedInitializer()
        {
            var connectionString = "Server=localhost;Port=5432;Database=churchmanager_db;User Id=admin;password=P455word1;";

            var optionsBuilder = new DbContextOptionsBuilder<ChurchManagerDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            _dbContext = new ChurchManagerDbContext(optionsBuilder.Options);
        }

        public async Task InitializeAsync()
        {
            var groupType = new GroupType {Name = "Cell", Description = "Cell Ministry"};

            if (!await _dbContext.Group.AnyAsync())
            {
                var cellLeaderRole = new GroupMemberRole { Name = "Leader", Description = "Cell Leader", IsLeader = true };
                var cellMemberRole = new GroupMemberRole { Name = "Member", Description = "Cell Member" };

                await _dbContext.GroupMemberRole.AddAsync(cellLeaderRole);
                await _dbContext.GroupMemberRole.AddAsync(cellMemberRole);

                await _dbContext.SaveChangesAsync();
            }
               

            if (!await _dbContext.Group.AnyAsync())
            {
                var faker = new Faker();
                var random = new Random();
                for (int i = 0; i < 20; i++)
                {
                    _dbContext.Group.Add(new Group
                    {
                        Name = faker.Address.City() + " Cell Group",
                        Description = faker.Address.City() + " Cell Group",
                        GroupType = groupType,
                        ChurchId = random.Next(1, 3),
                        Members = GenerateGroupMembers()
                    });
                }

                await _dbContext.SaveChangesAsync();
            }
        }

        private IList<GroupMember> GenerateGroupMembers()
        {
            var random = new Random();
            int personId = 1;

            var cellLeader = new Faker<GroupMember>()
                .RuleFor(u => u.PersonId, f => personId++)
                .RuleFor(u => u.GroupMemberRoleId, f => 1);

            var cellMember = new Faker<GroupMember>()
                .RuleFor(u => u.PersonId, f => personId++)
                .RuleFor(u => u.GroupMemberRoleId, f => 2);

            var cellGroupMembers = cellMember.Generate(random.Next(1, 30));

            cellGroupMembers.Add(cellLeader);

            return cellGroupMembers;
        }


        // Churches Ids
        private int[] Churches => new[] { 1, 2 };

        private Gender[] Genders => new[] { Gender.Male, Gender.Female, Gender.Unknown };

        private string[] ConnectionStatuses => new[] { "Member", "Visitor", "New Convert" };

        private AgeClassification[] AgeClassifications => new[] { AgeClassification.Adult, AgeClassification.Child, AgeClassification.Unknown };
    }
}

using System.Threading.Tasks;
using ChurchManager.Infrastructure.Persistence.Contexts;
using ChurchManager.Persistence.Models.Groups;
using CodeBoss.AspNetCore.Startup;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ChurchManager.Infrastructure.Persistence.Seeding.Production
{
    /// <summary>
    /// Seeds the database with some dummy data
    /// </summary>
    public class GroupsDbSeedInitializer : IInitializer
    {
        public int OrderNumber { get; } = 2;
        private readonly IServiceScopeFactory _scopeFactory;
        private ChurchManagerDbContext _dbContext;

        // Cell Group Type
        private readonly GroupType _cellGroupType  = new() { Name = "Cell", Description = "Cell Ministry" };

        public GroupsDbSeedInitializer(IServiceScopeFactory scopeFactory) => _scopeFactory = scopeFactory;

        public async Task InitializeAsync()
        {
            using var scope = _scopeFactory.CreateScope();
            _dbContext = scope.ServiceProvider.GetRequiredService<ChurchManagerDbContext>();

            if (!await _dbContext.GroupType.AnyAsync())
            {
                await _dbContext.AddAsync(_cellGroupType);
                await _dbContext.SaveChangesAsync();
            }

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
                await SeedGroups();
            }
        }

        private Task SeedGroups()
        {
            return Task.CompletedTask;
        }
    }
}

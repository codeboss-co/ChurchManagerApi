using Infrastructure.Persistence.Model;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class GroupsDbContext : DbContext
    {
        public GroupsDbContext(DbContextOptions<GroupsDbContext> options) : base(options) { }

        public DbSet<Group> Groups { get; set; }
    }
}

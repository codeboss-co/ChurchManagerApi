using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class GroupsDbContext : DbContext
    {
        public GroupsDbContext(DbContextOptions<GroupsDbContext> options) : base(options) { }
    }
}

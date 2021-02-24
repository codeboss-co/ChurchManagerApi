using Groups.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class GroupsDbContext : DbContext
    {
        public GroupsDbContext(DbContextOptions<GroupsDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Groups");
        }

        public DbSet<Group> Group { get; set; }
        public DbSet<GroupAttendance> GroupAttendance { get; set; }
        public DbSet<GroupMember> GroupMember { get; set; }
        public DbSet<GroupMemberAttendance> GroupMemberAttendance { get; set; }
        public DbSet<GroupMemberRole> GroupMemberRole { get; set; }
    }
}

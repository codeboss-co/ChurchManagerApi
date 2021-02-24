using Churches.Infrastructure.Persistence.Model;
using Microsoft.EntityFrameworkCore;

namespace Churches.Infrastructure.Persistence
{
    public class ChurchesDbContext : DbContext
    {
        public ChurchesDbContext(DbContextOptions<ChurchesDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Churches");
        }

        public DbSet<Church> Church { get; set; }
    }
}

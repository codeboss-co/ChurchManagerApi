using Churches.Infrastructure.Persistence.Model;
using Microsoft.EntityFrameworkCore;

namespace Churches.Infrastructure.Persistence
{
    public class ChurchesDbContext : DbContext
    {
        public ChurchesDbContext(DbContextOptions<ChurchesDbContext> options) : base(options) { }

        public DbSet<Church> Church { get; set; }
    }
}

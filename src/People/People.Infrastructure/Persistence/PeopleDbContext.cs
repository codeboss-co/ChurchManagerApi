using Microsoft.EntityFrameworkCore;
using People.Persistence.Models;

namespace People.Infrastructure.Persistence
{
    public class PeopleDbContext : DbContext
    {
        public PeopleDbContext(DbContextOptions<PeopleDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("People");
        }

        public DbSet<Person> Person { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using People.Persistence.Models;

namespace People.Infrastructure.Persistence
{
    public class PeopleDbContext : DbContext
    {
        public PeopleDbContext(DbContextOptions<PeopleDbContext> options) : base(options) { }

        public DbSet<Person> Person { get; set; }
    }
}

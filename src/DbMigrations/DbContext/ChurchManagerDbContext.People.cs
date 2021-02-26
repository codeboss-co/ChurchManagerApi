using Microsoft.EntityFrameworkCore;
using People.Persistence.Models;

namespace DbMigrations.DbContext
{
    public partial class ChurchManagerDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbSet<Person> Person { get; set; }

    }
}

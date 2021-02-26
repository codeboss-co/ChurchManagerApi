using Microsoft.EntityFrameworkCore;
using People.Persistence.Models;

namespace DbMigrations.DbContext
{
    public partial class ChurchManagerDbContext
    {
        public DbSet<Person> Person { get; set; }

    }
}

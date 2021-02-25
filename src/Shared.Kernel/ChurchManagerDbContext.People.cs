using Microsoft.EntityFrameworkCore;
using People.Persistence.Models;

namespace Shared.Kernel
{
    public partial class ChurchManagerDbContext : DbContext
    {
        public DbSet<Person> Person { get; set; }

    }
}

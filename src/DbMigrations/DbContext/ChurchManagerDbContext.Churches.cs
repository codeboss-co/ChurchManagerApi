using Churches.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace DbMigrations.DbContext
{
    public partial class ChurchManagerDbContext
    {
        public DbSet<Church> Church { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;

namespace DbMigrations.DbContext
{
    public partial class ChurchManagerDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public ChurchManagerDbContext(DbContextOptions<ChurchManagerDbContext> options) : base(options) { }
    }
}

using Microsoft.EntityFrameworkCore;

namespace Shared.Kernel
{
    public partial class ChurchManagerDbContext : DbContext
    {
        public ChurchManagerDbContext(DbContextOptions<ChurchManagerDbContext> options) : base(options) { }
    }
}

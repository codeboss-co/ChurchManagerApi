using Churches.Infrastructure.Persistence.Model;
using Microsoft.EntityFrameworkCore;

namespace Shared.Kernel
{
    public partial class ChurchManagerDbContext
    {
        public DbSet<Church> Church { get; set; }
    }
}

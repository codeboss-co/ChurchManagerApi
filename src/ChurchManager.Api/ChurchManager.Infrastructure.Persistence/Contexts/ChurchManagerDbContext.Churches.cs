using ChurchManager.Persistence.Models.Churches;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Infrastructure.Persistence.Contexts
{
    public partial class ChurchManagerDbContext
    {
        public DbSet<Church> Church { get; set; }
    }
}

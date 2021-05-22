using ChurchManager.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Infrastructure.Persistence.Contexts
{
    public partial class ChurchManagerDbContext
    {
        public DbSet<UserLogin> UserLogin { get; set; }
    }
}

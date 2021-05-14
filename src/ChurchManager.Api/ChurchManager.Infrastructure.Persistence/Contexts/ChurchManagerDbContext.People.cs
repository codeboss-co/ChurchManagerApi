using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.People;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Infrastructure.Persistence.Contexts
{
    public partial class ChurchManagerDbContext
    {
        public DbSet<Family> Family { get; set; }
        public DbSet<Person> Person { get; set; }
        public DbSet<OnlineUser> OnlineUser { get; set; }
        public DbSet<PushDevice> PushDevice { get; set; }

    }
}

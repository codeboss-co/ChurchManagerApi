using ChurchManager.Persistence.Models.People;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Infrastructure.Persistence.Contexts
{
    public partial class ChurchManagerDbContext
    {
        public DbSet<Family> Family { get; set; }
        public DbSet<Person> Person { get; set; }
        public DbSet<OnlineUser> OnlineUser { get; set; }

    }
}

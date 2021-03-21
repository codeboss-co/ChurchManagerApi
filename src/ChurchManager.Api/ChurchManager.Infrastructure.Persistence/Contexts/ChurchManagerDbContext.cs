using System;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.Persistence.Shared;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Infrastructure.Persistence.Contexts
{
    public partial class ChurchManagerDbContext : DbContext, IChurchManagerDbContext
    {
        public ChurchManagerDbContext(DbContextOptions<ChurchManagerDbContext> options) : base(options)
        {
            // ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ChurchManagerDbContext).Assembly);
            
            // Apply some basic seeding if needed here
            //builder.DomainEntity<Person>().HasData(seedPositions);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach(var entry in ChangeTracker.Entries<AuditableEntity<int>>())
            {
                switch(entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        entry.Entity.ModifiedDate = DateTime.UtcNow;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}

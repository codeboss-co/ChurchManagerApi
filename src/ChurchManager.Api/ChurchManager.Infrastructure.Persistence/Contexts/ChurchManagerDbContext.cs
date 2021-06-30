using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Domain.Features.SharedKernel.MultiTenant;
using ChurchManager.Infrastructure.Abstractions;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.Infrastructure.Shared;
using ChurchManager.Persistence.Shared;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Infrastructure.Persistence.Contexts
{
    public partial class ChurchManagerDbContext : DbContext, IChurchManagerDbContext
    {
        private Tenant _tenant;
        private readonly IDomainEventPublisher _events;
        private readonly ITenantProvider _tenantProvider;
        private readonly ITenantCurrentUser _currentUser;

        public ChurchManagerDbContext(
            DbContextOptions<ChurchManagerDbContext> options,
            IDomainEventPublisher events,
            ITenantCurrentUser currentUser,
            ITenantProvider tenantProvider) : base(options)
        {
            _events = events;
            _tenantProvider = tenantProvider;
            _currentUser = currentUser;

            if (currentUser is not null)
            {
                _tenant = _tenantProvider.Tenants().FirstOrDefault(x => x.Name == currentUser.Tenant);
            }

            _tenant ??= _tenantProvider.Tenants().First();
            // ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured == false && _tenantProvider.Enabled)
            {
                optionsBuilder.UseNpgsql(_tenant.ConnectionString,
                    x => x.MigrationsAssembly("ChurchManager.Infrastructure.Persistence"));
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ChurchManagerDbContext).Assembly);
        }

        public override int SaveChanges()
        {
            _PreSaveChanges();
            var result = base.SaveChanges();
            _PostSaveChanges().GetAwaiter().GetResult();
            return result;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            _PreSaveChanges(ct);
            var result = await base.SaveChangesAsync(ct);
            await _PostSaveChanges(ct);
            return result;
        }

        private void _PreSaveChanges(CancellationToken ct = default)
        {
            _AddAuditInfo();
        }

        private async Task _PostSaveChanges(CancellationToken ct = default)
        {
            await _DispatchDomainEvents(ct);
        }

        private void _AddAuditInfo()
        {
            if(_currentUser is null)
                return;

            foreach(var entry in ChangeTracker.Entries<AuditableEntity<int>>())
            {
                switch(entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.UtcNow;
                        entry.Entity.CreatedBy = _currentUser.Id;
                        break;
                    case EntityState.Modified:
                        entry.Entity.ModifiedDate = DateTime.UtcNow;
                        entry.Entity.ModifiedBy = _currentUser.Id;
                        break;
                }
            }
        }

        private async Task _DispatchDomainEvents(CancellationToken ct = default)
        {
            // ignore events if no dispatcher provided
            if(_events is null) return;

            var domainEventEntities = ChangeTracker.Entries<Entity<int>>()
                .Select(po => po.Entity)
                .Where(po => po.DomainEvents.Any())
                .ToArray();

            foreach(var entity in domainEventEntities)
            {
                foreach (var @event in entity.DomainEvents)
                {
                    await _events.PublishAsync(@event, ct);
                }
                entity.ClearDomainEvents();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Domain.Features.SharedKernel.MultiTenant;
using ChurchManager.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ChurchManager.Infrastructure.Persistence
{
    public class DbTenantMigrationHostedService :  IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public DbTenantMigrationHostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var tenantProvider = scope.ServiceProvider.GetRequiredService<ITenantProvider>();
                
            var tenants = tenantProvider.Tenants();

            IEnumerable<Task> tasks = tenants.Select(tenant => MigrateTenantDatabase(tenant, tenantProvider));

            Console.WriteLine("Starting parallel execution of pending migrations...");
            await Task.WhenAll(tasks);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private async Task MigrateTenantDatabase(Tenant tenant, ITenantProvider provider)
        {
            var dbContextOptions = CreateDefaultDbContextOptions(tenant.ConnectionString);
            try
            {
                Console.WriteLine($"*** Beginning migration: [{tenant.Name}]");

                using var context = new ChurchManagerDbContext(dbContextOptions, null, null, provider);
                await context.Database.MigrateAsync();
            }
            catch(Exception e)
            {
                Console.WriteLine($"Error occurred during migration: {e.Message}");
                throw;
            }
        }

        private DbContextOptions<ChurchManagerDbContext> CreateDefaultDbContextOptions(string connectionString) =>
            new DbContextOptionsBuilder<ChurchManagerDbContext>()
                .UseNpgsql(connectionString)
                .Options;
    }
}

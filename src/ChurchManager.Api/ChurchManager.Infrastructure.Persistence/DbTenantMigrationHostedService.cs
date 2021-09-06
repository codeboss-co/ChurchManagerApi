using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Infrastructure.Persistence.Contexts.Factory;
using ChurchManager.Persistence.Shared;
using CodeBoss.MultiTenant;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

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
            
            var dbOptions = _serviceProvider.GetRequiredService<IOptions<DbOptions>>().Value;
            if (!dbOptions.Migrate)
            {
                Console.WriteLine("Migrations disabled.");
                return;
            }

            Console.WriteLine("Migrations enabled.");

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

        private async Task MigrateTenantDatabase(ITenant tenant, ITenantProvider provider)
        {
            try
            {
                Console.WriteLine($"*** Beginning migration: [{tenant.Name}]");

                using var context = DbContextFactory.Create(tenant.ConnectionString, provider);
                await context.Database.MigrateAsync();

                Console.WriteLine($"*** Completed migration: [{tenant.Name}]");
            }
            catch(Exception e)
            {
                Console.WriteLine($"Error occurred during migration: {e.Message} --> {tenant.ConnectionString}");
                throw;
            }
        }
    }
}

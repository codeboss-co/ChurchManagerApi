using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ChurchManager.Persistence.Shared
{
    /// <summary>
    /// Migration Hosted service that will migrate the database DbContext <see cref="DbContext"/> specified
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    public class DbMigrationHostedService<TDbContext> : IHostedService where TDbContext : DbContext
    {
        private readonly IServiceProvider _serviceProvider;

        public DbMigrationHostedService(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<TDbContext>();
            await db.Database.MigrateAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}

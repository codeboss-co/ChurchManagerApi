using CodeBoss.MultiTenant;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Infrastructure.Persistence.Contexts.Factory
{
    public static class DbContextFactory
    {
        public static ChurchManagerDbContext Create(string connectionString, ITenantProvider provider) =>
            new (CreateDefaultDbContextOptions(connectionString), provider);

        public static DbContextOptions<ChurchManagerDbContext> CreateDefaultDbContextOptions(string connectionString) =>
            new DbContextOptionsBuilder<ChurchManagerDbContext>()
                .UseNpgsql(connectionString)
                .Options;
    }
}

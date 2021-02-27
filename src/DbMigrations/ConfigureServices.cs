using ChurchManager.Shared.Persistence;
using CodeBoss.AspNetCore.Startup;
using Convey;
using DbMigrations.DbContext;
using DbMigrations.Seeding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DbMigrations
{
    public static class ConfigureServices
    {
        public static void AddChurchManagerDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ChurchManagerDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                    x => x.MigrationsAssembly("DbMigrations")));
            
            // Migrate database
            services.AddHostedService<DbMigrationHostedService<ChurchManagerDbContext>>();

            // Seeding: Switch this off in `appsettings.json`
            bool seedDatabaseEnabled = configuration.GetOptions<DbOptions>(nameof(DbOptions)).Seed;
            if(seedDatabaseEnabled)
            {
                services.AddInitializer<ChurchesDbSeedInitializer>();
            }
        }
    }
}

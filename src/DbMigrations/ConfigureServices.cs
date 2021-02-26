using ChurchManager.Shared.Persistence;
using DbMigrations.DbContext;
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


            services.AddHostedService<DbMigrationHostedService<ChurchManagerDbContext>>();
        }
    }
}

using Churches.Infrastructure.Persistence;
using ChurchManager.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Churches.Infrastructure
{
    public static class ConfigureServices
    {
        public static void AddChurchesInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ChurchesDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                    x => x.MigrationsAssembly("DbMigrations")));


            services.AddHostedService<DbMigrationHostedService<ChurchesDbContext>>();
        }
    }
}

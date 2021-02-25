using ChurchManager.Shared.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Kernel
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

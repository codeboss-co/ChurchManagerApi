using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class ConfigureServices
    {
        public static void AddGroupInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<GroupsDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                    x => x.MigrationsAssembly("DbMigrations")));


            services.AddHostedService<DbMigrationHostedService>();
        }
    }
}

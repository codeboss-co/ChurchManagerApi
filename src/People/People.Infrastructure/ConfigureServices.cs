using ChurchManager.Shared.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using People.Infrastructure.Persistence;

namespace People.Infrastructure
{
    public static class ConfigureServices
    {
        public static void AddPeopleInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PeopleDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                    x => x.MigrationsAssembly("DbMigrations")));


            services.AddHostedService<DbMigrationHostedService<PeopleDbContext>>();
        }
    }
}

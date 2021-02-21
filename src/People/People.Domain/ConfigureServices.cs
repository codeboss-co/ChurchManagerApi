using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using People.Infrastructure.Persistence.Repositories;

namespace People.Domain
{
    public static class ConfigureServices
    {
        public static void AddPeopleDomain(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IPersonDbRepository, PersonDbRepository>();
        }
    }
}

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using People.Domain.Repositories;
using People.Infrastructure;
using People.Infrastructure.Persistence.Repositories;

namespace People.Application
{
    public static class ConfigureServices
    {
        public static void AddPeopleDomain(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddPeopleInfrastructure(configuration);
            services.AddScoped<IPersonDbRepository, PersonDbRepository>();
        }
    }
}

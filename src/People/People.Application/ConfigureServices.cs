using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using People.Domain.Repositories;
using People.Infrastructure.Persistence.Repositories;

namespace People.Application
{
    public static class ConfigureServices
    {
        public static void AddPeopleApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IPersonDbRepository, PersonDbRepository>();
        }
    }
}

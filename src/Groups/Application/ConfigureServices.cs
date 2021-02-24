using CodeBoss.CQRS.Queries;
using Domain.Repositories;
using Infrastructure;
using Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class ConfigureServices
    {
        public static void AddGroupsDomain(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddGroupInfrastructure(configuration);
            services.AddScoped<IGroupDbRepository, GroupDbRepository>();
        }
    }
}

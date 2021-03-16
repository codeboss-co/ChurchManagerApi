using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Infrastructure.Abstractions;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.Infrastructure.Persistence.Contexts;
using ChurchManager.Infrastructure.Persistence.Repositories;
using ChurchManager.Infrastructure.Persistence.Seeding;
using ChurchManager.Infrastructure.Shared;
using ChurchManager.Persistence.Models.Groups;
using ChurchManager.Persistence.Models.People;
using ChurchManager.Persistence.Shared;
using CodeBoss.AspNetCore.Startup;
using Convey;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChurchManager.Infrastructure.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ChurchManagerDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                    x => x.MigrationsAssembly("ChurchManager.Infrastructure.Persistence")));
            
            // Database Health Check 
            services
                .AddHealthChecks()
                .AddDbContextCheck<ChurchManagerDbContext>();

            // Migrate database
            services.AddHostedService<DbMigrationHostedService<ChurchManagerDbContext>>();

            // Seeding: Switch this off in `appsettings.json`
            bool seedDatabaseEnabled = configuration.GetOptions<DbOptions>(nameof(DbOptions)).Seed;
            if(seedDatabaseEnabled)
            {
                services.AddInitializer<ChurchesDbSeedInitializer>();
                services.AddInitializer<PeopleDbSeedInitializer>();
                services.AddInitializer<GroupsDbSeedInitializer>();
            }

            #region Repositories

            services.AddTransient(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));
            services.AddScoped<IGroupDbRepository, GroupDbRepository>();
            services.AddScoped<IPersonDbRepository, PersonDbRepository>();
            services.AddScoped<IGroupAttendanceDbRepository, GroupAttendanceDbRepository>();

            #endregion

            services.AddScoped<IDataShapeHelper<Group>, DataShapeHelper<Group>>();
            services.AddScoped<IDataShapeHelper<Person>, DataShapeHelper<Person>>();
        }
    }
}

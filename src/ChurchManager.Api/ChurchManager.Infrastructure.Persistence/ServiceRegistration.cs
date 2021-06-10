using ChurchManager.Domain;
using ChurchManager.Domain.Features.Churches.Repositories;
using ChurchManager.Domain.Features.Communication.Services;
using ChurchManager.Domain.Features.Discipleship;
using ChurchManager.Domain.Features.Discipleship.Repositories;
using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.Infrastructure.Persistence.Contexts;
using ChurchManager.Infrastructure.Persistence.Repositories;
using ChurchManager.Infrastructure.Persistence.Seeding;
using ChurchManager.Infrastructure.Persistence.Seeding.Development;
using ChurchManager.Infrastructure.Persistence.Seeding.Production;
using ChurchManager.Persistence.Shared;
using CodeBoss.AspNetCore.Startup;
using Convey;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ChurchManager.Infrastructure.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services,
            IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            services.AddDbContext<ChurchManagerDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                    x => x.MigrationsAssembly("ChurchManager.Infrastructure.Persistence")));
            
            // Database Health Check 
            services
                .AddHealthChecks()
                .AddDbContextCheck<ChurchManagerDbContext>();

            services.AddScoped<IChurchManagerDbContext>(s => s.GetService<ChurchManagerDbContext>());
            services.AddScoped<DbContext>(s => s.GetService<ChurchManagerDbContext>());

            // Migrate database
            services.AddHostedService<DbMigrationHostedService<ChurchManagerDbContext>>();

            // Seeding: Switch this off in `appsettings.json`
            bool seedDatabaseEnabled = configuration.GetOptions<DbOptions>(nameof(DbOptions)).Seed;
            if (seedDatabaseEnabled)
            {
                services.AddInitializer<ChurchAttendanceTypeDbInitializer>();
                services.AddInitializer<DiscipleshipDbSeedInitializer>();

                if(environment.IsProduction())
                {
                    services.AddInitializer<ChurchesDbSeedInitializer>();
                    services.AddInitializer<PeopleDbSeedInitializer>();
                    services.AddInitializer<GroupsDbSeedInitializer>();
                }
                // Development / Test -  Seeding
                else
                {
                    /*services.AddInitializer<ChurchesDbSeedInitializer>();
                    services.AddInitializer<PeopleDbSeedInitializer>();
                    services.AddInitializer<GroupsDbSeedInitializer>();*/
                    services.AddInitializer<ChurchesFakeDbSeedInitializer>();
                    services.AddInitializer<PeopleFakeDbSeedInitializer>();
                    services.AddInitializer<GroupsFakeDbSeedInitializer>();
                    services.AddInitializer<ChurchAttendanceFakeDbInitializer>();
                    services.AddInitializer<GroupAttendanceFakeDbSeedInitializer>();
                }
            }

            #region Repositories

            services.AddScoped(typeof(IGenericDbRepository<>), typeof(GenericRepositoryBase<>));
            services.AddScoped<IGroupAttendanceDbRepository, GroupAttendanceDbRepository>();
            services.AddScoped<IChurchAttendanceDbRepository, ChurchAttendanceDbRepository>();
            services.AddScoped<IDiscipleshipStepDefinitionDbRepository, DiscipleshipDbRepository>();
            services.AddScoped<IGroupMemberDbRepository, GroupMemberDbRepository>();
            services.AddScoped<IGroupTypeRoleDbRepository, GroupTypeRoleDbRepository>();
            services.AddScoped<IPersonDbRepository, PersonDbRepository>();
            services.AddScoped<IGroupDbRepository, GroupDbRepository>();
            services.AddScoped<IPushSubscriptionsService, PushSubscriptionsService>();

            #endregion
        }
    }
}

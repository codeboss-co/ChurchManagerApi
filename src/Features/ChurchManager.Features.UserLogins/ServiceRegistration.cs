using Microsoft.Extensions.DependencyInjection;

namespace ChurchManager.Features.UserLogins
{
    public static class ServiceRegistration
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            /*services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(OpenTelemetryBehavior<,>));

            #region Application Services

            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IPersonService, PersonService>();
            services.AddScoped<IGroupsService, GroupsService>();
            services.AddScoped<IGroupAttendanceAppService, GroupAttendanceAppService>();
            services.AddScoped<IPushNotificationService, WebPushPushNotification>();

            #endregion

            services.AddAspNetCurrentUser<ICognitoCurrentUser, CognitoCurrentUser>(); }*/
        }
    }
}
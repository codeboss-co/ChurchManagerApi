using System.Reflection;
using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Application.Behaviours;
using ChurchManager.Application.Common;
using ChurchManager.Application.Features.Communication.Services;
using ChurchManager.Application.Features.Groups.Services;
using ChurchManager.Application.Features.Profile.Services;
using CodeBoss.AspNetCore;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ChurchManager.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            
            #region Application Services

            services.AddScoped<IProfileService, ProfileService>(); 
            services.AddScoped<IPersonService, PersonService>(); 
            services.AddScoped<IGroupsService, GroupsService>();
            services.AddScoped<IGroupAttendanceAppService, GroupAttendanceAppService>(); 
            services.AddScoped<IPushNotificationService, WebPushPushNotification>(); 

            #endregion

            services.AddAspNetCurrentUser<ICognitoCurrentUser, CognitoCurrentUser>();
        }
    }
}

using System.Reflection;
using ChurchManager.Application.Behaviours;
using ChurchManager.Application.Features.People.Services;
using ChurchManager.Domain;
using ChurchManager.Infrastructure.Abstractions;
using ChurchManager.Infrastructure.Shared;
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

            services.AddScoped<IModelHelper, ModelHelper>();

            services.AddScoped<IPersonAppService, PersonAppService>();

            services.AddAspNetCurrentUser<ICognitoCurrentUser, CognitoCurrentUser>();
        }
    }
}

﻿using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Features.People.Services;
using ChurchManager.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChurchManager.Features.People.Startup
{
    public class StartupApplication : IStartupApplication
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IPersonService, PersonService>();
        }

        public void Configure(IApplicationBuilder application, IWebHostEnvironment webHostEnvironment)
        {
        }

        public int Priority => 100;
        public bool BeforeConfigure => false;
    }
}

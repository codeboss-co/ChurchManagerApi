using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using ChurchManager.Api.Models;
using CodeBoss.AspNetCore.DependencyInjection;
using CodeBoss.Extensions;
using Convey;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace ChurchManager.Api._DependencyInjection
{
    public class AuthenticationDependencyInstaller : IDependencyInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            var jwtSettings = configuration.GetOptions<JwtSettings>(nameof(JwtSettings));

            // https://developerhandbook.com/aws/how-to-use-aws-cognito-with-net-core/
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = SigningKey(jwtSettings.Key, "AQAB"),
                        ValidIssuer = jwtSettings.Issuer,
                        //ValidAudience = jwtSettings.Audience,
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateLifetime = false,
                        ValidateAudience = false,   // Not provided by cognito,
                        RoleClaimType = "cognito:groups",
                        ClockSkew = TimeSpan.FromSeconds(5)
                    };

                    // We have to hook the OnMessageReceived event in order to
                    // allow the JWT authentication handler to read the access
                    // token from the query string when a WebSocket or 
                    // Server-Sent Events request comes in.

                    // Sending the access token in the query string is required due to
                    // a limitation in Browser APIs. We restrict it to only calls to the
                    // SignalR hub in this code.
                    // See https://docs.microsoft.com/aspnet/core/signalr/security#access-token-logging
                    // for more information about security considerations when using
                    // the query string to transmit the access token.
                    options.Events = new SignalRJwtBearerEvents();
                });

            RsaSecurityKey SigningKey(string key, string expo)
            {
                RSA rsa = RSA.Create();

                rsa.ImportParameters(new RSAParameters
                {
                    Modulus = Base64UrlEncoder.DecodeBytes(key),
                    Exponent = Base64UrlEncoder.DecodeBytes(expo)
                });

                return new RsaSecurityKey(rsa);
            }
        }
    }

    /// <summary>
    ///  https://marcstan.net/blog/2018/05/31/Home-App-SignalR/
    /// </summary>
    public class SignalRJwtBearerEvents : JwtBearerEvents
    {
        public override Task MessageReceived(MessageReceivedContext context)
        {
            // pull token from  query string; web sockets don't support headers so fall back to query is required
            var accessToken = context.Request.Query["access_token"];

            if(IsHubPath() && !accessToken.IsNullOrEmpty())
            {
                context.Token = accessToken;
            }

            return Task.CompletedTask;

            // If the request is for our hub...
            bool IsHubPath()
            {
                var path = context.HttpContext.Request.Path;
                var hubPathSegments = new[]
                {
                    ApiRoutes.Hubs.NotificationHub,
                    //ApiRoutes.Hubs.AppHub,
                    // ApiRoutes.Hubs.GroupsHub
                };

                bool isHubPath = hubPathSegments.Any(hubPath => path.StartsWithSegments(hubPath));
                return isHubPath;
            }
        }
    }
}

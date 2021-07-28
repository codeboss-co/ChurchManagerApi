using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Bugsnag;
using ChurchManager.Api.Models;
using ChurchManager.Application.Exceptions;
using ChurchManager.Application.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ChurchManager.Api.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;
        private IClient _bugsnag;

        public ErrorHandlerMiddleware(
            RequestDelegate next,
            ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                var responseModel = new ApiResponse() { Succeeded = false, Message = error?.Message };

                switch(error)
                {
                    case Application.Exceptions.ApiException e:
                        // custom application error
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case ValidationException e:
                        // custom application error
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel.Errors = e.Errors;
                        break;
                    case KeyNotFoundException e:
                        // not found error
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    default:
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                _logger.LogError(error, "Application Error");

                var bugsnagOptions = context.RequestServices.GetRequiredService<IOptions<BugsnagOptions>>().Value;
                if (bugsnagOptions.Enabled)
                {
                    _bugsnag = context.RequestServices.GetRequiredService<IClient>();
                    _bugsnag.Notify(error);
                }

                var result = JsonSerializer.Serialize(responseModel);
                await response.WriteAsync(result);
            }
        }
    }
}

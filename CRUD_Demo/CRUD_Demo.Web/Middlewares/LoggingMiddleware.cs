using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;

using Serilog;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUD_Demo.Web.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger logger)
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
            catch (Exception ex)
            {
                await HandleHttpError(context, ex);
            }
        }

        private async Task HandleHttpError(HttpContext context, Exception ex)
        {
            if (ex is BadHttpRequestException requestException)
            {
                context.Response.StatusCode = requestException.StatusCode;
            }
            else
            {
                _logger.Error(ex, "Unhandled exception.");

                context.Response.StatusCode = 400;

                await context.Response.WriteAsync("Invalid configuration.");
            }
        }
    }
}

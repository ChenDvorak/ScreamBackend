using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System;
using System.Threading.Tasks;

namespace ScreamBackend.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        public ExceptionHandlerMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<ExceptionHandlerMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                if (context.Response.HasStarted)
                    throw;

                try
                {
                    context.Response.Clear();

                    context.Response.Headers[HeaderNames.CacheControl] = "no-cache";
                    context.Response.Headers[HeaderNames.Pragma] = "no-cache";
                    context.Response.Headers[HeaderNames.Expires] = "-1";
                    context.Response.Headers.Remove(HeaderNames.ETag);
                    return;
                }
                catch (Exception ex2)
                {
                    _logger.LogError(ex2, "");
                }
                throw;
            }
        }
    }
}

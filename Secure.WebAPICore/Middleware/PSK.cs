using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Concurrent;

namespace Secure.WebAPICore.Middleware
{
    /// <summary>
    /// Middleware for PSK
    /// </summary>
    public class PSKMiddleware
    {
        private static ConcurrentDictionary<string, Tuple<int, DateTime>>
                    _nonces = new ConcurrentDictionary<string, Tuple<int, DateTime>>();

        private readonly RequestDelegate _next;
        public PSKMiddleware(RequestDelegate next)
        {
            _next = next;
        }
    }

    /// <summary>
    /// Extension method for the PSK
    /// </summary>
    public static class PSKMiddlewareExtensions
    {
        public static IApplicationBuilder UsePSK(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<PSKMiddleware>();
        }
    }
}

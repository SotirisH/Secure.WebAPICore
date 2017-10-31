using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Secure.WebAPICore.Common;
using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Secure.WebAPICore.Middleware
{
    /// <summary>
    /// This simple middleware checks if the client has sent a key with a specific value
    /// on the Authorization header. If the key matches then the request advanced in the pipeline otherwise an 401 is returned
    /// </summary>
    public class SimpleAPIKeyMiddleware
    {
        private readonly RequestDelegate _next;
        public SimpleAPIKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {

            IHeaderDictionary headersDictionary = context.Request.Headers;
            // GetTypedHeaders extension method provides strongly typed access to many headers
            // For known header, knownheaderValues has 1 item and knownheaderValue is the value
            // Obtain strongly typed header class in ASP.NET Core using AuthenticationHeaderValue.Parse
            var authenticationHeaderValue = AuthenticationHeaderValue.Parse(headersDictionary[HeaderNames.Authorization]);
            if (!(authenticationHeaderValue.Scheme == APIKeyValues.ApplicationKeyAuthenticationScheme && APIKeyValues.ApplicationKeyValue == new Guid(authenticationHeaderValue.Parameter)))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized Application");
                return;
            }
            // Call the next delegate/middleware in the pipeline
            await _next.Invoke(context);
        }
    }

    public static class RequestSimpleAPIKeyMiddlewareExtensions
    {
        public static IApplicationBuilder UseSimpleAPIKey(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SimpleAPIKeyMiddleware>();
        }
    }
}

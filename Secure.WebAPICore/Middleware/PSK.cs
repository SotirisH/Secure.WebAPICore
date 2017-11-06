using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Secure.WebAPICore.Common;
using System.Threading.Tasks;

namespace Secure.WebAPICore.Middleware
{
    /// <summary>
    /// Middleware for PSK
    /// </summary>
    public class PSKMiddleware
    {
        /// <summary>
        /// The time window in seconds
        /// where the messages are accepted as valid defaulted to 3minutes
        /// </summary>
        private const int TimeWindow = 3 * 60;

        private readonly RequestDelegate _next;
        public PSKMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            //IHeaderDictionary has a different indexer contract than IDictionary, where it will return StringValues.Empty for missing entries.
            IHeaderDictionary headersDictionary = context.Request.Headers;
            // Extract the values from the headers
            string applicationKeyValue, nonce, stamp, requestUri, requestHttpMethod;

            applicationKeyValue = headersDictionary[APIKeyValues.ApplicationKeyAuthenticationScheme].ToString();
            nonce = headersDictionary[APIKeyValues.NonceScheme].ToString();
            stamp = headersDictionary[APIKeyValues.StampScheme].ToString();

            requestUri = context.Request.GetEncodedUrl();
            requestHttpMethod = context.Request.Method;

            // Call the next delegate/middleware in the pipeline
            await _next.Invoke(context);
        }
    }

    /// <summary>
    /// Extension method for the PSK
    /// </summary>
    public static class PSKMiddlewareExtensions
    {
        public static IApplicationBuilder UsePSKSecurity(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<PSKMiddleware>();
        }
    }
}

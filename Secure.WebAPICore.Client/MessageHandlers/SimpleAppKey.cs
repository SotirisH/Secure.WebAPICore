using Secure.WebAPICore.Common;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Secure.WebAPICore.Client.MessageHandlers
{
    /// <summary>
    /// Simple message handler that adds an application key into the AuthenticationHeader
    /// </summary>
    class SimpleAppKeyMessageHandler : HttpClientHandler
    {
        private readonly Guid _applKey;
        public SimpleAppKeyMessageHandler(Guid applKey)
        {
            _applKey = applKey;

        }
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue(APIKeyValues.ApplicationKeyAuthenticationScheme, _applKey.ToString());
            var response = await base.SendAsync(request, cancellationToken);
            return response;
        }
    }
}

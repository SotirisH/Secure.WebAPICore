using Secure.WebAPICore.Common;
using System;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Secure.WebAPICore.Client.MessageHandlers
{
    /// <summary>
    /// Http message handler for the client
    /// Adds all the necessary headers, creates a signature and sings it using the private key
    /// </summary>
    public class PSK : HttpClientHandler
    {
        private readonly HttpMessageHandler _wrappedMessageHandler;
        /// <summary>
        /// Constructor for testing purposes.  Usually accepts an MessageHandler from the TestServer.CreateHandler()
        /// </summary>
        /// <param name="wrappedMessageHandler"></param>
        public PSK(HttpMessageHandler wrappedMessageHandler)
        {
            _wrappedMessageHandler = wrappedMessageHandler ?? throw new ArgumentNullException("wrappedMessageHandler");

        }
        /// <summary>
        /// Default constructor
        /// </summary>
        public PSK()
        {

        }



        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // The nonce is generated aby our helper
            string nonce = NonceHelper.Generate();

            string requestUri = System.Web.HttpUtility.UrlEncode(request.RequestUri.AbsoluteUri.ToLower());
            // Create headers with the values
            request.Headers.Add(APIKeyValues.ApplicationKeyAuthenticationScheme, APIKeyValues.ApplicationKeyValue.ToString());
            request.Headers.Add(APIKeyValues.NonceScheme, nonce);

            // Create timestamp Unix format
            DateTime epochStart = new DateTime(1970, 01, 01, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan ts = DateTime.UtcNow - epochStart;
            string stamp = Convert.ToUInt64(ts.TotalSeconds).ToString();
            request.Headers.Add(APIKeyValues.StampScheme, stamp);

            // Create message signature and add into the header
            string signatureData = String.Format("{0}{1}{2}{3}{4}", APIKeyValues.ApplicationKeyValue, nonce, stamp, requestUri, request.Method);

            byte[] signature = Encoding.UTF8.GetBytes(signatureData);
            // Digitally sign the signatureData using the private key
            using (HMACSHA256 hmac = new HMACSHA256(Convert.FromBase64String(APIKeyValues.ClientPrivateKeyKeyValue)))
            {
                byte[] signatureBytes = hmac.ComputeHash(signature);
                request.Headers.Add(APIKeyValues.SignatuerScheme,
                                                Convert.ToBase64String(signatureBytes));
            }
if (_wrappedMessageHandler != null)
{
    var method = typeof(HttpMessageHandler).GetMethod("SendAsync", BindingFlags.Instance | BindingFlags.NonPublic);
    var result = method.Invoke(_wrappedMessageHandler, new object[] { request, cancellationToken });
    return await (Task<HttpResponseMessage>)result;
}
else
{
    var response = await base.SendAsync(request, cancellationToken);
    return response;

}
        }
    }
}

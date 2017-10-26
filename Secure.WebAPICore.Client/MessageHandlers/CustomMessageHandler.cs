using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Secure.WebAPICore.Client
{
    /// <summary>
    /// Implement the HTTPClient Custom Handler
    /// HTTPClient allows us to create custom message handler which get created 
    /// and added to the request message handlers chain, the nice thing here 
    /// that this handler will allow us to write out custom logic 
    /// (logic needed to build the hash and set in the Authorization header before firing the request to the back-end API)
    /// </summary>
    public class CustomMessageHandler : HttpClientHandler
    {

        private readonly Guid _APPId; //= new Guid("06673FAD-E3E9-43D7-9B59-EAC04E112D87");
        private readonly string _secretKey; // = "cE81fL+VU253rM3nebPB6jbBtKNV1NT2kcDBZcPmY+M=";

        /// <summary>
        /// Primary constructor
        /// </summary>
        /// <param name="apiId">The Id of the API</param>
        /// <param name="secretSharedKey">Teh sharedKey that the generated hash is based on</param>
        public CustomMessageHandler(Guid apiId, string secretSharedKey)
        {
            _APPId = apiId;
            _secretKey = secretSharedKey;
        }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = null;
            string requestContentBase64String = string.Empty;

            string requestUri = System.Web.HttpUtility.UrlEncode(request.RequestUri.AbsoluteUri.ToLower());

            string requestHttpMethod = request.Method.Method;

            // Request time stamp is calculated using UNIX time (number of seconds since Jan. 1st 1970) to overcome any issues related to a different timezone between client and server
            DateTime epochStart = new DateTime(1970, 01, 01, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan timeSpan = DateTime.UtcNow - epochStart;
            string requestTimeStamp = Convert.ToUInt64(timeSpan.TotalSeconds).ToString();

            //create random nonce for each request
            string nonce = Guid.NewGuid().ToString("N");

            //Checking if the request contains body, usually will be null wiht HTTP GET and DELETE
            if (request.Content != null)
            {
                byte[] content = await request.Content.ReadAsByteArrayAsync();
                MD5 md5 = MD5.Create();
                //Hashing the request body, any change in request body will result in different hash, we'll incure message integrity
                byte[] requestContentHash = md5.ComputeHash(content);
                requestContentBase64String = Convert.ToBase64String(requestContentHash);
            }

            //Creating the raw signature string
            string signatureRawData = $"{_APPId}{nonce}{requestTimeStamp}{requestHttpMethod}{requestUri}{requestContentBase64String}";

            var secretKeyByteArray = Convert.FromBase64String(_secretKey);

            byte[] signatureByteArray = Encoding.UTF8.GetBytes(signatureRawData);

            //Computes a Hash-based Message Authentication Code (HMAC) by using the SHA256 hash function
            using (HMACSHA256 hmac = new HMACSHA256(secretKeyByteArray))
            {
                byte[] signatureBytes = hmac.ComputeHash(signatureByteArray);
                string requestSignatureBase64String = Convert.ToBase64String(signatureBytes);
                //Setting the values in the Authorization header using custom scheme (amx)
                //The format for the Authorization header is: [Authorization: amx APPId:Signature:Nonce:Timestamp]
                request.Headers.Authorization = new AuthenticationHeaderValue("amx", $"{_APPId}:{nonce}:{requestTimeStamp}:{requestSignatureBase64String}");
            }
            response = await base.SendAsync(request, cancellationToken); //Task<HttpResponseMessage>.Factory.StartNew(() => request.CreateResponse());


            return response;
        }
    }
}

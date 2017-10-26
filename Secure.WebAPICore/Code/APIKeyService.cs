using System;
using System.Security.Cryptography;

namespace Secure.WebAPICore.Code
{
    /// <summary>
    /// Class the generates the Shared Private Key (API Key) and APP Id 
    /// </summary>
    public class APIKeyService
    {
        private readonly Guid APPId = new Guid("06673FAD-E3E9-43D7-9B59-EAC04E112D87");
        private const string SecretKey = "cE81fL+VU253rM3nebPB6jbBtKNV1NT2kcDBZcPmY+M=";


        /// <summary>
        /// Creates an API key that will be shared between the client and the server
        /// </summary>
        /// <returns></returns>
        public string CreateKey()
        {
            // RNGCryptoServiceProvider is a stronger cryptographically random number
            // meaning it would be better for determining encryption keys and the likes.
            using (var cryptoProvider = new RNGCryptoServiceProvider())
            {
                byte[] secretKeyByteArray = new byte[32]; //256 bit
                cryptoProvider.GetBytes(secretKeyByteArray);
                return Convert.ToBase64String(secretKeyByteArray);
            }
        }
    }

}

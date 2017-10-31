using Secure.WebAPICore.Common;
using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;

namespace Secure.WebAPICore.Client.Clients
{
    /// <summary>
    /// Client using preshared Key
    /// </summary>
    public class PSK
    {
        public void CallWebAPIGet()
        {
            using (HttpClient client = new HttpClient())
            {
                int counter = (new Random()).Next();
                Uri uri = new Uri("http://localhost:5050/Orders");

                // Create headers with the values
                client.DefaultRequestHeaders.Add(APIKeyValues.ApplicationKeyAuthenticationScheme, APIKeyValues.ApplicationKeyValue.ToString());
                client.DefaultRequestHeaders.Add(APIKeyValues.CounterScheme, counter.ToString());

                // Create timestamp Unix format
                DateTime epochStart = new DateTime(1970, 01, 01, 0, 0, 0, 0, DateTimeKind.Utc);
                TimeSpan ts = DateTime.UtcNow - epochStart;
                string stamp = Convert.ToUInt64(ts.TotalSeconds).ToString();
                client.DefaultRequestHeaders.Add(APIKeyValues.StampScheme, stamp);

                // Create message signature and add into the header
                string signatureData = String.Format("{0}{1}{2}{3}{4}", APIKeyValues.ApplicationKeyValue, counter, stamp, uri.ToString(), "GET");

                byte[] signature = Encoding.UTF8.GetBytes(signatureData);
                using (HMACSHA256 hmac = new HMACSHA256(Convert.FromBase64String(APIKeyValues.ClientPrivateKeyKeyValue)))
                {
                    byte[] signatureBytes = hmac.ComputeHash(signature);
                    client.DefaultRequestHeaders.Add(APIKeyValues.SignatuerScheme,
                                                    Convert.ToBase64String(signatureBytes));
                }
                var httpMessage = client.GetAsync(uri).Result;
                if (httpMessage.IsSuccessStatusCode)
                    Console.WriteLine(httpMessage.Content.ReadAsStringAsync().Result);
            }
        }
    }
}

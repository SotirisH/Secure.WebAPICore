using Secure.WebAPICore.Client.MessageHandlers;
using Secure.WebAPICore.Code;
using Secure.WebAPICore.Common;
using Secure.WebAPICore.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Secure.WebAPICore.Client
{
    class Program
    {
        private static readonly Guid _APPId = new Guid("06673FAD-E3E9-43D7-9B59-EAC04E112D87");
        private static readonly string _secretKey = "cE81fL+VU253rM3nebPB6jbBtKNV1NT2kcDBZcPmY+M=";
        private const string apiBaseAddress = "http://localhost:5050/";

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Console.WriteLine("Options");
            Console.WriteLine("------------------------------");
            Console.WriteLine("1:Create Shared Private Key");
            Console.WriteLine("2:Call Web API Secure");
            Console.WriteLine("3:Try simple app-key(success)");
            Console.WriteLine("4:Try simple app-key(fail)");
            Console.WriteLine("5:PreShared Key-Secure");
            Console.WriteLine("------------------------------");
            Console.Write("Your Choise:");
            var ans = Console.ReadKey();
            Console.WriteLine();
            switch (ans.Key)
            {
                case ConsoleKey.D1:
                    var keyGenerator = new APIKeyService();
                    Console.WriteLine($"New Key:{ keyGenerator.CreateKey() }");
                    break;

                case ConsoleKey.D3:
                    SimpleKeySuccess().Wait();
                    break;
                case ConsoleKey.D4:
                    SimpleKeyError().Wait();
                    break;
            }
            Console.Write("Press any key to end the program");
            Console.ReadKey();

        }
        static async Task SimpleKeyError()
        {
            HttpClient client = new HttpClient(new SimpleAppKeyMessageHandler(Guid.Empty));
            // Call asynchronous network methods in a try/catch block to handle exceptions
            try
            {
                HttpResponseMessage response = await client.GetAsync(apiBaseAddress + "api/orders");
                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseString);
                    Console.WriteLine("HTTP Status: {0}, Reason {1}. Press ENTER to exit", response.StatusCode, response.ReasonPhrase);
                }
                else
                {
                    Console.WriteLine("Failed to call the API. HTTP Status: {0}, Reason {1}", response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }

            // Need to call dispose on the HttpClient object
            // when done using it, so the app doesn't leak resources
            client.Dispose();
        }

        static async Task SimpleKeySuccess()
        {
            HttpClient client = new HttpClient(new SimpleAppKeyMessageHandler(APIKeyValues.ApplicationKeyValue));
            // Call asynchronous network methods in a try/catch block to handle exceptions
            try
            {
                HttpResponseMessage response = await client.GetAsync(apiBaseAddress + "api/orders");
                //The idiomatic usage of EnsureSuccessStatusCode is to concisely verify success of a request, 
                //when you don't want to handle failure cases in any specific way. 
                //This is especially useful when you want to quickly prototype a client.

                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseString);
                    Console.WriteLine("HTTP Status: {0}, Reason {1}. Press ENTER to exit", response.StatusCode, response.ReasonPhrase);
                }
                else
                {
                    Console.WriteLine("Failed to call the API. HTTP Status: {0}, Reason {1}", response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }

            // Need to call dispose on the HttpClient object
            // when done using it, so the app doesn't leak resources
            client.Dispose();
        }

        /// <summary>
        /// Call the back-end API using HTTPClient
        /// </summary>
        /// <returns></returns>
        static async Task RunAsync()
        {

            Console.WriteLine("Calling the back-end API");



            // Create a New HttpClient object.


            HttpClient client = new HttpClient(new CustomMessageHandler(_APPId, _secretKey));

            var order = new Order { OrderID = 10248, CustomerName = "Taiseer Joudeh", ShipperCity = "Amman", IsShipped = true };

            // Call asynchronous network methods in a try/catch block to handle exceptions
            try
            {
                HttpResponseMessage response = await client.GetAsync(apiBaseAddress + "api/orders");
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseString);
                    Console.WriteLine("HTTP Status: {0}, Reason {1}. Press ENTER to exit", response.StatusCode, response.ReasonPhrase);
                }
                else
                {
                    Console.WriteLine("Failed to call the API. HTTP Status: {0}, Reason {1}", response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }

            // Need to call dispose on the HttpClient object
            // when done using it, so the app doesn't leak resources
            client.Dispose();
        }
    }
}

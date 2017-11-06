using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Secure.WebAPICore.Middleware;
using System;
using System.Net.Http;
using Xunit;

namespace Secure.WebAPICore.Tests
{
    public class PSKMiddleWareTests
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;
        //https://docs.microsoft.com/en-us/aspnet/core/testing/integration-testing
        //https://stackoverflow.com/questions/32750457/access-cookies-inside-unit-test-in-aspnet-testhost-testserver-context-on-asp-net

        public PSKMiddleWareTests()
        {
            // create a new WebHostBuilder and add my midlewware into the configuration section
            var hostBuilder = new WebHostBuilder()
            .UseUrls("http://TestServer:5000")
            .Configure(app =>
            {
                app.UsePSKSecurity();
            });

            // This is the standard way of initializing the Startup on the Test server
            //_server = new TestServer(new WebHostBuilder()
            //    .UseStartup<Startup>());

            _server = new TestServer(hostBuilder);

            _client = _server.CreateClient();


        }

[Fact]
public async void BasicCallTest()
{
    var pskMessageHandler = new Secure.WebAPICore.Client.MessageHandlers.PSK();
    var innerHttpMessageHandler = _server.CreateHandler();
    // wrap the server handler with my client handler
    var clientHandler = new Secure.WebAPICore.Client.MessageHandlers.PSK(innerHttpMessageHandler);
    using (HttpClient client = new HttpClient(clientHandler))
    {


        Uri uri = new Uri("http://TestServer:5050/Orders");
        var response = await client.GetAsync(uri);
    }

}
    }
}

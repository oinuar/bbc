using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TestStack.BDDfy;
using Xunit;

namespace tech.haamu.Movie.IntegrationTest
{
    public class GenerateAccesssToken
    {
        private readonly HttpClient httpClient;
        private string token;
        private IHost host;

        private async Task WhenUserRequestsAnAccessToken()
        {
            var response = await httpClient.PostAsync("http://localhost:5000/api/user/token/1", null);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            token = await response.Content.ReadAsStringAsync();
        }

        private void ThenGenerateAJwtToken()
        {
            Assert.NotNull(token);
        }

        public GenerateAccesssToken()
        {
            httpClient = new HttpClient();
        }

        [Fact]
        public Task Scenario()
        {
            // Set timeout to 30 seconds.
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));

            // Build a hosting service for the web server.
            host = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).Build();

            // Execute the test when the application is started, and then shutdown the application.
            host.Services.GetRequiredService<IHostApplicationLifetime>().ApplicationStarted.Register(() =>
            {
                this.BDDfy();
                cts.Cancel();
            });

            // Run the application.
            return host.RunAsync(cts.Token);
        }
    }
}

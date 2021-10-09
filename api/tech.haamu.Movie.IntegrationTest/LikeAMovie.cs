using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using TestStack.BDDfy;
using Xunit;
using tech.haamu.Movie.Services;

namespace tech.haamu.Movie.IntegrationTest
{
    public class LikeAMovie
    {
        private readonly HttpClient httpClient;
        private IHost host;

        private async Task GivenUserHasAnAccessToken()
        {
            var token = await httpClient.GetStringAsync("http://localhost:5000/api/user/token/1");

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        private async Task WhenUserLikesAMovie()
        {
            var response = await httpClient.PostAsync("http://localhost:5000/api/movie/like/tt0118715", null);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        private void ThenAddTheLikedMovieToTheUserPreference()
        {
            var users = host.Services.GetRequiredService<Users>();
            var user = users.GetById("1");

            Assert.Collection(user.LikedMovies, x => Assert.Equal("tt0118715", x.Id));
        }

        public LikeAMovie()
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

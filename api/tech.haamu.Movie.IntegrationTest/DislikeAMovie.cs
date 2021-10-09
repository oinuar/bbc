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
    public class DislikeAMovie
    {
        private readonly HttpClient httpClient;
        private IHost host;

        private async Task GivenUserHasAnAccessToken()
        {
            var token = await httpClient.GetStringAsync("http://localhost:5000/user/token/1");

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        private async Task AndUserHasLikedTheMovie()
        {
            var response = await httpClient.PostAsync("http://localhost:5000/movie/like/tt0118715", null);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        private async Task WhenUserDislikesAMovie()
        {
            var response = await httpClient.PostAsync("http://localhost:5000/movie/dislike/tt0118715", null);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        private void ThenAddTheLikedMovieToTheUserPreference()
        {
            var users = host.Services.GetRequiredService<Users>();
            var user = users.GetById("1");

            Assert.Empty(user.LikedMovies);
        }

        public DislikeAMovie()
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

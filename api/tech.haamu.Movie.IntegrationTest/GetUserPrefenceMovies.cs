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
using System.Collections.Generic;
using System.Text.Json;
using System.Text;

namespace tech.haamu.Movie.IntegrationTest
{
    public class GetUserPreferenceMovies
    {
        private readonly HttpClient httpClient;
        private IList<string> movieIds;
        private IHost host;

        private async Task GivenUserHasAnAccessToken()
        {
            var response = await httpClient.PostAsync("http://localhost:5000/api/user/token/1", null);
            var token = await response.Content.ReadAsStringAsync();

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        private async Task GivenUserHasLikedSomeMovies()
        {
            var responses = new[] {
                await httpClient.PostAsync("http://localhost:5000/api/movie/like/tt0163651", null),
                await httpClient.PostAsync("http://localhost:5000/api/movie/like/tt0111161", null),
                await httpClient.PostAsync("http://localhost:5000/api/movie/like/tt2527336", null)
            };

            Assert.All(responses, x => Assert.Equal(HttpStatusCode.OK, x.StatusCode));
        }

        private async Task WhenUserQueriesWhichMoviesAreTheirPreference()
        {
            var content = new StringContent(JsonSerializer.Serialize(new string[]
            {
                "tt0163651", "tt0848228", "tt0111161", "tt0068646"
            }), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("http://localhost:5000/api/user/moviePreference", content);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var stream = await response.Content.ReadAsStreamAsync();

            movieIds = await JsonSerializer.DeserializeAsync<IList<string>>(stream);
        }

        private void ThenCreateASubsetOfGivenMoviesToContainOnlyOnesThatUserHasLiked()
        {
            Assert.Equal(new[] { "tt0163651", "tt0111161" }, movieIds);
        }

        public GetUserPreferenceMovies()
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

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TestStack.BDDfy;
using Xunit;

namespace tech.haamu.Movie.IntegrationTest
{
    public class GetMovieRecommendationsForUserBasedOnGenrePreference
    {
        private readonly HttpClient httpClient;
        private IHost host;
        private IList<Models.Movie> recommendations;

        private async Task GivenUserHasAnAccessToken()
        {
            var response = await httpClient.PostAsync("http://localhost:5000/api/user/token/1", null);
            var token = await response.Content.ReadAsStringAsync();

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        private async Task GivenUserHasLikedAtLeastOneMovie()
        {
            var responses = new[] {
                await httpClient.PostAsync("http://localhost:5000/api/movie/like/tt0163651", null),
                await httpClient.PostAsync("http://localhost:5000/api/movie/like/tt0111161", null)
            };

            Assert.All(responses, x => Assert.Equal(HttpStatusCode.OK, x.StatusCode));
        }

        private async Task WhenUserRequestsMovieRecommendations()
        {
            var response = await httpClient.PostAsync("http://localhost:5000/api/movie/recommendations?limit=100", null);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var stream = await response.Content.ReadAsStreamAsync();

            recommendations = await JsonSerializer.DeserializeAsync<IList<Models.Movie>>(stream, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        private void ThenSearchMoviesFromAMovieLibraryAndListAMovieIfTheMoviesGenresContainAnyOfTheExtractedGenresAndFilterOutMoviesThatUserHasLikedAlready()
        {
            Assert.Equal(JsonSerializer.Serialize(new[] {
                new Models.Movie { Id = "tt0068646", Genres = new[] { "Crime", "Drama" }, Name = "The Godfather" },
                new Models.Movie { Id = "tt0088763", Genres = new[] { "Adventure", "Comedy", "Sci-fi" }, Name = "Back to the Future" },
                new Models.Movie { Id = "tt0118715", Genres = new[] { "Comedy", "Crime", "Sport" }, Name = "The Big Lebowski" }
            }), JsonSerializer.Serialize(recommendations));
        }

        public GetMovieRecommendationsForUserBasedOnGenrePreference()
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

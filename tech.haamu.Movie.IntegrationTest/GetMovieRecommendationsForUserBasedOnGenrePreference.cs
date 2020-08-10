using Microsoft.AspNetCore.Hosting;
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
            var token = await httpClient.GetStringAsync("http://localhost:5000/user/token/1");

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        private async Task GivenUserHasLikedAtLeastOneMovie()
        {
            var responses = new[] {
                await httpClient.PostAsync("http://localhost:5000/movie/like/tt0163651", null),
                await httpClient.PostAsync("http://localhost:5000/movie/like/tt0111161", null)
            };

            Assert.All(responses, x => Assert.Equal(HttpStatusCode.OK, x.StatusCode));
        }

        private async Task WhenUserRequestsMovieRecommendations()
        {
            var response = await httpClient.GetStreamAsync("http://localhost:5000/movie/recommend?limit=100");

            recommendations = await JsonSerializer.DeserializeAsync<IList<Models.Movie>>(response, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        private void ThenListAMovieIfTheMoviesGenresContainAnyOfTheExtractedGenres()
        {
            Assert.Equal(JsonSerializer.Serialize(new[] {
                new Models.Movie { Id = "tt0068646", Genres = new[] { "Crime", "Drama" }, Name = "The Godfather" },
                new Models.Movie { Id = "tt0088763", Genres = new[] { "Adventure", "Comedy", "Sci-fi" }, Name = "Back to the Future" },
                new Models.Movie { Id = "tt0111161", Genres = new[] { "Drama" }, Name = "The Shawshank Redemption" },
                new Models.Movie { Id = "tt0118715", Genres = new[] { "Comedy", "Crime", "Sport" }, Name = "The Big Lebowski" },
                new Models.Movie { Id = "tt0163651", Genres = new[] { "Comedy" }, Name = "American Pie" }
            }), JsonSerializer.Serialize(recommendations));
        }

        public GetMovieRecommendationsForUserBasedOnGenrePreference()
        {
            httpClient = new HttpClient();
        }

        [Fact]
        public void Scenario()
        {
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));

            host = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder => 
                {
                    webBuilder.UseStartup<Startup>();
                }).Build();

            Assert.True(ThreadPool.QueueUserWorkItem(async s => await host.RunAsync(cts.Token)));

            this.BDDfy();
            cts.Cancel();
        }
    }
}

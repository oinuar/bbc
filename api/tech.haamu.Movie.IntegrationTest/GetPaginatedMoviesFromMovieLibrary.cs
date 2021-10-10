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

namespace tech.haamu.Movie.IntegrationTest
{
    public class GetPaginatedMoviesFromMovieLibrary
    {
        private readonly HttpClient httpClient;
        private IList<IList<Models.Movie>> chunks;
        private IHost host;

        private async Task GivenUserHasAnAccessToken()
        {
            var response = await httpClient.PostAsync("http://localhost:5000/api/user/token/1", null);
            var token = await response.Content.ReadAsStringAsync();

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        private async Task WhenUserScrollsDownTheMovieList()
        {
            for (int offset = 0; ; offset += 2)
            {
                var stream = await httpClient.GetStreamAsync($"http://localhost:5000/api/movie?limit=2&offset={offset}");

                var chunk = await JsonSerializer.DeserializeAsync<IList<Models.Movie>>(stream, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (chunk.Count == 0)
                    break;

                chunks.Add(chunk);
            }
        }

        private void ThenQueryNextChunkOfMovieResultsFromMovieLibrary()
        {
            Assert.Equal(4, chunks.Count);

            Assert.Equal(JsonSerializer.Serialize(new[] {
                new Models.Movie { Id = "tt0068646", Genres = new[] { "Crime", "Drama" }, Name = "The Godfather" },
                new Models.Movie { Id = "tt0088763", Genres = new[] { "Adventure", "Comedy", "Sci-fi" }, Name = "Back to the Future" }
            }), JsonSerializer.Serialize(chunks[0]));

            Assert.Equal(JsonSerializer.Serialize(new[] {
                new Models.Movie { Id = "tt0111161", Genres = new[] { "Drama" }, Name = "The Shawshank Redemption" },
                new Models.Movie { Id = "tt0118715", Genres = new[] { "Comedy", "Crime", "Sport" }, Name = "The Big Lebowski" }
            }), JsonSerializer.Serialize(chunks[1]));

            Assert.Equal(JsonSerializer.Serialize(new[] {
                new Models.Movie { Id = "tt0163651", Genres = new[] { "Comedy" }, Name = "American Pie" },
                new Models.Movie { Id = "tt0848228", Genres = new[] { "Action", "Adventure", "Sci-Fi" }, Name = "The Avengers" }
            }), JsonSerializer.Serialize(chunks[2]));

            Assert.Equal(JsonSerializer.Serialize(new[] {
                new Models.Movie { Id = "tt2527336", Genres = new[] { "Action", "Adventure", "Fantasy" }, Name = "Star Wars: Episode VIII - The Last Jedi" }
            }), JsonSerializer.Serialize(chunks[3]));
        }

        public GetPaginatedMoviesFromMovieLibrary()
        {
            httpClient = new HttpClient();
            chunks = new List<IList<Models.Movie>>();
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

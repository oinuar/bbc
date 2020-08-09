using System.Collections.Immutable;

namespace tech.haamu.Movie.Services
{
    // TODO: remove InMemoryLibrary base and implement IMovieLibrary directly to use real IMDB's API.
    public class IMDBMovieLibrary : InMemoryMovieLibrary
    {
        public IMDBMovieLibrary(IImmutableSet<Models.Movie> emptySet) : base(emptySet
            .Add(new Models.Movie { Id = "tt0111161", Genres = new[] { "Drama" }, Name = "The Shawshank Redemption" })
            .Add(new Models.Movie { Id = "tt0068646", Genres = new[] { "Crime", "Drama" }, Name = "The Godfather" })
            .Add(new Models.Movie { Id = "tt0071562", Genres = new[] { "Crime", "Drama" }, Name = "The Godfather: Part II" })
            .Add(new Models.Movie { Id = "tt0468569", Genres = new[] { "Action", "Crime", "Drama" }, Name = "The Dark Knight" })
            .Add(new Models.Movie { Id = "tt0050083", Genres = new[] { "Crime", "Drama" }, Name = "12 Angry Men" })
            .Add(new Models.Movie { Id = "tt0108052", Genres = new[] { "Biography", "Drama", "History" }, Name = "Schindler's List" }))
        {
        }
    }
}

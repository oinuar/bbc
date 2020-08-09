using System.Collections.Immutable;

namespace tech.haamu.Movie.Services
{

    /// <summary>
    /// A specialized service class that implements IMDB's movie library.
    /// 
    /// TODO: remove InMemoryLibrary base and implement IMovieLibrary directly to use the real IMDB's API.
    /// </summary>
    public class IMDBMovieLibrary : InMemoryMovieLibrary
    {
        public IMDBMovieLibrary(IImmutableSet<Models.Movie> emptySet) : base(emptySet
            .Add(new Models.Movie { Id = "tt0111161", Genres = new[] { "Drama" }, Name = "The Shawshank Redemption" })
            .Add(new Models.Movie { Id = "tt0068646", Genres = new[] { "Crime", "Drama" }, Name = "The Godfather" })
            .Add(new Models.Movie { Id = "tt0118715", Genres = new[] { "Comedy", "Crime", "Sport" }, Name = "The Big Lebowski" })
            .Add(new Models.Movie { Id = "tt0848228", Genres = new[] { "Action", "Adventure", "Sci-Fi" }, Name = "The Avengers" })
            .Add(new Models.Movie { Id = "tt0088763", Genres = new[] { "Adventure", "Comedy", "Sci-fi" }, Name = "Back to the Future" })
            .Add(new Models.Movie { Id = "tt2527336", Genres = new[] { "Action", "Adventure", "Fantasy" }, Name = "Star Wars: Episode VIII - The Last Jedi" })
            .Add(new Models.Movie { Id = "tt0163651", Genres = new[] { "Comedy" }, Name = "American Pie" }))
        {
        }
    }
}

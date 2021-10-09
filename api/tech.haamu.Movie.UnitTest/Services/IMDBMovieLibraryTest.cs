using tech.haamu.Movie.Services;
using Moq;
using System.Collections.Immutable;
using System.Linq;
using Xunit;

namespace tech.haamu.Movie.UnitTest.Services
{
    public class IMDBMovieLibraryTest
    {
        [Fact]
        public void IMDBMovieLibrary()
        {
            var movies = new Mock<IImmutableSet<Movie.Models.Movie>>(MockBehavior.Strict);

            movies
                .Setup(x => x.Add(It.Is<Movie.Models.Movie>(y => y.Id == "tt0111161" && new[] { "Drama" }.SequenceEqual(y.Genres) && y.Name == "The Shawshank Redemption")))
                .Returns(movies.Object);

            movies
                .Setup(x => x.Add(It.Is<Movie.Models.Movie>(y => y.Id == "tt0068646" && new[] { "Crime", "Drama" }.SequenceEqual(y.Genres) && y.Name == "The Godfather")))
                .Returns(movies.Object);

            movies
                .Setup(x => x.Add(It.Is<Movie.Models.Movie>(y => y.Id == "tt0118715" && new[] { "Comedy", "Crime", "Sport" }.SequenceEqual(y.Genres) && y.Name == "The Big Lebowski")))
                .Returns(movies.Object);

            movies
                .Setup(x => x.Add(It.Is<Movie.Models.Movie>(y => y.Id == "tt0848228" && new[] { "Action", "Adventure", "Sci-Fi" }.SequenceEqual(y.Genres) && y.Name == "The Avengers")))
                .Returns(movies.Object);

            movies
                .Setup(x => x.Add(It.Is<Movie.Models.Movie>(y => y.Id == "tt0088763" && new[] { "Adventure", "Comedy", "Sci-fi" }.SequenceEqual(y.Genres) && y.Name == "Back to the Future")))
                .Returns(movies.Object);

            movies
                .Setup(x => x.Add(It.Is<Movie.Models.Movie>(y => y.Id == "tt2527336" && new[] { "Action", "Adventure", "Fantasy" }.SequenceEqual(y.Genres) && y.Name == "Star Wars: Episode VIII - The Last Jedi")))
                .Returns(movies.Object);

            movies
                .Setup(x => x.Add(It.Is<Movie.Models.Movie>(y => y.Id == "tt0163651" && new[] { "Comedy" }.SequenceEqual(y.Genres) && y.Name == "American Pie")))
                .Returns(movies.Object);

            var movieLibrary = new IMDBMovieLibrary(movies.Object);

            movies.VerifyAll();
        }
    }
}

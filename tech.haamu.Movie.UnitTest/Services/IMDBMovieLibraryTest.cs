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
                .Setup(x => x.Add(It.Is<Movie.Models.Movie>(y => y.Id == "tt0071562" && new[] { "Crime", "Drama" }.SequenceEqual(y.Genres) && y.Name == "The Godfather: Part II")))
                .Returns(movies.Object);

            movies
                .Setup(x => x.Add(It.Is<Movie.Models.Movie>(y => y.Id == "tt0468569" && new[] { "Action", "Crime", "Drama" }.SequenceEqual(y.Genres) && y.Name == "The Dark Knight")))
                .Returns(movies.Object);

            movies
                .Setup(x => x.Add(It.Is<Movie.Models.Movie>(y => y.Id == "tt0050083" && new[] { "Crime", "Drama" }.SequenceEqual(y.Genres) && y.Name == "12 Angry Men")))
                .Returns(movies.Object);

            movies
                .Setup(x => x.Add(It.Is<Movie.Models.Movie>(y => y.Id == "tt0108052" && new[] { "Biography", "Drama", "History" }.SequenceEqual(y.Genres) && y.Name == "Schindler's List")))
                .Returns(movies.Object);

            var movieLibrary = new IMDBMovieLibrary(movies.Object);

            movies.VerifyAll();
        }
    }
}

using Xunit;

namespace tech.haamu.Movie.UnitTest.Models
{
    public class MovieTest
    {
        [Fact]
        public void Comparer_Equals_NullNull()
        {
            var comparer = new Movie.Models.Movie.Comparer();

            var result = comparer.Equals(null, null);

            Assert.True(result);
        }

        [Fact]
        public void Comparer_Equals_XNull()
        {
            var comparer = new Movie.Models.Movie.Comparer();

            var result = comparer.Equals(new Movie.Models.Movie(), null);

            Assert.False(result);
        }

        [Fact]
        public void Comparer_Equals_NullX()
        {
            var comparer = new Movie.Models.Movie.Comparer();

            var result = comparer.Equals(null, new Movie.Models.Movie());

            Assert.False(result);
        }

        [Fact]
        public void Comparer_Equals_True()
        {
            var comparer = new Movie.Models.Movie.Comparer();

            var result = comparer.Equals(new Movie.Models.Movie { Id = "1" }, new Movie.Models.Movie { Id = "1" });

            Assert.True(result);
        }

        [Fact]
        public void Comparer_Equals_False()
        {
            var comparer = new Movie.Models.Movie.Comparer();

            var result = comparer.Equals(new Movie.Models.Movie { Id = "1" }, new Movie.Models.Movie { Id = "2" });

            Assert.False(result);
        }

        [Fact]
        public void Comparer_GetHashCode()
        {
            var comparer = new Movie.Models.Movie.Comparer();
            var movie = new Movie.Models.Movie { Id = "1" };

            var result = comparer.GetHashCode(movie);

            Assert.Equal(movie.Id.GetHashCode(), result);
        }
    }
}

using tech.haamu.Movie.Models;
using Xunit;

namespace tech.haamu.Movie.UnitTest.Models
{
    public class IdModelTest
    {
        [Fact]
        public void Comparer_Equals_NullNull()
        {
            var comparer = new IdModel<int>.Comparer<IdModel<int>>();

            var result = comparer.Equals(null, null);

            Assert.True(result);
        }

        [Fact]
        public void Comparer_Equals_XNull()
        {
            var comparer = new IdModel<int>.Comparer<IdModel<int>>();

            var result = comparer.Equals(new IdModel<int>(), null);

            Assert.False(result);
        }

        [Fact]
        public void Comparer_Equals_NullX()
        {
            var comparer = new IdModel<int>.Comparer<IdModel<int>>();

            var result = comparer.Equals(null, new IdModel<int>());

            Assert.False(result);
        }

        [Fact]
        public void Comparer_Equals_True()
        {
            var comparer = new IdModel<int>.Comparer<IdModel<int>>();

            var result = comparer.Equals(new IdModel<int> { Id = 1 }, new IdModel<int> { Id = 1 });

            Assert.True(result);
        }

        [Fact]
        public void Comparer_Equals_False()
        {
            var comparer = new IdModel<int>.Comparer<IdModel<int>>();

            var result = comparer.Equals(new IdModel<int> { Id = 1 }, new IdModel<int> { Id = 2 });

            Assert.False(result);
        }

        [Fact]
        public void Comparer_GetHashCode()
        {
            var comparer = new IdModel<int>.Comparer<IdModel<int>>();
            var movie = new IdModel<int> { Id = 1 };

            var result = comparer.GetHashCode(movie);

            Assert.Equal(movie.Id.GetHashCode(), result);
        }
    }
}

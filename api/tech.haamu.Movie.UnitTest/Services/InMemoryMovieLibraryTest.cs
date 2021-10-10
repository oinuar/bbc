using tech.haamu.Movie.Models;
using tech.haamu.Movie.Services;
using Moq;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using System;

namespace tech.haamu.Movie.UnitTest.Services
{
    public class InMemoryMovieLibraryTest
    {
        [Fact]
        public async Task GetById()
        {
            var movies = new Mock<IImmutableSet<Movie.Models.Movie>>(MockBehavior.Strict);
            var movie = new Movie.Models.Movie { Id = "unit test movie id" };

            movies
                .Setup(x => x.TryGetValue(It.Is<Movie.Models.Movie>(y => y.Id == movie.Id), out movie))
                .Returns(true);

            var movieLibrary = new InMemoryMovieLibrary(movies.Object);

            var result = await movieLibrary.GetById(movie.Id);

            Assert.Same(movie, result);

            movies.VerifyAll();
        }

        [Fact]
        public async Task GetById_ThrowsArgumentNullException()
        {
            var movieLibrary = new InMemoryMovieLibrary(null);

            await Assert.ThrowsAsync<ArgumentNullException>(() => movieLibrary.GetById(null));
        }

        [Theory]
        [InlineData(1000, 0)]
        [InlineData(1, 0)]
        [InlineData(1, 1)]
        public async Task GetAll(int limit, int offset)
        {
            var movies = ImmutableHashSet<Movie.Models.Movie>.Empty.WithComparer(new IdModel<string>.Comparer<Movie.Models.Movie>())
                .Add(new Movie.Models.Movie { Id = "3", Genres = new string[] { } })
                .Add(new Movie.Models.Movie { Id = "2", Genres = new string[] { } })
                .Add(new Movie.Models.Movie { Id = "1", Genres = new string[] {  } });

            var movieLibrary = new InMemoryMovieLibrary(movies);

            var result = await movieLibrary.GetAll(limit, offset);

            Assert.Equal(movies.OrderBy(x => x.Id).Skip(offset).Take(limit), result);
        }

        [Fact]
        public async Task GetMoviesByGenres()
        {
            var movies = ImmutableHashSet<Movie.Models.Movie>.Empty.WithComparer(new IdModel<string>.Comparer<Movie.Models.Movie>())
                .Add(new Movie.Models.Movie { Id = "unit test movie id 1", Genres = new[] { "unit test genre 1" } })
                .Add(new Movie.Models.Movie { Id = "unit test movie id 2", Genres = new[] { "unit test genre 1", "unit test genre 2" } })
                .Add(new Movie.Models.Movie { Id = "unit test movie id 3", Genres = new[] { "unit test genre 2" } });

            var movieLibrary = new InMemoryMovieLibrary(movies);

            var result = await movieLibrary.GetMoviesByGenres(new[] { "unit test genre 2" }, Enumerable.Empty<string>(), int.MaxValue);

            Assert.Equal(movies.OrderBy(x => x.Id).Skip(1), result);
        }

        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        public async Task GetMoviesByGenres_ThrowsArgumentNullException(bool isGenresNull, bool isExcludedMovieIdsNull)
        {
            var movieLibrary = new InMemoryMovieLibrary(null);

            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                movieLibrary.GetMoviesByGenres(isGenresNull ? null : Enumerable.Empty<string>(),
                    isExcludedMovieIdsNull ? null : Enumerable.Empty<string>(), 0));
        }

        [Fact]
        public async Task GetMoviesByGenres_Exclusions()
        {
            var movies = ImmutableHashSet<Movie.Models.Movie>.Empty.WithComparer(new IdModel<string>.Comparer<Movie.Models.Movie>())
                .Add(new Movie.Models.Movie { Id = "unit test movie id 1", Genres = new[] { "unit test genre 1" } })
                .Add(new Movie.Models.Movie { Id = "unit test movie id 2", Genres = new[] { "unit test genre 1", "unit test genre 2" } })
                .Add(new Movie.Models.Movie { Id = "unit test movie id 3", Genres = new[] { "unit test genre 2" } });

            var excludedMovieIds = new[]
            {
                "unit test movie id 2"
            };

            var movieLibrary = new InMemoryMovieLibrary(movies);

            var result = await movieLibrary.GetMoviesByGenres(new[] { "unit test genre 2" }, excludedMovieIds, int.MaxValue);

            Assert.Equal(movies.OrderBy(x => x.Id).Skip(2), result);
        }

        [Fact]
        public async Task GetMoviesByGenres_Limit()
        {
            var movies = ImmutableHashSet<Movie.Models.Movie>.Empty.WithComparer(new IdModel<string>.Comparer<Movie.Models.Movie>())
                .Add(new Movie.Models.Movie { Id = "unit test movie id 1", Genres = new[] { "unit test genre" } })
                .Add(new Movie.Models.Movie { Id = "unit test movie id 2", Genres = new[] { "unit test genre" } })
                .Add(new Movie.Models.Movie { Id = "unit test movie id 3", Genres = new[] { "unit test genre" } });

            var movieLibrary = new InMemoryMovieLibrary(movies);

            var result = await movieLibrary.GetMoviesByGenres(new[] { "unit test genre" }, Enumerable.Empty<string>(), 2);

            Assert.Equal(movies.OrderBy(x => x.Id).Take(2), result);
        }

        [Fact]
        public async Task GetMoviesByGenres_LimitOffset()
        {
            var movies = ImmutableHashSet<Movie.Models.Movie>.Empty.WithComparer(new IdModel<string>.Comparer<Movie.Models.Movie>())
                .Add(new Movie.Models.Movie { Id = "unit test movie id 1", Genres = new[] { "unit test genre" } })
                .Add(new Movie.Models.Movie { Id = "unit test movie id 2", Genres = new[] { "unit test genre" } })
                .Add(new Movie.Models.Movie { Id = "unit test movie id 3", Genres = new[] { "unit test genre" } });

            var movieLibrary = new InMemoryMovieLibrary(movies);

            var result = await movieLibrary.GetMoviesByGenres(new[] { "unit test genre" }, Enumerable.Empty<string>(), 2, 1);

            Assert.Equal(movies.OrderBy(x => x.Id).Skip(1).Take(2), result);
        }
    }
}

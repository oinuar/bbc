using tech.haamu.Movie.Models;
using tech.haamu.Movie.Services;
using Moq;
using System.Collections.Generic;
using Xunit;
using System;

namespace tech.haamu.Movie.UnitTest.Services
{
    public class UsersTest
    {
        [Fact]
        public void GetById()
        {
            var unitOfWork = new Mock<IUnitOfWork>(MockBehavior.Strict);
            var user = new User { Id = "1" };

            unitOfWork
                .Setup(x => x.Users)
                .Returns(new[] { user });

            var users = new Users(unitOfWork.Object, null);

            var result = users.GetById("1");

            Assert.Same(user, result);

            unitOfWork.VerifyAll();
        }

        [Fact]
        public void GetById_NotFound()
        {
            var unitOfWork = new Mock<IUnitOfWork>(MockBehavior.Strict);
            var user = new User { Id = "1" };

            unitOfWork
                .Setup(x => x.Users)
                .Returns(new[] { user });

            var users = new Users(unitOfWork.Object, null);

            var result = users.GetById("2");

            Assert.Null(result);

            unitOfWork.VerifyAll();
        }

        [Fact]
        public void GetById_ThrowsArgumentNullException()
        {
            var users = new Users(null, null);

            Assert.Throws<ArgumentNullException>(() => users.GetById(null));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void AddLikedMovie(bool isLikedMoviesNull)
        {
            var movieComparer = new IdModel<string>.Comparer<Movie.Models.Movie>();
            var users = new Users(null, movieComparer);
            var movie = new Movie.Models.Movie { Id = "unit test movie id" };
            var user = new User {
                LikedMovies = isLikedMoviesNull ? null : new HashSet<Movie.Models.Movie>(movieComparer)
            };

            users.AddLikedMovie(user, movie);

            Assert.Collection(user.LikedMovies, x => Assert.Same(movie, x));
        }

        [Fact]
        public void AddLikedMovie_AlreadyContained()
        {
            var movieComparer = new Mock<IEqualityComparer<Movie.Models.Movie>>(MockBehavior.Strict);
            var users = new Users(null, movieComparer.Object);
            var movie = new Movie.Models.Movie { Id = "unit test movie id" };
            var user = new User { LikedMovies = new[] { movie } };

            users.AddLikedMovie(user, movie);

            Assert.Collection(user.LikedMovies, x => Assert.Same(movie, x));

            movieComparer.VerifyAll();
        }

        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        public void AddLikedMovie_ThrowArgumentNullException(bool isUserNull, bool isMovieNull)
        {
            var users = new Users(null, null);

            Assert.Throws<ArgumentNullException>(() =>
                users.AddLikedMovie(isUserNull ? null : new User(), isMovieNull ? null : new Movie.Models.Movie()));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void RemoveLikedMovie(bool isLikedMoviesNull)
        {
            var movieComparer = new IdModel<string>.Comparer<Movie.Models.Movie>();
            var users = new Users(null, movieComparer);
            var movie = new Movie.Models.Movie { Id = "unit test movie id" };
            var user = new User
            {
                LikedMovies = isLikedMoviesNull ? null : new HashSet<Movie.Models.Movie>(movieComparer) { movie }
            };

            users.RemoveLikedMovie(user, movie);

            Assert.Empty(user.LikedMovies);
        }

        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        public void RemoveLikedMovie_ThrowArgumentNullException(bool isUserNull, bool isMovieNull)
        {
            var users = new Users(null, null);

            Assert.Throws<ArgumentNullException>(() =>
                users.RemoveLikedMovie(isUserNull ? null : new User(), isMovieNull ? null : new Movie.Models.Movie()));
        }

        [Fact]
        public void GetLikes()
        {
            var movieComparer = new IdModel<string>.Comparer<Movie.Models.Movie>();
            var users = new Users(null, null);
            var movie = new Movie.Models.Movie { Id = "2" };

            var user = new User
            {
                LikedMovies = new HashSet<Movie.Models.Movie>(movieComparer) { movie }
            };

            var movieIds = new string[] { "1", movie.Id };

            var result = users.GetLikes(user, movieIds);

            Assert.Equal(new[] { movie.Id }, result);
        }

        [Fact]
        public void GetLikes_LikedMoviesIsNull()
        {
            var users = new Users(null, null);
            var user = new User();

            var movieIds = new string[] { "1" };

            var result = users.GetLikes(user, movieIds);

            Assert.Empty(result);
        }

        [Fact]
        public void GetLikes_UserIsNull()
        {
            var users = new Users(null, null);

            var result = Assert.Throws<ArgumentNullException>(() => users.GetLikes(null, null));
        }

        [Fact]
        public void GetLikes_MoveIdsNull()
        {
            var users = new Users(null, null);

            var result = Assert.Throws<ArgumentNullException>(() => users.GetLikes(new User(), null));
        }
    }
}

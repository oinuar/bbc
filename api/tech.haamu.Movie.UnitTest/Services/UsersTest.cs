using tech.haamu.Movie.Models;
using tech.haamu.Movie.Services;
using Moq;
using System.Collections.Generic;
using Xunit;

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
        public void AddLikedMovie()
        {
            var movieComparer = new IdModel<string>.Comparer<Movie.Models.Movie>();
            var users = new Users(null, movieComparer);
            var movie = new Movie.Models.Movie { Id = "unit test movie id" };
            var user = new User { LikedMovies = new HashSet<Movie.Models.Movie>(movieComparer) };

            users.AddLikedMovie(user, movie);

            Assert.Collection(user.LikedMovies, x => Assert.Same(movie, x));
        }

        [Fact]
        public void AddLikedMovie_InitializeLikedMovies()
        {
            var movieComparer = new IdModel<string>.Comparer<Movie.Models.Movie>();
            var users = new Users(null, movieComparer);
            var movie = new Movie.Models.Movie { Id = "unit test movie id" };
            var user = new User();

            users.AddLikedMovie(user, movie);

            var likedMovies = Assert.IsType<HashSet<Movie.Models.Movie>>(user.LikedMovies);

            Assert.Same(movieComparer, likedMovies.Comparer);
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

        [Fact]
        public void RemoveLikedMovie()
        {
            var movieComparer = new IdModel<string>.Comparer<Movie.Models.Movie>();
            var users = new Users(null, movieComparer);
            var movie = new Movie.Models.Movie { Id = "unit test movie id" };
            var user = new User
            {
                LikedMovies = new HashSet<Movie.Models.Movie>(movieComparer) { movie }
            };

            users.RemoveLikedMovie(user, movie);

            Assert.Empty(user.LikedMovies);
        }
    }
}

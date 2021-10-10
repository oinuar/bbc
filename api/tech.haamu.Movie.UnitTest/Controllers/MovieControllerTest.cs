using tech.haamu.Movie.Controllers;
using tech.haamu.Movie.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;
using tech.haamu.Movie.Models;

namespace tech.haamu.Movie.UnitTest.Controllers
{
    public class MovieControllerTest
    {
        [Fact]
        public async Task Index()
        {
            var movieLibrary = new Mock<IMovieLibrary>(MockBehavior.Strict);
            var movie = new Movie.Models.Movie { Id = "unit test movie id" };

            movieLibrary
                .Setup(x => x.GetAll(1000, 11, default))
                .ReturnsAsync(new[] { movie });

            var controller = new MovieController(movieLibrary.Object, null);

            var result = await controller.Index(1000, 11);

            Assert.Equal(new[] { movie }, result, new IdModel<string>.Comparer<Movie.Models.Movie>());

            movieLibrary.VerifyAll();
        }

        [Fact]
        public async Task Like()
        {
            var movieLibrary = new Mock<IMovieLibrary>(MockBehavior.Strict);
            var users = new Mock<Users>(() => new Users(null, null), MockBehavior.Strict);
            var movie = new Movie.Models.Movie { Id = "unit test movie id" };
            var user = new User { Id = "unit test user id" };

            movieLibrary
                .Setup(x => x.GetById(movie.Id, default))
                .ReturnsAsync(movie);

            users
                .Setup(x => x.GetById(user.Id))
                .Returns(user);

            users
                .Setup(x => x.AddLikedMovie(user, movie));

            var controller = new MovieController(movieLibrary.Object, users.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                        {
                            new Claim("userId", user.Id)
                        }))
                    }
                }
            };

            await controller.Like(movie.Id);

            movieLibrary.VerifyAll();
            users.VerifyAll();
        }

        [Fact]
        public async Task Dislike()
        {
            var movieLibrary = new Mock<IMovieLibrary>(MockBehavior.Strict);
            var users = new Mock<Users>(() => new Users(null, null), MockBehavior.Strict);
            var movie = new Movie.Models.Movie { Id = "unit test movie id" };
            var user = new User { Id = "unit test user id" };

            movieLibrary
                .Setup(x => x.GetById(movie.Id, default))
                .ReturnsAsync(movie);

            users
                .Setup(x => x.GetById(user.Id))
                .Returns(user);

            users
                .Setup(x => x.RemoveLikedMovie(user, movie));

            var controller = new MovieController(movieLibrary.Object, users.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                        {
                            new Claim("userId", user.Id)
                        }))
                    }
                }
            };

            await controller.Dislike(movie.Id);

            movieLibrary.VerifyAll();
            users.VerifyAll();
        }

        [Fact]
        public async Task Recommendations()
        {
            var movieLibrary = new Mock<IMovieLibrary>(MockBehavior.Strict);
            var users = new Mock<Users>(() => new Users(null, null), MockBehavior.Strict);
            var movie = new Movie.Models.Movie();
            var user = new User
            {
                Id = "unit test user id",
                LikedMovies = new[]
                {
                    new Movie.Models.Movie { Genres = new [] { "unit test genre 1", "unit test genre 2" } },
                    new Movie.Models.Movie { Genres = new [] { "unit test genre 2" } }
                }
            };

            movieLibrary
                .Setup(x => x.GetMoviesByGenres(new[] { "unit test genre 1", "unit test genre 2" },
                    It.Is<IEnumerable<string>>(y => user.LikedMovies.Select(x => x.Id).SequenceEqual(y)), 1000, 11, default))
                .ReturnsAsync(new[] { movie });

            users
                .Setup(x => x.GetById(user.Id))
                .Returns(user);

            var controller = new MovieController(movieLibrary.Object, users.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                        {
                            new Claim("userId", user.Id)
                        }))
                    }
                }
            };

            var result = await controller.Recommendations(1000, 11);

            Assert.Equal(new[] { movie }, result, new IdModel<string>.Comparer<Movie.Models.Movie>());

            movieLibrary.VerifyAll();
            users.VerifyAll();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Recommendations_NoLikes(bool isLikedMoviesNull)
        {
            var movieLibrary = new Mock<IMovieLibrary>(MockBehavior.Strict);
            var users = new Mock<Users>(() => new Users(null, null), MockBehavior.Strict);
            var movie = new Movie.Models.Movie();
            var user = new User
            {
                Id = "unit test user id",
                LikedMovies = isLikedMoviesNull ? null : new Movie.Models.Movie[] { }
            };

            movieLibrary
                .Setup(x => x.GetMoviesByGenres(It.Is<IEnumerable<string>>(x => !x.Any()),
                    It.Is<IEnumerable<string>>(x => !x.Any()), 1000, 11, default))
                .ReturnsAsync(new Movie.Models.Movie[] { });

            users
                .Setup(x => x.GetById(user.Id))
                .Returns(user);

            var controller = new MovieController(movieLibrary.Object, users.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                        {
                            new Claim("userId", user.Id)
                        }))
                    }
                }
            };

            var result = await controller.Recommendations(1000, 11);

            Assert.Empty(result);

            movieLibrary.VerifyAll();
            users.VerifyAll();
        }
    }
}

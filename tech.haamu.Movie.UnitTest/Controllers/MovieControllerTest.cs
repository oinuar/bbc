using tech.haamu.Movie.Controllers;
using tech.haamu.Movie.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace tech.haamu.Movie.UnitTest.Controllers
{
    public class MovieControllerTest
    {

        [Fact]
        public async Task Like()
        {
            var movieLibrary = new Mock<IMovieLibrary>(MockBehavior.Strict);
            var users = new Mock<Users>(() => new Users(null, null), MockBehavior.Strict);
            var movie = new Movie.Models.Movie { Id = "unit test movie id" };
            var user = new Movie.Models.User { Id = "unit test user id" };

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
            var user = new Movie.Models.User { Id = "unit test user id" };

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
        public async Task Recommend()
        {
            var movieLibrary = new Mock<IMovieLibrary>(MockBehavior.Strict);
            var users = new Mock<Users>(() => new Users(null, null), MockBehavior.Strict);
            var movie = new Movie.Models.Movie();
            var user = new Movie.Models.User {
                Id = "unit test user id",
                LikedMovies = new []
                {
                    new Movie.Models.Movie { Genres = new [] { "unit test genre 1", "unit test genre 2"}},
                    new Movie.Models.Movie { Genres = new [] { "unit test genre 2"}}
                }
            };

            movieLibrary
                .Setup(x => x.GetMoviesByGenres(new[] { "unit test genre 1", "unit test genre 2" }, 1000, 11, default))
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

            await controller.Recommend(1000, 11);

            movieLibrary.VerifyAll();
            users.VerifyAll();
        }
    }
}

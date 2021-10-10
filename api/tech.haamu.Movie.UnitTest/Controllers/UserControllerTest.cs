using tech.haamu.Movie.Controllers;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;
using tech.haamu.Movie.Services;
using tech.haamu.Movie.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace tech.haamu.Movie.UnitTest.Controllers
{
    public class UserControllerTest
    {
        [Fact]
        public void Token()
        {
            var configuration = new Mock<IConfiguration>(MockBehavior.Strict);

            configuration
                .Setup(x => x["Jwt:SecretKey"])
                .Returns("unit test secret");

            configuration
                .Setup(x => x["Jwt:Issuer"])
                .Returns("unit test issuer");

            configuration
                .Setup(x => x["Jwt:Audience"])
                .Returns("unit test audience");

            var controller = new UserController(configuration.Object, null);

            var result = controller.Token("1");

            Assert.Equal("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJEdW1teSB1c2VyIiwidXNlcklkIjoiMSIsImlzcyI6InVuaXQgdGVzdCBpc3N1ZXIiLCJhdWQiOiJ1bml0IHRlc3QgYXVkaWVuY2UifQ.n8dNOAENFmVWBK8eBr3gLAgJz2GtzKPuG70Uy1nV9_Y", result);

            configuration.VerifyAll();
        }

        [Fact]
        public void MoviePreference()
        {
            var users = new Mock<Users>(() => new Users(null, null), MockBehavior.Strict);

            var user = new User { Id = "1" };
            var moveIds = new string[] { "1", "2" };

            users
                .Setup(x => x.GetById(user.Id))
                .Returns(user);

            users
                .Setup(x => x.GetLikes(user, moveIds))
                .Returns(new string[] { "1" });

            var controller = new UserController(null, users.Object)
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

            var result = controller.MoviePreference(moveIds);

            Assert.Equal(new string[] { "1" }, result);

            users.VerifyAll();
        }
    }
}

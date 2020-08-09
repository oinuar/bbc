using tech.haamu.Movie.Controllers;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

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

            var controller = new UserController(configuration.Object);

            var result = controller.Token("1");

            Assert.Equal("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJEdW1teSB1c2VyIiwidXNlcklkIjoiMSIsImlzcyI6InVuaXQgdGVzdCBpc3N1ZXIiLCJhdWQiOiJ1bml0IHRlc3QgYXVkaWVuY2UifQ.n8dNOAENFmVWBK8eBr3gLAgJz2GtzKPuG70Uy1nV9_Y", result);

            configuration.VerifyAll();
        }
    }
}

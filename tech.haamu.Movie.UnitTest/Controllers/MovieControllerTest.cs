using tech.haamu.Movie.Controllers;
using System.Threading.Tasks;
using Xunit;

namespace tech.haamu.Movie.UnitTest.Controllers
{
    public class MovieControllerTest
    {

        [Fact]
        public async Task Like_200()
        {
            var controller = new MovieController();

            var result = await controller.Like(1);

            Assert.Equal("I like 1", result);
        }

        [Fact]
        public async Task Dislike_200()
        {
            var controller = new MovieController();

            var result = await controller.Dislike(1);

            Assert.Equal("I don't like 1", result);
        }

        [Fact]
        public async Task Recommend_200()
        {
            var controller = new MovieController();

            var result = await controller.Recommend(1000);

            Assert.Equal("I recommend 1000, 0", result);
        }
    }
}

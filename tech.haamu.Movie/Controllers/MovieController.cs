using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace tech.haamu.Movie.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovieController : ControllerBase
    {
        [Route("[action]/{id}")]
        public Task<string> Like(int id)
        {
            return Task.FromResult("I like " + id);
        }

        [Route("[action]/{id}")]
        public Task<string> Dislike(int id)
        {
            return Task.FromResult("I don't like " + id);
        }

        [Route("[action]")]
        public Task<string> Recommend([FromQuery] int limit, [FromQuery] int offset = 0)
        {
            return Task.FromResult("I recommend " + limit + ", " + offset);
        }
    }
}

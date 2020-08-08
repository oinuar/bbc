using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace tech.haamu.Movie.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovieController : ControllerBase
    {
        public Task<string> Index()
        {
            return Task.FromResult("Hello World!");
        }
    }
}

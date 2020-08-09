using tech.haamu.Movie.Models;
using System.Collections.Generic;

namespace tech.haamu.Movie
{
    public class InMemoryDataContext
    {
        public ICollection<User> Users { get; set; }
    }
}

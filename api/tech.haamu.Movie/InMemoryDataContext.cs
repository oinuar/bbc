using tech.haamu.Movie.Models;
using System.Collections.Generic;

namespace tech.haamu.Movie
{

    /// <summary>
    /// A demonstration how data context works with unit of work pattern.
    /// 
    /// Note that you should not use this class in production since is can result
    /// a loss of data! The class is only for demonstration purposes. In production,
    /// we should use a data context that is capable of tracking the changes as they
    /// happen.
    /// </summary>
    public class InMemoryDataContext
    {
        public ICollection<User> Users { get; set; }

        public InMemoryDataContext(IEqualityComparer<User> userComparer)
        {
            Users = new HashSet<User>(userComparer)
            {
                new User { Id = "1" }
            };
        }
    }
}

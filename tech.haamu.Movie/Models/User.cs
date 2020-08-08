using System.Collections.Generic;

namespace tech.haamu.Movie.Models
{
    public class User
    {
        /// <summary>
        /// Unique user ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Movies that user has liked.
        /// </summary>
        public ICollection<Movie> LikedMovies { get; set; }
    }
}

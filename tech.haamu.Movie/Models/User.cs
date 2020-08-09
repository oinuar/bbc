using System.Collections.Generic;

namespace tech.haamu.Movie.Models
{
    public class User : IdModel<int>
    {
        /// <summary>
        /// Genres that user has liked.
        /// </summary>
        public ICollection<Movie> LikedMovies { get; set; }
    }
}

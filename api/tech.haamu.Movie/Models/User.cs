using System.Collections.Generic;

namespace tech.haamu.Movie.Models
{
    public class User : IdModel<string>
    {
        /// <summary>
        /// Genres that user has liked.
        /// </summary>
        public ICollection<Movie> LikedMovies { get; set; }
    }
}

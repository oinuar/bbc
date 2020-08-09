using System.Collections.Generic;

namespace tech.haamu.Movie.Models
{
    public class Movie : IdModel<string>
    {
        /// <summary>
        /// Name of the movie.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Movie genres.
        /// </summary>
        public ICollection<string> Genres { get; set; }
    }
}

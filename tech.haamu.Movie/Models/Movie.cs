using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace tech.haamu.Movie.Models
{
    public class Movie
    {
        /// <summary>
        /// Unique movie ID.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Name of the movie.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Movie genres.
        /// </summary>
        public ICollection<string> Genres { get; set; }

        /// <summary>
        /// A comparer for Movie that uses movie ID as a comparision key.
        /// </summary>
        public class Comparer : IEqualityComparer<Movie>
        {
            /// <summary>
            /// Check the equality of two movies.
            /// </summary>
            /// <param name="x">Left hand side movie.</param>
            /// <param name="y">Right hand side movie.</param>
            /// <returns>True if the movie IDs are equal. Otherwise, false.</returns>
            public bool Equals([AllowNull] Movie x, [AllowNull] Movie y)
            {
                if (x == null && y == null)
                    return true;

                if (x == null || y == null)
                    return false;

                return x.Id == y.Id;
            }

            /// <summary>
            /// Gets the hash code for movie.
            /// </summary>
            /// <param name="obj">Movie.</param>
            /// <returns>The hash code for movie ID.</returns>
            public int GetHashCode([DisallowNull] Movie obj)
            {
                return obj.Id?.GetHashCode() ?? 0;
            }
        }
    }
}

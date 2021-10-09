using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace tech.haamu.Movie.Services
{
    /// <summary>
    /// A service class that handles movie library operations in-memory.
    /// </summary>
    public class InMemoryMovieLibrary : IMovieLibrary
    {
        public readonly IImmutableSet<Models.Movie> movies;

        public InMemoryMovieLibrary(IImmutableSet<Models.Movie> movies)
        {
            this.movies = movies;
        }

        public Task<Models.Movie> GetById(string movieId, CancellationToken cancellationToken = default)
        {
            if (movieId == null)
                throw new ArgumentNullException(nameof(movieId));

            // Attempt to retrieve a movie by ID. We don't care the return value since movie is null if
            // it cannot be found.
            movies.TryGetValue(new Models.Movie { Id = movieId }, out var movie);

            return Task.FromResult(movie);
        }

        public Task<IReadOnlyList<Models.Movie>> GetAll(int limit, int offset = 0, CancellationToken cancellationToken = default)
        {
            var results = movies

                // Order results by ID.
                .OrderBy(x => x.Id)

                // Add pagination support.
                .Skip(offset)
                .Take(limit)

                // Convert it to list to force execution of the enumerator.
                .ToList();

            return Task.FromResult((IReadOnlyList<Models.Movie>)results);
        }

        public Task<IReadOnlyList<Models.Movie>> GetMoviesByGenres(IEnumerable<string> genres, IEnumerable<string> excludedMovieIds, int limit, int offset = 0, CancellationToken cancellationToken = default)
        {
            if (genres == null)
                throw new ArgumentNullException(nameof(genres));

            // Make genres a lookup to speed up existence checks from O(n) to O(1).
            var lookup = genres.ToLookup(x => x);

            // Make excluded movies a lookup to speed up existence checks from O(n) to O(1).
            var exclusions = excludedMovieIds.ToLookup(x => x);

            var results = movies
                // Take the movies that contain at least some of the given genres and that have not been excluded.
                .Where(x => x.Genres.Any(lookup.Contains) && !exclusions.Contains(x.Id))

                // Order results by ID.
                .OrderBy(x => x.Id)

                // Add pagination support.
                .Skip(offset)
                .Take(limit)

                // Convert it to list to force execution of the enumerator.
                .ToList();

            return Task.FromResult((IReadOnlyList<Models.Movie>)results);
        }
    }
}

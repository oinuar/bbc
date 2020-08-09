using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace tech.haamu.Movie.Services
{
    public class InMemoryMovieLibrary : IMovieLibrary
    {
        public readonly IImmutableSet<Models.Movie> movies;

        public InMemoryMovieLibrary(IImmutableSet<Models.Movie> movies)
        {
            this.movies = movies;
        }

        public Task<Models.Movie> GetById(string movieId, CancellationToken cancellationToken = default)
        {
            movies.TryGetValue(new Models.Movie { Id = movieId }, out var movie);

            return Task.FromResult(movie);
        }

        public Task<IReadOnlyList<Models.Movie>> GetMoviesByGenres(IEnumerable<string> genres, int limit, int offset = 0, CancellationToken cancellationToken = default)
        {
            var lookup = genres.ToLookup(x => x);

            var results = movies
                .Where(x => x.Genres.Any(lookup.Contains))
                .OrderBy(x => x.Id)
                .Skip(offset)
                .Take(limit)
                .ToList();

            return Task.FromResult((IReadOnlyList<Models.Movie>)results);
        }
    }
}

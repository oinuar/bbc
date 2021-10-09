using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace tech.haamu.Movie.Services
{
    /// <summary>
    /// An interface that represents a movie library.
    /// </summary>
    public interface IMovieLibrary
    {
        /// <summary>
        /// Gets a movie by ID.
        /// </summary>
        /// <param name="movieId">Movie ID to get.</param>
        /// <param name="cancellationToken">A cancellation token that is capable of cancelling the asynchronous operation.</param>
        /// <returns>Asynchronous result that completes when the movie is retrieved.</returns>
        public Task<Models.Movie> GetById(string movieId, CancellationToken cancellationToken = default);


        /// <summary>
        /// Get all movies.
        /// </summary>
        /// <param name="limit">Sets the maximum amount of returned movie results.</param>
        /// <param name="offset">Sets result offset.</param>
        /// <param name="cancellationToken">A cancellation token that is capable of cancelling the asynchronous operation.</param>
        /// <returns>Asynchronous result that completes when a list of movies are retrieved.</returns>
        public Task<IReadOnlyList<Models.Movie>> GetAll(int limit, int offset = 0, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets movies by genres.
        /// 
        /// The method searches movies from movie library and returns movies that have at least
        /// one of the allowed genres.
        /// </summary>
        /// <param name="genres">Allowed genres.</param>
        /// <param name="excludedMovieIds">Excluded movie IDs that are not included in the results.</param>
        /// <param name="limit">Sets the maximum amount of returned movie results.</param>
        /// <param name="offset">Sets result offset.</param>
        /// <param name="cancellationToken">A cancellation token that is capable of cancelling the asynchronous operation.</param>
        /// <returns>Asynchronous result that completes when a list of movies are retrieved.</returns>
        public Task<IReadOnlyList<Models.Movie>> GetMoviesByGenres(IEnumerable<string> genres, IEnumerable<string> excludedMovieIds, int limit, int offset = 0, CancellationToken cancellationToken = default);
    }
}

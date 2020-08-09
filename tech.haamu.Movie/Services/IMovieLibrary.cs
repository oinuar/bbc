﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace tech.haamu.Movie.Services
{
    public interface IMovieLibrary
    {
        /// <summary>
        /// Gets a movie by ID.
        /// </summary>
        /// <param name="movieId">Movie ID to get.</param>
        /// <returns>Asynchronous result that completes when the movie is retrieved.</returns>
        public Task<Models.Movie> GetById(int movieId);

        /// <summary>
        /// Gets movies by genres.
        /// 
        /// The method searches movies from movie library and returns movies that have at least
        /// one of the allowed genres.
        /// </summary>
        /// <param name="genres">Allowed genres.</param>
        /// <param name="limit">Sets the maximum amount of returned movie results.</param>
        /// <param name="offset">Sets result offset.</param>
        /// <returns>Asynchronous result that completes when a list of movies are retrieved.</returns>
        public Task<IList<Models.Movie>> GetMoviesByGenres(IEnumerable<string> genres, int limit, int offset = 0);
    }
}
using tech.haamu.Movie.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace tech.haamu.Movie.Services
{
    /// <summary>
    /// A service class that handles user related operations.
    /// </summary>
    public class Users
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IEqualityComparer<Models.Movie> movieComparer;

        public Users(IUnitOfWork unitOfWork, IEqualityComparer<Models.Movie> movieComparer)
        {
            this.unitOfWork = unitOfWork;
            this.movieComparer = movieComparer;
        }

        /// <summary>
        /// Retrieves a user by an ID.
        /// </summary>
        /// <param name="userId">User ID to be retrieved.</param>
        /// <returns>A user that has been retrieved.</returns>
        public virtual User GetById(string userId)
        {
            if (userId == null)
                throw new ArgumentNullException(nameof(userId));

            return unitOfWork.Users.FirstOrDefault(x => x.Id == userId);
        }

        /// <summary>
        /// Adds a movie to user's liked movies.
        /// </summary>
        /// <param name="user">User that likes the movie.</param>
        /// <param name="movie">Movie that is liked.</param>
        public virtual void AddLikedMovie(User user, Models.Movie movie)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (movie == null)
                throw new ArgumentNullException(nameof(movie));

            user.LikedMovies ??= new HashSet<Models.Movie>(movieComparer);

            if (!user.LikedMovies.Contains(movie))
                user.LikedMovies.Add(movie);
        }

        /// <summary>
        /// Removes a movie from user's liked movies.
        /// </summary>
        /// <param name="user">User that dislikes the movie.</param>
        /// <param name="movie">Movie that is disliked.</param>
        public virtual void RemoveLikedMovie(User user, Models.Movie movie)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (movie == null)
                throw new ArgumentNullException(nameof(movie));

            user.LikedMovies ??= new HashSet<Models.Movie>(movieComparer);

            user.LikedMovies.Remove(movie);
        }
    }
}

﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tech.haamu.Movie.Services;
using Microsoft.AspNetCore.Mvc;

namespace tech.haamu.Movie.Controllers
{
    [ApiController]
    [User]
    [Route("[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly IMovieLibrary movieLibrary;
        private readonly Users users;

        public MovieController(IMovieLibrary movieLibrary, Users users)
        {
            this.movieLibrary = movieLibrary;
            this.users = users;
        }

        [Route("[action]/{id}")]
        public async Task Like(string id)
        {
            var movie = await movieLibrary.GetById(id);
            var user = users.GetById(this.GetUserId());

            users.AddLikedMovie(user, movie);
        }

        [Route("[action]/{id}")]
        public async Task Dislike(string id)
        {
            var movie = await movieLibrary.GetById(id);
            var user = users.GetById(this.GetUserId());

            users.RemoveLikedMovie(user, movie);
        }

        [Route("[action]")]
        public Task<IReadOnlyList<Models.Movie>> Recommend([FromQuery] int limit, [FromQuery] int offset = 0)
        {
            var user = users.GetById(this.GetUserId());

            var likedGenres = (user.LikedMovies ?? Enumerable.Empty<Models.Movie>())
                .AsQueryable()
                .SelectMany(x => x.Genres)
                .Distinct();

            return movieLibrary.GetMoviesByGenres(likedGenres, limit, offset);
        }
    }
}

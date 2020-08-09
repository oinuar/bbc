using tech.haamu.Movie.Models;
using tech.haamu.Movie.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace tech.haamu.Movie
{
    public static class IServiceCollectionExtension
    {
        /// <summary>
        /// Registers IMDB movie library backend.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <returns>The service collection that has a IMDB movie library registered.</returns>
        public static IServiceCollection AddIMDBMovieLibrary(this IServiceCollection services)
        {
            return services.AddSingleton<IMovieLibrary, IMDBMovieLibrary>();
        }

        /* To change the movie library backend to, for example, Netflix, do this:
         * 
         * 1. Implement IMovieLibrary.
         * 2. Add a service injection, something like this:
         * 
         *  public static IServiceCollection AddNetflixMovieLibrary(this IServiceCollection services)
         *  {
         *      return services.AddSingleton<IMovieLibrary, NetflixMovieLibrary>();
         *  }
         *  
         * 3. Call the method in Startup.cs.
        */

        /// <summary>
        /// Registers in-memory unit of work.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <returns>The service collection that has an in-memory unit of work registered.</returns>
        public static IServiceCollection AddInMemoryUnitOfWork(this IServiceCollection services)
        {
            return services
                .AddSingleton<InMemoryDataContext>()
                .AddScoped<IUnitOfWork, InMemoryUnitOfWork>();
        }

        /// <summary>
        /// Registers application data models.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <returns>The service collection that has application data models registered.</returns>
        public static IServiceCollection AddDataModels(this IServiceCollection services)
        {
            return services
                .AddSingleton<IEqualityComparer<Models.Movie>>(x => new IdModel<string>.Comparer<Models.Movie>())
                .AddSingleton<IEqualityComparer<User>>(x => new IdModel<int>.Comparer<User>())

                .AddSingleton<IImmutableSet<Models.Movie>>(x =>
                    ImmutableHashSet<Models.Movie>.Empty.WithComparer(x.GetRequiredService<IEqualityComparer<Models.Movie>>()));
        }
    }
}

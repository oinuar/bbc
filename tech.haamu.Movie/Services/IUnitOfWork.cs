using System;
using System.Collections.Generic;

namespace tech.haamu.Movie.Services
{
    /// <summary>
    /// Interface that represents a work unit. The unit records the state changes of data models 
    /// and writes the changes to data source when disposed.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// User data collection.
        /// </summary>
        public ICollection<Models.User> Users { get; }
    }
}

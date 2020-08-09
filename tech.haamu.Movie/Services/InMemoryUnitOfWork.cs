using tech.haamu.Movie.Models;
using System.Collections.Generic;
using System.Linq;

namespace tech.haamu.Movie.Services
{
    /// <summary>
    /// In memory implementation of the unit of work.
    /// </summary>
    public class InMemoryUnitOfWork : IUnitOfWork
    {
        private readonly InMemoryDataContext context;

        public ICollection<User> Users { get; set; }

        public InMemoryUnitOfWork(IEqualityComparer<User> userComparer, InMemoryDataContext context)
        {
            this.context = context;
            Users = context.Users.ToHashSet(userComparer);
        }

        public void Dispose()
        {
            // Lock the shared context and replace all of contents in Users collection. Note that
            // in real world, you should never do this since it can lose data. For example, when
            // two requests are changing the Users at the same time, they can overwrite each other
            // changes. This is fine here since InMemoryUnitOfWork is used only to demonstrate how
            // unit of work pattern works and should not be used in production. In production, we should
            // use a database context that has a proper concurrency handling. Entity Framework's 
            // DbContext implements it already very well since it can record the changes made 
            // in-memory that our InMemoryDataContext is not capable of doing.
            lock (context)
                context.Users = Users;
        }
    }
}

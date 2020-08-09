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

        public InMemoryUnitOfWork(InMemoryDataContext context)
        {
            this.context = context;
            Users = context.Users.ToHashSet(new IdModel<int>.Comparer<User>());
        }

        public void Dispose()
        {
            lock (context)
                context.Users = Users;
        }
    }
}

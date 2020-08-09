using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace tech.haamu.Movie.Models
{
    /// <summary>
    /// A model that has an ID.
    /// </summary>
    /// <typeparam name="T">Type of the ID.</typeparam>
    public class IdModel<T>
    {
        public T Id { get; set; }

        /// <summary>
        /// An equality comparer for ID model.
        /// </summary>
        public class Comparer<U> : IEqualityComparer<U> where U : IdModel<T>
        {
            /// <summary>
            /// Check the equality of two models.
            /// </summary>
            /// <param name="x">Left hand side movie.</param>
            /// <param name="y">Right hand side movie.</param>
            /// <returns>True if the ID models are equal. Otherwise, false.</returns>
            public bool Equals([AllowNull] U x, [AllowNull] U y)
            {
                if (x == null && y == null)
                    return true;

                if (x == null || y == null)
                    return false;

                return EqualityComparer<T>.Default.Equals(x.Id, y.Id);
            }

            /// <summary>
            /// Gets the hash code for model.
            /// </summary>
            /// <param name="obj">ID model.</param>
            /// <returns>The hash code for ID model.</returns>
            public int GetHashCode([DisallowNull] U obj)
            {
                return EqualityComparer<T>.Default.GetHashCode(obj.Id);
            }
        }
    }
}

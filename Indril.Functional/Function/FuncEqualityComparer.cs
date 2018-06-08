using System;
using System.Collections.Generic;

namespace Indril.Functional.Function
{
    /// <summary>
    /// An equality comparer that uses a binary predicate to compare two objects for equality.
    /// </summary>
    /// <typeparam name="T">The type of objects that this comparer can test for equality.</typeparam>
    public class FuncEqualityComparer<T> : IEqualityComparer<T>
    {
        private Func<T, T, bool> f;
        
        /// <summary>
        /// Creates an <see cref="IEqualityComparer{T}"/> out of a binary predicate.
        /// </summary>
        /// <param name="f">The binary predicate to lift.</param>
        public FuncEqualityComparer(Func<T, T, bool> f)
        {
            this.f = f;
        }

        /// <summary>
        /// Uses the supplied binary predicate to compare two objects for equality.
        /// </summary>
        /// <param name="x">The first object.</param>
        /// <param name="y">The second object.</param>
        public bool Equals(T x, T y) => f(x, y);

        /// <summary>
        /// Uses the object's <see cref="object.GetHashCode"/> function to return a hash code.
        /// </summary>
        /// <param name="obj">The object to hash.</param>
        public int GetHashCode(T obj) => obj.GetHashCode();
    }

    /// <summary>
    /// Extension methods for <see cref="FuncEqualityComparer{T}"/>.
    /// </summary>
    public static class FuncEqualityComparerExtensions
    {
        /// <summary>
        /// Turns a binary predicate into a <see cref="IEqualityComparer{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of objects to compare.</typeparam>
        /// <param name="f">The comparison function.</param>
        public static IEqualityComparer<T> ToEqualityComparer<T>(this Func<T, T, bool> f) => new FuncEqualityComparer<T>(f);
    }
}

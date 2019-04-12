using System;
using System.Collections.Generic;

namespace Nordril.Functional
{
    /// <summary>
    /// An equality comparer that uses a binary predicate to compare two objects for equality.
    /// </summary>
    /// <typeparam name="T">The type of objects that this comparer can test for equality.</typeparam>
    public class FuncEqualityComparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> f;
        private readonly Func<T, int> getHashCode;

        /// <summary>
        /// Creates an <see cref="IEqualityComparer{T}"/> out of a binary predicate.
        /// If two objects should be equal, be sure that their hashcodes are equal as well, otherwise you will get spurious inequalities. Per default, <see cref="object.GetHashCode"/> will be used. See <see cref="FuncEqualityComparer{T}(Func{T,T,bool}, Func{T, int})"/> for specifying the hash-function.
        /// </summary>
        /// <param name="f">The binary predicate to lift.</param>
        public FuncEqualityComparer(Func<T, T, bool> f)
        {
            this.f = f;
            getHashCode = x => x.GetHashCode();
        }

        /// <summary>
        /// Creates an <see cref="IEqualityComparer{T}"/> out of a binary predicate.
        /// If two objects should be equal, be sure that their hashcodes are equal as well, otherwise you will get spurious inequalities.
        /// </summary>
        /// <param name="f">The binary predicate to lift.</param>
        /// <param name="getHashCode">The hashing-function for objects.</param>
        public FuncEqualityComparer(Func<T, T, bool> f, Func<T, int> getHashCode)
        {
            this.f = f;
            this.getHashCode = getHashCode;
        }

        /// <summary>
        /// Uses the supplied binary predicate to compare two objects for equality.
        /// </summary>
        /// <param name="x">The first object.</param>
        /// <param name="y">The second object.</param>
        public bool Equals(T x, T y) => f(x, y);

        /// <summary>
        /// Uses the object's <see cref="object.GetHashCode"/> function or the hashing-function passed in the constructor.
        /// </summary>
        /// <param name="obj">The object to hash.</param>
        public int GetHashCode(T obj) => getHashCode(obj);
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

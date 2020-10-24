using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional
{
    /// <summary>
    /// A comparer that uses a binary function to compare two objects.
    /// </summary>
    /// <typeparam name="T">The type of objects that this comparer can test for equality.</typeparam>
    public class FuncComparer<T> : FuncEqualityComparer<T>, IComparer<T>
    {
        private readonly Func<T, T, int> f;

        /// <summary>
        /// Creates an <see cref="IComparer{T}"/> out of a binary function.
        /// </summary>
        /// <param name="f">The binary function to lift.</param>
        /// <param name="getHashCode"></param>
        public FuncComparer(Func<T, T, int> f, Func<T, int> getHashCode) : base((x,y) => f(x,y) == 0, getHashCode)
        {
            this.f = f;
        }

        /// <summary>
        /// Uses the supplied binary function to compare two objects.
        /// </summary>
        /// <param name="x">The first object.</param>
        /// <param name="y">The second object.</param>
        public int Compare(T x, T y) => f(x, y);
    }

    /// <summary>
    /// Extension methods for <see cref="FuncComparer{T}"/>.
    /// </summary>
    public static class FuncComparer
    {
        /// <summary>
        /// Creates a <see cref="FuncComparer{T}"/> for a type implementing <see cref="IComparable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of objects to compare.</typeparam>
        public static FuncComparer<T> Make<T>() where T : IComparable<T> => new FuncComparer<T>((x, y) => x.CompareTo(y), x => x.GetHashCode());

        /// <summary>
        /// Create a <see cref="FuncComparer{T}"/> out of a comparison-function and a hash-function.
        /// </summary>
        /// <typeparam name="T">The type of object to compare.</typeparam>
        /// <param name="f">The comparison function.</param>
        /// <param name="getHashCode">The hash-function.</param>
        public static FuncComparer<T> Make<T>(Func<T, T, int> f, Func<T, int> getHashCode)
            => new FuncComparer<T>(f, getHashCode);
    }
}

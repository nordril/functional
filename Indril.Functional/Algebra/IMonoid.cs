using System;
using System.Collections.Generic;
using System.Linq;

namespace Indril.Functional.Algebra
{
    /// <summary>
    /// A semigroup which has a neutral element with respect to the binary operation.
    /// </summary>
    /// <typeparam name="T">The type of the implementor.</typeparam>
    public interface IMonoid<T> : ISemigroup<T>
        where T : IMonoid<T>
    {
        /// <summary>
        /// Returns the neutral element. The neutral element must fulfill the following for any X and Y:
        /// <code>
        ///     X.Neutral().Op(Y) == Y (left-neutrality)
        ///     X.Op(Y.Neutral()) == X (right-neutrality)
        /// </code>
        /// </summary>
        T Neutral { get; }
    }

    /// <summary>
    /// Extensions for monoids.
    /// </summary>
    public static class MonoidExtensions
    {
        /// <summary>
        /// Sums a list of monoid elements using the monoid operation. If the list is empty, the neutral element is returned. 
        /// </summary>
        /// <typeparam name="T">The monoid type.</typeparam>
        /// <param name="xs">The list of elements to sum.</param>
        public static T Msum<T>(this IEnumerable<T> xs) where T : IMonoid<T>, new() 
            => xs.Aggregate(new T().Neutral, (x, y) => x.Op(y));

        /// <summary>
        /// Sums a list of monoid elements using the monoid operation. If the list is empty, the neutral element is returned. 
        /// </summary>
        /// <typeparam name="T">The monoid type.</typeparam>
        /// <param name="xs">The list of elements to sum.</param>
        /// <param name="empty">The neutral element.</param>
        public static T Msum<T>(this IEnumerable<T> xs, Func<T> empty) where T : IMonoid<T>
            => xs.Aggregate(empty(), (x, y) => x.Op(y));
    }
}
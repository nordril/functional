using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional.Algebra
{
    /// <summary>
    /// A type which supports a total order. A total order has a "less then or equal" relation (<see cref="Leq(T)"/> with the following properties:
    /// <list type="table">
    ///     <listheader>
    ///         <term>Property</term>
    ///         <description>Definition</description>
    ///     </listheader>
    ///     <item>
    ///         <term>reflexivity</term>
    ///         <description>[forall a] a.Leq(a) == true</description>
    ///     </item>
    ///     <item>
    ///         <term>antisymmetry</term>
    ///         <description>[forall a,b] if a.Leq(b) == b.Leq(a) == true then a.Equals(b)</description>
    ///     </item>
    ///     <item>
    ///         <term>transitivity</term>
    ///         <description>[forall a,b,c] if a.Leq(b) == b.Leq(c) == true then a.Leq(c) == true</description>
    ///     </item>
    ///     <item>
    ///         <term>agreement</term>
    ///         <description>[forall a,b, t] if a.Leq(b) == t &lt;==&gt; a.LeqPartial(b) == Maybe.Just(t)</description>
    ///     </item>
    /// </list>
    /// </summary>
    /// <typeparam name="T">The type of the implementor.</typeparam>
    public interface ITotallyOrdered<T> : IPartiallyOrdered<T>
        where T : ITotallyOrdered<T>, IEquatable<T>
    {
        /// <summary>
        /// The "less than or equals"-predicate.
        /// </summary>
        /// <param name="that">The second element to compare to the first.</param>
        /// <returns>true iff the first element is less than or equal to the second.</returns>
        bool Leq(T that);
    }

    /// <summary>
    /// Extension methods for <see cref="ITotallyOrdered{T}"/>.
    /// </summary>
    public static class TotallyOrderedExtenions
    {
        /// <summary>
        /// The "strictly less than"-predicate
        /// </summary>
        /// <typeparam name="T">The type of the element.</typeparam>
        /// <param name="x">The first element.</param>
        /// <param name="y">The second element.</param>
        public static bool Le<T>(this T x, T y)
            where T : ITotallyOrdered<T>, IEquatable<T>
            => x.Leq(y) && !x.Equals(y);

        /// <summary>
        /// The "strictly greater than"-predicate
        /// </summary>
        /// <typeparam name="T">The type of the element.</typeparam>
        /// <param name="x">The first element.</param>
        /// <param name="y">The second element.</param>
        public static bool Ge<T>(this T x, T y)
            where T : ITotallyOrdered<T>, IEquatable<T>
            => !x.Leq(y);

        /// <summary>
        /// The "greater than or equals"-predicate
        /// </summary>
        /// <typeparam name="T">The type of the element.</typeparam>
        /// <param name="x">The first element.</param>
        /// <param name="y">The second element.</param>
        public static bool Geq<T>(this T x, T y)
            where T : ITotallyOrdered<T>, IEquatable<T>
            => !x.Leq(y) || x.Equals(y);
    }
}

using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional.Algebra
{
    /// <summary>
    /// A value-level total order.
    /// </summary>
    /// <typeparam name="T">The type of element in this structure.</typeparam>
    public class TotalOrder<T> : PartialOrder<T>
    {
        /// <summary>
        /// The "less than or equals"-predicate. See <see cref="IPartiallyOrdered{T}.LeqPartial(T)"/>.
        /// </summary>
        public Func<T, T, bool> Leq { get; }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="leq">The "less than or equals"-predicate.</param>
        public TotalOrder(Func<T, T, bool> leq) : base((x,y) => Maybe.Just(leq(x,y)))
        {
            Leq = leq;
        }
    }

    /// <summary>
    /// Extension methods for <see cref="TotalOrder{T}"/>.
    /// </summary>
    public static class TotalOrder
    {
        /// <summary>
        /// The "strictly less than"-predicate
        /// </summary>
        /// <typeparam name="T">The type of the element.</typeparam>
        /// <param name="order">The total order.</param>
        /// <param name="x">The first element.</param>
        /// <param name="y">The second element.</param>
        public static bool Le<T>(this TotalOrder<T> order, T x, T y)
            where T : IEquatable<T>
            => order.Leq(x, y) && !x.Equals(y);

        /// <summary>
        /// The "strictly greater than"-predicate
        /// </summary>
        /// <typeparam name="T">The type of the element.</typeparam>
        /// <param name="order">The total order.</param>
        /// <param name="x">The first element.</param>
        /// <param name="y">The second element.</param>
        public static bool Ge<T>(this TotalOrder<T> order, T x, T y)
            => !order.Leq(x,y);

        /// <summary>
        /// The "greater than or equals"-predicate
        /// </summary>
        /// <typeparam name="T">The type of the element.</typeparam>
        /// <param name="order">The total order.</param>
        /// <param name="x">The first element.</param>
        /// <param name="y">The second element.</param>
        public static bool Geq<T>(this TotalOrder<T> order, T x, T y)
            where T : IEquatable<T>
            => !order.Leq(x,y) || x.Equals(y);

        /// <summary>
        /// Returns the total order on a type <typeparamref name="T"/> which implements <see cref="IComparable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type that implements <see cref="IComparable{T}"/>.</typeparam>
        public static TotalOrder<T> FromComparable<T>()
            where T : IComparable<T>
            => new TotalOrder<T>((x, y) => x.CompareTo(y) <= 0);

        /// <summary>
        /// Returns a total order on a type <typeparamref name="T"/> based on a <paramref name="comparer"/>.
        /// </summary>
        /// <typeparam name="T">The type for which we have an <see cref="IComparer{T}"/>.</typeparam>
        /// <param name="comparer">The comparer.</param>
        public static TotalOrder<T> ToTotalOrder<T>(this IComparer<T> comparer)
            => new TotalOrder<T>((x, y) => comparer.Compare(x, y) <= 0);

        /// <summary>
        /// Lifts a total order into one which supports positive infinity in the form of <see cref="Maybe.Nothing{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in this structure.</typeparam>
        /// <param name="order">The total order to lift.</param>
        public static TotalOrder<Maybe<T>> LiftTotalOrderWithInfinity<T>(this TotalOrder<T> order)
            => new TotalOrder<Maybe<T>>((x, y) => {
                var comp = x.HasValue.CompareTo(y.HasValue);
                //Exactly the first one is infinite -> the first one is greater
                if (comp < 0)
                    return false;
                //Exactly the second is infinite -> the second one is greater
                else if (comp > 0)
                    return true;
                //Neither have a value <-> both are infinite <-> true
                else if (!x.HasValue)
                    return true;
                //Both have a value <-> neither are infinite <-> use underlying order
                else
                    return order.Leq(x.Value(), y.Value());
            });
    }
}

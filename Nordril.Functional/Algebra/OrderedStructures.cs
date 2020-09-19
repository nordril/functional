using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional.Algebra
{
    /// <summary>
    /// A grouplike structure equipped with a partial order.
    /// </summary>
    /// <typeparam name="T">The type of the carrier set.</typeparam>
    /// <typeparam name="TStructure">The type of the grouplike structure.</typeparam>
    /// <typeparam name="TOrder">The type of the order.</typeparam>
    public interface IPartiallyOrderedGrouplike<T, out TStructure, out TOrder>
        : IContainsFirst<TStructure>
        , IContainsOrder<TOrder>
        where TStructure : IMagma<T>
        where TOrder : IPartialOrder<T>
    {
    }

    /// <summary>
    /// A ringlike structure equipped with a partial order.
    /// </summary>
    /// <typeparam name="T">The type of the carrier set.</typeparam>
    /// <typeparam name="TRinglike">The type of the ringlike structure.</typeparam>
    /// <typeparam name="TFirst">The type of the first grouplike structure.</typeparam>
    /// <typeparam name="TSecond">The type of the second grouplike structure.</typeparam>
    /// <typeparam name="TOrder">The type of the order.</typeparam>
    public interface IPartiallyOrderedRinglike<T, out TRinglike, out TFirst, out TSecond, out TOrder>
        : IContainsFirst<TFirst>
        , IContainsSecond<TSecond>
        , IContainsRinglike<TRinglike>
        , IContainsOrder<TOrder>
        where TFirst : IMagma<T>
        where TSecond : IMagma<T>
        where TRinglike : IRinglike<T, TFirst, TSecond>
        where TOrder : IPartialOrder<T>
    {
    }

    /// <summary>
    /// A rouplike structure equipped with a total order.
    /// </summary>
    /// <typeparam name="T">The type of the carrier set.</typeparam>
    /// <typeparam name="TStructure">The type of the grouplike structure.</typeparam>
    /// <typeparam name="TOrder">The type of the order.</typeparam>
    public interface ITotallyOrderedGrouplike<T, out TStructure, out TOrder>
        : IContainsFirst<TStructure>
        , IContainsOrder<TOrder>
        , IPartiallyOrderedGrouplike<T, TStructure, TOrder>
        where TStructure : IMagma<T>
        where TOrder : ITotalOrder<T>
    {
    }

    /// <summary>
    /// A ringlike structure equipped with a total order.
    /// </summary>
    /// <typeparam name="T">The type of the carrier set.</typeparam>
    /// <typeparam name="TRinglike">The type of the ringlike structure.</typeparam>
    /// <typeparam name="TFirst">The type of the first grouplike structure.</typeparam>
    /// <typeparam name="TSecond">The type of the second grouplike structure.</typeparam>
    /// <typeparam name="TOrder">The type of the order.</typeparam>
    public interface ITotallyOrderedRinglike<T, out TRinglike, out TFirst, out TSecond, out TOrder>
        :  IPartiallyOrderedRinglike<T, TRinglike, TFirst, TSecond, TOrder>
        where TFirst : IMagma<T>
        where TSecond : IMagma<T>
        where TRinglike : IRinglike<T, TFirst, TSecond>
        where TOrder : IPartialOrder<T>
    {
    }

    /// <summary>
    /// Extension methods for ordered sets.
    /// </summary>
    public static class Ordered
    {
        /// <summary>
        /// Equips a grouplike structure with a partial order.
        /// </summary>
        /// <typeparam name="T">The type of the carrier set.</typeparam>
        /// <typeparam name="TStructure">The type of the structure.</typeparam>
        /// <typeparam name="TOrder">The type of the order.</typeparam>
        /// <param name="structure">The structure.</param>
        /// <param name="order">The order.</param>
        public static IPartiallyOrderedGrouplike<T, TStructure, TOrder> WithPartialOrder<T, TStructure, TOrder>(this TStructure structure, TOrder order)
            where TStructure : IMagma<T>
            where TOrder : IPartialOrder<T>
            => new PartiallyOrderedGrouplike<T, TStructure, TOrder>(structure, order);

        /// <summary>
        /// Equips a ringlike structure with a partial order.
        /// </summary>
        /// <typeparam name="T">The type of the carrier set.</typeparam>
        /// <typeparam name="TRinglike">The type of the ringlike structure.</typeparam>
        /// <typeparam name="TFirst">The type of the first grouplike structure.</typeparam>
        /// <typeparam name="TSecond">The type of the second grouplike structure.</typeparam>
        /// <typeparam name="TOrder">The type of the order.</typeparam>
        /// <param name="structure">The ringlike structure.</param>
        /// <param name="order">The order.</param>
        public static IPartiallyOrderedRinglike<T, TRinglike, TFirst, TSecond, TOrder> WithPartialOrder<T, TRinglike, TFirst, TSecond, TOrder>(this TRinglike structure, TOrder order)
            where TFirst : IMagma<T>
            where TSecond : IMagma<T>
            where TRinglike : IRinglike<T, TFirst, TSecond>
            where TOrder : IPartialOrder<T>
            => new PartiallyOrderedRinglike<T, TRinglike, TFirst, TSecond, TOrder>(structure, order);

        /// <summary>
        /// Equips a grouplike structure with a total order.
        /// </summary>
        /// <typeparam name="T">The type of the carrier set.</typeparam>
        /// <typeparam name="TStructure">The type of the structure.</typeparam>
        /// <typeparam name="TOrder">The type of the order.</typeparam>
        /// <param name="structure">The structure.</param>
        /// <param name="order">The order.</param>
        public static ITotallyOrderedGrouplike<T, TStructure, TOrder> WithTotalOrder<T, TStructure, TOrder>(this TStructure structure, TOrder order)
            where TStructure : IMagma<T>
            where TOrder : ITotalOrder<T>
            => new TotallyOrderedGrouplike<T, TStructure, TOrder>(structure, order);

        /// <summary>
        /// Equips a ringlike structure with a total order.
        /// </summary>
        /// <typeparam name="T">The type of the carrier set.</typeparam>
        /// <typeparam name="TRinglike">The ringlike structure.</typeparam>
        /// <typeparam name="TFirst">The type of the first grouplike structure.</typeparam>
        /// <typeparam name="TSecond">The type of the second grouplike structure.</typeparam>
        /// <typeparam name="TOrder">The type of the order.</typeparam>
        /// <param name="structure">The ringlike structure.</param>
        /// <param name="order">The order.</param>
        public static ITotallyOrderedRinglike<T, TRinglike, TFirst, TSecond, TOrder> WithTotalOrder<T, TRinglike, TFirst, TSecond, TOrder>(this TRinglike structure, TOrder order)
            where TRinglike : IRinglike<T, TFirst, TSecond>
            where TFirst : IMagma<T>
            where TSecond : IMagma<T>
            where TOrder : ITotalOrder<T>
            => new TotallyOrderedRinglike<T, TRinglike, TFirst, TSecond, TOrder>(structure, order);

        /// <summary>
        /// Returns the absolute value of an element <paramref name="x"/>, as defined via the piecewise function:
        /// <code>
        /// abs(x) = if x &lt; 0 then negate(x) else x
        /// </code>
        /// Where <c>&lt;</c> is defined by the total order, <c>0</c> is the neutral element of the first grouplike structure, and <c>negate</c> is the inverse-function of the first grouplike structure.
        /// </summary>
        /// <typeparam name="TStructure">The ringlike structure.</typeparam>
        /// <typeparam name="TFirst">The first grouplike structure.</typeparam>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="st">The structure to use.</param>
        /// <param name="x">The element whose absolute value to get.</param>
        public static T Abs<TStructure, TFirst, T>(this TStructure st, T x)
            where TStructure : IContainsFirst<TFirst>, IContainsOrder<ITotalOrder<T>>
            where TFirst : IInverse<T>, INeutralElement<T>
        {
            return st.Order.Le(x, st.Zero<T, TFirst>()) ? st.Negate(x) : x;
        }

        /// <summary>
        /// Returns the absolute value of an element <paramref name="x"/>, as defined via the piecewise function:
        /// <code>
        /// abs(x) = if x &lt; 0 then negate(x) else x
        /// </code>
        /// Where <c>&lt;</c> is defined by the total order, <c>0</c> is the neutral element of the first grouplike structure, and <c>negate</c> is the inverse-function of the first grouplike structure.
        /// </summary>
        /// <typeparam name="TStructure">The ringlike structure.</typeparam>
        /// <typeparam name="TFirst">The first grouplike structure.</typeparam>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="st">The structure to use.</param>
        /// <param name="order">The total order.</param>
        /// <param name="x">The element whose absolute value to get.</param>
        public static T Abs<T, TStructure, TFirst>(this TStructure st, ITotalOrder<T> order, T x)
            where TStructure : IContainsFirst<TFirst>
            where TFirst : IInverse<T>, INeutralElement<T>
            => order.Le(x, st.Zero<T, TFirst>()) ? st.Negate(x) : x;

        /// <summary>
        /// Returns the sign (-1 for negative values, 0 for 0, 1 for positive values) of an element <paramref name="x"/>.
        /// </summary>
        /// <typeparam name="TStructure">The type of the algebraic structure to use.</typeparam>
        /// <typeparam name="TFirst">The first grouplike structure.</typeparam>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="st">The algebraic structure to use</param>
        /// <param name="x">The element whose sign to get.</param>
        public static int Signum<T, TStructure, TFirst>(this TStructure st, T x)
            where TStructure : IContainsFirst<TFirst>, IContainsOrder<ITotalOrder<T>>
            where TFirst : INeutralElement<T>
            => st.Order.Compare(x, st.First.Neutral);

        /// <summary>
        /// Returns the sign (-1 for negative values, 0 for 0, 1 for positive values) of an element <paramref name="x"/>.
        /// </summary>
        /// <typeparam name="TStructure">The type of the algebraic structure to use.</typeparam>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="st">The algebraic structure to use</param>
        /// <param name="order">The total order.</param>
        /// <param name="x">The element whose sign to get.</param>
        public static int Signum<TStructure, T>(this TStructure st, ITotalOrder<T> order, T x)
            where TStructure : INeutralElement<T>
            => order.Compare(x, st.Neutral);
    }
}

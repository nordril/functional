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
    public struct PartiallyOrderedGrouplike<T, TStructure, TOrder>
        : IPartiallyOrderedGrouplike<T, TStructure, TOrder>
        where TStructure : IMagma<T>
        where TOrder : IPartialOrder<T>
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="structure">The structure.</param>
        /// <param name="order">The order.</param>
        public PartiallyOrderedGrouplike(TStructure structure, TOrder order)
        {
            First = structure;
            Order = order;
        }

        /// <inheritdoc />
        public TStructure First { get; }

        /// <inheritdoc />
        public TOrder Order { get; }
    }

    /// <summary>
    /// A ringlike structure equipped with a partial order.
    /// </summary>
    /// <typeparam name="T">The type of the carrier set.</typeparam>
    /// <typeparam name="TRinglike">The type of the ringlike structure.</typeparam>
    /// <typeparam name="TFirst">The type of the first grouplike structure.</typeparam>
    /// <typeparam name="TSecond">The type of the second grouplike structure.</typeparam>
    /// <typeparam name="TOrder">The type of the order.</typeparam>
    public struct PartiallyOrderedRinglike<T, TRinglike, TFirst, TSecond, TOrder>
        : IPartiallyOrderedRinglike<T, TRinglike, TFirst, TSecond, TOrder>
        where TFirst : IMagma<T>
        where TSecond : IMagma<T>
        where TRinglike : IRinglike<T, TFirst, TSecond>
        where TOrder : IPartialOrder<T>
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="ringlike">The ringlike structure.</param>
        /// <param name="order">The order.</param>
        public PartiallyOrderedRinglike(TRinglike ringlike, TOrder order)
        {
            Ringlike = ringlike;
            First = ringlike.First;
            Second = ringlike.Second;
            Order = order;
        }

        /// <inheritdoc />
        public TFirst First { get; }

        /// <inheritdoc />
        public TSecond Second { get; }

        /// <inheritdoc />
        public TOrder Order { get; }

        /// <inheritdoc />
        public TRinglike Ringlike { get; }
    }

    /// <summary>
    /// A grouplike structure equipped with a total order.
    /// </summary>
    /// <typeparam name="T">The type of the carrier set.</typeparam>
    /// <typeparam name="TStructure">The type of the grouplike structure.</typeparam>
    /// <typeparam name="TOrder">The type of the order.</typeparam>
    public struct TotallyOrderedGrouplike<T, TStructure, TOrder>
        : ITotallyOrderedGrouplike<T, TStructure, TOrder>
        where TStructure : IMagma<T>
        where TOrder : ITotalOrder<T>
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="structure">The structure.</param>
        /// <param name="order">The order.</param>
        public TotallyOrderedGrouplike(TStructure structure, TOrder order)
        {
            First = structure;
            Order = order;
        }

        /// <inheritdoc />
        public TStructure First { get; }

        /// <inheritdoc />
        public TOrder Order { get; }
    }

    /// <summary>
    /// A ringlike structure equipped with a total order.
    /// </summary>
    /// <typeparam name="T">The type of the carrier set.</typeparam>
    /// <typeparam name="TRinglike">The type of the ringlike structure.</typeparam>
    /// <typeparam name="TFirst">The type of the first grouplike structure.</typeparam>
    /// <typeparam name="TSecond">The type of the second grouplike structure.</typeparam>
    /// <typeparam name="TOrder">The type of the order.</typeparam>
    public struct TotallyOrderedRinglike<T, TRinglike, TFirst, TSecond, TOrder>
        : ITotallyOrderedRinglike<T, TRinglike, TFirst, TSecond, TOrder>
        where TFirst : IMagma<T>
        where TSecond : IMagma<T>
        where TRinglike : IRinglike<T, TFirst, TSecond>
        where TOrder : ITotalOrder<T>
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="ringlike">The ringlike structure.</param>
        /// <param name="order">The order.</param>
        public TotallyOrderedRinglike(TRinglike ringlike, TOrder order)
        {
            Ringlike = ringlike;
            First = ringlike.First;
            Second = ringlike.Second;
            Order = order;
        }

        /// <inheritdoc />
        public TFirst First { get; }

        /// <inheritdoc />
        public TSecond Second { get; }

        /// <inheritdoc />
        public TOrder Order { get; }

        /// <inheritdoc />
        public TRinglike Ringlike { get; }
    }
}

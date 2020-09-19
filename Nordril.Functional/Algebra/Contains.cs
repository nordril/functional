using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional.Algebra
{
    /// <summary>
    /// A product which contains a first grouplike structure.
    /// </summary>
    /// <typeparam name="T">The type of the grouplike.</typeparam>
    public interface IContainsFirst<out T>
    {
        /// <summary>
        /// Gets the first grouplike.
        /// </summary>
        T First { get; }
    }

    /// <summary>
    /// A product which contains a second grouplike structure.
    /// </summary>
    /// <typeparam name="T">The type of the grouplike.</typeparam>
    public interface IContainsSecond<out T>
    {
        /// <summary>
        /// Gets the second grouplike.
        /// </summary>
        T Second { get; }
    }

    /// <summary>
    /// A product which contains a ringlike structure.
    /// </summary>
    /// <typeparam name="T">The type of the ringlike.</typeparam>
    public interface IContainsRinglike<out T>
    {
        /// <summary>
        /// Gets the second grouplike.
        /// </summary>
        T Ringlike { get; }
    }

    /// <summary>
    /// A product which contains an ordering like <see cref="ITotalOrder{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the ordering.</typeparam>
    public interface IContainsOrder<out T>
    {
        /// <summary>
        /// Gets the order.
        /// </summary>
        T Order { get; }
    }
}

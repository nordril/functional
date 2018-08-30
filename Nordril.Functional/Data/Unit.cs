using Nordril.Functional.Algebra;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Nordril.Functional.Data
{
    /// <summary>
    /// A unit-type containing no values and having just one instance.
    /// Forms the trivial, one-element commutative group and is a 0-tuple.
    /// </summary>
    public struct Unit : ICommutativeGroup<Unit>, ITuple, IEquatable<Unit>, IComparable<Unit>
    {
        /// <inheritdoc />
        public Unit Inverse => new Unit();

        /// <inheritdoc />
        public Unit Neutral => new Unit();

        /// <summary>
        /// Always returns true.
        /// </summary>
        /// <param name="a">The first unit.</param>
        /// <param name="b">The second unit</param>
        public static bool operator ==(Unit a, Unit b) => true;
        /// <summary>
        /// Always returns false.
        /// </summary>
        /// <param name="a">The first unit.</param>
        /// <param name="b">The second unit</param>
        public static bool operator !=(Unit a, Unit b) => false;
        /// <summary>
        /// Always returns false.
        /// </summary>
        /// <param name="a">The first unit.</param>
        /// <param name="b">The second unit</param>
        public static bool operator <(Unit a, Unit b) => false;
        /// <summary>
        /// Always returns false.
        /// </summary>
        /// <param name="a">The first unit.</param>
        /// <param name="b">The second unit</param>
        public static bool operator >(Unit a, Unit b) => false;
        /// <summary>
        /// Always returns true.
        /// </summary>
        /// <param name="a">The first unit.</param>
        /// <param name="b">The second unit</param>
        public static bool operator <=(Unit a, Unit b) => true;
        /// <summary>
        /// Always returns true.
        /// </summary>
        /// <param name="a">The first unit.</param>
        /// <param name="b">The second unit</param>
        public static bool operator >=(Unit a, Unit b) => true;

        /// <summary>
        /// Returns true iff the other object is a <see cref="Unit"/>.
        /// </summary>
        /// <param name="obj">The object to which to compare this.</param>
        public override bool Equals(object obj) => obj != null && obj is Unit;
        /// <summary>
        /// Gets a hash code for a <see cref="Unit"/>. A constant.
        /// </summary>
        public override int GetHashCode() => this.DefaultHash();

        /// <inheritdoc />
        public Unit Op(Unit that) => new Unit();

        /// <summary>
        /// Always returns 0.
        /// </summary>
        public int Length => 0;

        /// <summary>
        /// Always throws an <see cref="IndexOutOfRangeException"/>, since <see cref="Unit"/> is a 0-tuple.
        /// </summary>
        /// <param name="index">The index of the element to get.</param>
        /// <exception cref="IndexOutOfRangeException">Always thrown when this indexer is called.</exception>
        [SuppressMessage("Microsoft.Design", "CA1065", Justification = "It's an indexer.")]
        public object this[int index] => throw new IndexOutOfRangeException();

        /// <summary>
        /// Always returns 0.
        /// </summary>
        /// <param name="other">The other unit.</param>
        public int CompareTo(Unit other) => 0;

        /// <summary>
        /// Always returns true.
        /// </summary>
        /// <param name="other">The other unit.</param>
        public bool Equals(Unit other) => true;
    }
}

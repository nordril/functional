using System;

namespace Indril.Functional.Algebra
{
    /// <summary>
    /// A value-level semigroup whose binary operation is associative.
    /// </summary>
    /// <typeparam name="T">The type of element in this structure.</typeparam>
    public class Semigroup<T> : Magma<T>
    {
        /// <summary>
        /// Creates a new semigroup.
        /// </summary>
        /// <param name="f">The binary operation.</param>
        public Semigroup(Func<T, T, T> f) : base(f)
        {
        }
    }

    /// <summary>
    /// Extension methods for <see cref="Semigroup{T}"/>.
    /// </summary>
    public static class Semigroup
    {
        /// <summary>
        /// The (^) semigroup for <see cref="bool"/>.
        /// </summary>
        public static readonly Semigroup<bool> BoolXor = new Semigroup<bool>((x, y) => x ^ y);
    }
}

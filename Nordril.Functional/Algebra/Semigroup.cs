using Nordril.Functional.Category;
using Nordril.Functional.Data;
using System;

namespace Nordril.Functional.Algebra
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

        /// <summary>
        /// The semigroup whose operation always returns the first element.
        /// </summary>
        /// <typeparam name="T">The type of the element in the structure.</typeparam>
        public static Semigroup<T> First<T>() => new Semigroup<T>((x, _) => x);

        /// <summary>
        /// The semigroup whose operation always returns the last element.
        /// </summary>
        /// <typeparam name="T">The type of the element in the structure.</typeparam>
        public static Semigroup<T> Last<T>() => new Semigroup<T>((_, y) => y);

        /// <summary>
        /// Lifts a <see cref="Semigroup{T}"/> into one which has positive infinity (<see cref="Maybe.Nothing{T}"/>) as a special element.
        /// </summary>
        /// <typeparam name="T">The type of the element in the structure.</typeparam>
        /// <param name="m">The monoid to lift.</param>
        public static Semigroup<Maybe<T>> LiftSemigroupWithInfinity<T>(this Semigroup<T> m)
            => new Semigroup<Maybe<T>>((x, y) => m.Op.LiftA()(x, y).ToMaybe());
    }
}

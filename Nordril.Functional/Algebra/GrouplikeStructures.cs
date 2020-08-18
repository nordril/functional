using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nordril.Functional.Algebra
{
    #region Grouplike
    /// <summary>
    /// An algebraic structure supporting a binary operation with no further guaranteed properties.
    /// </summary>
    /// <typeparam name="T">The type of the carrier set.</typeparam>
    public interface IMagma<T>
    {
        /// <summary>
        /// The binary operation.
        /// This operation should not change either operand.
        /// </summary>
        /// <param name="x">The first operand.</param>
        /// <param name="y">The second operand.</param>
        T Op(T x, T y);
    }

    #endregion

    #region Single properties

    /// <summary>
    /// A magma whose binary operation is associative.
    /// The binary operation must fulfill the following for all X and Y:
    /// <code>
    ///     m.Op(m.Op(X,Y),Z) == m.Op(X, m.Op(Y,Z)) (associativity)
    /// </code>
    /// </summary>
    /// <typeparam name="T">The type of the carrier set.</typeparam>
    public interface IAssociative<T> : IMagma<T>
    {
    }

    /// <summary>
    /// A magma whose binary operation is commutative.
    /// The binary operation must fulfill the following for all X and Y:
    /// <code>
    ///     m.(X, Y) == m.Op(Y, X) (commutativity)
    /// </code>
    /// </summary>
    /// <typeparam name="T">The type of the carrier set.</typeparam>
    public interface ICommutative<T> : IMagma<T>
    {
    }

    /// <summary>
    /// A magma whose binary operation has a neutral element.
    /// The binary operation must fulfill the following for all X and Y:
    /// <code>
    ///     m.Op(m.Neutral, X) == Y (left-neutrality)
    ///     m.Op(X, m.Neutral) == X (right-neutrality)
    /// </code>
    /// </summary>
    /// <typeparam name="T">The type of the carrier set.</typeparam>
    public interface INeutralElement<T> : IMagma<T>
    {
        /// <summary>
        /// Returns the neutral element.
        /// </summary>
        T Neutral { get; }
    }

    /// <summary>
    /// A monoid which supports a unary inversion operator with respect to the associative binary operation.
    /// </summary>
    /// <typeparam name="T">The type of the carrier set.</typeparam>
    public interface IInverse<T> : IMagma<T>
    {
        /// <summary>
        /// Returns the inverse of the object. The inverse must fulfill the following for all X:
        /// <code>
        ///     m.Op(X.Inverse, X) == m.Neutral (left-inverse)
        ///     m.Op(X, X.Inverse) == m.Neutral (right-inverse)
        /// </code>
        /// </summary>
        T Inverse(T x);
    }

    /// <summary>
    /// A magma where performing the binary operation <see cref="IMagma{T}.Op(T, T)"/> on an element <c>X</c> with itself results in that initial element, i.e.
    /// <code>
    ///     m.Op(X, X) == X (idempotence)
    /// </code>
    /// </summary>
    /// <typeparam name="T">The type of the carrier set.</typeparam>
    public interface IIdempotent<T> : IMagma<T>
    {
    }
    #endregion

    #region Named structures
    /// <summary>
    /// A semigroup whose binary operation is associative.
    /// </summary>
    /// <typeparam name="T">The type of the carrier set.</typeparam>
    public interface ISemigroup<T> : IMagma<T>, IAssociative<T>
    {
    }

    /// <summary>
    /// A semigroup which has a neutral element with respect to the binary operation.
    /// </summary>
    /// <typeparam name="T">The type of the carrier set.</typeparam>
    public interface IMonoid<T> : ISemigroup<T>, INeutralElement<T>
    {
    }

    /// <summary>
    /// A monoid which supports a unary inversion operator with respect to the associative binary operation.
    /// </summary>
    /// <typeparam name="T">The type of the carrier set.</typeparam>
    public interface IGroup<T> : IMonoid<T>, IInverse<T>
    {
    }

    /// <summary>
    /// A monoid which supports a unary inversion operator with respect to the associative binary operation.
    /// </summary>
    /// <typeparam name="T">The type of the carrier set.</typeparam>
    public interface ICommutativeGroup<T> : IGroup<T>, ICommutative<T>
    {
    }

    /// <summary>
    /// A semi-lattice, which is commutative, associative, and idempotent.
    /// </summary>
    /// <typeparam name="T">The type of the carrier set.</typeparam>
    public interface ISemilattice<T>
        : ICommutative<T>
        , IAssociative<T>
        , IIdempotent<T>
    {
    }
    #endregion 

    /// <summary>
    /// Extension methods for algebraic structures.
    /// </summary>
    public static class StructuresExtensions
    {
        /// <summary>
        /// Applies the <see cref="IMagma{T}"/>'s binary operation to this object and a second operand.
        /// </summary>
        /// <typeparam name="T">The type of the carrier set.</typeparam>
        /// <param name="x">The first operand.</param>
        /// <param name="y">The second operand.</param>
        public static T Op<T>(this T x, T y) where T : IMagma<T> => x.Op(x, y);

        /// <summary>
        /// Applies the <see cref="IInverse{T}.Inverse(T)"/> to itself.
        /// </summary>
        /// <typeparam name="T">The type of the carrier set.</typeparam>
        /// <param name="x">The operand.</param>
        public static T Inverse<T>(this T x) where T : IGroup<T> => x.Inverse(x);
    }
}

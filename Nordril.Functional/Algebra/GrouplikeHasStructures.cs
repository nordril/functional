using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nordril.Functional.Algebra
{
    /// <summary>
    /// An algebraic structure supporting a binary operation with no further guaranteed properties.
    /// </summary>
    /// <typeparam name="T">The type of the implementor.</typeparam>
    public interface IHasMagma<T> : IMagma<T> where T : IHasMagma<T>
    {
    }

    #region Single properties
    /// <summary>
    /// A magma whose binary operation is associative.
    /// The binary operation must fulfill the following for all X and Y:
    /// <code>
    ///     X.Op(Y).Op(Z) == X.Op(Y.Op(Z)) (associativity)
    /// </code>
    /// </summary>
    /// <typeparam name="T">The type of the implementor.</typeparam>
    public interface IHasAssociativity<T> : IHasMagma<T>, IAssociative<T>
        where T : IHasAssociativity<T>
    {
    }

    /// <summary>
    /// A magma whose binary operation is commutative.
    /// The binary operation must fulfill the following for all X and Y:
    /// <code>
    ///     X.Op(Y) == Y.Op(X) (commutativity)
    /// </code>
    /// </summary>
    /// <typeparam name="T">The type of the implementor.</typeparam>
    public interface IHasCommutativity<T> : IHasMagma<T>, ICommutative<T>
        where T : IHasCommutativity<T>
    {
    }

    /// <summary>
    /// A magma whose binary operation has a neutral element.
    /// The binary operation must fulfill the following for all X and Y:
    /// <code>
    ///     X.Neutral().Op(Y) == Y (left-neutrality)
    ///     X.Op(Y.Neutral()) == X (right-neutrality)
    /// </code>
    /// </summary>
    /// <typeparam name="T">The type of the implementor.</typeparam>
    public interface IHasNeutralElement<T> : IHasMagma<T>, INeutralElement<T>
        where T : IHasNeutralElement<T>
    {
    }

    #endregion

    #region Named structures
    /// <summary>
    /// A semigroup whose binary operation is associative.
    /// </summary>
    /// <typeparam name="T">The type of the implementor.</typeparam>
    public interface IHasSemigroup<T> : IHasMagma<T>, IHasAssociativity<T>, ISemigroup<T>
        where T : IHasSemigroup<T>
    {
    }

    /// <summary>
    /// A semigroup which has a neutral element with respect to the binary operation.
    /// </summary>
    /// <typeparam name="T">The type of the implementor.</typeparam>
    public interface IHasMonoid<T> : IHasSemigroup<T>, IHasNeutralElement<T>, IMonoid<T>
        where T : IHasMonoid<T>
    {
    }

    /// <summary>
    /// A monoid which supports a unary inversion operator with respect to the associative binary operation.
    /// </summary>
    /// <typeparam name="T">The type of the implementor.</typeparam>
    public interface IHasGroup<T> : IHasMonoid<T>, IGroup<T>
        where T : IHasGroup<T>
    {
    }

    /// <summary>
    /// A monoid which supports a unary inversion operator with respect to the associative binary operation.
    /// </summary>
    /// <typeparam name="T">The type of the implementor.</typeparam>
    public interface IHasCommutativeGroup<T> : ICommutativeGroup<T>
        where T : IHasCommutativeGroup<T>
    {
    }

    /// <summary>
    /// A semi-lattice, which is commutative, associative, and idempotent.
    /// </summary>
    /// <typeparam name="T">The type of the carrier set.</typeparam>
    public interface IHasSemilattice<T> : ISemilattice<T>
        where T : IHasSemilattice<T>
    {
    }
    #endregion 
}

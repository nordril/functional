using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional.Algebra
{
    #region Single properties
    /// <summary>
    /// A reflexive relation, for which the following holds:
    /// <code>
    ///     r.Contains(X,X) == true (reflexivity)
    /// </code>
    /// </summary>
    /// <typeparam name="T">The type of elements contained in the relation.</typeparam>
    public interface IHasReflexiveRelation<T> : IReflexiveRelation<T>
        where T : IHasReflexiveRelation<T>
    {
    }

    /// <summary>
    /// A symmetric relation, for which the following holds:
    /// <code>
    ///     r.Contains(X,Y) == r.Contains(Y,X) (symmetric)
    /// </code>
    /// </summary>
    /// <typeparam name="T">The type of elements contained in the relation.</typeparam>
    public interface IHasSymmetricRelation<T> : ISymmetricRelation<T>
        where T : IHasSymmetricRelation<T>
    {
    }

    /// <summary>
    /// An antisymmetric relation, for which the following holds:
    /// <code>
    ///     if r.Contains(X,Y) and r.Contains(Y,X) then a = b (anti-symmetry)
    /// </code>
    /// </summary>
    /// <typeparam name="T">The type of elements contained in the relation.</typeparam>
    public interface IHasAntisymmetricRelation<T> : IAntisymmetricRelation<T>
        where T : IHasAntisymmetricRelation<T>
    {
    }

    /// <summary>
    /// A transitive relation, for which the following holds:
    /// <code>
    ///     if r.Contains(X,Y) and r.Contains(Y,Z) then r.Contains(X,Z) (transitivity)
    /// </code>
    /// </summary>
    /// <typeparam name="T">The type of elements contained in the relation.</typeparam>
    public interface IHasTransitiveRelation<T> : ITransitiveRelation<T>
        where T : IHasTransitiveRelation<T>
    {
    }

    /// <summary>
    /// For all X and Y, the following holds:
    /// <code>
    ///     r.Contains(X,Y) or r.Contains(Y,X) (connex)
    /// </code>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IHasConnexRelation<T> : IConnexRelation<T>
        where T : IHasConnexRelation<T>
    {
    }
    #endregion

    #region Named structures
    /// <summary>
    /// A partial order, i.e. a reflexive, anti-symmetric, and transitive relationship.
    /// </summary>
    /// <typeparam name="T">The type of elements contained in the relation.</typeparam>
    public interface IHasPartialOrder<T> : IPartialOrder<T>
        where T : IHasPartialOrder<T>
    {
    }

    /// <summary>
    /// A total order, i.e. a partial order which is also total.
    /// A total allows us to compare every pair of elements.
    /// </summary>
    /// <typeparam name="T">The type of elements contained in the relation.</typeparam>
    public interface IHasTotalOrder<T> : ITotalOrder<T>
        where T : IHasTotalOrder<T>
    {
    }
    #endregion
}

using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional.Algebra
{
    /// <summary>
    /// A binary relation that can decide whether it contains a pair of elements.
    /// </summary>
    /// <typeparam name="T1">The type of first elements contained in the relation.</typeparam>
    /// <typeparam name="T2">The type of second elements contained in the relation.</typeparam>
    public interface IBinaryRelation<T1, T2>
    {
        /// <summary>
        /// Returns true iff two elements occur as a pair in the relation.
        /// </summary>
        /// <param name="x">The first element.</param>
        /// <param name="y">The second element.</param>
        bool Contains(T1 x, T2 y);
    }

    /// <summary>
    /// An extensible binary relation that can enumerate the pairs it contains.
    /// Iff a a pair <c>(x,y)</c> occurs in the enumeration, then <c>r.Contains(x,y)</c> is true.
    /// </summary>
    /// <typeparam name="T1">The type of first elements contained in the relation.</typeparam>
    /// <typeparam name="T2">The type of second elements contained in the relation.</typeparam>
    public interface IExtensionalBinaryRelation<T1, T2>
        : IBinaryRelation<T1, T2>
        , IEnumerable<(T1, T2)>
    {
    }

    #region Single properties
    /// <summary>
    /// A reflexive relation, for which the following holds:
    /// <code>
    ///     r.Contains(X,X) == true (reflexivity)
    /// </code>
    /// </summary>
    /// <typeparam name="T">The type of elements contained in the relation.</typeparam>
    public interface IReflexiveRelation<T> : IBinaryRelation<T, T>
    {
    }

    /// <summary>
    /// A symmetric relation, for which the following holds:
    /// <code>
    ///     r.Contains(X,Y) == r.Contains(Y,X) (symmetric)
    /// </code>
    /// </summary>
    /// <typeparam name="T">The type of elements contained in the relation.</typeparam>
    public interface ISymmetricRelation<T> : IBinaryRelation<T, T>
    {
    }

    /// <summary>
    /// An antisymmetric relation, for which the following holds:
    /// <code>
    ///     if r.Contains(X,Y) and r.Contains(Y,X) then a = b (anti-symmetry)
    /// </code>
    /// </summary>
    /// <typeparam name="T">The type of elements contained in the relation.</typeparam>
    public interface IAntisymmetricRelation<T> : IBinaryRelation<T, T>
    {
    }

    /// <summary>
    /// A transitive relation, for which the following holds:
    /// <code>
    ///     if r.Contains(X,Y) and r.Contains(Y,Z) then r.Contains(X,Z) (transitivity)
    /// </code>
    /// </summary>
    /// <typeparam name="T">The type of elements contained in the relation.</typeparam>
    public interface ITransitiveRelation<T> : IBinaryRelation<T, T>
    {
    }

    /// <summary>
    /// A total relation which is equivalent to the Cartesian product of <typeparamref name="T1"/> and <typeparamref name="T2"/>.
    /// <code>
    ///     r.Contains
    /// </code>
    /// </summary>
    /// <typeparam name="T1">The type of first elements contained in the relation.</typeparam>
    /// <typeparam name="T2">The type of second elements contained in the relation.</typeparam>
    public interface ITotalRelation<T1, T2>
        : ISerialRelation<T1, T2>
        , ISurjectiveRelation<T1, T2>
    {
    }

    /// <summary>
    /// An injective relation, for which the following holds:
    /// <code>
    ///     if r.Contains(X,Y) and r.Contains(X,Z), then X = Y (injectivity)
    /// </code>
    /// </summary>
    /// <typeparam name="T1">The type of first elements contained in the relation.</typeparam>
    /// <typeparam name="T2">The type of second elements contained in the relation.</typeparam>
    public interface IInjectiveRelation<T1, T2> : IBinaryRelation<T1, T2>
    {
    }

    /// <summary>
    /// A surjective relation, for which the following holds:
    /// <code>
    ///     for all Y, there exists an X such that r.Contains(X,Y) (surjectivity)
    /// </code>
    /// </summary>
    /// <typeparam name="T1">The type of first elements contained in the relation.</typeparam>
    /// <typeparam name="T2">The type of second elements contained in the relation.</typeparam>
    public interface ISurjectiveRelation<T1, T2> : IBinaryRelation<T1, T2>
    {
    }

    /// <summary>
    /// A functional relation, where, for all X in <typeparamref name="T1"/>
    /// <code>
    ///     if r.Contains(X,Y) and r.Contains(X,Z) then Y == Z (functional)
    /// </code>
    /// </summary>
    /// <typeparam name="T1">The type of first elements contained in the relation.</typeparam>
    /// <typeparam name="T2">The type of second elements contained in the relation.</typeparam>
    public interface IFunctionalRelation<T1, T2> : IBinaryRelation<T1, T2>
    {
        /// <summary>
        /// Returns the <typeparamref name="T2"/> associated with an <typeparamref name="T1"/>, if it exists.
        /// </summary>
        /// <param name="x">The <typeparamref name="T1"/>.</param>
        Maybe<T2> MaybeResult(T1 x);
    }

    /// <summary>
    /// A serial relation, where, for all X in <typeparamref name="T1"/>, there exists an Y in <typeparamref name="T2"/> such that
    /// <code>
    ///     r.Contains(X,Y) (serial)
    /// </code>
    /// </summary>
    /// <typeparam name="T1">The type of first elements contained in the relation.</typeparam>
    /// <typeparam name="T2">The type of second elements contained in the relation.</typeparam>
    public interface ISerialRelation<T1, T2> : IBinaryRelation<T1, T2>
    {
    }

    /// <summary>
    /// For all X and Y, the following holds:
    /// <code>
    ///     r.Contains(X,Y) or r.Contains(Y,X) (connex)
    /// </code>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IConnexRelation<T> : IBinaryRelation<T, T>
    {
    }
    #endregion

    #region Named structures

    /// <summary>
    /// A one-to-one mapping between some elements of <typeparamref name="T1"/> and some elements of <typeparamref name="T2"/>.
    /// </summary>
    /// <typeparam name="T1">The type of first elements contained in the relation.</typeparam>
    /// <typeparam name="T2">The type of second elements contained in the relation.</typeparam>
    public interface IOneToOneRelation<T1, T2>
        : IInjectiveRelation<T1, T2>
        , IFunctionalRelation<T1, T2>
    {
        /// <summary>
        /// Gets the unique left-key associated with a given right-key, if it exists.
        /// </summary>
        /// <param name="x">The right-key.</param>
        Maybe<T1> GetMaybeLeft(T2 x);

        /// <summary>
        /// Gets the unique right-key associated with a given right-key, if it exists.
        /// </summary>
        /// <param name="x">The right-key.</param>
        Maybe<T2> GetMaybeRight(T1 x);
    }

    /// <summary>
    /// A bijective-relation, which is a one-to-one mapping from all elements of <typeparamref name="T1"/> to all elements of <typeparamref name="T2"/>.
    /// </summary>
    /// <typeparam name="T1">The type of first elements contained in the relation.</typeparam>
    /// <typeparam name="T2">The type of second elements contained in the relation.</typeparam>
    public interface IBijectiveRelation<T1, T2>
    : IOneToOneRelation<T1, T2>
    , ISurjectiveRelation<T1, T2>
    {
        /// <summary>
        /// Gets the unique left-key associated with a given right-key.
        /// </summary>
        /// <param name="x">The right-key.</param>
        T1 GetLeft(T2 x);

        /// <summary>
        /// Gets the unique right-key associated with a given left-key.
        /// </summary>
        /// <param name="x">The right-key.</param>
        T2 GetRight(T1 x);
    }

    /// <summary>
    /// A relation which is equivalent to a total function.
    /// </summary>
    /// <typeparam name="T1">The type of the inputs.</typeparam>
    /// <typeparam name="T2">The type of the outputs.</typeparam>
    public interface IFunctionRelation<T1, T2>
        : ISerialRelation<T1, T2>
        , IFunctionalRelation<T1, T2>

    {
        /// <summary>
        /// Returns the output associated with the input <paramref name="x"/>.
        /// </summary>
        /// <param name="x">The input.</param>
        T2 Result(T1 x);
    }

    /// <summary>
    /// A partial order, i.e. a reflexive, anti-symmetric, and transitive relationship.
    /// </summary>
    /// <typeparam name="T">The type of elements contained in the relation.</typeparam>
    public interface IPartialOrder<T>
        : IReflexiveRelation<T>
        , IAntisymmetricRelation<T>
        , ITransitiveRelation<T>
    {
    }

    /// <summary>
    /// A total order, i.e. a partial order which is also total.
    /// A total allows us to compare every pair of elements.
    /// </summary>
    /// <typeparam name="T">The type of elements contained in the relation.</typeparam>
    public interface ITotalOrder<T>
        : IPartialOrder<T>
        , IConnexRelation<T>
    {
        /// <summary>
        /// Compares two elements and returns a number smaller than 0 if <c>x &lt; y</c>, 0 if <c>x == y</c>, and a number greater than 0 if <c>x &gt; y</c>.
        /// </summary>
        /// <param name="x">The first element to compare.</param>
        /// <param name="y">The second element to compare.</param>
        short Compare(T x, T y);
    }
    #endregion
}

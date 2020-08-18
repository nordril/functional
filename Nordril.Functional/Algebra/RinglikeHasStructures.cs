using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional.Algebra
{
    #region Single properties
    /// <summary>
    /// Ringlike structures where the first operation left-distributes over the second, i.e.
    /// <code>
    /// X * (Y + Z) == (X * Y) + (X * Z)
    /// </code>
    /// </summary>
    /// <typeparam name="T">The type of the carrier set.</typeparam>
    /// <typeparam name="TFirst">The type of the first grouplike operation.</typeparam>
    /// <typeparam name="TSecond">The type of the second groupike operation.</typeparam>
    public interface IHasLeftDistributive<T, out TFirst, out TSecond>
        : ILeftDistributive<T, TFirst, TSecond>
        where T : IHasLeftDistributive<T, TFirst, TSecond>
        where TFirst : IMagma<T>
        where TSecond : IMagma<T>
    {

    }

    /// <summary>
    /// Ringlike structures where the first operation right-distributes over the second, i.e.
    /// <code>
    /// (Y + Z) * X == (Y * X) + (Z * X)
    /// </code>
    /// </summary>
    /// <typeparam name="T">The type of the carrier set.</typeparam>
    /// <typeparam name="TFirst">The type of the first grouplike operation.</typeparam>
    /// <typeparam name="TSecond">The type of the second groupike operation.</typeparam>
    public interface IHasRightDistributive<T, out TFirst, out TSecond>
        : IRightDistributive<T, TFirst, TSecond>
        where T : IHasRightDistributive<T, TFirst, TSecond>
        where TFirst : IMagma<T>
        where TSecond : IMagma<T>
    {

    }

    /// <summary>
    /// Ringlike structures where which satisfy the Jacobi-identity, e.g.
    /// <code>
    /// X * (Y * Z) == (X * Y) * Z + Y * (X * Z)
    /// </code>
    /// </summary>
    /// <typeparam name="T">The type of the carrier set.</typeparam>
    /// <typeparam name="TFirst">The type of the first grouplike operation.</typeparam>
    /// <typeparam name="TSecond">The type of the second groupike operation.</typeparam>
    public interface IHasJacobi<T, out TFirst, out TSecond>
        : IJacobi<T, TFirst, TSecond>
        where T : IHasJacobi<T, TFirst, TSecond>
        where TFirst : IMagma<T>
        where TSecond : IMagma<T>
    {

    }

    /// <summary>
    /// Ringlike structures where the neutral element of the first operation annihiliates another element, if applied with the second operation, i.e.
    /// <code>
    /// 0 * X = X * 0 = 0
    /// </code>
    /// </summary>
    /// <typeparam name="T">The type of the carrier set.</typeparam>
    /// <typeparam name="TFirst">The type of the first grouplike operation.</typeparam>
    /// <typeparam name="TSecond">The type of the second groupike operation.</typeparam>
    public interface IHasAnnihilation<T, out TFirst, out TSecond>
        : IAnnihilation<T, TFirst, TSecond>
        where T : IHasAnnihilation<T, TFirst, TSecond>
        where TFirst : IMagma<T>, INeutralElement<T>
        where TSecond : IMagma<T>
    {

    }
    #endregion

    #region Named structures
    /// <summary>
    /// General algebraic structures with two operations. The two operations are grouplike and there may be addition relationships between them,
    /// e.g. the second operation may distribute over the first.
    /// </summary>
    /// <typeparam name="T">The type of the carrier set.</typeparam>
    /// <typeparam name="TFirst">The type of the first grouplike operation.</typeparam>
    /// <typeparam name="TSecond">The type of the second groupike operation.</typeparam>
    public interface IHasRinglike<T, out TFirst, out TSecond>
        : IRinglike<T, TFirst, TSecond>
        where T : IHasRinglike<T, TFirst, TSecond>
        where TFirst : IMagma<T>
        where TSecond : IMagma<T>
    {
    }

    /// <summary>
    /// A semiring; a structure with two operations, where the first operation is a commutative monoid and the second operation is a monoid.
    /// In addition, the second operation, left- and right-distributes over the first and the neutral elemenent of the first operation is annihiliating in the second operation.
    /// </summary>
    /// <typeparam name="T">The type of the carrier set.</typeparam>
    /// <typeparam name="TFirst">The type of the first grouplike operation.</typeparam>
    /// <typeparam name="TSecond">The type of the second groupike operation.</typeparam>
    public interface IHasSemiring<T, out TFirst, out TSecond>
        : ISemiring<T, TFirst, TSecond>
        where T : IHasSemiring<T, TFirst, TSecond>
        where TFirst : ICommutative<T>, IMonoid<T>
        where TSecond : IMonoid<T>
    {
    }

    /// <summary>
    /// A near-ring; a structure with two operations, where the first operation is a group and the second operation is a semigroup.
    /// In addition, the second operation, right-distributes over the first and the neutral elemenent of the first operation.
    /// </summary>
    /// <typeparam name="T">The type of the carrier set.</typeparam>
    /// <typeparam name="TFirst">The type of the first grouplike operation.</typeparam>
    /// <typeparam name="TSecond">The type of the second groupike operation.</typeparam>
    public interface IHasNearRing<T, out TFirst, out TSecond>
        : INearRing<T, TFirst, TSecond>
        where T : IHasNearRing<T, TFirst, TSecond>
        where TFirst : IGroup<T>
        where TSecond : ISemigroup<T>
    {
    }

    /// <summary>
    /// A ring. Like a semiring, but the first operation forms a commutative group, i.e. subtraction is also defined.
    /// </summary>
    /// <typeparam name="T">The type of the carrier set.</typeparam>
    /// <typeparam name="TFirst">The type of the first grouplike operation.</typeparam>
    /// <typeparam name="TSecond">The type of the second groupike operation.</typeparam>
    public interface IHasRing<T, out TFirst, out TSecond>
        : IRing<T, TFirst, TSecond>
        where T : IHasRing<T, TFirst, TSecond>
        where TFirst : IGroup<T>, ICommutative<T>
        where TSecond : IMonoid<T>
    {
    }

    /// <summary>
    /// A Boolean ring; a ring where the second operation is idempotent.
    /// </summary>
    /// <typeparam name="T">The type of the carrier set.</typeparam>
    /// <typeparam name="TFirst">The type of the first grouplike operation.</typeparam>
    /// <typeparam name="TSecond">The type of the second groupike operation.</typeparam>
    public interface IHasBooleanRing<T, out TFirst, out TSecond>
        : IBooleanRing<T, TFirst, TSecond>
        where T : IHasBooleanRing<T, TFirst, TSecond>
        where TFirst : IGroup<T>, ICommutative<T>
        where TSecond : IMonoid<T>, IIdempotent<T>
    {
    }

    /// <summary>
    /// A ring where the second operation is commutative.
    /// </summary>
    /// <typeparam name="T">The type of the carrier set.</typeparam>
    /// <typeparam name="TFirst">The type of the first grouplike operation.</typeparam>
    /// <typeparam name="TSecond">The type of the second groupike operation.</typeparam>
    public interface IHasCommutativeRing<T, out TFirst, out TSecond>
        : ICommutativeRing<T, TFirst, TSecond>
        where T : IHasCommutativeRing<T, TFirst, TSecond>
        where TFirst : ICommutativeGroup<T>
        where TSecond : IMonoid<T>, ICommutative<T>
    {

    }

    /// <summary>
    /// A domain in which the product of two non-zero elements is also non-zero.
    /// </summary>
    /// <typeparam name="T">The type of the carrier set.</typeparam>
    /// <typeparam name="TFirst">The type of the first grouplike operation.</typeparam>
    /// <typeparam name="TSecond">The type of the second groupike operation.</typeparam>
    public interface IHasIntegralDomain<T, out TFirst, out TSecond>
        : IIntegralDomain<T, TFirst, TSecond>
        where T : IHasIntegralDomain<T, TFirst, TSecond>
        where TFirst : ICommutativeGroup<T>
        where TSecond : IMonoid<T>, ICommutative<T>
    {
    }

    /// <summary>
    /// A domain in which every two elements have a greatest common divisor or, equivalently, a least common multiple, through an efficient algorithm for finding them might not exist.
    /// See <see cref="IEuclideanDomain{T, TFirst, TSecond}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the carrier set.</typeparam>
    /// <typeparam name="TFirst">The type of the first grouplike operation.</typeparam>
    /// <typeparam name="TSecond">The type of the second groupike operation.</typeparam>
    public interface IHasGcdDomain<T, out TFirst, out TSecond>
        : IGcdDomain<T, TFirst, TSecond>
        where T : IHasGcdDomain<T, TFirst, TSecond>
        where TFirst : ICommutativeGroup<T>
        where TSecond : IMonoid<T>, ICommutative<T>
    {

    }

    /// <summary>
    /// A domain in which every element can be uniquely factored into a product of prime elements.
    /// </summary>
    /// <typeparam name="T">The type of the carrier set.</typeparam>
    /// <typeparam name="TFirst">The type of the first grouplike operation.</typeparam>
    /// <typeparam name="TSecond">The type of the second groupike operation.</typeparam>
    public interface IHasUniqueFactorizationDomain<T, out TFirst, out TSecond>
        : IUniqueFactorizationDomain<T, TFirst, TSecond>
        where T : IHasUniqueFactorizationDomain<T, TFirst, TSecond>
        where TFirst : ICommutativeGroup<T>
        where TSecond : IMonoid<T>, ICommutative<T>
    {
    }

    /// <summary>
    /// A domain which has an Euclidean division-function (every two elements <c>X</c> and <c>Y</c> can be divided, resulting in <c>Divide(X,Y) = Q * Y + R</c>.
    /// </summary>
    /// <typeparam name="T">The type of the carrier set.</typeparam>
    /// <typeparam name="TFirst">The type of the first grouplike operation.</typeparam>
    /// <typeparam name="TSecond">The type of the second groupike operation.</typeparam>
    public interface IHasEuclideanDomain<T, out TFirst, out TSecond>
        : IEuclideanDomain<T, TFirst, TSecond>
        where T : IHasEuclideanDomain<T, TFirst, TSecond>
        where TFirst : ICommutativeGroup<T>
        where TSecond : IMonoid<T>, ICommutative<T>
    {
    }

    /// <summary>
    /// A domain in which both operations form commutative groups, i.e. addition, multiplication, subtraction, and division (except for zero-divisors) are defined.
    /// </summary>
    /// <typeparam name="T">The type of the carrier set.</typeparam>
    /// <typeparam name="TFirst">The type of the first grouplike operation.</typeparam>
    /// <typeparam name="TSecond">The type of the second groupike operation.</typeparam>
    public interface IHasField<T, TFirst, TSecond>
        : IField<T, TFirst, TSecond>
        where T : IHasField<T, TFirst, TSecond>
        where TFirst : ICommutativeGroup<T>
        where TSecond : ICommutativeGroup<T>
    {
    }

    /// <summary>
    /// A field with finitely many elements.
    /// </summary>
    /// <typeparam name="T">The type of the carrier set.</typeparam>
    /// <typeparam name="TFirst">The type of the first grouplike operation.</typeparam>
    /// <typeparam name="TSecond">The type of the second groupike operation.</typeparam>
    public interface IHasFiniteField<T, TFirst, TSecond>
        : IFiniteField<T, TFirst, TSecond>
        where T : IHasFiniteField<T, TFirst, TSecond>
        where TFirst : ICommutativeGroup<T>
        where TSecond : ICommutativeGroup<T>
    {
    }

    /// <summary>
    /// A lattice, having a meet- and join-operation, which are commutative, associative, idempotent, and connected via the absorption-law.
    /// </summary>
    /// <typeparam name="T">The type of the carrier set.</typeparam>
    /// <typeparam name="TFirst">The type of the first grouplike operation.</typeparam>
    /// <typeparam name="TSecond">The type of the second groupike operation.</typeparam>
    public interface IHasLattice<T, out TFirst, out TSecond>
        : ILattice<T, TFirst, TSecond>
        where T : IHasLattice<T, TFirst, TSecond>
        where TFirst : ISemilattice<T>
        where TSecond : ISemilattice<T>
    {

    }

    /// <summary>
    /// A bounded lattice with a unique minimal and maximal element.
    /// </summary>
    /// <typeparam name="T">The type of the carrier set.</typeparam>
    /// <typeparam name="TFirst">The type of the first grouplike operation.</typeparam>
    /// <typeparam name="TSecond">The type of the second groupike operation.</typeparam>
    public interface IHasBoundedLattice<T, out TFirst, out TSecond>
        : IBoundedLattice<T, TFirst, TSecond>
        where T : IHasBoundedLattice<T, TFirst, TSecond>
        where TFirst : ISemilattice<T>, INeutralElement<T>
        where TSecond : ISemilattice<T>, INeutralElement<T>
    {

    }

    /// <summary>
    /// A distributed latter where the join and meet distribute over each other.
    /// </summary>
    /// <remarks>
    /// We cannot specify that the first operation should also distribute over the second
    /// (in addition to the second distributing over the first) via inheriting from <see cref="ILeftDistributive{T, TFirst, TSecond}"/> and <see cref="IRightDistributive{T, TFirst, TSecond}"/> twice with <typeparamref name="TFirst"/> and <typeparamref name="TSecond"/>, since the C# forbids type parameter unification (<typeparamref name="TFirst"/> in the first instance of <see cref="ILeftDistributive{T, TFirst, TSecond}"/> may be instantiated to the same type as <typeparamref name="TSecond"/> in the second instance of <see cref="ILeftDistributive{T, TFirst, TSecond}"/>, etc.). One could work around this, but the workaround was considered clunky and inconvenient to users, so we consciously omit trying to model both distributive laws in favor of just one.
    /// </remarks>
    /// <typeparam name="T">The type of the carrier set.</typeparam>
    /// <typeparam name="TFirst">The type of the first grouplike operation.</typeparam>
    /// <typeparam name="TSecond">The type of the second groupike operation.</typeparam>
    public interface IHasDistributedLattice<T, out TFirst, out TSecond>
        : IDistributedLattice<T, TFirst, TSecond>
        where T : IHasDistributedLattice<T, TFirst, TSecond>
        where TFirst : ISemilattice<T>
        where TSecond : ISemilattice<T>
    {

    }
    #endregion
}

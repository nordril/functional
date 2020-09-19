using Nordril.Functional.Data;
using System.Collections.Generic;

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
    public interface ILeftDistributive<T, out TFirst, out TSecond>
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
    public interface IRightDistributive<T, out TFirst, out TSecond>
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
    public interface IJacobi<T, out TFirst, out TSecond>
        where TFirst : IMagma<T>
        where TSecond : IMagma<T>
    {

    }

    /// <summary>
    /// Ringlike structures where the neutral element of the first operation annihiliates another element, if applied with the second operation, i.e.
    /// <code>
    /// 0 * X = X * 0 == 0
    /// </code>
    /// </summary>
    /// <typeparam name="T">The type of the carrier set.</typeparam>
    /// <typeparam name="TFirst">The type of the first grouplike operation.</typeparam>
    /// <typeparam name="TSecond">The type of the second groupike operation.</typeparam>
    public interface IAnnihilation<T, out TFirst, out TSecond>
        where TFirst : IMagma<T>, INeutralElement<T>
        where TSecond : IMagma<T>
    {

    }

    /// <summary>
    /// Ringlike structures which obey the absorption law:
    /// <code>
    ///     X * (X + Y) == X (absorption)
    /// </code>
    /// </summary>
    /// <typeparam name="T">The type of the carrier set.</typeparam>
    /// <typeparam name="TFirst">The type of the first grouplike operation.</typeparam>
    /// <typeparam name="TSecond">The type of the second groupike operation.</typeparam>
    public interface IAbsorbption<T, out TFirst, out TSecond>
        where TFirst : IMagma<T>
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
    public interface IRinglike<T, out TFirst, out TSecond>
        : IContainsFirst<TFirst>, IContainsSecond<TSecond>
        where TFirst : IMagma<T>
        where TSecond : IMagma<T>
    {
    }

    /// <summary>
    /// A covariant wrapper around a ringlike structure.
    /// </summary>
    /// <typeparam name="TRinglike">The type of the ringlike structure.</typeparam>
    /// <typeparam name="TFirst">The type of the first grouplike operation.</typeparam>
    /// <typeparam name="TSecond">The type of the second groupike operation.</typeparam>
    public interface IRinglikeWrapper<TRinglike, TFirst, TSecond>
        : IContainsRinglike<TRinglike>
        , IContainsFirst<TFirst>
        , IContainsSecond<TSecond>
    {
    }

    /// <summary>
    /// A semiring; a structure with two operations, where the first operation is a commutative monoid and the second operation is a monoid.
    /// In addition, the second operation, left- and right-distributes over the first and the neutral elemenent of the first operation is annihiliating in the second operation.
    /// </summary>
    /// <typeparam name="T">The type of the carrier set.</typeparam>
    /// <typeparam name="TFirst">The type of the first grouplike operation.</typeparam>
    /// <typeparam name="TSecond">The type of the second groupike operation.</typeparam>
    public interface ISemiring<T, out TFirst, out TSecond>
        : IRinglike<T, TFirst, TSecond>
        , ILeftDistributive<T, TFirst, TSecond>
        , IRightDistributive<T, TFirst, TSecond>
        , IAnnihilation<T, TFirst, TSecond>
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
    public interface INearRing<T, out TFirst, out TSecond>
        : IRinglike<T, TFirst, TSecond>
        , IRightDistributive<T, TFirst, TSecond>
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
    public interface IRing<T, out TFirst, out TSecond>
        : ISemiring<T, TFirst, TSecond>
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
    public interface IBooleanRing<T, out TFirst, out TSecond>
        : IRing<T, TFirst, TSecond>
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
    public interface ICommutativeRing<T, out TFirst, out TSecond> : IRing<T, TFirst, TSecond>
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
    public interface IIntegralDomain<T, out TFirst, out TSecond> : ICommutativeRing<T, TFirst, TSecond>
        where TFirst : ICommutativeGroup<T>
        where TSecond : IMonoid<T>, ICommutative<T>
    {
        /// <summary>
        /// Returns whether an element <c>X</c> is the neutral element of the first operation of the structure, i.e. whether it is "zero". This is a special case of the general equatability of two elements.
        /// </summary>
        /// <param name="x">The element to check.</param>
        bool IsZero(T x);
    }

    /// <summary>
    /// A domain in which every two elements have a greatest common divisor or, equivalently, a least common multiple, through an efficient algorithm for finding them might not exist.
    /// See <see cref="IEuclideanDomain{T, TFirst, TSecond}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the carrier set.</typeparam>
    /// <typeparam name="TFirst">The type of the first grouplike operation.</typeparam>
    /// <typeparam name="TSecond">The type of the second groupike operation.</typeparam>
    public interface IGcdDomain<T, out TFirst, out TSecond>
        : IIntegralDomain<T, TFirst, TSecond>
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
    public interface IUniqueFactorizationDomain<T, out TFirst, out TSecond>
        : IGcdDomain<T, TFirst, TSecond>
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
    public interface IEuclideanDomain<T, out TFirst, out TSecond>
        : IUniqueFactorizationDomain<T, TFirst, TSecond>
        where TFirst : ICommutativeGroup<T>
        where TSecond : IMonoid<T>, ICommutative<T>
    {
        /// <summary>
        /// Performs Euclidean division on two elements <paramref name="x"/> and <paramref name="y"/> and returns a quotient <c>q</c> and a remainder <c>r</c> such that the following holds:
        /// <code>
        ///     var (q, r) = r.Divide(x,y);
        ///     x = y * q + r
        /// </code>
        /// if <c>r.IsZero(y)</c> is false.
        /// </summary>
        /// <param name="x">The divident.</param>
        /// <param name="y">The divisor.</param>
        /// <example>
        /// In the field of integers
        /// <code>
        ///     r.Divide(138, 16) = (8, 10)
        /// </code>
        /// </example>
        (T quotient, T remainder) EuclideanDivide(T x, T y);
    }

    /// <summary>
    /// A domain in which both operations form commutative groups, i.e. addition, multiplication, subtraction, and division (except for zero-divisors) are defined.
    /// </summary>
    /// <typeparam name="T">The type of the carrier set.</typeparam>
    /// <typeparam name="TFirst">The type of the first grouplike operation.</typeparam>
    /// <typeparam name="TSecond">The type of the second groupike operation.</typeparam>
    public interface IField<T, out TFirst, out TSecond> : IEuclideanDomain<T, TFirst, TSecond>
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
    public interface IFiniteField<T, out TFirst, out TSecond> : IField<T, TFirst, TSecond>
        where TFirst : ICommutativeGroup<T>
        where TSecond : ICommutativeGroup<T>
    {
        /// <summary>
        /// Enumerates the finitely many elements of the field.
        /// </summary>
        IEnumerable<T> Elements { get; }
    }

    /// <summary>
    /// A lattice, having a meet- and join-operation, which are commutative, associative, idempotent, and connected via the absorption-law.
    /// </summary>
    /// <typeparam name="T">The type of the carrier set.</typeparam>
    /// <typeparam name="TFirst">The type of the first grouplike operation.</typeparam>
    /// <typeparam name="TSecond">The type of the second groupike operation.</typeparam>
    public interface ILattice<T, out TFirst, out TSecond>
        : IRinglike<T, TFirst, TSecond>
        , IAbsorbption<T, TFirst, TSecond>
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
    public interface IBoundedLattice<T, out TFirst, out TSecond>
        : ILattice<T, TFirst, TSecond>
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
    public interface IDistributedLattice<T, out TFirst, out TSecond>
        : ILattice<T, TFirst, TSecond>
        , ILeftDistributive<T, TFirst, TSecond>
        , IRightDistributive<T, TFirst, TSecond>
        where TFirst : ISemilattice<T>
        where TSecond : ISemilattice<T>
    {

    }
    #endregion
}

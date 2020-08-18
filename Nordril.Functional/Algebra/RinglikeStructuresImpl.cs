using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional.Algebra
{
    #region Ringlike
    /// <summary>
    /// A value-level ringlike structure.
    /// </summary>
    /// <typeparam name="T">The type of the carrier set.</typeparam>
    /// <typeparam name="TFirst">The type of the first grouplike operation.</typeparam>
    /// <typeparam name="TSecond">The type of the second groupike operation.</typeparam>
    public struct Ringlike<T, TFirst, TSecond> : IRinglike<T, TFirst, TSecond>
        where TFirst : IMagma<T>
        where TSecond : IMagma<T>
    {
        /// <summary>
        /// Creates a new ringlike structure.
        /// </summary>
        /// <param name="first">The first grouplike structure.</param>
        /// <param name="second">The second grouplike structure.</param>
        public Ringlike(TFirst first, TSecond second)
        {
            FirstGrouplike = first;
            SecondGrouplike = second;
        }

        /// <inheritdoc />
        public TFirst FirstGrouplike { get; }

        /// <inheritdoc />
        public TSecond SecondGrouplike { get; }
    }

    /// <summary>
    /// Extension methods for <see cref="IRinglike{T, TFirst, TSecond}"/>s.
    /// </summary>
    public static class Ringlike
    {
        /// <summary>
        /// Performs the operation of the first grouplike structure, commonly called "addition".
        /// </summary>
        /// <typeparam name="T">The type of the carrier set.</typeparam>
        /// <typeparam name="TFirst">The type of the first grouplike operation.</typeparam>
        /// <typeparam name="TSecond">The type of the second groupike operation.</typeparam>
        /// <param name="r">The ringlike structure.</param>
        /// <param name="x">The first element.</param>
        /// <param name="y">The second element.</param>
        public static T Plus<T, TFirst, TSecond>(this IRinglike<T, TFirst, TSecond> r, T x, T y)
            where TFirst : IMagma<T>
            where TSecond : IMagma<T>
            => r.FirstGrouplike.Op(x, y);

        /// <summary>
        /// Performs the operation of the second grouplike structure, commonly called "multiplication".
        /// </summary>
        /// <typeparam name="T">The type of the carrier set.</typeparam>
        /// <typeparam name="TFirst">The type of the first grouplike operation.</typeparam>
        /// <typeparam name="TSecond">The type of the second groupike operation.</typeparam>
        /// <param name="r">The ringlike structure.</param>
        /// <param name="x">The first element.</param>
        /// <param name="y">The second element.</param>
        public static T Mult<T, TFirst, TSecond>(this IRinglike<T, TFirst, TSecond> r, T x, T y)
            where TFirst : IMagma<T>
            where TSecond : IMagma<T>
            => r.SecondGrouplike.Op(x, y);

        /// <summary>
        /// Returns the neutral element of the first grouplike structure, commonly called "zero".
        /// </summary>
        /// <typeparam name="T">The type of the carrier set.</typeparam>
        /// <typeparam name="TFirst">The type of the first grouplike operation.</typeparam>
        /// <typeparam name="TSecond">The type of the second groupike operation.</typeparam>
        /// <param name="r">The ringlike structure.</param>
        public static T Zero<T, TFirst, TSecond>(this IRinglike<T, TFirst, TSecond> r)
            where TFirst : INeutralElement<T>
            where TSecond : IMagma<T>
            => r.FirstGrouplike.Neutral;

        /// <summary>
        /// Returns the neutral element of the second grouplike structure, commonly called "zero".
        /// </summary>
        /// <typeparam name="T">The type of the carrier set.</typeparam>
        /// <typeparam name="TFirst">The type of the first grouplike operation.</typeparam>
        /// <typeparam name="TSecond">The type of the second groupike operation.</typeparam>
        /// <param name="r">The ringlike structure.</param>
        public static T One<T, TFirst, TSecond>(this IRinglike<T, TFirst, TSecond> r)
            where TFirst : IMagma<T>
            where TSecond : INeutralElement<T>
            => r.SecondGrouplike.Neutral;
    }
    #endregion

    #region Near-ring
    /// <summary>
    /// A value-level near-ring.
    /// </summary>
    /// <typeparam name="T">The type of the carrier set.</typeparam>
    /// <typeparam name="TFirst">The type of the first grouplike operation.</typeparam>
    /// <typeparam name="TSecond">The type of the second groupike operation.</typeparam>
    public struct NearRing<T, TFirst, TSecond> : INearRing<T, TFirst, TSecond>
        where TFirst : IGroup<T>
        where TSecond : ISemigroup<T>
    {
        /// <summary>
        /// Creates a new near-ring.
        /// </summary>
        /// <param name="first">The first grouplike structure.</param>
        /// <param name="second">The second grouplike structure.</param>
        public NearRing(TFirst first, TSecond second)
        {
            FirstGrouplike = first;
            SecondGrouplike = second;
        }

        /// <inheritdoc />
        public TFirst FirstGrouplike { get; }

        /// <inheritdoc />
        public TSecond SecondGrouplike { get; }
    }
    #endregion

    #region Semigring
    /// <summary>
    /// A value-level semiring.
    /// </summary>
    /// <typeparam name="T">The type of the carrier set.</typeparam>
    /// <typeparam name="TFirst">The type of the first grouplike operation.</typeparam>
    /// <typeparam name="TSecond">The type of the second groupike operation.</typeparam>
    public struct Semiring<T, TFirst, TSecond> : ISemiring<T, TFirst, TSecond>
        where TFirst : IMonoid<T>, ICommutative<T>
        where TSecond : IMonoid<T>
    {
        /// <summary>
        /// Creates a new semiring.
        /// </summary>
        /// <param name="first">The first grouplike structure.</param>
        /// <param name="second">The second grouplike structure.</param>
        public Semiring(TFirst first, TSecond second)
        {
            FirstGrouplike = first;
            SecondGrouplike = second;
        }

        /// <inheritdoc />
        public TFirst FirstGrouplike { get; }

        /// <inheritdoc />
        public TSecond SecondGrouplike { get; }
    }
    #endregion

    #region Ring
    /// <summary>
    /// A value-level ring.
    /// </summary>
    /// <typeparam name="T">The type of the carrier set.</typeparam>
    /// <typeparam name="TFirst">The type of the first grouplike operation.</typeparam>
    /// <typeparam name="TSecond">The type of the second groupike operation.</typeparam>
    public struct Ring<T, TFirst, TSecond> : IRing<T, TFirst, TSecond>
        where TFirst : IGroup<T>, ICommutative<T>
        where TSecond : IMonoid<T>
    {
        /// <summary>
        /// Creates a new ring.
        /// </summary>
        /// <param name="first">The first grouplike structure.</param>
        /// <param name="second">The second grouplike structure.</param>
        public Ring(TFirst first, TSecond second)
        {
            FirstGrouplike = first;
            SecondGrouplike = second;
        }

        /// <inheritdoc />
        public TFirst FirstGrouplike { get; }

        /// <inheritdoc />
        public TSecond SecondGrouplike { get; }
    }
    #endregion

    #region Boolean ring
    /// <summary>
    /// Extension methods for boolean rings.
    /// </summary>
    public static class BooleanRing
    {
        /// <summary>
        /// The boolean ring (B,XOR,&amp;&amp;,false,true).
        /// </summary>
        public static BoolXorRing Bool = new BoolXorRing();

        /// <summary>
        /// The boolean ring (B,XOR,&amp;&amp;,false,true).
        /// </summary>
        public class BoolXorRing : IBooleanRing<bool, Group.BoolXorGroup, Monoid.BoolAndMonoid>
        {
            /// <inheritdoc />
            public Group.BoolXorGroup FirstGrouplike => Group.BoolXor;

            /// <inheritdoc />
            public Monoid.BoolAndMonoid SecondGrouplike => Monoid.BoolAnd;
        }
    }
    #endregion

    #region Euclidean domain
    /// <summary>
    /// Extension methods for <see cref="IEuclideanDomain{T, TFirst, TSecond}"/>s.
    /// </summary>
    public static class EuclideanDomain
    {
        /// <summary>
        /// Gets the Euclidean domain of integers.
        /// </summary>
        public static readonly IntegerEuclideanDomain Integers = new IntegerEuclideanDomain();

        /// <summary>
        /// The Euclidean domain of integers.
        /// </summary>
        public class IntegerEuclideanDomain : IEuclideanDomain<int, Group.IntAddGroup, Monoid.IntMultMonoid>
        {
            /// <inheritdoc />
            public Group.IntAddGroup FirstGrouplike => Group.IntAdd;

            /// <inheritdoc />
            public Monoid.IntMultMonoid SecondGrouplike => Monoid.IntMult;

            /// <inheritdoc />
            public (int quotient, int remainder) Divide(int x, int y)
            {
                var q = x / y;
                var r = x - q * y;
                return (q, r);
            }

            /// <inheritdoc />
            public bool IsZero(int x) => x == 0;
        }

        /// <summary>
        /// Performs the modulus-operation on two elements, returning the remainder of <see cref="IEuclideanDomain{T, TFirst, TSecond}.Divide(T, T)"/>.
        /// </summary>
        /// <typeparam name="T">The type of the carrier set.</typeparam>
        /// <typeparam name="TFirst">The type of the first grouplike operation.</typeparam>
        /// <typeparam name="TSecond">The type of the second groupike operation.</typeparam>
        /// <param name="r">The ringlike structure.</param>
        /// <param name="x">The first element.</param>
        /// <param name="y">The second element.</param>
        public static T Mod<T, TFirst, TSecond>(this IEuclideanDomain<T, TFirst, TSecond> r, T x, T y)
            where TFirst : ICommutativeGroup<T>
            where TSecond : IMonoid<T>, ICommutative<T>
            => r.Divide(x, y).remainder;

        /// <summary>
        /// Gets the greatest common divisor of two elements in a <see cref="IEuclideanDomain{T, TFirst, TSecond}"/> via Euclid's algorithm. The GCD of two elements <c>X</c> and <c>Y</c> is the unique minimal principal ideal.
        /// </summary>
        /// <typeparam name="T">The type of the carrier set.</typeparam>
        /// <typeparam name="TFirst">The type of the first grouplike operation.</typeparam>
        /// <typeparam name="TSecond">The type of the second groupike operation.</typeparam>
        /// <param name="r">The ringlike structure.</param>
        /// <param name="x">The first element.</param>
        /// <param name="y">The second element.</param>
        public static T Gcd<T, TFirst, TSecond>(this IEuclideanDomain<T, TFirst, TSecond> r, T x, T y)
            where TFirst : ICommutativeGroup<T>
            where TSecond : IMonoid<T>, ICommutative<T>
        {
            while (!r.IsZero(y))
            {
                var t = y;
                y = r.Divide(x, y).remainder;
                x = t;
            }

            return x;
        }

        /// <summary>
        /// Gets the least common multiple of two elements in a <see cref="IEuclideanDomain{T, TFirst, TSecond}"/> via Euclid's algorithm. The LCDM is defined as:
        /// <code>
        ///     r.Mult(r.Gcd(x,y), r.Lcm(x,y)) == r.Mult(x,y)
        /// </code>
        /// </summary>
        /// <typeparam name="T">The type of the carrier set.</typeparam>
        /// <typeparam name="TFirst">The type of the first grouplike operation.</typeparam>
        /// <typeparam name="TSecond">The type of the second groupike operation.</typeparam>
        /// <param name="r">The ringlike structure.</param>
        /// <param name="x">The first element.</param>
        /// <param name="y">The second element.</param>
        public static T Lcm<T, TFirst, TSecond>(this IEuclideanDomain<T, TFirst, TSecond> r, T x, T y)
            where TFirst : ICommutativeGroup<T>
            where TSecond : IMonoid<T>, ICommutative<T>
        {
            var gcd = r.Gcd(x, y);
            var numerator = r.Mult(x, y);
            var ret = r.Divide(numerator, gcd).quotient;
            return ret;
        }
    }
    #endregion

    #region Fields
    /// <summary>
    /// Extension methods for <see cref="IField{T, TFirst, TSecond}"/>s.
    /// </summary>
    public static class Field
    {
        /// <summary>
        /// The double-field with addition and multiplication.
        /// </summary>
        public static DoubleField Double = new DoubleField();

        /// <summary>
        /// The field of <see cref="double"/>s, which works the same way as the Euclidean domain of integers, though the <see cref="IEuclideanDomain{T, TFirst, TSecond}.Divide(T, T)"/>-function may be numerically unstable.
        /// </summary>
        public class DoubleField : IField<double, Group.DoubleAddGroup, Group.DoubleMultGroup>
        {
            /// <inheritdoc />
            public Group.DoubleAddGroup FirstGrouplike => Group.DoubleAdd;

            /// <inheritdoc />
            public Group.DoubleMultGroup SecondGrouplike => Group.DoubleMult;

            /// <inheritdoc />
            public (double quotient, double remainder) Divide(double x, double y)
            {
                var q = x / y;
                var r = x - q * y;
                return (q, r);
            }

            /// <inheritdoc />
            public bool IsZero(double x) => Equals(x, 0D);
        }
    }
    #endregion

    #region Lattices
    /// <summary>
    /// Extension methods for lattices.
    /// </summary>
    public static class Lattice
    {
        /// <summary>
        /// The set-lattice, with set-union as the meet and set-intersection as the join.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sets.</typeparam>
        public class SetLattice<T> : ILattice<ISet<T>, Semilattice.SetMeetSemilattice<T>, Semilattice.SetJoinSemilattice<T>>
            where T : IEquatable<T>
        {
            /// <inheritdoc />
            public Semilattice.SetMeetSemilattice<T> FirstGrouplike => throw new NotImplementedException();

            /// <inheritdoc />
            public Semilattice.SetJoinSemilattice<T> SecondGrouplike => throw new NotImplementedException();
        }
    }
    #endregion
}

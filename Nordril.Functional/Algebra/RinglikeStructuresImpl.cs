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
            First = first;
            Second = second;
        }

        /// <inheritdoc />
        public TFirst First { get; }

        /// <inheritdoc />
        public TSecond Second { get; }
    }

    /// <summary>
    /// A covariant product-wrapper around a grouplike structure.
    /// </summary>
    /// <typeparam name="TRinglike">The type of the ringlike to wrap.</typeparam>
    /// <typeparam name="TFirst">The type of first grouplike structure.</typeparam>
    /// <typeparam name="TSecond">The type of second grouplike structure.</typeparam>
    public struct ContainsRinglike<TRinglike, TFirst, TSecond> : IRinglikeWrapper<TRinglike, TFirst, TSecond>
        where TRinglike : IContainsFirst<TFirst>, IContainsSecond<TSecond>
    {
        /// <inheritdoc />
        public TRinglike Ringlike { get; }

        /// <inheritdoc />
        public TFirst First { get; }

        /// <inheritdoc />
        public TSecond Second { get; }

        /// <summary>
        /// Creates a new instance. See also <see cref="Magma.AsProduct{T, TGrouplike}(TGrouplike)"/>.
        /// </summary>
        /// <param name="ringlike">The contained ringlike.</param>
        public ContainsRinglike(TRinglike ringlike)
        {
            Ringlike = ringlike;
            First = ringlike.First;
            Second = ringlike.Second;
        }
    }

    /// <summary>
    /// Extension methods for <see cref="IRinglike{T, TFirst, TSecond}"/>s.
    /// </summary>
    public static class Ringlike
    {
        /// <summary>
        /// Wraps a grouplike structure in a single-element product so that it can be used in functions which expect products like <see cref="Ringlike.Zero{T, TFirst}(IContainsFirst{TFirst})"/>.
        /// </summary>
        /// <typeparam name="T">The type of the carrier set.</typeparam>
        /// <typeparam name="TRinglike">The type of the ringlike to wrap.</typeparam>
        /// <typeparam name="TFirst">The type of first grouplike structure.</typeparam>
        /// <typeparam name="TSecond">The type of second grouplike structure.</typeparam>
        /// <param name="m">The grouplike to wrap.</param>
        public static ContainsRinglike<TRinglike, TFirst, TSecond> AsProduct<T, TRinglike, TFirst, TSecond>(this TRinglike m)
            where TRinglike : IRinglike<T, TFirst, TSecond>
            where TFirst : IMagma<T>
            where TSecond : IMagma<T>
            => new ContainsRinglike<TRinglike, TFirst, TSecond>(m);

        /// <summary>
        /// Performs the operation of the first grouplike structure, commonly called "addition".
        /// </summary>
        /// <typeparam name="T">The type of the carrier set.</typeparam>
        /// <typeparam name="TFirst">The type of the first grouplike operation.</typeparam>
        /// <param name="r">The ringlike structure.</param>
        /// <param name="x">The first element.</param>
        /// <param name="y">The second element.</param>
        public static T Plus<T, TFirst>(this IContainsFirst<TFirst> r, T x, T y)
            where TFirst : IMagma<T>
            => r.First.Op(x, y);

        /// <summary>
        /// Performs the operation of the second grouplike structure, commonly called "multiplication".
        /// </summary>
        /// <typeparam name="T">The type of the carrier set.</typeparam>
        /// <typeparam name="TSecond">The type of the second groupike operation.</typeparam>
        /// <param name="r">The ringlike structure.</param>
        /// <param name="x">The first element.</param>
        /// <param name="y">The second element.</param>
        public static T Mult<T, TSecond>(this IContainsSecond<TSecond> r, T x, T y)
            where TSecond : IMagma<T>
            => r.Second.Op(x, y);

        /// <summary>
        /// Returns the neutral element of the first grouplike structure, commonly called "zero".
        /// </summary>
        /// <typeparam name="T">The type of the carrier set.</typeparam>
        /// <typeparam name="TFirst">The type of the first grouplike operation.</typeparam>
        /// <typeparam name="TSecond">The type of the second grouplike operation.</typeparam>
        /// <param name="r">The ringlike structure.</param>
        public static T Zero<T, TFirst, TSecond>(this IRinglike<T, TFirst, TSecond> r)
            where TFirst : INeutralElement<T>
            where TSecond : IMagma<T>
            => r.First.Neutral;

        /// <summary>
        /// Returns the neutral element of the first grouplike structure, commonly called "zero".
        /// </summary>
        /// <typeparam name="T">The type of the carrier set.</typeparam>
        /// <typeparam name="TFirst">The type of the first grouplike operation.</typeparam>
        /// <param name="r">The ringlike structure.</param>
        public static T Zero<T, TFirst>(this IContainsFirst<TFirst> r)
            where TFirst : INeutralElement<T>
            => r.First.Neutral;

        /// <summary>
        /// Returns the neutral element of the second grouplike structure, commonly called "zero".
        /// </summary>
        /// <typeparam name="T">The type of the carrier set.</typeparam>
        /// <typeparam name="TSecond">The type of the second groupike operation.</typeparam>
        /// <param name="r">The ringlike structure.</param>
        public static T One<T, TSecond>(this IContainsSecond<TSecond> r)
            where TSecond : INeutralElement<T>
            => r.Second.Neutral;

        /// <summary>
        /// Returns the neutral element of the second grouplike structure, commonly called "zero".
        /// </summary>
        /// <typeparam name="T">The type of the carrier set.</typeparam>
        /// <typeparam name="TFirst">The type of the first groupike operation.</typeparam>
        /// <typeparam name="TSecond">The type of the second groupike operation.</typeparam>
        /// <param name="r">The ringlike structure.</param>
        public static T One<T, TFirst, TSecond>(this IRinglike<T, TFirst, TSecond> r)
            where TFirst : IMagma<T>
            where TSecond : INeutralElement<T>
            => r.Second.Neutral;

        /// <summary>
        /// Returns the inverse element with respect to the first grouplike structure, typically called the "negation" of the element (as in subtraction).
        /// </summary>
        /// <typeparam name="T">The type of the carrier set.</typeparam>
        /// <typeparam name="TFirst">The type of the first grouplike operation.</typeparam>
        /// <param name="r">The ringlike structure.</param>
        /// <param name="x">The element whose inverse to return.</param>
        public static T Negate<T, TFirst>(this IContainsFirst<TFirst> r, T x)
            where TFirst : IInverse<T>
            => r.First.Inverse(x);

        /// <summary>
        /// Applies <see cref="IMagma{T}.Op(T, T)"/> of the first grouplike structure to the first element <paramref name="x"/> and the inverse of the second element <paramref name="y"/>, equivalent to "subtraction" in the case of addition.
        /// </summary>
        /// <typeparam name="T">The type of the carrier set.</typeparam>
        /// <typeparam name="TFirst">The type of the first grouplike operation.</typeparam>
        /// <param name="r">The ringlike structure.</param>
        /// <param name="x">The element.</param>
        /// <param name="y">The second element whose inverse to add to the first.</param>
        public static T Minus<T, TFirst>(this IContainsFirst<TFirst> r, T x, T y)
            where TFirst : IInverse<T>
            => r.First.Op(x, r.First.Inverse(y));

        /// <summary>
        /// Returns the inverse element with respect to the second grouplike structure, typically called the "reciprocal" of the element (as in division).
        /// </summary>
        /// <typeparam name="T">The type of the carrier set.</typeparam>
        /// <typeparam name="TSecond">The type of the second groupike operation.</typeparam>
        /// <param name="r">The ringlike structure.</param>
        /// <param name="x">The element whose inverse to return.</param>
        public static T Reciprocal<T, TSecond>(this IContainsSecond<TSecond> r, T x)
            where TSecond : IInverse<T>
            => r.Second.Inverse(x);

        /// <summary>
        /// Applies <see cref="IMagma{T}.Op(T, T)"/> of the second grouplike structure to the first element <paramref name="x"/> and the inverse of the second element <paramref name="y"/>, equivalent to "division" in the case of multiplication.
        /// </summary>
        /// <typeparam name="T">The type of the carrier set.</typeparam>
        /// <typeparam name="TSecond">The type of the second groupike operation.</typeparam>
        /// <param name="r">The ringlike structure.</param>
        /// <param name="x">The element.</param>
        /// <param name="y">The second element whose inverse to add to the first.</param>
        public static T Divide<T, TSecond>(this IContainsSecond<TSecond> r, T x, T y)
            where TSecond : IInverse<T>
            => r.Second.Op(x, r.Second.Inverse(y));
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
            First = first;
            Second = second;
        }

        /// <inheritdoc />
        public TFirst First { get; }

        /// <inheritdoc />
        public TSecond Second { get; }
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
            First = first;
            Second = second;
        }

        /// <inheritdoc />
        public TFirst First { get; }

        /// <inheritdoc />
        public TSecond Second { get; }
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
            First = first;
            Second = second;
        }

        /// <inheritdoc />
        public TFirst First { get; }

        /// <inheritdoc />
        public TSecond Second { get; }
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
            public Group.BoolXorGroup First => Group.BoolXor;

            /// <inheritdoc />
            public Monoid.BoolAndMonoid Second => Monoid.BoolAnd;
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
            public Group.IntAddGroup First => Group.IntAdd;

            /// <inheritdoc />
            public Monoid.IntMultMonoid Second => Monoid.IntMult;

            /// <inheritdoc />
            public (int quotient, int remainder) EuclideanDivide(int x, int y)
            {
                var q = x / y;
                var r = x - q * y;
                return (q, r);
            }

            /// <inheritdoc />
            public bool IsZero(int x) => x == 0;
        }

        /// <summary>
        /// Performs the modulus-operation on two elements, returning the remainder of <see cref="IEuclideanDomain{T, TFirst, TSecond}.EuclideanDivide(T, T)"/>.
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
            => r.EuclideanDivide(x, y).remainder;

        /// <summary>
        /// Performs the modulus-operation on two elements, returning the remainder of <see cref="IEuclideanDomain{T, TFirst, TSecond}.EuclideanDivide(T, T)"/>.
        /// </summary>
        /// <typeparam name="T">The type of the carrier set.</typeparam>
        /// <typeparam name="TRinglike">The type of the ringlike structure.</typeparam>
        /// <typeparam name="TFirst">The type of the first grouplike operation.</typeparam>
        /// <typeparam name="TSecond">The type of the second groupike operation.</typeparam>
        /// <param name="r">The ringlike structure.</param>
        /// <param name="x">The first element.</param>
        /// <param name="y">The second element.</param>
        public static T Mod<T, TRinglike, TFirst, TSecond>(this IContainsRinglike<TRinglike> r, T x, T y)
            where TRinglike : IEuclideanDomain<T, TFirst, TSecond>
            where TFirst : ICommutativeGroup<T>
            where TSecond : IMonoid<T>, ICommutative<T>
            => r.Ringlike.Mod(x, y);

        /// <summary>
        /// Performs Euclidean division on two elements <paramref name="x"/> and <paramref name="y"/> and returns a quotient <c>q</c> and a remainder <c>r</c>
        /// </summary>
        /// <typeparam name="T">The type of the carrier set.</typeparam>
        /// <typeparam name="TRinglike">The type of the ringlike structure.</typeparam>
        /// <typeparam name="TFirst">The type of the first grouplike structure.</typeparam>
        /// <typeparam name="TSecond">The type of the second grouplike structure.</typeparam>
        /// <param name="r">The ringlike structure.</param>
        /// <param name="x">The divident.</param>
        /// <param name="y">The divisor.</param>
        public static (T quotient, T remainder) EuclideanDivide<T, TRinglike, TFirst, TSecond>(this IContainsRinglike<TRinglike> r, T x, T y)
            where TRinglike : IEuclideanDomain<T, TFirst, TSecond>
            where TFirst : ICommutativeGroup<T>
            where TSecond : IMonoid<T>, ICommutative<T>
            => r.Ringlike.EuclideanDivide(x, y);

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
            if (r.IsZero(x) || r.IsZero(y))
                return r.Zero<T, TFirst>();

            while (!r.IsZero(y))
            {
                var t = y;
                y = r.EuclideanDivide(x, y).remainder;
                x = t;
            }

            return x;
        }

        /// <summary>
        /// Gets the greatest common divisor of two elements in a <see cref="IEuclideanDomain{T, TFirst, TSecond}"/> via Euclid's algorithm. The GCD of two elements <c>X</c> and <c>Y</c> is the unique minimal principal ideal.
        /// </summary>
        /// <typeparam name="T">The type of the carrier set.</typeparam>
        /// <typeparam name="TRinglike">The type of the ringlike structure.</typeparam>
        /// <typeparam name="TFirst">The type of the first grouplike operation.</typeparam>
        /// <typeparam name="TSecond">The type of the second groupike operation.</typeparam>
        /// <param name="r">The ringlike structure.</param>
        /// <param name="x">The first element.</param>
        /// <param name="y">The second element.</param>
        public static T Gcd<T, TRinglike, TFirst, TSecond>(this IContainsRinglike<TRinglike> r, T x, T y)
            where TRinglike : IEuclideanDomain<T, TFirst, TSecond>
            where TFirst : ICommutativeGroup<T>
            where TSecond : IMonoid<T>, ICommutative<T>
            => r.Ringlike.Gcd(x, y);

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
        /// <exception cref="DivideByZeroException">If <paramref name="x"/> or <paramref name="y"/> is zero.</exception>
        public static T Lcm<T, TFirst, TSecond>(this IEuclideanDomain<T, TFirst, TSecond> r, T x, T y)
            where TFirst : ICommutativeGroup<T>
            where TSecond : IMonoid<T>, ICommutative<T>
        {

            if (r.IsZero(x) || r.IsZero(y))
                throw new DivideByZeroException();

            var gcd = r.Gcd(x, y);
            var numerator = r.Mult(x, y);
            var ret = r.EuclideanDivide(numerator, gcd).quotient;
            return ret;
        }

        /// <summary>
        /// Gets the least common multiple of two elements in a <see cref="IEuclideanDomain{T, TFirst, TSecond}"/> via Euclid's algorithm. The LCDM is defined as:
        /// <code>
        ///     r.Mult(r.Gcd(x,y), r.Lcm(x,y)) == r.Mult(x,y)
        /// </code>
        /// </summary>
        /// <typeparam name="T">The type of the carrier set.</typeparam>
        /// <typeparam name="TRinglike">The type of the ringlike structure.</typeparam>
        /// <typeparam name="TFirst">The type of the first grouplike operation.</typeparam>
        /// <typeparam name="TSecond">The type of the second groupike operation.</typeparam>
        /// <param name="r">The ringlike structure.</param>
        /// <param name="x">The first element.</param>
        /// <param name="y">The second element.</param>
        /// <exception cref="DivideByZeroException">If <paramref name="x"/> or <paramref name="y"/> is zero.</exception>
        public static T Lcm<T, TRinglike, TFirst, TSecond>(this IContainsRinglike<TRinglike> r, T x, T y)
            where TRinglike : IEuclideanDomain<T, TFirst, TSecond>
            where TFirst : ICommutativeGroup<T>
            where TSecond : IMonoid<T>, ICommutative<T>
            => r.Ringlike.Lcm(x, y);
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
        /// The decimal-field with addition and multiplication.
        /// </summary>
        public static DecimalField Decimal = new DecimalField();

        /// <summary>
        /// The field of <see cref="double"/>s, which works the same way as the Euclidean domain of integers, though the <see cref="IEuclideanDomain{T, TFirst, TSecond}.EuclideanDivide(T, T)"/>-function may be numerically unstable.
        /// </summary>
        public class DoubleField : IField<double, Group.DoubleAddGroup, Group.DoubleMultGroup>
        {
            /// <inheritdoc />
            public Group.DoubleAddGroup First => Group.DoubleAdd;

            /// <inheritdoc />
            public Group.DoubleMultGroup Second => Group.DoubleMult;

            /// <inheritdoc />
            public (double quotient, double remainder) EuclideanDivide(double x, double y)
            {
                var q = Math.Floor(x / y);
                var r = x - q * y;
                return (q, r);
            }

            /// <inheritdoc />
            public bool IsZero(double x) => Equals(x, 0D);
        }

        /// <summary>
        /// The field of <see cref="decimal"/>s, which works the same way as the Euclidean domain of integers, though the <see cref="IEuclideanDomain{T, TFirst, TSecond}.EuclideanDivide(T, T)"/>-function may be numerically unstable.
        /// </summary>
        public class DecimalField : IField<decimal, Group.DecimalAddGroup, Group.DecimalMultGroup>
        {
            /// <inheritdoc />
            public Group.DecimalAddGroup First => Group.DecimalAdd;

            /// <inheritdoc />
            public Group.DecimalMultGroup Second => Group.DecimalMult;

            /// <inheritdoc />
            public (decimal quotient, decimal remainder) EuclideanDivide(decimal x, decimal y)
            {
                var q = Math.Floor(x / y);
                var r = x - q * y;
                return (q, r);
            }

            /// <inheritdoc />
            public bool IsZero(decimal x) => Equals(x, 0M);
        }

        /// <summary>
        /// Computes the average value of a sequence iteratively using the operations of the field <paramref name="field"/>. See the <see cref="CollectionExtensions.AverageIterative(IEnumerable{double})"/>-family of functions. The average of the empty sequence is defined as <see cref="Ringlike.Zero{T, TFirst, TSecond}(IRinglike{T, TFirst, TSecond})"/>.
        /// </summary>
        /// <param name="xs">The sequence to traverse.</param>
        /// <param name="field">The field whose operations to use.</param>
        public static T Average<T, TFirst, TSecond>(this IEnumerable<T> xs, IField<T, TFirst, TSecond> field)
            where TFirst : ICommutativeGroup<T>
            where TSecond : ICommutativeGroup<T>
        {
            var avg = field.Zero<T, TFirst>();
            var one = field.One<T, TSecond>();

            foreach (var (x, i) in xs.ZipWithStream(one, i => field.Plus(i, one)))
            {
                avg = field.Plus(avg, field.Divide(field.Minus(x, avg), i));
            }

            return avg;
        }

        /// <summary>
        /// Computes the average value of a sequence iteratively using the operations of the field <paramref name="field"/>. See the <see cref="CollectionExtensions.AverageIterative(IEnumerable{double})"/>-family of functions. The average of the empty sequence is defined as <see cref="Ringlike.Zero{T, TFirst, TSecond}(IRinglike{T, TFirst, TSecond})"/>.
        /// </summary>
        /// <typeparam name="T">The type of the carrier set.</typeparam>
        /// <typeparam name="TRinglike">The type of the ringlike structure.</typeparam>
        /// <typeparam name="TFirst">The type of the first grouplike operation.</typeparam>
        /// <typeparam name="TSecond">The type of the second groupike operation.</typeparam>
        /// <param name="xs">The sequence to traverse.</param>
        /// <param name="field">The field whose operations to use.</param>
        public static T Average<T, TRinglike, TFirst, TSecond>(this IEnumerable<T> xs, IContainsRinglike<TRinglike> field)
            where TRinglike : IField<T, TFirst, TSecond>
            where TFirst : ICommutativeGroup<T>
            where TSecond : ICommutativeGroup<T>
            => xs.Average(field.Ringlike);

        /// <summary>
        /// Computes the weighted average value of a sequence iteratively using the operations of the field <paramref name="field"/>. See the <see cref="CollectionExtensions.AverageIterative(IEnumerable{double})"/>-family of functions. The average of the empty sequence is defined as <see cref="Ringlike.Zero{T, TFirst}(IContainsFirst{TFirst})"/>.
        /// </summary>
        /// <param name="xs">The sequence to traverse.</param>
        /// <param name="field">The field whose operations to use.</param>
        public static T WeightedAverage<T, TFirst, TSecond>(this IEnumerable<(T elem, T weight)> xs, IField<T, TFirst, TSecond> field)
            where TFirst : ICommutativeGroup<T>
            where TSecond : ICommutativeGroup<T>
        {
            var avg = field.Zero<T, TFirst>();
            var totalWeight = field.Zero<T, TFirst>();

            foreach (var (x, weight) in xs)
            {
                var newWeight = field.Plus(totalWeight, weight);
                var weighted = field.Mult(x, weight);

                avg = field.Divide(field.Plus(field.Mult(avg, totalWeight), weighted), newWeight);
                totalWeight = newWeight;
            }

            return avg;
        }

        /// <summary>
        /// Computes the weighted average value of a sequence iteratively using the operations of the field <paramref name="field"/>. See the <see cref="CollectionExtensions.AverageIterative(IEnumerable{double})"/>-family of functions. The average of the empty sequence is defined as <see cref="Ringlike.Zero{T, TFirst}(IContainsFirst{TFirst})"/>.
        /// </summary>
        /// <typeparam name="T">The type of the carrier set.</typeparam>
        /// <typeparam name="TRinglike">The type of the ringlike structure.</typeparam>
        /// <typeparam name="TFirst">The type of the first grouplike operation.</typeparam>
        /// <typeparam name="TSecond">The type of the second groupike operation.</typeparam>
        /// <param name="xs">The sequence to traverse.</param>
        /// <param name="field">The field whose operations to use.</param>
        public static T WeightedAverage<T, TRinglike, TFirst, TSecond>(this IEnumerable<(T elem, T weight)> xs, IContainsRinglike<TRinglike> field)
            where TRinglike : IField<T, TFirst, TSecond>
            where TFirst : ICommutativeGroup<T>
            where TSecond : ICommutativeGroup<T>
            => xs.WeightedAverage(field.Ringlike);
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
            public Semilattice.SetMeetSemilattice<T> First => new Semilattice.SetMeetSemilattice<T>();

            /// <inheritdoc />
            public Semilattice.SetJoinSemilattice<T> Second => new Semilattice.SetJoinSemilattice<T>();

            /// <inheritdoc />
            public ISet<T> Op(ISet<T> x, ISet<T> y) => First.Op(x, y);
        }
    }
    #endregion
}

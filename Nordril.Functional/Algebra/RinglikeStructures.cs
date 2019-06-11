namespace Nordril.Functional.Algebra
{
    /*
     * near-ring: group +, associative *, right-distrib
     * commutative ring: cgroup +, monoid *, distrib
     * ring: cgroup +, monoid *, distrib
     */

    /*public interface ISemiring<T, TFirst, TSecond> : IFirstGrouplike<T, TFirst>, ISecondGrouplike<T, TSecond>
        where TFirst : ICommutative<T>, IMonoid<T>
        where TSecond : IMonoid<T>
    {
        T Plus(T x, T y);
        T Mult(T x, T y);
        T One { get; }
        T Zero { get; }
    }

    public interface IRing<T, TFirst, TSecond> : ISemiring<T, TFirst, TSecond>
        where TFirst : ICommutativeGroup<T>
        where TSecond : IMonoid<T>
    {
        T PlusInverse(T x);
    }

    public interface ICommutativeRing<T, TFirst, TSecond> : IRing<T, TFirst, TSecond>
        where TFirst : ICommutativeGroup<T>
        where TSecond : IMonoid<T>, ICommutative<T>
    {

    }

    public interface IIntegralDomain<T, TFirst, TSecond> : ICommutativeRing<T, TFirst, TSecond>
        where TFirst : ICommutativeGroup<T>
        where TSecond : IMonoid<T>, ICommutative<T>
    {

    }

    public interface IField<T, TFirst, TSecond> : ICommutativeRing<T, TFirst, TSecond>
        where TFirst : ICommutativeGroup<T>
        where TSecond : IMonoid<T>, ICommutative<T>
    {
        T Minus(T x, T y);
        T Div(T x, T y);
    }

    public interface IFirstGrouplike<T, TThis>
    {
        TThis Unwrap();
    }

    public interface ISecondGrouplike<T, TThis>
    {
        TThis Unwrap();
    }

    public struct FirstGrouplike<T, TThis> : IFirstGrouplike<T, TThis>
    {
        private readonly TThis wrapped;

        public TThis Unwrap() => wrapped;

        public FirstGrouplike(TThis wrapped)
        {
            this.wrapped = wrapped;
        }
    }

    public struct SecondGrouplike<T, TThis> : ISecondGrouplike<T, TThis>
    {
        private readonly TThis wrapped;

        public TThis Unwrap() => wrapped;

        public SecondGrouplike(TThis wrapped)
        {
            this.wrapped = wrapped;
        }
    }

    public static class GrouplikeExtensions
    {
        public static IFirstGrouplike<T, TThis> WrapAsFirst<T, TThis>(this TThis grouplike)
            where TThis : IMagma<T>
            => new FirstGrouplike<T, TThis>(grouplike);

        public static ISecondGrouplike<T, TThis> WrapAsSecond<T, TThis>(this TThis grouplike)
            where TThis : IMagma<T>
            => new SecondGrouplike<T, TThis>(grouplike);
    }*/
}

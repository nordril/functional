namespace Nordril.Functional.Algebra
{
    /*
     * near-ring: group +, associative *, right-distrib
     * commutative ring: cgroup +, monoid *, distrib
     * ring: cgroup +, monoid *, distrib
     */

    /*public interface ISemiring<T, TFirst, TSecond> : IFirstGrouplike<T, TFirst>, ISecondGrouplike<T, TSecond>
        where TFirst : IFirstGrouplike<ICommutative<T>, IMonoid<T>>
        where TSecond : IMonoid<T>
    {
        T Plus(T x, T y);
        T Mult(T x, T y);
        T One { get; }
        T Zero { get; }
    }

    public struct Semiring<T, TFirst, TSecond> : ISemiring<T, TFirst, TSecond>
        where TFirst : ICommutative<T>, IMonoid<T>
        where TSecond : IMonoid<T>
    {
        public Semiring()
        public T One => throw new System.NotImplementedException();

        public T Zero => throw new System.NotImplementedException();

        public T Mult(T x, T y)
        {
            throw new System.NotImplementedException();
        }

        public T Plus(T x, T y)
        {
            throw new System.NotImplementedException();
        }

        public TFirst UnwrapFirst()
        {
            throw new System.NotImplementedException();
        }

        public TSecond UnwrapSecond()
        {
            throw new System.NotImplementedException();
        }
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

    public interface IFirstGrouplike<T, out TThis>
    {
        TThis UnwrapFirst();
    }

    public interface ISecondGrouplike<T, out TThis>
    {
        TThis UnwrapSecond();
    }

    public struct FirstGrouplike<T, TThis> : IFirstGrouplike<T, TThis>
    {
        private readonly TThis wrapped;

        public TThis UnwrapFirst() => wrapped;

        public FirstGrouplike(TThis wrapped)
        {
            this.wrapped = wrapped;
        }
    }

    public struct SecondGrouplike<T, TThis> : ISecondGrouplike<T, TThis>
    {
        private readonly TThis wrapped;

        public TThis UnwrapSecond() => wrapped;

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

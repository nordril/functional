namespace Nordril.Functional.Algebra
{
    /*
     * near-ring: group +, associative *, right-distrib
     * commutative ring: cgroup +, monoid *, distrib
     * ring: cgroup +, monoid *, distrib
     */

    /*public interface ISemiring<T, TFirst, TSecond> : IRinglike<T, TFirst, TSecond>
        where TFirst : ICommutative<T>, IMonoid<T>
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
        private readonly TFirst first;
        private readonly TSecond second;

        public Semiring(TFirst first, TSecond second)
        {
            this.first = first;
            this.second = second;
        }

        public T One => second.Neutral;

        public T Zero => first.Neutral;

        public TFirst FirstGrouplike() => first;

        public T Mult(T x, T y) => second.Op(x, y);

        public T Plus(T x, T y) => first.Op(x, y);

        public TSecond SecondGrouplike() => second;
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

    public interface IRinglike<T, out TFirst, out TSecond>
        where TFirst : IMagma<T>
        where TSecond : IMagma<T>
    {
        TFirst FirstGrouplike();

        TSecond SecondGrouplike();
    }

    public interface ITagged<out TThis, TTag>
    {
        TThis Untagged { get; }
    }

    public class Tagged<TThis, TTag> : ITagged<TThis, TTag>
    {
        public TThis Untagged { get; }

        public Tagged(TThis untagged)
        {
            Untagged = untagged;
        }
    }

    public static class GrouplikeExtensions
    {
        public static ITagged<TThis, TagFirst> TagFirst<T, TThis>(this TThis grouplike)
            where TThis : IMagma<T>
            => new Tagged<TThis, TagFirst>(grouplike);

        public static ITagged<TThis, TagSecond> TagSecond<T, TThis>(this TThis grouplike)
            where TThis : IMagma<T>
            => new Tagged<TThis, TagSecond>(grouplike);
    }

    public class TagFirst
    {
        private TagFirst() { }
    }

    public class TagSecond
    {
        private TagSecond() { }
    }*/
}

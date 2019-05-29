using Nordril.Functional.Data;

namespace Nordril.Functional.Algebra
{



    /*public interface IRing<T, TPlus, TMult> : ITaggedGroup<T, TPlus>, ITaggedMonoid<T, TMult>
        where T : IGroup<T>
    {
        T Plus(T y);
        T Mult(T y);
        T One { get; }
        T Zero { get; }
    }

    public struct IntRing : IRing<IntRing, TagPlus, TagMult>
    {
        public int Value { get;  }

        public IntRing One => new IntRing(1);

        public IntRing Zero => new IntRing(0);

        public IntRing Inverse => new IntRing(-Value);

        public IntRing Neutral => One;

        public IntRing(int value) { Value = value; }

        public IntRing Plus(IntRing y) => new IntRing(Value + y.Value);

        public IntRing Mult(IntRing y) => new IntRing(Value * y.Value);

        public IntRing Op(IntRing that) => Plus(that);
    }*/
}

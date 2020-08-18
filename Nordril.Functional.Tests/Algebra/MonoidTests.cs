using Nordril.Functional.Algebra;
using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Nordril.Functional.Tests.Algebra
{
    public class MonoidTests
    {
        [Fact]
        public static void IntMonoidTest()
        {
            var intMult = new Monoid<int>(1, (x, y) => x * y);

            Assert.Equal(5, intMult.Op(intMult.Neutral, 5));
            Assert.Equal(5, intMult.Op(5, intMult.Neutral));
            Assert.Equal(15, intMult.Op(3, 5));
        }

        [Fact]
        public static void IntMultMonoidTest()
        {
            var intMult = Monoid.IntMult;

            Assert.Equal(5, intMult.Op(intMult.Neutral, 5));
            Assert.Equal(5, intMult.Op(5, intMult.Neutral));
            Assert.Equal(15, intMult.Op(3, 5));
        }

        [Fact]
        public static void BoolAndMonoidPremadeTest()
        {
            var boolAnd = Monoid.BoolAnd;

            Assert.True(boolAnd.Neutral);
            Assert.True(boolAnd.Op(true, boolAnd.Neutral));
            Assert.False(boolAnd.Op(false, boolAnd.Neutral));
            Assert.True(boolAnd.Op(boolAnd.Neutral, true));
            Assert.False(boolAnd.Op(boolAnd.Neutral, false));

            Assert.False(boolAnd.Op(false, false));
            Assert.False(boolAnd.Op(false, true));
            Assert.False(boolAnd.Op(true, false));
            Assert.True(boolAnd.Op(true, true));
        }

        [Fact]
        public static void BoolOrMonoidPremadeTest()
        {
            var boolOr = Monoid.BoolOr;

            Assert.False(boolOr.Neutral);
            Assert.True(boolOr.Op(true, boolOr.Neutral));
            Assert.False(boolOr.Op(false, boolOr.Neutral));
            Assert.True(boolOr.Op(boolOr.Neutral, true));
            Assert.False(boolOr.Op(boolOr.Neutral, false));

            Assert.False(boolOr.Op(false, false));
            Assert.True(boolOr.Op(false, true));
            Assert.True(boolOr.Op(true, false));
            Assert.True(boolOr.Op(true, true));
        }

        [Fact]
        public static void StringAppendTest()
        {
#pragma warning disable CS0618 // Type or member is obsolete
            var stringAppend = Monoid.StringAppend;
#pragma warning restore CS0618 // Type or member is obsolete

            Assert.Equal("", stringAppend.Neutral);
            Assert.Equal("abc", stringAppend.Op("abc", stringAppend.Neutral));
            Assert.Equal("abc", stringAppend.Op(stringAppend.Neutral, "abc"));

            Assert.Equal("abcdef", stringAppend.Op("abc", "def"));
        }

        [Fact]
        public static void StringBuilderAppendTest()
        {
            var stringAppend = Monoid.StringBuilderAppend;

            Assert.Equal("", stringAppend.Neutral.ToString());

            var abc = new StringBuilder("abc");
            var def = new StringBuilder("def");

            Assert.Equal("abc", stringAppend.Op(abc, stringAppend.Neutral).ToString());
            Assert.Equal("abc", stringAppend.Op(stringAppend.Neutral, abc).ToString());

            Assert.Equal("abcdef", stringAppend.Op(abc, def).ToString());

            Assert.Equal("abcdef", abc.ToString());
            Assert.Equal("def", def.ToString());
        }

        [Fact]
        public static void FirstOrDefaultMonoidTest()
        {
            var m = Monoid.FirstOrDefault(99);

            Assert.Equal(99, m.Neutral);
            Assert.Equal(99, m.Op(99, 5));
            Assert.Equal(5, m.Op(5, 99));
        }

        [Fact]
        public static void LastOrDefaultMonoidTest()
        {
            var m = Monoid.LastOrDefault(99);

            Assert.Equal(99, m.Neutral);
            Assert.Equal(5, m.Op(99, 5));
            Assert.Equal(99, m.Op(5, 99));
        }

        [Fact]
        public static void ListAppendMonoidTest()
        {
            var m = Monoid.ListAppend<int>();

            var xs = new List<int> { };

            Assert.Empty(m.Neutral);
            Assert.Equal(new int[] { 3, 4 }, m.Op(xs, new List<int> { 3, 4 }));

            Assert.Equal(new int[] { 3, 4 }, xs);

            xs = new List<int> { };

            Assert.Equal(new int[] { 3, 4 }, m.Op(new List<int> { 3, 4 }, xs));

            Assert.Empty(xs);
        }

        [Fact]
        public static void ImmutableListAppendMonoidTest()
        {
            var m = Monoid.ImmutableListAppend<int>();

            var xs = new List<int> { };
            var ys = new List<int> { 1, 2, 3 };
            var zs = new List<int> { 4, 1, 2 };

            Assert.Empty(m.Neutral);
            Assert.Equal(new int[] { 1, 2, 3 }, m.Op(xs, ys));
            Assert.Equal(new int[] { 1, 2, 3 }, m.Op(ys, xs));
            Assert.Equal(new int[] { 1, 2, 3, 4, 1, 2 }, m.Op(ys, zs));

            Assert.Empty(xs);
            Assert.Equal(new int[] { 1, 2, 3 }, ys);
            Assert.Equal(new int[] { 4, 1, 2 }, zs);
        }

        [Fact]
        public static void UnsafeTest()
        {
            var x = new IntWithAddMonoid(5);
            var y = new IntWithAddMonoid(7);

            Assert.Equal(0, Monoid.NeutralUnsafe<IntWithAddMonoid, IntWithAddMonoid>().Value);
            Assert.Equal(12, Monoid.OpUnsafe<IntWithAddMonoid, IntWithAddMonoid>()(x,y).Value);
        }

        private class IntWithAddMonoid : IHasMonoid<IntWithAddMonoid>
        {
            public int Value {get;}

            public IntWithAddMonoid Neutral => new IntWithAddMonoid(0);

            public IntWithAddMonoid(int value)
            {
                Value = value;
            }

            public IntWithAddMonoid Op(IntWithAddMonoid x, IntWithAddMonoid y)
            {
                return new IntWithAddMonoid(x.Value + y.Value);
            }
        }

        [Fact]
        public static void MsumTest()
        {
            var xs = new List<int> { 3, 5, 6 };
            var ys = xs.Select(x => new IntWithAddMonoid(x)).ToList();

            Assert.Equal(1, Array.Empty<int>().Msum(Monoid.IntMult));
            Assert.Equal(3 * 5 * 6, xs.Msum(Monoid.IntMult));

            Assert.Equal(0, Array.Empty<IntWithAddMonoid>().Msum().Value);
            Assert.Equal(3 + 5 + 6, ys.Msum().Value);
        }

        [Fact]
        public static void IntLiftInfinityTest()
        {
            var intPlus = new Monoid<int>(0, (x, y) => x + y);
            var intPlusInf = intPlus.LiftMonoidWithInfinity();

            Assert.True(intPlusInf.Neutral.HasValue);
            Assert.Equal(0, intPlusInf.Neutral.Value());

            var r = intPlusInf.Op(Maybe.Just(3), Maybe.Just(8));
            Assert.True(r.HasValue);
            Assert.Equal(11, r.Value());

            r = intPlusInf.Op(Maybe.Nothing<int>(), Maybe.Just(8));
            Assert.False(r.HasValue);

            r = intPlusInf.Op(Maybe.Just(3), Maybe.Nothing<int>());
            Assert.False(r.HasValue);

            r = intPlusInf.Op(Maybe.Nothing<int>(), Maybe.Nothing<int>());
            Assert.False(r.HasValue);
        }
    }
}

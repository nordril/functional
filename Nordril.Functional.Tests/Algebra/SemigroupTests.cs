using Nordril.Functional.Algebra;
using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Nordril.Functional.Tests.Algebra
{
    public static class SemigroupTests
    {
        [Fact]
        public static void FirstSemigroupTest()
        {
            var m = Semigroup.First<int>();

            Assert.Equal(99, m.Op(99, 5));
            Assert.Equal(5, m.Op(5, 99));
        }

        [Fact]
        public static void LastSemigroupTest()
        {
            var m = Semigroup.Last<int>();

            Assert.Equal(5, m.Op(99, 5));
            Assert.Equal(99, m.Op(5, 99));
        }

        [Fact]
        public static void IntLiftInfinityTest()
        {
            var intPlus = new Semigroup<int>((x, y) => x + y);
            var intPlusInf = intPlus.LiftSemigroupWithInfinity();

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

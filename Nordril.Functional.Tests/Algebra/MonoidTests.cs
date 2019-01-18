using Nordril.Functional.Algebra;
using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Nordril.Functional.Tests.Algebra
{
    public class MonoidTests
    {
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

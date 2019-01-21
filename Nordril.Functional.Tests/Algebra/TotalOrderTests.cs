using Nordril.Functional.Algebra;
using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Nordril.Functional.Tests.Algebra
{
    public class TotalOrderTests
    {
        public static IEnumerable<object[]> IntInfinityTotalOrderTestData()
        {
            //Both arguments are finite
            yield return new object[] { Maybe.Just(1), Maybe.Just(1), 0 };
            yield return new object[] { Maybe.Just(1), Maybe.Just(3), -1 };
            yield return new object[] { Maybe.Just(2), Maybe.Just(1), 1 };
            yield return new object[] { Maybe.Just(5), Maybe.Just(3), 1 };
            yield return new object[] { Maybe.Just(7), Maybe.Just(7), 0 };
            yield return new object[] { Maybe.Just(7), Maybe.Just(-1), 1 };
            yield return new object[] { Maybe.Just(-1), Maybe.Just(4), -1 };
            yield return new object[] { Maybe.Just(-5), Maybe.Just(-5), 0 };
            yield return new object[] { Maybe.Just(14), Maybe.Just(3), 1 };
            yield return new object[] { Maybe.Just(14), Maybe.Just(18), -1 };

            //First argument is infinite
            yield return new object[] { Maybe.Nothing<int>(), Maybe.Just(0), 1 };
            yield return new object[] { Maybe.Nothing<int>(), Maybe.Just(-5), 1 };
            yield return new object[] { Maybe.Nothing<int>(), Maybe.Just(8), 1 };
            yield return new object[] { Maybe.Nothing<int>(), Maybe.Just(int.MaxValue), 1 };
            yield return new object[] { Maybe.Nothing<int>(), Maybe.Just(int.MinValue), 1 };

            //Second argument is infinite
            yield return new object[] { Maybe.Just(0), Maybe.Nothing<int>(), -1 };
            yield return new object[] { Maybe.Just(-5), Maybe.Nothing<int>(), -1 };
            yield return new object[] { Maybe.Just(8), Maybe.Nothing<int>(), -1 };
            yield return new object[] { Maybe.Just(int.MaxValue), Maybe.Nothing<int>(), -1 };
            yield return new object[] { Maybe.Just(int.MinValue), Maybe.Nothing<int>(), -1 };

            //Both arguments are infinite
            yield return new object[] { Maybe.Nothing<int>(), Maybe.Nothing<int>(), 0 };
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(1, 3)]
        [InlineData(2, 1)]
        [InlineData(5, 3)]
        [InlineData(7, 7)]
        [InlineData(7, -1)]
        [InlineData(-1, 4)]
        [InlineData(-5, -5)]
        [InlineData(14, 3)]
        [InlineData(14, 18)]
        public static void IntTotalOrderTest(int x, int y)
        {
            var order = new TotalOrder<int>((a, b) => a <= b);

            Assert.Equal(x < y, order.Le(x, y));
            Assert.Equal(x <= y, order.Leq(x, y));
            Assert.Equal(x >= y, order.Geq(x, y));
            Assert.Equal(x > y, order.Ge(x, y));
        }

        [Theory]
        [MemberData(nameof(IntInfinityTotalOrderTestData))]
        public static void IntInfinityTotalOrderTest(Maybe<int> x, Maybe<int> y, int expected)
        {
            var order = new TotalOrder<int>((a, b) => a <= b).LiftTotalOrderWithInfinity();

            Assert.Equal(expected < 0, order.Le(x, y));
            Assert.Equal(expected >= 0, !order.Le(x, y));

            Assert.Equal(expected <= 0, order.Leq(x, y));
            Assert.Equal(expected > 0, !order.Leq(x, y));

            Assert.Equal(expected >= 0, order.Geq(x, y));
            Assert.Equal(expected < 0, !order.Geq(x, y));

            Assert.Equal(expected > 0, order.Ge(x, y));
            Assert.Equal(expected <= 0, !order.Ge(x, y));
        }
    }
}

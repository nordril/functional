using Nordril.Functional.Category;
using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Nordril.Functional.Tests.Category
{
    public class MonadTests
    {
        [Fact]
        public static void AggregateMTest()
        {
            var xs1 = new List<int>();
            var res1 = xs1.AggregateM(5, (x, acc) => Maybe.Nothing<int>());

            Assert.True(res1.HasValue);
            Assert.Equal(5, res1.Value());

            var xs2 = new List<int> { 4 };
            var res2 = xs2.AggregateM(5, (x, acc) => Maybe.Nothing<int>());

            Assert.False(res2.HasValue);

            var xs3 = new List<int> { 4 };
            var res3 = xs3.AggregateM(5, (x, acc) => Maybe.Just(x+acc));

            Assert.True(res3.HasValue);
            Assert.Equal(9, res3.Value());

            var xs4 = new List<int> { 4, 2, 7 };
            var res4 = xs4.AggregateM(5, (x, acc) => Maybe.Just(x + acc));

            Assert.True(res4.HasValue);
            Assert.Equal(5+4+2+7, res4.Value());

            var xs5 = new List<int> { 4, 2, 99, 7 };
            var res5 = xs5.AggregateM(5, (x, acc) => Maybe.JustIf(x != 99, () => x + acc));

            Assert.False(res5.HasValue);
        }
    }
}

using Nordril.Functional.Algebra;
using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Nordril.Functional.Tests.Data
{
    public static class PredicateTests
    {
        [Fact]
        public static void ContramapTest()
        {
            var pred = Pred.Create((int x) => x > 5).ContraMap((IEnumerable<int> x) => x.Count()).ToPredicate();

            Assert.False(pred.Run(new int[] { 1, 2, 3, 4 }));
            Assert.True(pred.Run(new int[] { 1, 2, 3, 4, 5, 6 }));
        }

        [Fact]
        public static void OpTest()
        {
            var pred1 = Pred.Create((int x) => x > 10);
            var pred2 = Pred.Create((int x) => x % 2 == 0);
            var pred = pred1.Op(pred2);

            Assert.True(pred1.Run(11));
            Assert.False(pred1.Run(8));
            Assert.True(pred2.Run(4));
            Assert.False(pred2.Run(11));
            Assert.True(pred.Run(12));
            Assert.False(pred.Run(8));
            Assert.False(pred.Run(11));
        }
    }
}

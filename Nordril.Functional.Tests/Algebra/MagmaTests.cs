using Nordril.Functional.Algebra;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Nordril.Functional.Tests.Algebra
{
    public sealed class MagmaTests
    {
        [Fact]
        public static void ToRelationTest()
        {
            var m = new Magma<int>((x, y) => x + y);
            var r = m.ToRelation();

            Assert.True(r.Contains((0, 0), 0));
            Assert.True(r.Contains((0, 0), 0));
            Assert.False(r.Contains((7, 3), 9));
        }
    }
}

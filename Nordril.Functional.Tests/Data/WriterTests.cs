using Nordril.Functional.Algebra;
using Nordril.Functional.Category;
using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Nordril.Functional.Tests.Data
{
    public static class WriterTests
    {
        [Fact]
        public static void MapTestEmptyOutput()
        {
            var w = new Writer<IList<int>, int>(7, Monoid.ImmutableListAppend<int>());

            w = (Writer<IList<int>, int>)w.Map(x => x + 5);

            Assert.Equal(12, w.Result);
            Assert.Equal(new int[0], w.Output);
        }

        [Fact]
        public static void MapTestWithOutput()
        {
            var w = new Writer<IList<int>, int>(7, Monoid.ImmutableListAppend<int>());

            w.Tell(new int[] { 2, 3 });
            w.Tell(new int[] { 4 });
            w = (Writer<IList<int>, int>)w.Map(x => x + 5);

            Assert.Equal(12, w.Result);
            Assert.Equal(new int[] { 2, 3, 4 }, w.Output);
        }
    }
}

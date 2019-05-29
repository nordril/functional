using Nordril.Functional.Algebra;
using Nordril.Functional.Category;
using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Nordril.Functional.Tests.Data
{
    using ListWriter = Writer<List<int>, int, Monoid.ListAppendMonoid<int>>;

    public static class WriterTests
    {
        [Fact]
        public static void MapTestEmptyOutput()
        {
            var w = new Writer<IList<int>, int, Monoid.ListAppendImmutableMonoid<int>>(7);

            w = (Writer<IList<int>, int, Monoid.ListAppendImmutableMonoid<int>>)w.Map(x => x + 5);

            Assert.Equal(12, w.Result);
            Assert.Equal(new int[0], w.State);
        }

        [Fact]
        public static void MapTestWithOutput()
        {
            var w = new Writer<IList<int>, int, Monoid.ListAppendImmutableMonoid<int>>(7);

            var actual1 = w.Tell(new int[] { 2, 3 });
            var actual2 = actual1.Tell(new int[] { 4 });
            var actual3 = (Writer<IList<int>, int, Monoid.ListAppendImmutableMonoid<int>>)actual2.Map(x => x + 5);

            Assert.Equal(7, actual2.Result);
            Assert.Equal(12, actual3.Result);
            Assert.Equal(new int[0], w.State);
            Assert.Equal(new int[] { 2, 3 }, actual1.State);
            Assert.Equal(new int[] { 2, 3, 4 }, actual2.State);
        }

        [Fact]
        public static void PureTest()
        {
            var actual = 5.PureUnsafe<int, ListWriter>();

            Assert.Equal(5, actual.Result);
            Assert.Equal(new List<int>(), actual.State);
        }

        [Fact]
        public static void ApTest()
        {
            var x = 5.PureUnsafe<int, ListWriter>();
            var y = 12.PureUnsafe<int, ListWriter>();

            Func<int, bool> f = i => i % 2 == 0;

            var ff = f.PureUnsafe<Func<int, bool>, Writer<List<int>, Func<int, bool>, Monoid.ListAppendMonoid<int>>>();

            var actual = x.Ap(ff).ToWriter<List<int>, bool, Monoid.ListAppendMonoid<int>>();

            Assert.False(actual.Result);
            Assert.Equal(new List<int>(), actual.State);

            actual = y.Ap(ff).ToWriter<List<int>, bool, Monoid.ListAppendMonoid<int>>();

            Assert.True(actual.Result);
            Assert.Equal(new List<int>(), actual.State);
        }

        [Fact]
        public static void TellTest()
        {
            var x = 5.PureUnsafe<int, Writer<List<int>, int, Monoid.ListAppendMonoid<int>>>();

            var comp = x.BindTell(y => new List<int> { y }).MapWriter(y => y + 3).BindTell(y => new List<int> { y, y, y }).MapWriter(y => y + 7);

            Assert.Equal(15, comp.Result);
            Assert.Equal(new List<int> { 5, 8, 8, 8 }, comp.State);
        }
    }
}

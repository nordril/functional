using Nordril.Functional.Algebra;
using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Nordril.Functional.Tests.Data
{
    public class RwsTests
    {
        [Fact]
        public static void LinqSelectTest()
        {
            var rws =
                from env in Rws.GetEnvironment<Dictionary<int, string>, StringBuilder, Monoid.StringBuilderAppendMonoid, int>()
                from s in Rws.Get<Dictionary<int, string>, StringBuilder, Monoid.StringBuilderAppendMonoid, int>()
                from _ in Rws.Put<Dictionary<int, string>, StringBuilder, Monoid.StringBuilderAppendMonoid, int>(s + 1)
                select env[s] + 2;

            var dict = new Dictionary<int, string>
            {
                { 5, "e" }
            };

            var (res, state, w) = rws.Run(dict, 5);

            Assert.Equal("e2", res);
            Assert.Equal(6, state);
            Assert.Equal("", w.ToString());
        }

        [Fact]
        public static void LinqTellSelectTest()
        {
            var rws =
                from env in Rws.GetEnvironment<Dictionary<int, string>, IList<string>, Monoid.ListAppendImmutableMonoid<string>, int>()
                from s in Rws.Get<Dictionary<int, string>, IList<string>, Monoid.ListAppendImmutableMonoid<string>, int>()
                from _ in Rws.Put<Dictionary<int, string>, IList<string>, Monoid.ListAppendImmutableMonoid<string>, int>(s + 1)
                from __ in Rws.Tell<Dictionary<int, string>, IList<string>, Monoid.ListAppendImmutableMonoid<string>, int>(FuncList.Make("bla"))
                select env[s] + 2;

            var dict = new Dictionary<int, string>
            {
                { 5, "e" }
            };

            var (res, state, w) = rws.Run(dict, 5);

            Assert.Equal("e2", res);
            Assert.Equal(6, state);
            Assert.Equal(new string[] { "bla" }, w);
        }

        [Fact]
        public static void LinqTellIntSelectTest()
        {
            var rws =
                from env in Rws.GetEnvironment<Dictionary<int, string>, IList<int>, Monoid.ListAppendImmutableMonoid<int>, int>()
                from s in Rws.Get<Dictionary<int, string>, IList<int>, Monoid.ListAppendImmutableMonoid<int>, int>()
                from _ in Rws.Put<Dictionary<int, string>, IList<int>, Monoid.ListAppendImmutableMonoid<int>, int>(s + 1)
                from __ in Rws.Tell<Dictionary<int, string>, IList<int>, Monoid.ListAppendImmutableMonoid<int>, int>(FuncList.Make(28))
                select env[s] + 2;

            var dict = new Dictionary<int, string>
            {
                { 5, "e" }
            };

            var (res, state, w) = rws.Run(dict, 5);

            Assert.Equal("e2", res);
            Assert.Equal(6, state);
            Assert.Equal(new int[] { 28 }, w);
        }
    }
}

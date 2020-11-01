using Nordril.Functional.Data;
using Nordril.Functional.Lens;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Nordril.Functional.Tests.LensTests
{
    public sealed class Traversal
    {
        [Fact]
        public void TraversalAsSetterMaybeTest()
        {
            var m = Maybe.Nothing<int>();
            var lens = L.Make.Traversal<Maybe<int>, int>();
            var actual = L.Set(lens, m, 5);

            Assert.False(actual.HasValue);

            m = Maybe.Just(6);
            actual = L.Set(lens, m, 5);

            Assert.Equal(5, actual.Value());
        }

        [Fact]
        public void TraverseMaybeTest()
        {
            var m = Maybe.Nothing<int>();
            var lens = L.Make.Traversal<Maybe<int>, Maybe<string>, int, string>();
            var actual = L.Traverse(lens, typeof(FuncList<string>), a => FuncList.Make(a + ""), m).ToFuncList();

            Assert.Single(actual);
            Assert.False(actual[0].HasValue);

            m = Maybe.Just(6);
            actual = L.Traverse(lens, typeof(FuncList<string>), a => FuncList.Make(a + ""), m).ToFuncList();

            Assert.Single(actual);
            Assert.Equal("6", actual[0].Value());
        }

        [Fact]
        public void TraveseFuncListTest()
        {
            var m = FuncList.Make<int>();
            var lens = L.Make.Traversal<FuncList<int>, FuncList<string>, int, string>();
            var actual = L.Traverse(lens, typeof(Maybe<string>), a => Maybe.Just(a + "_" + a), m).ToMaybe();

            Assert.Equal(Array.Empty<string>(), actual.Value());

            m = FuncList.Make(1, 2, 3);
            actual = L.Traverse(lens, typeof(Maybe<string>), a => Maybe.Just(a + "_" + a), m).ToMaybe();

            Assert.Single(actual);
            Assert.Equal(new string[] { "1_1", "2_2", "3_3" }, actual.Value());

            actual = L.Traverse(lens, a => Maybe.Just(a + "_" + a), m).ToMaybe();
            Assert.Equal(new string[] { "1_1", "2_2", "3_3" }, actual.Value());

            actual = L.Traverse(lens, typeof(Maybe<string>), a => Maybe.JustIf(a % 2 == 0, () => a + "_" + a), m).ToMaybe();
            Assert.False(actual.HasValue);
        }

        [Fact]
        public void TraverseThenTest()
        {
            var outer = L.Make.Traversal<FuncList<FuncList<int>>, FuncList<FuncList<string>>, FuncList<int>, FuncList<string>>();
            var inner = L.Make.Traversal<FuncList<int>, FuncList<string>, int, string>();

            var combined = outer.Then(inner);

            var actual = L.Traverse(combined, typeof(Maybe<string>), i => Maybe.Just(i + ""), FuncList.Make<FuncList<int>>()).ToMaybe();

            Assert.Empty(actual.Value());

            actual = L.Traverse(combined, typeof(Maybe<string>), i => Maybe.Just(i + ""), FuncList.Make(FuncList.Make<int>())).ToMaybe();

            Assert.Single(actual.Value());
            Assert.Empty(actual.Value()[0]);

            actual = L.Traverse(combined, typeof(Maybe<string>), i => Maybe.JustIf(i % 2 == 0, () => i + ""), FuncList.Make(FuncList.Make(0,2,4,6), FuncList.Make(10,12))).ToMaybe();

            Assert.Equal(2, actual.Value().Count);
            Assert.Equal(new string[] { "0", "2", "4", "6" }, actual.Value()[0]);
            Assert.Equal(new string[] { "10", "12" }, actual.Value()[1]);

            actual = L.Traverse(combined, typeof(Maybe<string>), i => Maybe.JustIf(i % 2 == 0, () => i + ""), FuncList.Make(FuncList.Make(0, 1, 2, 4, 6), FuncList.Make(10, 12))).ToMaybe();

            Assert.False(actual.HasValue);
        }
    }
}

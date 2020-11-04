using Nordril.Functional.Data;
using Nordril.Functional.Lens;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Nordril.Functional.Tests.Lens
{
    public sealed class WitheringTests
    {
        [Fact]
        public void WitherTest()
        {
            var lens = L.Make.Withering<FuncList<string>, FuncList<int>, string, int>();

            var actual = L.TraverseMaybe(lens, typeof(Maybe<int>), s => Maybe.JustIf(s.Length % 2 == 0, () => s.Length), FuncList.Make<string>()).ToMaybe();
            Assert.Equal(new int[] { }, actual.Value());

            actual = L.TraverseMaybe(lens, typeof(Maybe<int>), s => Maybe.JustIf(s.Length % 2 == 0, () => s.Length), FuncList.Make("a")).ToMaybe();
            Assert.Equal(new int[] { }, actual.Value());

            actual = L.TraverseMaybe(lens, typeof(Maybe<int>), s => Maybe.JustIf(s.Length % 2 == 0, () => s.Length), FuncList.Make("bb")).ToMaybe();
            Assert.Equal(new int[] { 2 }, actual.Value());

            actual = L.TraverseMaybe(lens, typeof(Maybe<int>), s => Maybe.JustIf(s.Length % 2 == 0, () => s.Length), FuncList.Make("a", "bb", "cccc", "", "gg")).ToMaybe();
            Assert.Equal(new int[] { 2, 4, 0, 2 }, actual.Value());
        }

        [Fact]
        public void WitherThenTest()
        {
            var lensOuter = L.Make.Withering<FuncList<FuncList<string>>, FuncList<FuncList<int>>, FuncList<string>, FuncList<int>>();
            var lensInner = L.Make.Withering<FuncList<string>, FuncList<int>, string, int>();

            var lens = lensOuter.Then(lensInner);

            var actual = L.TraverseMaybe(lens, typeof(Maybe<int>), s => Maybe.JustIf(s.Length % 2 == 0, () => s.Length), FuncList.Make<FuncList<string>>()).ToMaybe();
            Assert.Empty(actual.Value());

            actual = L.TraverseMaybe(lens, typeof(Maybe<int>), s => Maybe.JustIf(s.Length % 2 == 0, () => s.Length), FuncList.Make(FuncList.Make<string>())).ToMaybe();
            Assert.Single(actual.Value());
            Assert.Equal(new int[] { }, actual.Value()[0]);

            actual = L.TraverseMaybe(lens, typeof(Maybe<int>), s => Maybe.JustIf(s.Length % 2 == 0, () => s.Length), FuncList.Make(FuncList.Make("a", "bb", "cccc", "", "gg"), FuncList.Make<string>(), FuncList.Make("hhhh"))).ToMaybe();
            Assert.Equal(3, actual.Value().Count);
            Assert.Equal(new int[] { 2, 4, 0, 2 }, actual.Value()[0]);
            Assert.Equal(new int[] { }, actual.Value()[1]);
            Assert.Equal(new int[] { 4 }, actual.Value()[2]);
        }
    }
}

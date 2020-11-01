using Nordril.Functional.Algebra;
using Nordril.Functional.Data;
using Nordril.Functional.Lens;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Nordril.Functional.Tests.Lens
{
    public sealed class FoldTests
    {
        [Fact]
        public void FoldTest()
        {
            var fl = FuncList.Make<int>();
            var fold = L.Make.Folding<FuncList<int>, FuncList<int>, int>(x => x);

            var actual = L.Fold<FuncList<int>, int, int, Group.IntAddGroup>(fold, x => x, fl);
            Assert.Equal(0, actual);

            actual = L.Fold(fold, x => new IntMult(x), fl).Value;
            Assert.Equal(1, actual);

            actual = L.Fold(fold, (x, y) => x * y, 1, fl);
            Assert.Equal(1, actual);

            fl = FuncList.Make(1, 2, 3, 4);
            actual = L.Fold<FuncList<int>, int, int, Group.IntAddGroup>(fold, x => x, fl);
            Assert.Equal(10, actual);

            actual = L.Fold(fold, x => new IntMult(x), fl).Value;
            Assert.Equal(24, actual);

            actual = L.Fold(fold, (x, y) => x * y, 1, fl);
            Assert.Equal(24, actual);
        }

        [Fact]
        public void ToListTest()
        {
            var fl = FuncList.Make<int>();
            var fold = L.Make.Folding<FuncList<int>, FuncList<int>, int>(x => x);
            var actual = L.ToList(fold, fl);
            Assert.Empty(actual);

            fl = FuncList.Make(1);
            actual = L.ToList(fold, fl);
            Assert.Equal(new int[] { 1 }, actual);

            fl = FuncList.Make(1, 2, 3);
            actual = L.ToList(fold, fl);
            Assert.Equal(new int[] { 1, 2, 3 }, actual);
        }

        [Fact]
        public void FoldThen()
        {
            var foldOuter = L.Make.Folding<FuncList<FuncList<int>>, FuncList<int>>();
            var foldInner = L.Make.Folding<FuncList<int>, int>();

            var fold = foldOuter.Then(foldInner);

            var fl = FuncList.Make<FuncList<int>>();
            var actual = L.Fold(fold, (x, y) => x + y, 0, fl);

            Assert.Equal(0, actual);

            fl = FuncList.Make(FuncList.Make<int>(), FuncList.Make(1,2,3), FuncList.Make<int>(), FuncList.Make(4,5,6));
            actual = L.Fold(fold, (x, y) => x + y, 0, fl);

            Assert.Equal(21, actual);
        }

        private class IntMult : IHasMonoid<IntMult>
        {
            public int Value { get; }

            public IntMult(int value)
            {
                Value = value;
            }

            public IntMult Neutral => new IntMult(1);

            public IntMult Op(IntMult x, IntMult y)
                => new IntMult(x.Value * y.Value);
        }
    }
}

using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Nordril.Functional.Tests.Data
{
    public static class FuncListTests
    {
        public static IEnumerable<object[]> Ap1Data()
        {
            yield return new object[] {
                FuncList.Make<Func<int, int>>(),
                FuncList.Make<int>(),
                FuncList.Make<int>(),
            };

            yield return new object[] {
                FuncList.Make<Func<int, int>>(),
                FuncList.Make(1),
                FuncList.Make<int>(),
            };

            yield return new object[] {
                FuncList.Make<Func<int, int>>(x => x + 1),
                FuncList.Make<int>(),
                FuncList.Make<int>(),
            };

            yield return new object[] {
                FuncList.Make<Func<int, int>>(x => x + 1),
                FuncList.Make(4),
                FuncList.Make(5),
            };

            yield return new object[] {
                FuncList.Make<Func<int, int>>(x => x + 1),
                FuncList.Make(1, 2, 3),
                FuncList.Make(2, 3, 4),
            };

            yield return new object[] {
                FuncList.Make<Func<int, int>>(x => x + 1, x => x * 2),
                FuncList.Make(5),
                FuncList.Make(6, 10),
            };

            yield return new object[] {
                FuncList.Make<Func<int, int>>(x => x + 1, x => x * 2),
                FuncList.Make(5, 6, 7),
                FuncList.Make(6, 7, 8, 10, 12, 14),
            };
        }

        public static IEnumerable<object[]> Ap2Data()
        {
            yield return new object[] {
                FuncList.Make<Func<int, Func<int, int>>>(),
                FuncList.Make<int>(),
                FuncList.Make<int>(),
                FuncList.Make<int>(),
            };

            yield return new object[] {
                FuncList.Make<Func<int, Func<int, int>>>(),
                FuncList.Make(1),
                FuncList.Make<int>(),
                FuncList.Make<int>(),
            };

            yield return new object[] {
                FuncList.Make<Func<int, Func<int, int>>>(),
                FuncList.Make(1),
                FuncList.Make(1),
                FuncList.Make<int>(),
            };

            yield return new object[] {
                FuncList.Make<Func<int, Func<int, int>>>(x => y => x + y),
                FuncList.Make<int>(),
                FuncList.Make<int>(),
                FuncList.Make<int>(),
            };

            yield return new object[] {
                FuncList.Make<Func<int, Func<int, int>>>(x => y => x + y),
                FuncList.Make(4),
                FuncList.Make(6),
                FuncList.Make(10),
            };

            yield return new object[] {
                FuncList.Make<Func<int, Func<int, int>>>(x => y => x + y),
                FuncList.Make(1, 2, 3),
                FuncList.Make(7, 8, 9),
                FuncList.Make(8, 9, 10, 9, 10, 11, 10, 11, 12),
            };

            yield return new object[] {
                FuncList.Make<Func<int, Func<int, int>>>(x => y => x + y, x => y => x * y),
                FuncList.Make(5),
                FuncList.Make(8),
                FuncList.Make(13, 40),
            };

            yield return new object[] {
                FuncList.Make<Func<int, Func<int, int>>>(x => y => x + y, x => y => x * y),
                FuncList.Make(5, 6, 7),
                FuncList.Make(10, 20, 30),
                FuncList.Make(15, 25, 35, 16, 26, 36, 17, 27, 37, 50, 100, 150, 60, 120, 180, 70, 140, 210),
            };
        }

        public static IEnumerable<object[]> EqualsData()
        {
            yield return new object[] {
                FuncList.Make<int>(),
                FuncList.Make<int>(),
                true
            };

            yield return new object[] {
                FuncList.Make(1),
                FuncList.Make(1),
                true
            };

            yield return new object[] {
                FuncList.Make(1,2,3),
                FuncList.Make(1,2,3),
                true
            };

            yield return new object[] {
                FuncList.Make(4),
                FuncList.Make<int>(),
                false
            };

            yield return new object[] {
                FuncList.Make<int>(),
                FuncList.Make(7),
                false
            };

            yield return new object[] {
                FuncList.Make(1,2),
                FuncList.Make(5,6),
                false
            };

            yield return new object[] {
                FuncList.Make(1,2),
                FuncList.Make(1),
                false
            };

            yield return new object[] {
                FuncList.Make(1,2,3),
                FuncList.Make(1,2,3,4),
                false
            };
        }

        [Theory]
        [MemberData(nameof(Ap1Data))]
        public static void FuncListAp1(FuncList<Func<int, int>> funcs, FuncList<int> args, IEnumerable<int> expected)
        {
            var res = args.Ap(funcs);

            Assert.Equal(expected, res as IEnumerable<int>);
        }

        [Theory]
        [MemberData(nameof(Ap2Data))]
        public static void FuncListAp2(FuncList<Func<int, Func<int, int>>> funcs, FuncList<int> args1, FuncList<int> args2, IEnumerable<int> expected)
        {
            var res = args2.Ap(args1.Ap(funcs));

            Assert.Equal(expected, res as IEnumerable<int>);
        }

        [Theory]
        [MemberData(nameof(EqualsData))]
        public static void FuncListEquals(FuncList<int> xs, IList<int> ys, bool expected)
        {
            Assert.Equal(expected, xs.Equals(ys));
        }

        [Theory]
        [InlineData(null, 3)]
        [InlineData(new int[0], 3)]
        [InlineData(new int[] { 2 }, 3)]
        [InlineData(new int[] { 1, 2, 3 }, 3)]
        [InlineData(new int[] { 5, 4, 6, 7, 8 }, 47)]
        public static void FuncListAdd(int[] xs, int elem)
        {
            var fl = new FuncList<int>(xs);
            var origLength = fl.Count;
            fl.Add(elem);

            var list = new List<int>(xs ?? new int[0]);
            list.Add(elem);

            Assert.Equal(list, fl);
            Assert.Equal(origLength + 1, fl.Count);
        }

        [Theory]
        [InlineData(new int[] { }, new int[] { })]
        [InlineData(new int[] { 1 }, new int[] { 1 })]
        [InlineData(new int[] { 1, 2, 3 }, new int[] { 1, 2, 3 })]
        [InlineData(new int[] { 5, 4, 2, 8, 3, 8, 2, 0 }, new int[] { 5, 4, 2, 8, 3, 8, 2, 0 })]
        [InlineData(new int[] { 4 }, new int[] { })]
        [InlineData(new int[] { }, new int[] { 7 })]
        [InlineData(new int[] { 1, 5 }, new int[] { 1 })]
        [InlineData(new int[] { 1, 2, 3 }, new int[] { 1, 2, 3, 4 })]
        [InlineData(new int[] { 5, 4, 2, 8, 3, 8, 2, 0 }, new int[] { 5, 4, 2, 8, 8, 8, 3, 8, 2, 0 })]
        public static void EqualTest(int[] xs, int[] ys)
        {
            Assert.Equal(xs.SequenceEqual(ys), new FuncList<int>(xs).Equals(new FuncList<int>(ys)));
        }

        [Theory]
        [InlineData(new int[] { }, new int[] { })]
        [InlineData(new int[] { 1 }, new int[] { 1 })]
        [InlineData(new int[] { 1, 2, 3 }, new int[] { 1, 2, 3 })]
        [InlineData(new int[] { 5, 4, 2, 8, 3, 8, 2, 0 }, new int[] { 5, 4, 2, 8, 3, 8, 2, 0 })]
        [InlineData(new int[] { 4 }, new int[] { })]
        [InlineData(new int[] { }, new int[] { 7 })]
        [InlineData(new int[] { 1, 5 }, new int[] { 1 })]
        [InlineData(new int[] { 1, 2, 3 }, new int[] { 1, 2, 3, 4 })]
        [InlineData(new int[] { 5, 4, 2, 8, 3, 8, 2, 0 }, new int[] { 5, 4, 2, 8, 8, 8, 3, 8, 2, 0 })]
        public static void HashCodeTest(int[] xs, int[] ys)
        {
            if (xs.SequenceEqual(ys))
                Assert.Equal(new FuncList<int>(xs).GetHashCode(), new FuncList<int>(ys).GetHashCode());
            else
                Assert.NotEqual(new FuncList<int>(xs).GetHashCode(), new FuncList<int>(ys).GetHashCode());
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("a", "a")]
        [InlineData("ab", "")]
        [InlineData("ab", "c")]
        [InlineData("ab", "ba")]
        [InlineData("abc", "cba")]
        [InlineData("abc", "acb")]
        [InlineData("abc", "bac")]
        public static void HashCodeTest2(IEnumerable<char> xs, IEnumerable<char> ys)
        {
            if (xs.SequenceEqual(ys))
                Assert.Equal(new FuncList<char>(xs).GetHashCode(), new FuncList<char>(ys).GetHashCode());
            else
                Assert.NotEqual(new FuncList<char>(xs).GetHashCode(), new FuncList<char>(ys).GetHashCode());
        }
    }
}

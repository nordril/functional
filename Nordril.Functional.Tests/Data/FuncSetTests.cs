using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Nordril.Functional.Tests.Data
{
    public class FuncSetTests
    {
        public static IEnumerable<object[]> Ap1Data()
        {
            yield return new object[] {
                FuncSet.Make<Func<int, int>>(),
                FuncSet.Make<int>(),
                FuncSet.Make<int>(),
            };

            yield return new object[] {
                FuncSet.Make<Func<int, int>>(),
                FuncSet.Make(1),
                FuncSet.Make<int>(),
            };

            yield return new object[] {
                FuncSet.Make<Func<int, int>>(x => x + 1),
                FuncSet.Make<int>(),
                FuncSet.Make<int>(),
            };

            yield return new object[] {
                FuncSet.Make<Func<int, int>>(x => x + 1),
                FuncSet.Make(4),
                FuncSet.Make(5),
            };

            yield return new object[] {
                FuncSet.Make<Func<int, int>>(x => x + 1),
                FuncSet.Make(1, 2, 3),
                FuncSet.Make(2, 3, 4),
            };

            yield return new object[] {
                FuncSet.Make<Func<int, int>>(x => x + 1, x => x * 2),
                FuncSet.Make(5),
                FuncSet.Make(6, 10),
            };

            yield return new object[] {
                FuncSet.Make<Func<int, int>>(x => x + 1, x => x * 2),
                FuncSet.Make(5, 6, 7),
                FuncSet.Make(6, 7, 8, 10, 12, 14),
            };
        }

        public static IEnumerable<object[]> Ap2Data()
        {
            yield return new object[] {
                FuncSet.Make<Func<int, Func<int, int>>>(),
                FuncSet.Make<int>(),
                FuncSet.Make<int>(),
                FuncSet.Make<int>(),
            };

            yield return new object[] {
                FuncSet.Make<Func<int, Func<int, int>>>(),
                FuncSet.Make(1),
                FuncSet.Make<int>(),
                FuncSet.Make<int>(),
            };

            yield return new object[] {
                FuncSet.Make<Func<int, Func<int, int>>>(),
                FuncSet.Make(1),
                FuncSet.Make(1),
                FuncSet.Make<int>(),
            };

            yield return new object[] {
                FuncSet.Make<Func<int, Func<int, int>>>(x => y => x + y),
                FuncSet.Make<int>(),
                FuncSet.Make<int>(),
                FuncSet.Make<int>(),
            };

            yield return new object[] {
                FuncSet.Make<Func<int, Func<int, int>>>(x => y => x + y),
                FuncSet.Make(4),
                FuncSet.Make(6),
                FuncSet.Make(10),
            };

            yield return new object[] {
                FuncSet.Make<Func<int, Func<int, int>>>(x => y => x + y),
                FuncSet.Make(1, 2, 3),
                FuncSet.Make(7, 8, 9),
                FuncSet.Make(8, 9, 10, 9, 10, 11, 10, 11, 12),
            };

            yield return new object[] {
                FuncSet.Make<Func<int, Func<int, int>>>(x => y => x + y, x => y => x * y),
                FuncSet.Make(5),
                FuncSet.Make(8),
                FuncSet.Make(13, 40),
            };

            yield return new object[] {
                FuncSet.Make<Func<int, Func<int, int>>>(x => y => x + y, x => y => x * y),
                FuncSet.Make(5, 6, 7),
                FuncSet.Make(10, 20, 30),
                FuncSet.Make(15, 25, 35, 16, 26, 36, 17, 27, 37, 50, 100, 150, 60, 120, 180, 70, 140, 210),
            };
        }

        public static IEnumerable<object[]> EqualsData()
        {
            yield return new object[] {
                FuncSet.Make<int>(),
                FuncSet.Make<int>(),
                true
            };

            yield return new object[] {
                FuncSet.Make(1),
                FuncSet.Make(1),
                true
            };

            yield return new object[] {
                FuncSet.Make(1,2,3),
                FuncSet.Make(1,2,3),
                true
            };

            yield return new object[] {
                FuncSet.Make(4),
                FuncSet.Make<int>(),
                false
            };

            yield return new object[] {
                FuncSet.Make<int>(),
                FuncSet.Make(7),
                false
            };

            yield return new object[] {
                FuncSet.Make(1,2),
                FuncSet.Make(5,6),
                false
            };

            yield return new object[] {
                FuncSet.Make(1,2),
                FuncSet.Make(1),
                false
            };

            yield return new object[] {
                FuncSet.Make(1,2,3),
                FuncSet.Make(1,2,3,4),
                false
            };
        }

        [Theory]
        [MemberData(nameof(Ap1Data))]
        public static void FuncSetAp1(FuncSet<Func<int, int>> funcs, FuncSet<int> args, IEnumerable<int> expected)
        {
            var res = args.Ap(funcs);

            Assert.Equal(expected, res as IEnumerable<int>);
        }

        [Theory]
        [MemberData(nameof(Ap2Data))]
        public static void FuncSetAp2(FuncSet<Func<int, Func<int, int>>> funcs, FuncSet<int> args1, FuncSet<int> args2, IEnumerable<int> expected)
        {
            var res = args2.Ap(args1.Ap(funcs));

            Assert.Equal(expected, res as IEnumerable<int>);
        }

        [Theory]
        [MemberData(nameof(EqualsData))]
        public static void FuncSetEquals(FuncSet<int> xs, ISet<int> ys, bool expected)
        {
            Assert.Equal(expected, xs.Equals(ys));
        }

        [Theory]
        [InlineData(null, 3)]
        [InlineData(new int[0], 3)]
        [InlineData(new int[] { 2 }, 3)]
        [InlineData(new int[] { 1, 2, 3 }, 3)]
        [InlineData(new int[] { 5, 4, 6, 7, 8 }, 47)]
        public static void FuncSetAdd(int[] xs, int elem)
        {
            var fl = new FuncSet<int>(xs);
            var origLength = fl.Count;
            fl.Add(elem);

            var set = new HashSet<int>(xs ?? new int[0]) { elem };

            Assert.Equal(set, fl);
            Assert.Equal(set.Count, fl.Count);
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
        public static void EqualTest(int[] xs, int[] ys)
        {
            Assert.Equal(xs.ToHashSet().SequenceEqual(ys.ToHashSet()), new FuncSet<int>(xs).Equals(new FuncSet<int>(ys)));
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
            if (xs.ToHashSet().SequenceEqual(ys.ToHashSet()))
                Assert.Equal(new FuncSet<int>(xs).GetHashCode(), new FuncSet<int>(ys).GetHashCode());
            else
                Assert.NotEqual(new FuncSet<int>(xs).GetHashCode(), new FuncSet<int>(ys).GetHashCode());
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
                Assert.Equal(new FuncSet<char>(xs).GetHashCode(), new FuncSet<char>(ys).GetHashCode());
            else
                Assert.NotEqual(new FuncSet<char>(xs).GetHashCode(), new FuncSet<char>(ys).GetHashCode());
        }

        [Theory]
        [InlineData(new int[0], new int[0], true)]
        [InlineData(new int[] { 3 }, new int[0], false)]
        [InlineData(new int[] { 3, 13 }, new int[0], false)]
        [InlineData(new int[] { 3, 13 }, new int[] { 4, 20 }, false)]
        [InlineData(new int[] { 3, 23 }, new int[] { 4, 20 }, true)]
        [InlineData(new int[] { 3, 23 }, new int[] { 4, 20, 30 }, false)]
        [InlineData(new int[] { 3, 23, 30 }, new int[] { 4, 35, 30 }, false)]
        [InlineData(new int[] { 3, 33, 30 }, new int[] { 4, 35, 30 }, true)]
        public static void SetEqualsTest(IEnumerable<int> xs, IEnumerable<int> ys, bool expected)
        {
            //Ignores the last digit.
            var comparer = new FuncEqualityComparer<int>((x, y) =>
            {
                x = x / 10;
                y = y / 10;

                return x == y;
            }, x => (x/10).GetHashCode());

            var xfs = new FuncSet<int>(comparer, xs);
            var yfs = new HashSet<int>(ys, comparer);

            Assert.Equal(expected, xfs.SetEquals(yfs));
        }

        [Fact]
        public static void LinqSelectTest()
        {
            var res =
                from x in FuncSet.Make(4, 3, 2)
                select x * 2;

            Assert.Equal(new int[] { 8, 6, 4 }, res);
        }

        [Fact]
        public static void LinqSelectManyTest()
        {
            var res =
                from x in FuncSet.Make(2, 3, 5)
                from y in FuncSet.Make(7, 11, 13)
                select x * y;

            Assert.Equal(new int[] { 14, 22, 26, 21, 33, 39, 35, 55, 65 }, res);
        }

        [Fact]
        public static async Task LinqSelectAsyncTest()
        {
            var res =
                from x in Task.FromResult(Lst.Make(4, 3, 2))
                select x * 2;

            Assert.Equal(new int[] { 8, 6, 4 }, await res);
        }

        [Fact]
        public static async Task LinqSelectManyAsyncTest()
        {
            var res =
                from x in Task.FromResult(Lst.Make(2, 3, 5))
                from y in Task.FromResult(Lst.Make(7, 11, 13))
                select x * y;

            Assert.Equal(new int[] { 14, 22, 26, 21, 33, 39, 35, 55, 65 }, await res);
        }
    }
}

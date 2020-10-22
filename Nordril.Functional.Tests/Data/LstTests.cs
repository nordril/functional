using Nordril.Functional.Algebra;
using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Nordril.Functional.Tests.Data
{
    public sealed class LstTests
    {
        public static IEnumerable<object[]> Ap1Data()
        {
            yield return new object[] {
                Lst.Make<Func<int, int>>(),
                Lst.Make<int>(),
                Lst.Make<int>(),
            };

            yield return new object[] {
                Lst.Make<Func<int, int>>(),
                Lst.Make(1),
                Lst.Make<int>(),
            };

            yield return new object[] {
                Lst.Make<Func<int, int>>(x => x + 1),
                Lst.Make<int>(),
                Lst.Make<int>(),
            };

            yield return new object[] {
                Lst.Make<Func<int, int>>(x => x + 1),
                Lst.Make(4),
                Lst.Make(5),
            };

            yield return new object[] {
                Lst.Make<Func<int, int>>(x => x + 1),
                Lst.Make(1, 2, 3),
                Lst.Make(2, 3, 4),
            };

            yield return new object[] {
                Lst.Make<Func<int, int>>(x => x + 1, x => x * 2),
                Lst.Make(5),
                Lst.Make(6, 10),
            };

            yield return new object[] {
                Lst.Make<Func<int, int>>(x => x + 1, x => x * 2),
                Lst.Make(5, 6, 7),
                Lst.Make(6, 7, 8, 10, 12, 14),
            };
        }

        public static IEnumerable<object[]> Ap2Data()
        {
            yield return new object[] {
                Lst.Make<Func<int, Func<int, int>>>(),
                Lst.Make<int>(),
                Lst.Make<int>(),
                Lst.Make<int>(),
            };

            yield return new object[] {
                Lst.Make<Func<int, Func<int, int>>>(),
                Lst.Make(1),
                Lst.Make<int>(),
                Lst.Make<int>(),
            };

            yield return new object[] {
                Lst.Make<Func<int, Func<int, int>>>(),
                Lst.Make(1),
                Lst.Make(1),
                Lst.Make<int>(),
            };

            yield return new object[] {
                Lst.Make<Func<int, Func<int, int>>>(x => y => x + y),
                Lst.Make<int>(),
                Lst.Make<int>(),
                Lst.Make<int>(),
            };

            yield return new object[] {
                Lst.Make<Func<int, Func<int, int>>>(x => y => x + y),
                Lst.Make(4),
                Lst.Make(6),
                Lst.Make(10),
            };

            yield return new object[] {
                Lst.Make<Func<int, Func<int, int>>>(x => y => x + y),
                Lst.Make(1, 2, 3),
                Lst.Make(7, 8, 9),
                Lst.Make(8, 9, 10, 9, 10, 11, 10, 11, 12),
            };

            yield return new object[] {
                Lst.Make<Func<int, Func<int, int>>>(x => y => x + y, x => y => x * y),
                Lst.Make(5),
                Lst.Make(8),
                Lst.Make(13, 40),
            };

            yield return new object[] {
                Lst.Make<Func<int, Func<int, int>>>(x => y => x + y, x => y => x * y),
                Lst.Make(5, 6, 7),
                Lst.Make(10, 20, 30),
                Lst.Make(15, 25, 35, 16, 26, 36, 17, 27, 37, 50, 100, 150, 60, 120, 180, 70, 140, 210),
            };
        }

        public static IEnumerable<object[]> EqualsData()
        {
            yield return new object[] {
                Lst.Make<int>(),
                Lst.Make<int>(),
                true
            };

            yield return new object[] {
                Lst.Make(1),
                Lst.Make(1),
                true
            };

            yield return new object[] {
                Lst.Make(1,2,3),
                Lst.Make(1,2,3),
                true
            };

            yield return new object[] {
                Lst.Make(4),
                Lst.Make<int>(),
                false
            };

            yield return new object[] {
                Lst.Make<int>(),
                Lst.Make(7),
                false
            };

            yield return new object[] {
                Lst.Make(1,2),
                Lst.Make(5,6),
                false
            };

            yield return new object[] {
                Lst.Make(1,2),
                Lst.Make(1),
                false
            };

            yield return new object[] {
                Lst.Make(1,2,3),
                Lst.Make(1,2,3,4),
                false
            };
        }

        [Theory]
        [MemberData(nameof(Ap1Data))]
        public static void FuncListAp1(Lst<Func<int, int>> funcs, Lst<int> args, IEnumerable<int> expected)
        {
            var res = args.Ap(funcs);

            Assert.Equal(expected, res as IEnumerable<int>);
        }

        [Theory]
        [MemberData(nameof(Ap2Data))]
        public static void FuncListAp2(Lst<Func<int, Func<int, int>>> funcs, Lst<int> args1, Lst<int> args2, IEnumerable<int> expected)
        {
            var res = args2.Ap(args1.Ap(funcs));

            Assert.Equal(expected, res as IEnumerable<int>);
        }

        [Theory]
        [MemberData(nameof(EqualsData))]
        public static void FuncListEquals(Lst<int> xs, IEnumerable<int> ys, bool expected)
        {
            Assert.Equal(expected, xs.Equals(ys));
        }

        [Theory]
        [InlineData(null, 3)]
        [InlineData(new int[0], 3)]
        [InlineData(new int[] { 2 }, 3)]
        [InlineData(new int[] { 1, 2, 3 }, 3)]
        [InlineData(new int[] { 5, 4, 6, 7, 8 }, 47)]
        public static void FuncListAppend(int[] xs, int elem)
        {
            var fl = new Lst<int>(xs);
            var origLength = fl.Count;
            var fl2 = fl.Append(elem);

            var list = new List<int>(xs ?? new int[0])
            {
                elem
            };

            Assert.Equal(list, fl2);
            Assert.Equal(origLength, fl.Count);
            Assert.Equal(origLength + 1, fl2.Count);
        }

        [Theory]
        [InlineData(null, 3)]
        [InlineData(new int[0], 3)]
        [InlineData(new int[] { 2 }, 3)]
        [InlineData(new int[] { 1, 2, 3 }, 3)]
        [InlineData(new int[] { 5, 4, 6, 7, 8 }, 47)]
        public static void FuncListPrepend(int[] xs, int elem)
        {
            var fl = new Lst<int>(xs);
            var origLength = fl.Count;
            var fl2 = fl.Prepend(elem);

            var list = new List<int>(xs ?? new int[0]);
            list.Insert(0, elem);

            Assert.Equal(list, fl2);
            Assert.Equal(origLength, fl.Count);
            Assert.Equal(origLength + 1, fl2.Count);
        }

        [Theory]
        [InlineData(null, new int[] { 3, 7, 8})]
        [InlineData(new int[0], new int[] { 3, 7, 8 })]
        [InlineData(new int[] { 2 }, new int[] { 3, 7, 8 })]
        [InlineData(new int[] { 1, 2, 3 }, new int[] { 3, 7, 8 })]
        [InlineData(new int[] { 5, 4, 6, 7, 8 }, new int[] { 3, 7, 8 })]
        public static void FuncListAppendRange(int[] xs, IEnumerable<int> elem)
        {
            var fl = new Lst<int>(xs);
            var origLength = fl.Count;
            var fl2 = fl.AppendRange(elem);

            var list = new List<int>(xs ?? new int[0]);
            list.AddRange(elem);

            Assert.Equal(list, fl2);
            Assert.Equal(origLength, fl.Count);
            Assert.Equal(origLength + elem.Count(), fl2.Count);
        }

        [Theory]
        [InlineData(null, new int[] { 3, 7, 8 })]
        [InlineData(new int[0], new int[] { 3, 7, 8 })]
        [InlineData(new int[] { 2 }, new int[] { 3, 7, 8 })]
        [InlineData(new int[] { 1, 2, 3 }, new int[] { 3, 7, 8 })]
        [InlineData(new int[] { 5, 4, 6, 7, 8 }, new int[] { 3, 7, 8 })]
        public static void FuncListPrependRange(int[] xs, IEnumerable<int> elem)
        {
            var fl = new Lst<int>(xs);
            var origLength = fl.Count;
            var fl2 = fl.PrependRange(elem);

            var list = elem.Concat(new List<int>(xs ?? new int[0])).ToList();

            Assert.Equal(list, fl2);
            Assert.Equal(origLength, fl.Count);
            Assert.Equal(origLength + elem.Count(), fl2.Count);
        }

        [Theory]
        [InlineData(true, new int[] { }, new int[] { })]
        [InlineData(true, new int[] { 1 }, new int[] { 1 })]
        [InlineData(true, new int[] { 1, 2, 3 }, new int[] { 1, 2, 3 })]
        [InlineData(true, new int[] { 5, 4, 2, 8, 3, 8, 2, 0 }, new int[] { 5, 4, 2, 8, 3, 8, 2, 0 })]
        [InlineData(false, new int[] { 4 }, new int[] { })]
        [InlineData(false, new int[] { }, new int[] { 7 })]
        [InlineData(false, new int[] { 1, 5 }, new int[] { 1 })]
        [InlineData(false, new int[] { 1, 5 }, new int[] { 5, 1 })]
        [InlineData(false, new int[] { 1, 2, 3 }, new int[] { 1, 2, 3, 4 })]
        [InlineData(false, new int[] { 5, 4, 2, 8, 3, 8, 2, 0 }, new int[] { 5, 4, 2, 8, 8, 8, 3, 8, 2, 0 })]
        [InlineData(false, new int[] { 5, 4, 2, 8, 3, 8, 2, 0 }, new int[] { 5, 4, 2, 8, 8, 8, 3, 8, 2, 0, 7 })]
        [InlineData(false, new int[] { 5, 4, 2, 8, 3, 8, 2, 0 }, new int[] { 2, 4, 2, 8, 8, 8, 3, 8, 2, 0 })]
        public static void EqualTest(bool expectedEqual, int[] xs, int[] ys)
        {
            if (expectedEqual)
                Assert.True(new Lst<int>(xs).Equals(new Lst<int>(ys)));
            else
                Assert.False(new Lst<int>(xs).Equals(new Lst<int>(ys)));
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
                Assert.Equal(new Lst<int>(xs).GetHashCode(), new Lst<int>(ys).GetHashCode());
            else
                Assert.NotEqual(new Lst<int>(xs).GetHashCode(), new Lst<int>(ys).GetHashCode());
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
                Assert.Equal(new Lst<char>(xs).GetHashCode(), new Lst<char>(ys).GetHashCode());
            else
                Assert.NotEqual(new Lst<char>(xs).GetHashCode(), new Lst<char>(ys).GetHashCode());
        }

        [Theory]
        [InlineData(new char[] { })]
        [InlineData(new char[] { 'x' })]
        [InlineData(new char[] { 'a', 'b' })]
        [InlineData(new char[] { 'b', 'f', 'a', 'á' })]
        public static void FoldMapTest(IEnumerable<char> xs)
        {
            var actual = new Lst<char>(xs).FoldMap(new Monoid<string>("", (x, y) => x + y), c => c + "");
            var expected = new string(xs.ToArray());

            Assert.Equal(expected, actual);

        }

        [Fact]
        public static void LinqSelectTest()
        {
            var res =
                from x in Lst.Make(4, 3, 2)
                select x * 2;

            Assert.Equal(new int[] { 8, 6, 4 }, res);
        }

        [Fact]
        public static void LinqSelectManyTest()
        {
            var res =
                from x in Lst.Make(2, 3, 5)
                from y in Lst.Make(7,11,13)
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

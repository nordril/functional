using Nordril.Functional.Algebra;
using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Nordril.Functional.Tests.Algebra
{
    public static class RelationsTests
    {
        [Fact]
        public static void ContainsTest()
        {
            var r = Relations.Make<string, int>((s, i) => s.Length == i);

            Assert.True(r.Contains("", 0));
            Assert.True(r.Contains("x", 1));
            Assert.True(r.Contains("xyz", 3));

            Assert.False(r.Contains("", 1));
            Assert.False(r.Contains("x", 0));
            Assert.False(r.Contains("x", -1));
            Assert.False(r.Contains("xyz", 88));
        }

        [Theory]
        [InlineData(1,1, true)]
        [InlineData(1,3, true)]
        [InlineData(1,6, true)]
        [InlineData(3, 1, false)]
        [InlineData(2, 3, false)]
        [InlineData(6, 3, false)]
        public static void PartialOrderTest(int x, int y, bool shouldContain)
        {
            var r = PartialOrder.Make<int>((x, y) => Maybe.JustIf(y >= x, () => y % x == 0));

            if (shouldContain)
            {
                Assert.True(r.Leq(x, y));
                Assert.True(r.Contains(x, y));
            }
            else
            {
                Assert.False(r.Leq(x, y));
                Assert.False(r.Contains(x, y));
            }
        }

        [Theory]
        [InlineData(1, 1, true)]
        [InlineData(1, 3, true)]
        [InlineData(1, 6, true)]
        [InlineData(5, 6, true)]
        [InlineData(3, 1, false)]
        [InlineData(6, 3, false)]
        public static void PartialOrderPartialLeqTest(int x, int y, bool shouldContain)
        {
            var r = PartialOrder.Make<int>((x, y) => Maybe.JustIf(x <= y, () => true));

            if (shouldContain)
            {
                Assert.True(r.Leq(x, y));
                Assert.True(r.Contains(x, y));
            }
            else
            {
                Assert.False(r.Leq(x, y));
                Assert.False(r.Contains(x, y));
            }
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(1, 0)]
        [InlineData(0, 0)]
        [InlineData(4, 4)]
        [InlineData(-3, 4)]
        [InlineData(8, 7)]
        [InlineData(8, 1)]
        public static void TotalOrderTest(int x, int y)
        {
            var r = TotalOrder.Make<int>((x, y) => x < y ? (short)-1 : x > y ? (short)1 : (short)0);

            Assert.Equal(x < y, r.Le(x, y));
            Assert.Equal(x <= y, r.Leq(x, y));
            Assert.Equal(x == y, r.Eq(x, y));
            Assert.Equal(x != y, r.Neq(x, y));
            Assert.Equal(x >= y, r.Geq(x, y));
            Assert.Equal(x > y, r.Ge(x, y));
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(1, 0)]
        [InlineData(0, 0)]
        [InlineData(4, 4)]
        [InlineData(-3, 4)]
        [InlineData(8, 7)]
        [InlineData(8, 1)]
        public static void FromEquatableTest(int x, int y)
        {
            var r = PartialOrder.FromEquatable<int>();

            Assert.Equal(x == y, r.Leq(x, y));
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(1, 0)]
        [InlineData(0, 0)]
        [InlineData(4, 4)]
        [InlineData(-3, 4)]
        [InlineData(8, 7)]
        [InlineData(8, 1)]
        public static void FromComparableTest(int x, int y)
        {
            var r = TotalOrder.FromComparable<int>();

            Assert.Equal(x < y, r.Le(x, y));
            Assert.Equal(x <= y, r.Leq(x, y));
            Assert.Equal(x == y, r.Eq(x, y));
            Assert.Equal(x != y, r.Neq(x, y));
            Assert.Equal(x >= y, r.Geq(x, y));
            Assert.Equal(x > y, r.Ge(x, y));
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public static void OneToOneRelationTest(bool useTuples)
        {
            var arr = Array.Empty<(int, double)>();
            var r = useTuples
                ? Relations.AsBijective(arr)
                : Relations.AsBijective(arr.Select(t => KeyValuePair.Create(t.Item1, t.Item2)));

            Assert.Empty(r);

            Assert.False(r.MaybeResult(3).HasValue);
            Assert.False(r.Contains(1, 3D));
            Assert.False(r.Contains(2, 2D));
            Assert.False(r.GetMaybeLeft(3D).HasValue);
            Assert.False(r.GetMaybeRight(3).HasValue);

            arr = new[] { (1, 2D) };
            r = useTuples
                ? Relations.AsBijective(arr)
                : Relations.AsBijective(arr.Select(t => KeyValuePair.Create(t.Item1, t.Item2)));

            Assert.Equal(new (int, double)[] { (1, 2D) }, r);

            Assert.True(r.MaybeResult(1).HasValue);
            Assert.Equal(2D, r.MaybeResult(1).Value());
            Assert.False(r.MaybeResult(3).HasValue);
            Assert.True(r.Contains(1, 2D));
            Assert.False(r.Contains(1, 3D));
            Assert.False(r.Contains(2, 2D));
            Assert.True(r.GetMaybeLeft(2D).HasValue);
            Assert.Equal(1, r.GetMaybeLeft(2D).Value());
            Assert.True(r.GetMaybeRight(1).HasValue);
            Assert.Equal(2D, r.GetMaybeRight(1).Value());
            Assert.False(r.GetMaybeLeft(3D).HasValue);
            Assert.False(r.GetMaybeRight(3).HasValue);

            arr = new[] { (1, 2D), (2, 4D), (3, 6D) };
            r = useTuples
                ? Relations.AsBijective(arr)
                : Relations.AsBijective(arr.Select(t => KeyValuePair.Create(t.Item1, t.Item2)));

            Assert.Equal(new (int, double)[] { (1, 2D), (2,4D), (3,6D) }, r.OrderBy(x => x.Item1));

            Assert.True(r.MaybeResult(1).HasValue);
            Assert.Equal(2D, r.MaybeResult(1).Value());
            Assert.Equal(4D, r.MaybeResult(2).Value());
            Assert.Equal(6D, r.MaybeResult(3).Value());
            Assert.False(r.MaybeResult(4).HasValue);
            Assert.True(r.Contains(1, 2D));
            Assert.True(r.Contains(2, 4D));
            Assert.True(r.Contains(3, 6D));
            Assert.False(r.Contains(1, 3D));
            Assert.False(r.Contains(2, 2D));
            Assert.True(r.GetMaybeLeft(2D).HasValue);
            Assert.Equal(1, r.GetMaybeLeft(2D).Value());
            Assert.Equal(2, r.GetMaybeLeft(4D).Value());
            Assert.Equal(3, r.GetMaybeLeft(6D).Value());
            Assert.True(r.GetMaybeRight(1).HasValue);
            Assert.Equal(2D, r.GetMaybeRight(1).Value());
            Assert.Equal(4D, r.GetMaybeRight(2).Value());
            Assert.Equal(6D, r.GetMaybeRight(3).Value());
            Assert.False(r.GetMaybeLeft(3D).HasValue);
            Assert.False(r.GetMaybeRight(-1).HasValue);
        }

        [Fact]
        public static void OneToOneRelationErrorsTest()
        {
            //right-keys present twice
            Assert.Throws<KeyAlreadyPresentException>(
                () => Relations.AsBijective(new (int, double)[] {
                    (1, 2D),
                    (2, 3D),
                    (3, 2D),
                }));

            //left-keys present twice
            Assert.Throws<KeyAlreadyPresentException>(
                () => Relations.AsBijective(new (int, double)[] {
                    (1, 2D),
                    (2, 3D),
                    (1, 4D),
                }));

            //left- and right-keys present twice
            Assert.Throws<KeyAlreadyPresentException>(
                () => Relations.AsBijective(new (int, double)[] {
                    (1, 2D),
                    (2, 3D),
                    (3, 2D),
                    (1, 4D),
                }));
        }

        [Fact]
        public static void BijectiveRelationTest()
        {
            var r = Relations.AsBijective(
                (string s) => s.Length,
                (int x) => new string(Enumerable.Repeat('a', x).ToArray()));

            Assert.True(r.Contains("", 0));
            Assert.True(r.Contains("a", 1));
            Assert.True(r.Contains("aa", 2));
            Assert.True(r.Contains("aaa", 3));
            Assert.False(r.Contains("a", 5));
            Assert.Equal("aaa", r.GetLeft(3));
            Assert.Equal(3, r.GetRight("aaa"));
            Assert.True(r.MaybeResult("aaaa").HasValue);
            Assert.Equal(4, r.MaybeResult("aaaa").Value());
        }

        [Fact]
        public static void FunctionRelationTest()
        {
            var r = Relations.AsRelation<string, int>(s => s.Length);

            Assert.True(r.Contains("", 0));
            Assert.True(r.Contains("x", 1));
            Assert.True(r.Contains("xfha", 4));
            Assert.False(r.Contains("h", 2));

            Assert.Equal(0, r.Result(""));
            Assert.Equal(1, r.Result("x"));
            Assert.Equal(4, r.Result("xfha"));

            Assert.Equal(0, r.MaybeResult("").Value());
            Assert.Equal(1, r.MaybeResult("x").Value());
            Assert.Equal(4, r.MaybeResult("xfha").Value());

            var f = Relations.FromRelation<string, int>(r);
        }

        [Fact]
        public static void FromRelationTest()
        {
            var r = Relations.AsRelation<string, int>(s => s.Length);
            var f = Relations.FromRelation(r);

            Assert.Equal(0, f(""));
            Assert.Equal(1, f("x"));
            Assert.Equal(4, f("xfha"));
        }
    }
}

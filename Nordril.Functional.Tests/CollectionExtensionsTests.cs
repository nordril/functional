using Nordril.Functional.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Nordril.Functional.Tests
{
    public static class CollectionExtensionsTests
    {
        public static IEnumerable<object[]> FirstMaybeData()
        {
            yield return new object[] {
                new int[] { },
                Maybe.Nothing<int>()
            };
            yield return new object[] {
                new int[] { 0 },
                Maybe.Just(0)
            };
            yield return new object[] {
                new int[] { 3 },
                Maybe.Just(3)
            };
            yield return new object[] {
                new int[] { 3, 2, 1 },
                Maybe.Just(3)
            };
        }

        public static IEnumerable<object[]> FirstMaybePredicateData()
        {
            Func<int, bool> isEven = i => i % 2 == 0;

            yield return new object[] {
                new int[] { },
                isEven,
                Maybe.Nothing<int>()
            };
            yield return new object[] {
                new int[] { 0 },
                isEven,
                Maybe.Just(0)
            };
            yield return new object[] {
                new int[] { 3 },
                isEven,
                Maybe.Nothing<int>()
            };
            yield return new object[] {
                new int[] { 3, 2, 1 },
                isEven,
                Maybe.Just(2)
            };
            yield return new object[] {
                new int[] { 3, 2, 4, 1 },
                isEven,
                Maybe.Just(2)
            };
            yield return new object[] {
                new int[] { 3, 2, 1, 6 },
                isEven,
                Maybe.Just(2)
            };
            yield return new object[] {
                new int[] { 3, 5, 5, 6 },
                isEven,
                Maybe.Just(6)
            };
            yield return new object[] {
                new int[] { 3, 3, 5, 7 },
                isEven,
                Maybe.Nothing<int>()
            };
        }

        public static IEnumerable<object[]> ProductDecimalTest1Data()
        {
            yield return new object[] { new decimal[] { }, 1 };
            yield return new object[] { new decimal[] { 1 }, 1 };
            yield return new object[] { new decimal[] { 2 }, 2 };
            yield return new object[] { new decimal[] { 2, 3, 4 }, 24 };
            yield return new object[] { new decimal[] { 2, 3, 3.4M, 4 }, 81.6M };
        }

        public static IEnumerable<object[]> ProductIntTest2Data()
        {
            yield return new object[] { new (string, int)[] { }, 1 };
            yield return new object[] { new (string, int)[] { ("a",1) }, 1 };
            yield return new object[] { new (string, int)[] { ("b", 2) }, 2 };
            yield return new object[] { new (string, int)[] { ("b", 2), ("c",3), ("d",4) }, 24 };
            yield return new object[] { new (string, int)[] { ("b",2), ("c", 3), ("d", 4) }, 24 };
        }

        public static IEnumerable<object[]> ProductLongTest2Data()
        {
            yield return new object[] { new(string, long)[] { }, 1 };
            yield return new object[] { new(string, long)[] { ("a", 1) }, 1 };
            yield return new object[] { new(string, long)[] { ("b", 2) }, 2 };
            yield return new object[] { new(string, long)[] { ("b", 2), ("c", 3), ("d", 4) }, 24 };
            yield return new object[] { new(string, long)[] { ("b", 2), ("c", 3), ("d", 4) }, 24 };
        }

        public static IEnumerable<object[]> ProductFloatTest2Data()
        {
            yield return new object[] { new(string, float)[] { }, 1 };
            yield return new object[] { new(string, float)[] { ("a", 1) }, 1 };
            yield return new object[] { new(string, float)[] { ("b", 2) }, 2 };
            yield return new object[] { new(string, float)[] { ("b", 2), ("c", 3), ("d", 4) }, 24 };
            yield return new object[] { new(string, float)[] { ("b", 2), ("c", 3), ("c4", 3.4F), ("d", 4) }, 81.6F };
        }

        public static IEnumerable<object[]> ProductDoubleTest2Data()
        {
            yield return new object[] { new(string, double)[] { }, 1 };
            yield return new object[] { new(string, double)[] { ("a", 1) }, 1 };
            yield return new object[] { new(string, double)[] { ("b", 2) }, 2 };
            yield return new object[] { new(string, double)[] { ("b", 2), ("c", 3), ("d", 4) }, 24 };
            yield return new object[] { new(string, double)[] { ("b", 2), ("c", 3), ("c4", 3.4D), ("d", 4) }, 81.6D };
        }

        public static IEnumerable<object[]> ProductDecimalTest2Data()
        {
            yield return new object[] { new(string, decimal)[] { }, 1 };
            yield return new object[] { new(string, decimal)[] { ("a", 1) }, 1 };
            yield return new object[] { new(string, decimal)[] { ("b", 2) }, 2 };
            yield return new object[] { new(string, decimal)[] { ("b", 2), ("c", 3), ("d", 4) }, 24 };
            yield return new object[] { new(string, decimal)[] { ("b", 2), ("c", 3), ("c4", 3.4M), ("d", 4) }, 81.6M };
        }

        public static IEnumerable<object[]> ProductIntTest3Data()
        {
            yield return new object[] { new int?[] { }, 1 };
            yield return new object[] { new int?[] { null }, 1 };
            yield return new object[] { new int?[] { 1 }, 1 };
            yield return new object[] { new int?[] { 2 }, 2 };
            yield return new object[] { new int?[] { 2, 3, 4 }, 24 };
            yield return new object[] { new int?[] { 2, 3, 4 }, 24 };
            yield return new object[] { new int?[] { 2, 3, null, null, 4, null }, 24 };
        }

        public static IEnumerable<object[]> ProductLongTest3Data()
        {
            yield return new object[] { new long?[] { }, 1 };
            yield return new object[] { new long?[] { null }, 1 };
            yield return new object[] { new long?[] { 1 }, 1 };
            yield return new object[] { new long?[] { 2 }, 2 };
            yield return new object[] { new long?[] { 2, 3, 4 }, 24 };
            yield return new object[] { new long?[] { 2, 3, 4 }, 24 };
            yield return new object[] { new long?[] { 2, 3, null, null, 4, null }, 24 };
        }

        public static IEnumerable<object[]> ProductFloatTest3Data()
        {
            yield return new object[] { new float?[] { }, 1 };
            yield return new object[] { new float?[] { null }, 1 };
            yield return new object[] { new float?[] { 1 }, 1 };
            yield return new object[] { new float?[] { 2 }, 2 };
            yield return new object[] { new float?[] { 2, 3, 4 }, 24 };
            yield return new object[] { new float?[] { 2, 3, 4 }, 24 };
            yield return new object[] { new float?[] { 2, 3, null, null, 4, null }, 24 };
            yield return new object[] { new float?[] { 2, 3, 1.2F, null, 3.5F, null, 4, null }, 100.8F };
        }

        public static IEnumerable<object[]> ProductDoubleTest3Data()
        {
            yield return new object[] { new double?[] { }, 1 };
            yield return new object[] { new double?[] { null }, 1 };
            yield return new object[] { new double?[] { 1 }, 1 };
            yield return new object[] { new double?[] { 2 }, 2 };
            yield return new object[] { new double?[] { 2, 3, 4 }, 24 };
            yield return new object[] { new double?[] { 2, 3, 4 }, 24 };
            yield return new object[] { new double?[] { 2, 3, null, null, 4, null }, 24 };
            yield return new object[] { new double?[] { 2, 3, 1.2D, null, 3.5D, null, 4, null }, 100.8D };
        }

        public static IEnumerable<object[]> ProductDecimalTest3Data()
        {
            yield return new object[] { new decimal?[] { }, 1 };
            yield return new object[] { new decimal?[] { null }, 1 };
            yield return new object[] { new decimal?[] { 1 }, 1 };
            yield return new object[] { new decimal?[] { 2 }, 2 };
            yield return new object[] { new decimal?[] { 2, 3, 4 }, 24 };
            yield return new object[] { new decimal?[] { 2, 3, 4 }, 24 };
            yield return new object[] { new decimal?[] { 2, 3, null, null, 4, null }, 24 };
            yield return new object[] { new decimal?[] { 2, 3, 1.2M, null, 3.5M, null, 4, null }, 100.8M };
        }

        [Theory]
        [InlineData(new bool[] { }, true)]
        [InlineData(new bool[] { true }, true)]
        [InlineData(new bool[] { true, true }, true)]
        [InlineData(new bool[] { false }, false)]
        [InlineData(new bool[] { true, false }, false)]
        [InlineData(new bool[] { true, false, true }, false)]
        public static void AllTest(IEnumerable<bool> xs, bool expected)
        {
            Assert.Equal(expected, xs.All());
        }

        [Theory]
        [InlineData(new bool[] { }, false)]
        [InlineData(new bool[] { true }, true)]
        [InlineData(new bool[] { true, true }, true)]
        [InlineData(new bool[] { false }, false)]
        [InlineData(new bool[] { true, false }, true)]
        [InlineData(new bool[] { true, false, true }, true)]
        [InlineData(new bool[] { true, false, false, true }, true)]
        public static void AnyTrueTest(IEnumerable<bool> xs, bool expected)
        {
            Assert.Equal(expected, xs.AnyTrue());
        }

        [Theory]
        [InlineData(new int[] { }, true)]
        [InlineData(new int[] { 0 }, false)]
        [InlineData(new int[] { 1, 2, 3}, false)]
        public static void EmptyTest(IEnumerable<int> xs, bool expected)
        {
            Assert.Equal(expected, xs.Empty());
        }

        [Theory]
        [MemberData(nameof(FirstMaybeData))]
        public static void FirstMaybeTest(IEnumerable<int> xs, Maybe<int> expected)
        {
            Assert.Equal(xs.FirstMaybe(), expected);
        }

        [Theory]
        [MemberData(nameof(FirstMaybePredicateData))]
        public static void FirstMaybePredicateTest(IEnumerable<int> xs, Func<int, bool> f, Maybe<int> expected)
        {
            Assert.Equal(xs.FirstMaybe(f), expected);
        }

        [Theory]
        [InlineData(new int[] { })]
        [InlineData(new int[] { 1 })]
        [InlineData(new int[] { 1, 2, 3 })]
        public static void ForEachObjectTest(IEnumerable xs)
        {
            var actual = new List<int>();

            xs.ForEach(x => actual.Add((int)x));

            Assert.Equal(xs.Cast<int>(), actual);
        }

        [Theory]
        [InlineData(new int[] { })]
        [InlineData(new int[] { 1 })]
        [InlineData(new int[] { 1, 2, 3 })]
        public static void ForEachObjectTest2(IEnumerable xs)
        {
            var actual = 0;
            var expectedSize = xs.Cast<int>().Count();

            xs.ForEach(x => actual++);

            Assert.Equal(expectedSize, actual);
            Assert.Equal(xs.Cast<int>().Count(), actual);
        }

        [Theory]
        [InlineData(new int[] { })]
        [InlineData(new int[] { 1 })]
        [InlineData(new int[] { 1, 2, 3 })]
        public static void ForEachTest(IEnumerable<int> xs)
        {
            var actual = new List<int>();

            xs.ForEach(x => actual.Add(x));

            Assert.Equal(xs.Cast<int>(), actual);
        }

        [Theory]
        [InlineData(new int[] { })]
        [InlineData(new int[] { 1 })]
        [InlineData(new int[] { 1, 2, 3 })]
        public static void ForEachTest2(IEnumerable<int> xs)
        {
            var actual = 0;
            var expectedSize = xs.Count();

            xs.ForEach(x => actual++);

            Assert.Equal(expectedSize, actual);
            Assert.Equal(xs.Count(), actual);
        }

        [Theory]
        [InlineData(new int[] { }, new bool[] { }, 0)]
        [InlineData(new int[] { 1, 2, 3}, new bool[] { }, 0)]
        [InlineData(new int[] { }, new bool[] { true, true }, 0)]
        [InlineData(new int[] { 1, 5 }, new bool[] { true, true }, 6)]
        [InlineData(new int[] { 1, 5 }, new bool[] { true, true, true }, 6)]
        [InlineData(new int[] { 1, 5, 7, 10 }, new bool[] { true, true }, 6)]
        [InlineData(new int[] { 1, 5, 6 }, new bool[] { true, false, true }, 7)]
        public static void Foreach2Test(IEnumerable<int> xs, IEnumerable<bool> ys, int expected)
        {
            var actual = 0;

            xs.ForEach(ys, (i, b) => { if (b) actual += i; });

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(new int[] { }, new int[] { }, new bool[] { }, 0)]
        [InlineData(new int[] { 1, 2, 3 }, new int[] { 10, 20, 30}, new bool[] { }, 0)]
        [InlineData(new int[] { }, new int[] { 10, 20, 30 }, new bool[] { true, true }, 0)]
        [InlineData(new int[] { 1, 5 }, new int[] { 10, 20, 30 }, new bool[] { true, true }, 110)]
        [InlineData(new int[] { 1, 5 }, new int[] { 10, 20, 30 }, new bool[] { true, true, true }, 110)]
        [InlineData(new int[] { 1, 5, 7, 10 }, new int[] { 10, 20, 30 }, new bool[] { true, true }, 110)]
        [InlineData(new int[] { 1, 5, 6 }, new int[] { 10, 20, 30 }, new bool[] { true, false, true }, 190)]
        public static void Foreach3Test(IEnumerable<int> xs, IEnumerable<int> ys, IEnumerable<bool> zs, int expected)
        {
            var actual = 0;

            xs.ForEach(ys, zs, (i, j, b) => { if (b) actual += i*j; });

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(new int[] { }, new int[] { }, new bool[] { }, new bool[] { }, 0)]
        [InlineData(new int[] { 1, 2, 3 }, new int[] { 10, 20, 30 }, new bool[] { true }, new bool[] { }, 0)]
        [InlineData(new int[] { }, new int[] { 10, 20, 30 }, new bool[] { }, new bool[] { true, true }, 0)]
        [InlineData(new int[] { 1, 5 }, new int[] { 10, 20, 30 }, new bool[] { false, true}, new bool[] { true, true }, 100)]
        public static void Foreach4Test(
            IEnumerable<int> xs,
            IEnumerable<int> ys,
            IEnumerable<bool> zs,
            IEnumerable<bool> us, int expected)
        {
            var actual = 0;

            xs.ForEach(ys, zs, us, (i, j, b, d) => { if (b && d) actual += i * j; });

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(new int[] { }, 5, null)]
        [InlineData(new int[] { 4 }, 5, null)]
        [InlineData(new int[] { 4, 5, 3 }, 5, 1)]
        [InlineData(new int[] { 4, 3, 5 }, 5, 2)]
        [InlineData(new int[] { 5, 4, 5, 3 }, 5, 0)]
        public static void IndexOfTest(IEnumerable<int> xs, int elem, int? expected)
        {
            Assert.Equal(expected, xs.IndexOf(elem));
        }

        [Theory]
        [InlineData(new int[] { }, new int[] { })]
        [InlineData(new int[] { 1 }, new int[] { 1 })]
        [InlineData(new int[] { 1, 11, 111 }, new int[] { 1, 11, 111})]
        [InlineData(new int[] { 1, 2 }, new int[] { 0 })]
        [InlineData(new int[] { 1, 2, 3, 4 }, new int[] { 0 })]
        [InlineData(new int[] { 1, 2, 11, 22 }, new int[] { 0, 11, 22 })]
        [InlineData(new int[] { 1, 2, 11, 15, 22, 24, 3, 25 }, new int[] { 0, 10, 20, 3, 25 })]
        [InlineData(new int[] { 1, 2, 11, 15, 22, 24, 3, 25, 27 }, new int[] { 0, 10, 20, 3, 20 })]
        public static void MergeAdjacentTest(IEnumerable<int> xs, IEnumerable<int> expected)
        {
            var merged = xs.MergeAdjacent((i, j) => Maybe.JustIf((i / 10) == (j / 10), () => (i / 10) * 10));
            Assert.Equal(expected, merged);
        }

        [Theory]
        [InlineData(new int[] { }, new int[] { }, new int[] { })]
        [InlineData(new int[] { 1 }, new int[] { }, new int[] { 1 })]
        [InlineData(new int[] { 2 }, new int[] { 2 }, new int[] { })]
        [InlineData(new int[] { 1, 2, 3, 4, 5 }, new int[] { 2, 4 }, new int[] { 1, 3, 5})]
        [InlineData(new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 2, 4, 6 }, new int[] { 1, 3, 5 })]
        public static void PartitionTest(IEnumerable<int> xs, IEnumerable<int> expectedTrue, IEnumerable<int> expectedFalse)
        {
            var (actualTrue, actualFalse) = xs.Partition(x => x % 2 == 0);

            Assert.Equal(expectedTrue, actualTrue);
            Assert.Equal(expectedFalse, actualFalse);
        }

        [Theory]
        [InlineData(new int[] { }, 1)]
        [InlineData(new int[] { 1 }, 1)]
        [InlineData(new int[] { 2 }, 2)]
        [InlineData(new int[] { 2, 3, 4 }, 24)]
        public static void ProductIntTest1(IEnumerable<int> xs, int expected)
        {
            Assert.Equal(expected, xs.Product());
        }

        [Theory]
        [InlineData(new long[] { }, 1)]
        [InlineData(new long[] { 1 }, 1)]
        [InlineData(new long[] { 2 }, 2)]
        [InlineData(new long[] { 2, 3, 4 }, 24)]
        public static void ProductLongTest1(IEnumerable<long> xs, long expected)
        {
            Assert.Equal(expected, xs.Product());
        }

        [Theory]
        [InlineData(new float[] { }, 1)]
        [InlineData(new float[] { 1 }, 1)]
        [InlineData(new float[] { 2 }, 2)]
        [InlineData(new float[] { 2, 3, 4 }, 24)]
        [InlineData(new float[] { 2, 3, 3.4F, 4 }, 81.6F)]
        public static void ProductFloatTest1(IEnumerable<float> xs, float expected)
        {
            var error = Math.Abs(xs.Product() - expected);
            Assert.True(error < 0.001f);
        }

        [Theory]
        [InlineData(new double[] { }, 1)]
        [InlineData(new double[] { 1 }, 1)]
        [InlineData(new double[] { 2 }, 2)]
        [InlineData(new double[] { 2, 3, 4 }, 24)]
        [InlineData(new double[] { 2, 3, 3.4D, 4 }, 81.6D)]
        public static void ProductDoubleTest1(IEnumerable<double> xs, double expected)
        {
            Assert.Equal(expected, xs.Product());
        }

        [Theory]
        [MemberData(nameof(ProductDecimalTest1Data))]
        public static void ProductDecimalTest1(IEnumerable<decimal> xs, decimal expected)
        {
            Assert.Equal(expected, xs.Product());
        }

        [Theory]
        [MemberData(nameof(ProductIntTest2Data))]
        public static void ProductIntTest2(IEnumerable<(string, int)> xs, int expected)
        {
            Assert.Equal(expected, xs.Product(t => t.Item2));
        }

        [Theory]
        [MemberData(nameof(ProductLongTest2Data))]
        public static void ProductLongTest2(IEnumerable<(string, long)> xs, long expected)
        {
            Assert.Equal(expected, xs.Product(t => t.Item2));
        }

        [Theory]
        [MemberData(nameof(ProductFloatTest2Data))]
        public static void ProductFloatTest2(IEnumerable<(string, float)> xs, float expected)
        {
            var error = Math.Abs(xs.Product(t => t.Item2) - expected);
            Assert.True(error < 0.001f);
        }

        [Theory]
        [MemberData(nameof(ProductDoubleTest2Data))]
        public static void ProductDoubleTest2(IEnumerable<(string, double)> xs, double expected)
        {
            var error = Math.Abs(xs.Product(t => t.Item2) - expected);
            Assert.True(error < 0.00001D);
        }

        [Theory]
        [MemberData(nameof(ProductDecimalTest2Data))]
        public static void ProductDecimalTest2(IEnumerable<(string, decimal)> xs, decimal expected)
        {
            Assert.Equal(expected, xs.Product(t => t.Item2));
        }

        [Theory]
        [MemberData(nameof(ProductIntTest3Data))]
        public static void ProductIntTest3(IEnumerable<int?> xs, int expected)
        {
            Assert.Equal(expected, xs.Product());
        }

        [Theory]
        [MemberData(nameof(ProductLongTest3Data))]
        public static void ProductLongTest3(IEnumerable<long?> xs, long expected)
        {
            Assert.Equal(expected, xs.Product());
        }

        [Theory]
        [MemberData(nameof(ProductFloatTest3Data))]
        public static void ProductFloatTest3(IEnumerable<float?> xs, float expected)
        {
            var error = Math.Abs(xs.Product() - expected);
            Assert.True(error < 0.001f);
        }

        [Theory]
        [MemberData(nameof(ProductDoubleTest3Data))]
        public static void ProductDoubleTest3(IEnumerable<double?> xs, double expected)
        {
            var error = Math.Abs(xs.Product() - expected);
            Assert.True(error < 0.00001D);
        }

        [Theory]
        [MemberData(nameof(ProductDecimalTest3Data))]
        public static void ProductDecimalTest3(IEnumerable<decimal?> xs, decimal expected)
        {
            Assert.Equal(expected, xs.Product());
        }

        [Theory]
        [InlineData(new int[] { }, 0, new int[] { })]
        [InlineData(new int[] { }, -1, new int[] { })]
        [InlineData(new int[] { }, 3, new int[] { })]
        [InlineData(new int[] { 1, }, 1, new int[] { 1 })]
        [InlineData(new int[] { 1,2,3}, 0, new int[] { 10, 2, 3})]
        [InlineData(new int[] { 1, 2, 3 }, 1, new int[] { 1, 20, 3 })]
        [InlineData(new int[] { 1, 2, 3 }, 2, new int[] { 1, 2, 30 })]
        [InlineData(new int[] { 1, 2, 3 }, 4, new int[] { 1, 2, 3 })]
        public static void SelectAtTest(IEnumerable<int> xs, int point, IEnumerable<int> expected)
        {
            var actual = xs.SelectAt(point, i => i * 10);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, 0, new int[] { })]
        [InlineData(0, 1, new int[] { 0 })]
        [InlineData(0, 2, new int[] { 0, 10})]
        [InlineData(5, 5, new int[] { })]
        [InlineData(5, 6, new int[] { 50 })]
        [InlineData(5, 10, new int[] { 50, 60, 70, 80, 90 })]
        public static void UnfoldTest(int seed, int limit, IEnumerable<int> expected)
        {
            var actual = seed.Unfold(s => Maybe.JustIf(s < limit, () => (s + 1, s * 10)));
            Assert.Equal(expected, actual);
        }
    }
}

using Nordril.Functional.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xunit;

namespace Nordril.Functional.Tests
{
    public static partial class CollectionExtensionsTests
    {
        [Theory]
        [MemberData(nameof(Aggregate2Data))]
        public static void Aggregate2Test(IEnumerable<int> xs, int expected)
        {
            static int sumUp(int acc, int outer, int inner)
            {
                return acc + outer + inner;
            }

            var res = xs.Aggregate2(
                0,
                i => Enumerable.Range((i / 10) * 10, (((i / 10) + 1) * 10)),
                sumUp);

            Assert.Equal(expected, res);
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
        [InlineData(new int[] { })]
        [InlineData(new int[] { 5 })]
        [InlineData(new int[] { 5, 12 })]
        [InlineData(new int[] { 5, 6, 36, 175, 1, 763 })]
        public static void AverageIntTest(IEnumerable<int> xs)
        {
            if (xs.Count() == 0)
            {
                Assert.Equal(0D, xs.AverageIterative());
                return;
            }

            var actual = xs.AverageIterative();
            var expected = xs.Average();

            Assert.Equal(expected, actual, 5);
        }

        [Theory]
        [InlineData(new long[] { })]
        [InlineData(new long[] { 5 })]
        [InlineData(new long[] { 5, 12 })]
        [InlineData(new long[] { 5, 6, 36, 175, 1, 763, 23789621785 })]
        public static void AverageLongTest(IEnumerable<long> xs)
        {
            if (xs.Count() == 0)
            {
                Assert.Equal(0D, xs.AverageIterative());
                return;
            }

            var actual = xs.AverageIterative();
            var expected = xs.Average();

            Assert.Equal(expected, actual, 5);
        }

        [Theory]
        [InlineData(new float[] { })]
        [InlineData(new float[] { 5 })]
        [InlineData(new float[] { 5, 12 })]
        [InlineData(new float[] { 5, 6, 36, 175, 1, 763 })]
        [InlineData(new float[] { 5, 6, 36, 175, 1, 763, 18.4F })]
        [InlineData(new float[] { 5, 6, 36, 175, 1, 763, 23789.4F })]
        public static void AverageFloatTest(IEnumerable<float> xs)
        {
            if (xs.Count() == 0)
            {
                Assert.Equal(0D, xs.AverageIterative());
                return;
            }

            var actual = xs.AverageIterative();
            var expected = xs.Average();

            Assert.Equal(expected, actual, 2);
        }

        [Theory]
        [InlineData(new double[] { })]
        [InlineData(new double[] { 5 })]
        [InlineData(new double[] { 5, 12 })]
        [InlineData(new double[] { 5, 6, 36, 175, 1, 763 })]
        [InlineData(new double[] { 5, 6, 36, 175, 1, 763, 236.2 })]
        [InlineData(new double[] { 3.23786, 1.4, 7.862 })]
        public static void AverageDoubleTest(IEnumerable<double> xs)
        {
            if (xs.Count() == 0)
            {
                Assert.Equal(0D, xs.AverageIterative());
                return;
            }

            var actual = xs.AverageIterative();
            var expected = xs.Average();

            Assert.Equal(expected, actual, 5);
        }

        [Theory]
        [MemberData(nameof(AverageDecimalData))]
        public static void AverageDecimalTest(IEnumerable<decimal> xs)
        {
            if (xs.Count() == 0)
            {
                Assert.Equal(0M, xs.AverageIterative());
                return;
            }

            var actual = xs.AverageIterative();
            var expected = xs.Average();

            Assert.Equal(expected, actual, 5);
        }

        [Theory]
        [InlineData(new int[0], new string[0], new string[0])]
        [InlineData(new int[] { 1 }, new string[0], new string[0])]
        [InlineData(new int[] { 1 }, new string[] { "a" }, new string[] { "1a" })]
        [InlineData(new int[] { 1, 2, 3 }, new string[] { "a", "b", "c" }, new string[] { "1a", "1b", "1c", "2a", "2b", "2c", "3a", "3b", "3c" })]
        public static void Cartesian2Test(IEnumerable<int> xs, IEnumerable<string> ys, IEnumerable<string> expected)
        {
            var actual = xs.Cartesian(ys, (x, y) => x + y);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(
            new int[0],
            new string[0],
            new float[0],
            new string[0])]
        [InlineData(
            new int[] { 1 },
            new string[0],
            new float[0],
            new string[0])]
        [InlineData(
            new int[] { 1 },
            new string[] { "a" },
            new float[] { 4f },
            new string[] { "1a4" })]
        [InlineData(
            new int[] { 1, 2 },
            new string[] { "a", "b" },
            new float[] { 4f, 9f },
            new string[] { "1a4", "1a9", "1b4", "1b9", "2a4", "2a9", "2b4", "2b9" })]
        [InlineData(
            new int[] { 1, 2, 3 },
            new string[] { "a", "b", "c" },
            new float[] { 4f },
            new string[] { "1a4", "1b4", "1c4", "2a4", "2b4", "2c4", "3a4", "3b4", "3c4" })]
        public static void Cartesian3Test(
            IEnumerable<int> xs,
            IEnumerable<string> ys,
            IEnumerable<float> zs,
            IEnumerable<string> expected)
        {
            var actual = xs.Cartesian(ys, zs, (x, y, z) => x + y + z);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(
            new int[0],
            new string[0],
            new float[0],
            new bool[0],
            new string[0])]
        [InlineData(
            new int[] { 1 },
            new string[0],
            new float[0],
            new bool[0],
            new string[0])]
        [InlineData(
            new int[] { 1 },
            new string[] { "a" },
            new float[] { 4f },
            new bool[] { true},
            new string[] { "1a4True" })]
        [InlineData(
            new int[] { 1, 2 },
            new string[] { "a", "b" },
            new float[] { 4f, 9f },
            new bool[] {false, true},
            new string[] {
                "1a4False",
                "1a4True",
                "1a9False",
                "1a9True",
                "1b4False",
                "1b4True",
                "1b9False",
                "1b9True",
                "2a4False",
                "2a4True",
                "2a9False",
                "2a9True",
                "2b4False",
                "2b4True",
                "2b9False",
                "2b9True" })]
        public static void Cartesian4Test(
            IEnumerable<int> xs,
            IEnumerable<string> ys,
            IEnumerable<float> zs,
            IEnumerable<bool> us,
            IEnumerable<string> expected)
        {
            var actual = xs.Cartesian(ys, zs, us, (x, y, z, u) => x + y + z + u);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(
            new int[0],
            new string[0],
            new float[0],
            new bool[0],
            new double[0],
            new string[0])]
                [InlineData(
            new int[] { 1 },
            new string[0],
            new float[0],
            new bool[0],
            new double[0],
            new string[0])]
                [InlineData(
            new int[] { 1 },
            new string[] { "a" },
            new float[] { 4f },
            new bool[] { true },
            new double[] { 2.5d },
            new string[] { "1a4True2.5" })]
                [InlineData(
            new int[] { 1, 2 },
            new string[] { "a", "b" },
            new float[] { 4f, 9f },
            new bool[] { false, true },
            new double[] { 2d, 3d },
            new string[] {
                        "1a4False2",
                        "1a4False3",
                        "1a4True2",
                        "1a4True3",
                        "1a9False2",
                        "1a9False3",
                        "1a9True2",
                        "1a9True3",
                        "1b4False2",
                        "1b4False3",
                        "1b4True2",
                        "1b4True3",
                        "1b9False2",
                        "1b9False3",
                        "1b9True2",
                        "1b9True3",
                        "2a4False2",
                        "2a4False3",
                        "2a4True2",
                        "2a4True3",
                        "2a9False2",
                        "2a9False3",
                        "2a9True2",
                        "2a9True3",
                        "2b4False2",
                        "2b4False3",
                        "2b4True2",
                        "2b4True3",
                        "2b9False2",
                        "2b9False3",
                        "2b9True2",
                        "2b9True3" })]
                public static void Cartesian5Test(
            IEnumerable<int> xs,
            IEnumerable<string> ys,
            IEnumerable<float> zs,
            IEnumerable<bool> us,
            IEnumerable<double> vs,
            IEnumerable<string> expected)
        {
            var actual = xs.Cartesian(ys, zs, us, vs, (x, y, z, u, v) => x + y + z + u + v.ToString(CultureInfo.InvariantCulture));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public static void CartesianTest()
        {
            static int fromBits(bool[] arr)
            {
                int res = 0;
                for (int i = 0; i < arr.Length; i++)
                    res += (arr[i] ? 1 : 0) << (arr.Length - i - 1);

                return res;
            }

            var set = FuncList.Make(false, true);
            var actual6 = set.Cartesian(set, set, set, set, set,
                (x, y, z, u, v, w) => fromBits(new bool[] { x, y, z, u, v, w }));
            var actual7 = set.Cartesian(set, set, set, set, set, set,
                (x, y, z, u, v, w, s) => fromBits(new bool[] { x, y, z, u, v, w, s }));
            var actual8 = set.Cartesian(set, set, set, set, set, set, set,
                (x, y, z, u, v, w, s, t) => fromBits(new bool[] { x, y, z, u, v, w, s, t }));

            Assert.Equal(Enumerable.Range(0, 64), actual6);
            Assert.Equal(Enumerable.Range(0, 128), actual7);
            Assert.Equal(Enumerable.Range(0, 256), actual8);
        }

        [Fact]
        public static void CartesianManyTest()
        {
            static int fromBits(bool[] arr)
            {
                int res = 0;
                for (int i = 0; i < arr.Length; i++)
                    res += (arr[i] ? 1 : 0) << (arr.Length - i - 1);

                return res;
            }

            var empty = new IEnumerable<bool>[] { };
            Assert.Empty(empty.Cartesian(xs => fromBits(xs.ToArray())));

            var emptySet = new bool[] { };
            var set = (IEnumerable<bool>)FuncList.Make(false, true);

            Assert.Empty(new[] { emptySet, set, set, set }.Cartesian(xs => fromBits(xs.ToArray())));
            Assert.Empty(new[] { set, emptySet, set, set }.Cartesian(xs => fromBits(xs.ToArray())));
            Assert.Empty(new[] { set, set, emptySet, set }.Cartesian(xs => fromBits(xs.ToArray())));
            Assert.Empty(new[] { set, set, set, emptySet }.Cartesian(xs => fromBits(xs.ToArray())));
            Assert.Empty(new[] { emptySet, set, emptySet, set }.Cartesian(xs => fromBits(xs.ToArray())));

            var actual = Enumerable.Repeat(set, 4).Cartesian(xs => fromBits(xs.ToArray())).ToList();

            Assert.Equal(Enumerable.Range(0, 16), actual);

            actual = new int[][]
            {
                new int[]{1,2,3},
                new int[]{10,40,80},
                new int[]{200,300},
                new int[]{5000},
            }.Cartesian(xs => xs.Sum()).ToList();

            Assert.Equal(new int[] {
                5211, 5311,
                5241, 5341, 
                5281, 5381,

                5212, 5312,
                5242, 5342,
                5282, 5382,

                5213, 5313,
                5243, 5343,
                5283, 5383,
            }, actual);
        }

        [Theory]
        [MemberData(nameof(ConcatTestData))]
        public static void ConcatTest(IEnumerable<IEnumerable<int>> xs, IEnumerable<int> expected)
        {
            Assert.Equal(expected, xs.Concat());
        }

        [Theory]
        [InlineData(new string[0], "", null, null, "")]
        [InlineData(new string[0], ", ", null, null, "")]
        [InlineData(new string[0], "", "[", null, "[")]
        [InlineData(new string[0], "", null, "]", "]")]
        [InlineData(new string[0], "", "[", "]", "[]")]
        [InlineData(new string[0], ", ", "[", "]", "[]")]
        [InlineData(new string[] { "a" }, ", ", "[", "]", "[a]")]
        [InlineData(new string[] { "a","b" }, ", ", "[", "]", "[a, b]")]
        [InlineData(new string[] { "a","b, c" }, ", ", "[", "]", "[a, b, c]")]
        [InlineData(new string[] { "a","b", "c" }, ", ", null, null, "a, b, c")]
        [InlineData(new string[] { "a","b", "c" }, "", null, null, "abc")]
        [InlineData(new string[] { "a","b", "c" }, null, null, null, "abc")]
        [InlineData(new string[] { "a","b", "c" }, null, "{", "}", "{abc}")]
        public static void ConcatStringsTest(IEnumerable<string> xs, string separator, string prefix, string postfix, string expected)
        {
            Assert.Equal(expected, xs.ConcatStrings(separator, prefix, postfix));
        }

        [Theory]
        [InlineData(new int[] { }, true)]
        [InlineData(new int[] { 0 }, false)]
        [InlineData(new int[] { 1, 2, 3 }, false)]
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
        [InlineData(new int[] { 1, 2, 3 }, new bool[] { }, 0)]
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
        [InlineData(new int[] { 1, 2, 3 }, new int[] { 10, 20, 30 }, new bool[] { }, 0)]
        [InlineData(new int[] { }, new int[] { 10, 20, 30 }, new bool[] { true, true }, 0)]
        [InlineData(new int[] { 1, 5 }, new int[] { 10, 20, 30 }, new bool[] { true, true }, 110)]
        [InlineData(new int[] { 1, 5 }, new int[] { 10, 20, 30 }, new bool[] { true, true, true }, 110)]
        [InlineData(new int[] { 1, 5, 7, 10 }, new int[] { 10, 20, 30 }, new bool[] { true, true }, 110)]
        [InlineData(new int[] { 1, 5, 6 }, new int[] { 10, 20, 30 }, new bool[] { true, false, true }, 190)]
        public static void Foreach3Test(IEnumerable<int> xs, IEnumerable<int> ys, IEnumerable<bool> zs, int expected)
        {
            var actual = 0;

            xs.ForEach(ys, zs, (i, j, b) => { if (b) actual += i * j; });

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(new int[] { }, new int[] { }, new bool[] { }, new bool[] { }, 0)]
        [InlineData(new int[] { 1, 2, 3 }, new int[] { 10, 20, 30 }, new bool[] { true }, new bool[] { }, 0)]
        [InlineData(new int[] { }, new int[] { 10, 20, 30 }, new bool[] { }, new bool[] { true, true }, 0)]
        [InlineData(new int[] { 1, 5 }, new int[] { 10, 20, 30 }, new bool[] { false, true }, new bool[] { true, true }, 100)]
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
        [InlineData(new int[] { }, new int[] { }, new int[] { })]
        [InlineData(new int[] { 1 }, new int[] { }, new int[] { 1 })]
        [InlineData(new int[] { 1, 2 }, new int[] { }, new int[] { 1, 2 })]
        [InlineData(new int[] { 1, 2, 3 }, new int[] { }, new int[] { 1, 2, 3 })]
        [InlineData(new int[] { }, new int[] { 9 }, new int[] { })]
        [InlineData(new int[] { 1 }, new int[] { 9 }, new int[] { 1 })]
        [InlineData(new int[] { 1, 2 }, new int[] { 9 }, new int[] { 1, 9, 2 })]
        [InlineData(new int[] { 1, 2, 3 }, new int[] { 9, 9 }, new int[] { 1, 9, 9, 2, 9, 9, 3 })]
        public static void IntercalateTest(IEnumerable<int> xs, IEnumerable<int> separator, IEnumerable<int> expected)
        {
            Assert.Equal(expected, xs.Intercalate(separator));
        }

        [Theory]
        [InlineData(new int[] { }, 9, new int[] { })]
        [InlineData(new int[] { 1 }, 9, new int[] { 1 })]
        [InlineData(new int[] { 1, 2 }, 9, new int[] { 1, 9, 2 })]
        [InlineData(new int[] { 1, 2, 3 }, 9, new int[] { 1, 9, 2, 9, 3 })]
        public static void IntersperseTest(IEnumerable<int> xs, int separator, IEnumerable<int> expected)
        {
            Assert.Equal(expected, xs.Intersperse(separator));
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
        [InlineData(new int[] { }, false, 0)]
        [InlineData(new int[] { 1 }, true, 1)]
        [InlineData(new int[] { 1,2,3,11}, true, 3)]
        [InlineData(new int[] { 3, 22, 2, 16}, true, 16)]
        [InlineData(new int[] { 3, 22, 2, 16, 6}, true, 16)]
        public static void MaxByTest(IEnumerable<int> xs, bool hasValue, int expected)
        {
            var actual = xs.MaxBy(x => x % 10);

            if (!hasValue)
                Assert.True(actual.IsNothing);
            else
                Assert.Equal(expected, actual.Value());
        }

        [Theory]
        [InlineData(new int[] { }, false, 0)]
        [InlineData(new int[] { 1 }, true, 1)]
        [InlineData(new int[] { 1, 2, 3, 11 }, true, 3)]
        [InlineData(new int[] { 3, 22, 2, 16 }, true, 16)]
        [InlineData(new int[] { 3, 22, 2, 16, 6 }, true, 16)]
        [InlineData(new int[] { 3, 22, 2, 5, 16, 6 }, true, 5)]
        [InlineData(new int[] { 3, 22, 2, 75, 5, 16, 6 }, true, 75)]
        public static void MaxByNullableTest(IEnumerable<int> xs, bool hasValue, int expected)
        {
            var actual = xs.MaxBy(x => (x % 10) == 5 ? (int?)null : (x % 10));

            if (!hasValue)
                Assert.True(actual.IsNothing);
            else
                Assert.Equal(expected, actual.Value());
        }

        [Theory]
        [InlineData(new int[] { }, new int[] { })]
        [InlineData(new int[] { 1 }, new int[] { 1 })]
        [InlineData(new int[] { 1, 11, 111 }, new int[] { 1, 11, 111 })]
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
        [InlineData(new int[] { }, false, 0)]
        [InlineData(new int[] { 1 }, true, 1)]
        [InlineData(new int[] { 1, 2, 3, 11 }, true, 1)]
        [InlineData(new int[] { 3, 22, 2, 16 }, true, 22)]
        public static void MinByTest(IEnumerable<int> xs, bool hasValue, int expected)
        {
            var actual = xs.MinBy(x => x % 10);

            if (!hasValue)
                Assert.True(actual.IsNothing);
            else
                Assert.Equal(expected, actual.Value());
        }

        [Theory]
        [InlineData(new int[] { }, false, 0)]
        [InlineData(new int[] { 1 }, true, 1)]
        [InlineData(new int[] { 1, 2, 3, 11 }, true, 1)]
        [InlineData(new int[] { 3, 22, 2, 16 }, true, 22)]
        [InlineData(new int[] { 3, 22, 2, 16, 6 }, true, 22)]
        [InlineData(new int[] { 3, 22, 2, 5, 16, 6 }, true, 5)]
        [InlineData(new int[] { 3, 22, 2, 75, 5, 16, 6 }, true, 75)]
        public static void MinByNullableTest(IEnumerable<int> xs, bool hasValue, int expected)
        {
            var actual = xs.MinBy(x => (x % 10) == 5 ? (int?)null : (x % 10));

            if (!hasValue)
                Assert.True(actual.IsNothing);
            else
                Assert.Equal(expected, actual.Value());
        }

        [Theory]
        [InlineData(new int[] { }, new int[] { }, new int[] { })]
        [InlineData(new int[] { 1 }, new int[] { }, new int[] { 1 })]
        [InlineData(new int[] { 2 }, new int[] { 2 }, new int[] { })]
        [InlineData(new int[] { 1, 2, 3, 4, 5 }, new int[] { 2, 4 }, new int[] { 1, 3, 5 })]
        [InlineData(new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 2, 4, 6 }, new int[] { 1, 3, 5 })]
        public static void PartitionTest(IEnumerable<int> xs, IEnumerable<int> expectedTrue, IEnumerable<int> expectedFalse)
        {
            var (actualTrue, actualFalse) = xs.Partition(x => x % 2 == 0);

            Assert.Equal(expectedTrue, actualTrue);
            Assert.Equal(expectedFalse, actualFalse);
        }

        [Theory]
        [MemberData(nameof(PartitionEitherData))]
        public static void PartitionEitherTest(IEnumerable<Either<int, string>> xs, IEnumerable<int> leftExpected, IEnumerable<string> rightExpected)
        {
            var (leftActual, rightActual) = xs.Partition();

            Assert.Equal(leftExpected, leftActual);
            Assert.Equal(rightExpected, rightActual);
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
        [InlineData(new int[] { 1, 2, 3 }, 0, new int[] { 10, 2, 3 })]
        [InlineData(new int[] { 1, 2, 3 }, 1, new int[] { 1, 20, 3 })]
        [InlineData(new int[] { 1, 2, 3 }, 2, new int[] { 1, 2, 30 })]
        [InlineData(new int[] { 1, 2, 3 }, 4, new int[] { 1, 2, 3 })]
        public static void SelectAtTest(IEnumerable<int> xs, int point, IEnumerable<int> expected)
        {
            var actual = xs.SelectAt(point, i => i * 10);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(SelectKeyValueData))]
        public static void SelectKeyValueTest(IEnumerable<KeyValuePair<string, int>> xs)
        {
            string f(KeyValuePair<string, int> kv) => $"{kv.Key} is {kv.Value} yerars old.";

            Assert.Equal(xs.Select(f), xs.SelectKeyValue((k,v) => f(new KeyValuePair<string, int>(k,v))));
        }

        [Theory]
        [MemberData(nameof(SelectMaybeData))]
        public static void SelectMaybeTest(IEnumerable<int> xs, IEnumerable<int> expected)
        {
            Maybe<int> f(int x) => Maybe.JustIf(x > 5, () => x);

            Assert.Equal(expected, xs.SelectMaybe(f));
        }

        [Theory]
        [InlineData(0, 0, new int[] { })]
        [InlineData(0, 1, new int[] { 0 })]
        [InlineData(0, 2, new int[] { 0, 10 })]
        [InlineData(5, 5, new int[] { })]
        [InlineData(5, 6, new int[] { 50 })]
        [InlineData(5, 10, new int[] { 50, 60, 70, 80, 90 })]
        public static void UnfoldTest(int seed, int limit, IEnumerable<int> expected)
        {
            var actual = seed.Unfold(s => Maybe.JustIf(s < limit, () => (s + 1, s * 10)));
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(new string[0], new string[0])]
        [InlineData(new string[] { "ABC x", "DEF y", "FED y" }, new string [] { "ABC x", "DEF y", "FED y" })]
        [InlineData(new string[] { "ABCx", "ABCy","DEFz","DEG" }, new string [] {"ABCx","DEFz","DEG" })]
        [InlineData(new string[] {"ABCa","ABCb","ABCb","ABCj","XYZ","MNO","MNOh" }, new string [] { "ABCa", "XYZ", "MNO" })]
        [InlineData(new string[] { "aaaOne", "aaaa", "aaaaa" }, new string [] { "aaaOne" })]
        public static void UniqueTest(IEnumerable<string> xs, IEnumerable<string> expected)
        {
            var actual = xs.Unique(x => x.Substring(0, 3));

            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(Unzip1Data))]
        public static void UnzipTest1(IEnumerable<Person> xs, IEnumerable<string> names, IEnumerable<int> money)
        {
            var (actualNames, actualMoney) = xs.Unzip(p => (p.Name, p.Money));

            Assert.Equal(names, actualNames);
            Assert.Equal(money, actualMoney);
        }

        [Theory]
        [MemberData(nameof(UnzipTupleData))]
        public static void UnzipTupleTest(IEnumerable<(string, int)> xs, IEnumerable<string> names, IEnumerable<int> money)
        {
            var (actualNames, actualMoney) = xs.Unzip();

            Assert.Equal(names, actualNames);
            Assert.Equal(money, actualMoney);
        }

        [Theory]
        [MemberData(nameof(UnzipTuple3Data))]
        public static void UnzipTuple3Test(IEnumerable<(string, int, float)> xs, IEnumerable<string> names, IEnumerable<int> money, IEnumerable<float> height)
        {
            var (actualNames, actualMoney, actualHeight) = xs.Unzip();

            Assert.Equal(names, actualNames);
            Assert.Equal(money, actualMoney);
            Assert.Equal(height, actualHeight);
        }

        [Theory]
        [MemberData(nameof(UnzipTuple4Data))]
        public static void UnzipTuple4Test(IEnumerable<(string, int, float, bool)> xs, IEnumerable<string> names, IEnumerable<int> money, IEnumerable<float> height, IEnumerable<bool> employed)
        {
            var (actualNames, actualMoney, actualHeight, actualEmployed) = xs.Unzip();

            Assert.Equal(names, actualNames);
            Assert.Equal(money, actualMoney);
            Assert.Equal(height, actualHeight);
            Assert.Equal(employed, actualEmployed);
        }

        [Theory]
        [MemberData(nameof(ZipTuple3Data))]
        public static void ZipTuple3Test(IEnumerable<int> xs, IEnumerable<bool> ys, IEnumerable<string> zs, IEnumerable<(int, bool, string)> expected)
        {
            Assert.Equal(expected, xs.Zip(ys, zs));
        }

        [Theory]
        [MemberData(nameof(ZipTuple4Data))]
        public static void ZipTuple4Test(IEnumerable<int> xs, IEnumerable<bool> ys, IEnumerable<string> zs, IEnumerable<double> us, IEnumerable<(int, bool, string, double)> expected)
        {
            Assert.Equal(expected, xs.Zip(ys, zs, us));
        }

        [Theory]
        [MemberData(nameof(Zip3Data))]
        public static void Zip3Test(IEnumerable<int> xs, IEnumerable<bool> ys, IEnumerable<string> zs, IEnumerable<ZipRecord> expected)
        {
            Assert.Equal(expected, xs.Zip(ys, zs, (x, y, u) => new ZipRecord(x, y, u)));
        }

        [Theory]
        [MemberData(nameof(Zip4Data))]
        public static void Zip4Test(IEnumerable<int> xs, IEnumerable<bool> ys, IEnumerable<string> zs, IEnumerable<double> us, IEnumerable<ZipRecord> expected)
        {
            Assert.Equal(expected, xs.Zip(ys, zs, us, (x, y, u, z) => new ZipRecord(x, y, u, z)));
        }

        [Theory]
        [MemberData(nameof(ZipManyData))]
        public static void ZipManyTest(IEnumerable<IEnumerable<int>> xs, IEnumerable<int> expected)
        {
            Assert.Equal(expected, xs.Zip(x => x.Sum()));
        }

        [Fact]
        public static void ZipManyLengthTest()
        {
            var xs = new List<int> { 1, 2, 3 };
            var ys = new List<int> { 10, 20, 30, 40 };
            var zs = new List<int> { 100, 200 };

            var actual = new[] { xs, ys, zs }.Zip(x => x.Sum()).ToList();

            Assert.Equal(2, actual.Count);
            Assert.Equal(new[] { 111, 222 }, actual);
        }

        [Theory]
        [MemberData(nameof(ZipManyListsData))]
        public static void ZipManyListsTest(IEnumerable<IEnumerable<int>> xs, IEnumerable<List<int>> expected)
        {
            Assert.Equal(expected, xs.Zip());
        }

        [Theory]
        [MemberData(nameof(ZipWithStreamData))]
        public static void ZipWithStreamTest(IEnumerable<string> xs, IEnumerable<(string, int)> expected)
        {
            Assert.Equal(expected, xs.ZipWithStream(5, x => x + 2));
        }
    }

    public class Person
    {
        public string Name { get; private set; }
        public int Money { get; private set; }

        public Person(string name, int money) { Name = name; Money = money; }
    }

    public class ZipRecord : IEquatable<ZipRecord>
    {
        public int X { get; private set; } = 0;
        public bool Y { get; private set; } = false;
        public string Z { get; private set; } = "";
        public double U { get; private set; } = 0d;

        public ZipRecord(int x, bool y, string z, double u) { X = x; Y = y; Z = z; U = u; }

        public ZipRecord(int x, bool y, string z) : this(x,y,z, 0d) { }

        public bool Equals(ZipRecord other) => X == other.X && Y == other.Y && Z == other.Z && U == other.U;
    }
}

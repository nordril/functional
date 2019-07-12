using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Nordril.Functional.Tests
{
    public class TupleExtenstionTests
    {
        public static IEnumerable<object[]> Tuple1Data()
        {
            //n-th
            Func<ValueTuple<int>, ValueTuple<int>> f = t => t.First(x => x + 1);
            yield return new object[]
            {
                ValueTuple.Create(5),
                ValueTuple.Create(6),
                f
            };
        }

        public static IEnumerable<object[]> Tuple2Data()
        {
            //n-th
            Func<(int, int), (int, int)> f = t => t.First(x => x + 1);
            yield return new object[]
            {
                (5, 2),
                (6, 2),
                f
            };

            f = t => t.Second(x => x + 1);
            yield return new object[]
            {
                (5, 2),
                (5, 3),
                f
            };
        }

        public static IEnumerable<object[]> Tuple3Data()
        {
            Func<(int, int, int), (int, int, int)> f = t => t.First(x => x + 1);
            yield return new object[]
            {
                (5, 2, 9),
                (6, 2, 9),
                f
            };

            f = t => t.Second(x => x + 1);
            yield return new object[]
            {
                (5, 2, 9),
                (5, 3, 9),
                f
            };

            f = t => t.Third(x => x + 1);
            yield return new object[]
            {
                (5, 2, 9),
                (5, 2, 10),
                f
            };
        }

        public static IEnumerable<object[]> Tuple4Data()
        {
            Func<(int, int, int, int), (int, int, int, int)> f = t => t.First(x => x + 1);
            yield return new object[]
            {
                (5, 2, 9, 15),
                (6, 2, 9, 15),
                f
            };

            f = t => t.Second(x => x + 1);
            yield return new object[]
            {
                (5, 2, 9, 15),
                (5, 3, 9, 15),
                f
            };

            f = t => t.Third(x => x + 1);
            yield return new object[]
            {
                (5, 2, 9, 15),
                (5, 2, 10, 15),
                f
            };

            f = t => t.Fourth(x => x + 1);
            yield return new object[]
            {
                (5, 2, 9, 15),
                (5, 2, 9, 16),
                f
            };
        }

        public static IEnumerable<object[]> Tuple5Data()
        {
            Func<(int, int, int, int, int), (int, int, int, int, int)> f = t => t.First(x => x + 1);
            yield return new object[]
            {
                (5, 2, 9, 15, 29),
                (6, 2, 9, 15, 29),
                f
            };

            f = t => t.Second(x => x + 1);
            yield return new object[]
            {
                (5, 2, 9, 15, 29),
                (5, 3, 9, 15, 29),
                f
            };

            f = t => t.Third(x => x + 1);
            yield return new object[]
            {
                (5, 2, 9, 15, 29),
                (5, 2, 10, 15, 29),
                f
            };

            f = t => t.Fourth(x => x + 1);
            yield return new object[]
            {
                (5, 2, 9, 15, 29),
                (5, 2, 9, 16, 29),
                f
            };

            f = t => t.Fifth(x => x + 1);
            yield return new object[]
            {
                (5, 2, 9, 15, 29),
                (5, 2, 9, 15, 30),
                f
            };
        }

        public static IEnumerable<object[]> Tuple6Data()
        {
            Func<(int, int, int, int, int, int), (int, int, int, int, int, int)> f = t => t.First(x => x + 1);
            yield return new object[]
            {
                (5, 2, 9, 15, 29, 18),
                (6, 2, 9, 15, 29, 18),
                f
            };

            f = t => t.Second(x => x + 1);
            yield return new object[]
            {
                (5, 2, 9, 15, 29, 18),
                (5, 3, 9, 15, 29, 18),
                f
            };

            f = t => t.Third(x => x + 1);
            yield return new object[]
            {
                (5, 2, 9, 15, 29, 18),
                (5, 2, 10, 15, 29, 18),
                f
            };

            f = t => t.Fourth(x => x + 1);
            yield return new object[]
            {
                (5, 2, 9, 15, 29, 18),
                (5, 2, 9, 16, 29, 18),
                f
            };

            f = t => t.Fifth(x => x + 1);
            yield return new object[]
            {
                (5, 2, 9, 15, 29, 18),
                (5, 2, 9, 15, 30, 18),
                f
            };

            f = t => t.Sixth(x => x + 1);
            yield return new object[]
            {
                (5, 2, 9, 15, 29, 18),
                (5, 2, 9, 15, 29, 19),
                f
            };
        }

        public static IEnumerable<object[]> Tuple7Data()
        {
            Func<(int, int, int, int, int, int, int), (int, int, int, int, int, int, int)> f = t => t.First(x => x + 1);
            yield return new object[]
            {
                (5, 2, 9, 15, 29, 18, 40),
                (6, 2, 9, 15, 29, 18, 40),
                f
            };

            f = t => t.Second(x => x + 1);
            yield return new object[]
            {
                (5, 2, 9, 15, 29, 18, 40),
                (5, 3, 9, 15, 29, 18, 40),
                f
            };

            f = t => t.Third(x => x + 1);
            yield return new object[]
            {
                (5, 2, 9, 15, 29, 18, 40),
                (5, 2, 10, 15, 29, 18, 40),
                f
            };

            f = t => t.Fourth(x => x + 1);
            yield return new object[]
            {
                (5, 2, 9, 15, 29, 18, 40),
                (5, 2, 9, 16, 29, 18, 40),
                f
            };

            f = t => t.Fifth(x => x + 1);
            yield return new object[]
            {
                (5, 2, 9, 15, 29, 18, 40),
                (5, 2, 9, 15, 30, 18, 40),
                f
            };

            f = t => t.Sixth(x => x + 1);
            yield return new object[]
            {
                (5, 2, 9, 15, 29, 18, 40),
                (5, 2, 9, 15, 29, 19, 40),
                f
            };

            f = t => t.Seventh(x => x + 1);
            yield return new object[]
            {
                (5, 2, 9, 15, 29, 18, 40),
                (5, 2, 9, 15, 29, 18, 41),
                f
            };
        }

        public static IEnumerable<object[]> Tuple8Data()
        {
            Func<(int, int, int, int, int, int, int, int), (int, int, int, int, int, int, int, int)> f = t => t.First(x => x + 1);
            yield return new object[]
            {
                (5, 2, 9, 15, 29, 18, 40, 50),
                (6, 2, 9, 15, 29, 18, 40, 50),
                f
            };

            f = t => t.Second(x => x + 1);
            yield return new object[]
            {
                (5, 2, 9, 15, 29, 18, 40, 50),
                (5, 3, 9, 15, 29, 18, 40, 50),
                f
            };

            f = t => t.Third(x => x + 1);
            yield return new object[]
            {
                (5, 2, 9, 15, 29, 18, 40, 50),
                (5, 2, 10, 15, 29, 18, 40, 50),
                f
            };

            f = t => t.Fourth(x => x + 1);
            yield return new object[]
            {
                (5, 2, 9, 15, 29, 18, 40, 50),
                (5, 2, 9, 16, 29, 18, 40, 50),
                f
            };

            f = t => t.Fifth(x => x + 1);
            yield return new object[]
            {
                (5, 2, 9, 15, 29, 18, 40, 50),
                (5, 2, 9, 15, 30, 18, 40, 50),
                f
            };

            f = t => t.Sixth(x => x + 1);
            yield return new object[]
            {
                (5, 2, 9, 15, 29, 18, 40, 50),
                (5, 2, 9, 15, 29, 19, 40, 50),
                f
            };

            f = t => t.Seventh(x => x + 1);
            yield return new object[]
            {
                (5, 2, 9, 15, 29, 18, 40, 50),
                (5, 2, 9, 15, 29, 18, 41, 50),
                f
            };

            f = t => t.Eighth(x => x + 1);
            yield return new object[]
            {
                (5, 2, 9, 15, 29, 18, 40, 50),
                (5, 2, 9, 15, 29, 18, 40, 51),
                f
            };
        }

        public static IEnumerable<object[]> ApplyToTuple2Data()
        {
            //ApplyToTuple
            Func<int, int, int> g = (x, y) => x + y;
            yield return new object[]
            {
                (5, 2),
                7,
                g
            };
        }

        public static IEnumerable<object[]> ApplyToTuple3Data()
        {
            //ApplyToTuple
            Func<int, int, int, int> g = (x, y, z) => x + y + z;
            yield return new object[]
            {
                (5, 2, 8),
                15,
                g
            };
        }

        public static IEnumerable<object[]> ApplyToTuple4Data()
        {
            //ApplyToTuple
            Func<int, int, int, int, int> g = (x, y, z, u) => x + y + z+u;
            yield return new object[]
            {
                (5, 2, 8, 7),
                22,
                g
            };
        }

        public static IEnumerable<object[]> ApplyToTuple5Data()
        {
            //ApplyToTuple
            Func<int, int, int, int, int, int> g = (x, y, z, u, v) => x + y + z + u + v;
            yield return new object[]
            {
                (5, 2, 8, 7, 9),
                31,
                g
            };
        }

        public static IEnumerable<object[]> ApplyToTuple6Data()
        {
            //ApplyToTuple
            Func<int, int, int, int, int, int, int> g = (x, y, z, u, v, w) => x + y + z + u + v + w;
            yield return new object[]
            {
                (5, 2, 8, 7, 9, 3),
                34,
                g
            };
        }

        public static IEnumerable<object[]> ApplyToTuple7Data()
        {
            //ApplyToTuple
            Func<int, int, int, int, int, int, int, int> g = (x, y, z, u, v, w, s) => x + y + z + u + v + w + s;
            yield return new object[]
            {
                (5, 2, 8, 7, 9, 3, 11),
                45,
                g
            };
        }

        public static IEnumerable<object[]> ApplyToTuple8Data()
        {
            //ApplyToTuple
            Func<int, int, int, int, int, int, int, int, int> g = (x, y, z, u, v, w, s, t) => x + y + z + u + v + w + s + t;
            yield return new object[]
            {
                (5, 2, 8, 7, 9, 3, 11, 6),
                51,
                g
            };
        }

        [Theory]
        [MemberData(nameof(Tuple1Data))]
        public static void Tuple1Test(
            ValueTuple<int> tuple,
            ValueTuple<int> expected,
            Func<ValueTuple<int>, ValueTuple<int>> f)
        {
            var actual = f(tuple);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(Tuple2Data))]
        public static void Tuple2Test(
            (int, int) tuple,
            (int, int) expected,
            Func<(int, int), (int, int)> f)
        {
            var actual = f(tuple);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(Tuple3Data))]
        public static void Tuple3Test(
            (int, int, int) tuple,
            (int, int, int) expected,
            Func<(int, int, int), (int, int, int)> f)
        {
            var actual = f(tuple);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(Tuple4Data))]
        public static void Tuple4Test(
            (int, int, int, int) tuple,
            (int, int, int, int) expected,
            Func<(int, int, int, int), (int, int, int, int)> f)
        {
            var actual = f(tuple);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(Tuple5Data))]
        public static void Tuple5Test(
            (int, int, int, int, int) tuple,
            (int, int, int, int, int) expected,
            Func<(int, int, int, int, int), (int, int, int, int, int)> f)
        {
            var actual = f(tuple);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(Tuple6Data))]
        public static void Tuple6Test(
            (int, int, int, int, int, int) tuple,
            (int, int, int, int, int, int) expected,
            Func<(int, int, int, int, int, int), (int, int, int, int, int, int)> f)
        {
            var actual = f(tuple);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(Tuple7Data))]
        public static void Tuple7Test(
            (int, int, int, int, int, int, int) tuple,
            (int, int, int, int, int, int, int) expected,
            Func<(int, int, int, int, int, int, int), (int, int, int, int, int, int, int)> f)
        {
            var actual = f(tuple);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(Tuple8Data))]
        public static void Tuple8Test(
            (int, int, int, int, int, int, int, int) tuple,
            (int, int, int, int, int, int, int, int) expected,
            Func<(int, int, int, int, int, int, int, int), (int, int, int, int, int, int, int, int)> f)
        {
            var actual = f(tuple);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public static void BothTest()
        {
            Assert.Equal((7, false), (5, true).Both(x => x + 2, x => x && false));
        }

        [Theory]
        [MemberData(nameof(ApplyToTuple2Data))]
        public static void ApplyToTuple2Test((int, int) t, int expected, Func<int, int, int> f)
        {
            Assert.Equal(expected, t.ApplyToTuple(f));
        }

        [Theory]
        [MemberData(nameof(ApplyToTuple3Data))]
        public static void ApplyToTuple3Test((int, int, int) t, int expected, Func<int, int, int, int> f)
        {
            Assert.Equal(expected, t.ApplyToTuple(f));
        }

        [Theory]
        [MemberData(nameof(ApplyToTuple4Data))]
        public static void ApplyToTuple4Test((int, int, int, int) t, int expected, Func<int, int, int, int, int> f)
        {
            Assert.Equal(expected, t.ApplyToTuple(f));
        }

        [Theory]
        [MemberData(nameof(ApplyToTuple5Data))]
        public static void ApplyToTuple5Test((int, int, int, int, int) t, int expected, Func<int, int, int, int, int, int> f)
        {
            Assert.Equal(expected, t.ApplyToTuple(f));
        }

        [Theory]
        [MemberData(nameof(ApplyToTuple6Data))]
        public static void ApplyToTuple6Test((int, int, int, int, int, int) t, int expected, Func<int, int, int, int, int, int, int> f)
        {
            Assert.Equal(expected, t.ApplyToTuple(f));
        }

        [Theory]
        [MemberData(nameof(ApplyToTuple7Data))]
        public static void ApplyToTuple7Test((int, int, int, int, int, int, int) t, int expected, Func<int, int, int, int, int, int, int, int> f)
        {
            Assert.Equal(expected, t.ApplyToTuple(f));
        }

        [Theory]
        [MemberData(nameof(ApplyToTuple8Data))]
        public static void ApplyToTuple8Test((int, int, int, int, int, int, int, int) t, int expected, Func<int, int, int, int, int, int, int, int, int> f)
        {
            Assert.Equal(expected, t.ApplyToTuple(f));
        }

        [Fact]
        public static void AllTest()
        {
            Assert.Equal((6, 10), (3, 5).All(x => x * 2));
            Assert.Equal((6, 10, 14), (3, 5, 7).All(x => x * 2));
            Assert.Equal((6, 10, 14, 22), (3, 5, 7, 11).All(x => x * 2));
            Assert.Equal((6, 10, 14, 22, 26), (3, 5, 7, 11, 13).All(x => x * 2));
            Assert.Equal((6, 10, 14, 22, 26, 34), (3, 5, 7, 11, 13, 17).All(x => x * 2));
            Assert.Equal((6, 10, 14, 22, 26, 34, 38), (3, 5, 7, 11, 13, 17, 19).All(x => x * 2));
            Assert.Equal((6, 10, 14, 22, 26, 34, 38, 46), (3, 5, 7, 11, 13, 17, 19, 23).All(x => x * 2));

        }
    }
}

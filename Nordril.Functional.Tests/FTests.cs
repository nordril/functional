using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Nordril.Functional.Tests
{
    public class FTests
    {
        [Fact]
        public static void IdTest()
        {
            var id1 = F.Id<int>();
            var id2 = F.Id<char>();
            var id3 = F.Id<List<int>>();

            Assert.Equal(5, id1(5));
            Assert.Equal('c', id2('c'));
            Assert.Equal(new List<int> { 1, 2, 3 }, id3(new List<int> { 1, 2, 3 }));
        }

        [Fact]
        public static void ThenTest()
        {
            Func<List<int>, int> length = x => x.Count;
            Func<int, int> twice = x => x * 2;

            Assert.Equal(6, length.Then(twice)(new List<int> { 1, 2, 3 }));
        }

        [Fact]
        public static void AfterTest()
        {
            Func<List<int>, int> length = x => x.Count;
            Func<int, int> twice = x => x * 2;

            Assert.Equal(6, twice.After(length)(new List<int> { 1, 2, 3 }));
        }

        [Fact]
        public static void CurryTest()
        {
            Func<int, int, int> f = (x,y) => x + 2*y;

            Assert.Equal(19, f.Curry()(5)(7));
        }

        [Fact]
        public static void Curry3Test()
        {
            Func<int, int, int, int> f = (x, y, z) => x + 2 * y + 3*z;

            Assert.Equal(46, f.Curry()(5)(7)(9));
        }

        [Fact]
        public static void Curry4Test()
        {
            Func<int, int, int, int, int> f = (x, y, z, u) => x + 2 * y + 3 * z + 5*u;

            Assert.Equal(101, f.Curry()(5)(7)(9)(11));
        }

        [Fact]
        public static void UncurryTest()
        {
            Func<int, Func<int, int>> f = x => y => x + 2 * y;

            Assert.Equal(f(5)(7), f.Uncurry()(5, 7));
        }

        [Fact]
        public static void Uncurry3Test()
        {
            Func<int, Func<int, Func<int, int>>> f = x => y => z => x + 2 * y + 3 * z;

            Assert.Equal(f(5)(7)(9), f.Uncurry()(5, 7, 9));
        }

        [Fact]
        public static void Uncurry4Test()
        {
            Func<int, Func<int, Func<int, Func<int, int>>>> f = x => y => z => u => x + 2 * y + 3 * z + 5 * u;

            Assert.Equal(f(5)(7)(9)(11), f.Uncurry()(5,7,9,11));
        }

        [Fact]
        public static void SetTest()
        {
            var xs = new List<int> { 1, 2, 3 };
            var expected = new List<int> { 1, 8, 3 };

            Assert.Equal(expected, xs.Set(x => { x[1] = 8; }));
        }

        [Fact]
        public static void ApplyTest()
        {
            var xs = new List<int> { 1, 2, 3 };
            var expected = new List<int> { 1, 8, 3 };

            Assert.Equal(expected, xs.Apply(x => { x[1] = 8; return x; }));
        }
    }
}

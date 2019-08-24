using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Xunit;

namespace Nordril.Functional.Tests
{
    public class FETests
    {
        [Fact]
        public static void IdTest()
        {
            var id1 = FE.Id<int>();
            var id2 = FE.Id<char>();
            var id3 = FE.Id<List<int>>();

            Assert.Equal(5, id1.Compile()(5));
            Assert.Equal('c', id2.Compile()('c'));
            Assert.Equal(new List<int> { 1, 2, 3 }, id3.Compile()(new List<int> { 1, 2, 3 }));
        }

        [Fact]
        public static void ThenTest()
        {
            Expression<Func<List<int>, int>> length = x => x.Count;
            Expression<Func<int, int>> twice = x => x * 2;

            Assert.Equal(6, length.Then(twice).Compile()(new List<int> { 1, 2, 3 }));
        }

        [Fact]
        public static void AfterTest()
        {
            Expression<Func<List<int>, int>> length = x => x.Count;
            Expression<Func<int, int>> twice = x => x * 2;

            Assert.Equal(6, twice.After(length).Compile()(new List<int> { 1, 2, 3 }));
        }

        [Fact]
        public static void SplitParameterAndBodyTest()
        {
            Expression<Func<int, bool>> f = x => x % 2 == 0;
            Expression<Func<int, string, bool>> g = (x, y) => x % 2 == 0;
            Expression<Func<int, string, float, bool>> h = (x, y, z) => x % 2 == 0;
            Expression<Func<int, string, float, char, bool>> i = (x, y, z, u) => x % 2 == 0;

            var (f1, fBody) = f.SplitParameterAndBody();
            var (g1, g2, gBody) = g.SplitParameterAndBody();
            var (h1, h2, h3, hBody) = h.SplitParameterAndBody();
            var (i1, i2, i3, i4, iBody) = i.SplitParameterAndBody();

            Assert.Equal(typeof(int), f1.Type);
            Assert.Equal("x", f1.Name);
            Assert.Equal(typeof(bool), fBody.Type);

            Assert.Equal(typeof(int), g1.Type);
            Assert.Equal("x", g1.Name);
            Assert.Equal(typeof(string), g2.Type);
            Assert.Equal("y", g2.Name);
            Assert.Equal(typeof(bool), gBody.Type);

            Assert.Equal(typeof(int), h1.Type);
            Assert.Equal("x", h1.Name);
            Assert.Equal(typeof(string), h2.Type);
            Assert.Equal("y", h2.Name);
            Assert.Equal(typeof(float), h3.Type);
            Assert.Equal("z", h3.Name);
            Assert.Equal(typeof(bool), hBody.Type);

            Assert.Equal(typeof(int), i1.Type);
            Assert.Equal("x", i1.Name);
            Assert.Equal(typeof(string), i2.Type);
            Assert.Equal("y", i2.Name);
            Assert.Equal(typeof(float), i3.Type);
            Assert.Equal("z", i3.Name);
            Assert.Equal(typeof(char), i4.Type);
            Assert.Equal("u", i4.Name);
            Assert.Equal(typeof(bool), iBody.Type);

            var fNew = Expression.Lambda<Func<int, bool>>(fBody, f1).Compile();
            var gNew = Expression.Lambda<Func<int, string, bool>>(gBody, g1, g2).Compile();
            var hNew = Expression.Lambda<Func<int, string, float, bool>>(hBody, h1, h2, h3).Compile();
            var iNew = Expression.Lambda<Func<int, string, float, char, bool>>(iBody, i1, i2, i3, i4).Compile();

            Assert.True(fNew(4));
            Assert.False(fNew(7));

            Assert.True(gNew(4, "xh"));
            Assert.False(gNew(7, "xh"));

            Assert.True(hNew(4, "xh", 3.14F));
            Assert.False(hNew(7, "xh", 3.14F));

            Assert.True(iNew(4, "xh", 3.14F, 'r'));
            Assert.False(iNew(7, "xh", 3.14F, 'r'));
        }

        [Fact]
        public static void BetaTest()
        {
            Expression<Func<int, int>> f = x => x * 2;

            var fRes = f.Beta(Expression.Constant(5)).Compile()();
            Assert.Equal(10, fRes);
        }
    }
}

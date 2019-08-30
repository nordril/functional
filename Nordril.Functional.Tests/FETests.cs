using Nordril.Functional.Data;
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
        public static void LiftTest()
        {
            Func<List<int>, int> f = x => x.Count * 2;
            var expr = f.LiftToExpression().Compile();

            var xs = new List<int> { 4, 7, 218, 47 };

            Assert.Equal(f(xs), expr(xs));
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

        [Fact]
        public static void CastOutputType()
        {
            Expression<Func<string, Manager>> f = x => new Manager { Name = x, Age = 20, Role = "CEO", LevelOfManageritude = 3 };

            var fUpcast = f.CastReturnType<string, Manager, Person>();

            Assert.IsType<Func<string, Person>>(fUpcast.Compile());

            var res = fUpcast.Compile()("bill");

            Assert.Equal("bill", res.Name);
        }

        [Fact]
        public static void CastInputType()
        {
            Expression<Func<Employee, string>> f = x => x.Name;

            var fDowncast = f.CastInputType<Employee, Manager, string>();

            var e = new Manager { Name = "bob", Age = 30, Role = "accountant", LevelOfManageritude = 0 };

            Assert.IsType<Func<Manager, string>>(fDowncast.Compile());

            var res = fDowncast.Compile()(e);

            Assert.Equal("bob", res);
        }

        public class Person
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }

        public class Employee : Person
        {
            public string Role { get; set; }
        }

        public class Manager : Employee
        {
            public int LevelOfManageritude { get; set; }
        }

        [Fact]
        public static void BinaryTest()
        {
            Expression<Func<int, double>> f = x => x * 2;
            Expression<Func<int, double>> g = x => x * 3;
            Expression<Func<double, double, double>> c = (x, y) => x + y;

            var combined = f.Binary(g, c).Compile();

            Assert.Equal(40, combined(8));
        }

        [Fact]
        public static void FirstTest()
        {
            Expression<Func<int, string>> f = x => x.ToString();
            var fFirst = f.First<int, string, double>().Compile();

            Assert.Equal(("7", 3.14D), fFirst((7, 3.14D)));
        }

        [Fact]
        public static void SecondTest()
        {
            Expression<Func<int, string>> f = x => x.ToString();
            var fFirst = f.Second<int, string, double>().Compile();

            Assert.Equal((3.14D, "7"), fFirst((3.14D, 7)));
        }

        [Fact]
        public static void BothTest()
        {
            Expression<Func<int, double>> f = x => x * 2;
            Expression<Func<string, int>> g = x => x.Length;

            var compiled = f.Both(g).Compile();

            Assert.Equal((8, 3), compiled((4, "abc")));
        }

        [Fact]
        public static void FanoutTest()
        {
            Expression<Func<int, double>> f = x => x * 2;
            Expression<Func<int, string>> g = x => x.ToString();

            var compiled = f.Fanout(g).Compile();

            Assert.Equal((6, "3"), compiled(3));
        }

        [Fact]
        public static void LeftTest()
        {
            Expression<Func<int, string>> f = x => x.ToString();
            var fLifted = f.Left<int, string, bool>().Compile();

            Assert.True(fLifted(Either.FromLeft<int, bool>(5)).IsLeft);
            Assert.True(fLifted(Either.FromRight<int, bool>(false)).IsRight);
            Assert.False(fLifted(Either.FromRight<int, bool>(false)).Right());
            Assert.Equal("5", fLifted(Either.FromLeft<int, bool>(5)).Left());
        }

        [Fact]
        public static void RightTest()
        {
            Expression<Func<int, string>> f = x => x.ToString();
            var fLifted = f.Right<int, string, bool>().Compile();

            Assert.True(fLifted(Either.FromLeft<bool, int>(false)).IsLeft);
            Assert.False(fLifted(Either.FromLeft<bool, int>(false)).Left());
            Assert.True(fLifted(Either.FromRight<bool, int>(5)).IsRight);
            Assert.Equal("5", fLifted(Either.FromRight<bool, int>(5)).Right());
        }

        [Fact]
        public static void EitherOrtest()
        {
            Expression<Func<int, double>> f = x => x * 2;
            Expression<Func<string, char>> g = x => x[0];

            var fg = f.EitherOr(g).Compile();

            Assert.True(fg(Either.FromLeft<int, string>(5)).IsLeft);
            Assert.True(fg(Either.FromRight<int, string>("abc")).IsRight);
            Assert.Equal(10D, fg(Either.FromLeft<int, string>(5)).Left());
            Assert.Equal('a', fg(Either.FromRight<int, string>("abc")).Right());
        }

        [Fact]
        public static void FaninTest()
        {
            Expression<Func<double, int>> f = x => ((int)x) * 2;
            Expression<Func<string, int>> g = x => x.Length;

            var fg = f.Fanin(g).Compile();

            Assert.Equal(14, fg(Either.FromLeft<double, string>(7.4D)));
            Assert.Equal(3, fg(Either.FromRight<double, string>("xyz")));
        }
    }
}

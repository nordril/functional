using Nordril.Functional.Data;
using Nordril.Functional.Lens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Xunit;

namespace Nordril.Functional.Tests.Data
{
    public sealed class LensTests
    {
        [Fact]
        public void GetterTest()
        {
            var s = new List<string> { "abc", "def", "ghi", "jkl", "mno", "pqr", "stu", "vwx", "yz" };

            var getAtIndex = L.Make.Getter<List<string>, string>(xs => xs[5]);
            var getChar = L.Make.Getter<string, char>(xs => xs.First());

            Assert.Equal("pqr", L.Get(getAtIndex, s));
            Assert.Equal('x', L.Get(getChar, "xyz"));

            var getter = getAtIndex.Then(getChar);

            var actual = L.Get(getter, s);

            Assert.Equal('p', actual);
        }

        [Fact]
        public void SetterTest()
        {
            var s = new List<string> { "abc", "def", "ghi", "jkl", "mno", "pqr", "stu", "vwx", "yz" };

            var setAtIndex = L.Make.Setter<List<string>, string>(update => xss => { xss[1] = update(xss[1]); return xss; });
            var setChar = L.Make.Setter<string, char>((Func<char, char> update) => (string ss) => new string(ss.SelectAt(0, update).ToArray()));

            var setter = setAtIndex.Then(setChar);

            var actual = L.Set(setter, s, '0');

            Assert.Equal(new string[] { "abc", "0ef", "ghi", "jkl", "mno", "pqr", "stu", "vwx", "yz" }, actual);

            s = new List<string> { "abc", "def", "ghi", "jkl", "mno", "pqr", "stu", "vwx", "yz" };

            actual = L.Update(setter, s, c => (char)(c + 1));

            Assert.Equal(new string[] { "abc", "eef", "ghi", "jkl", "mno", "pqr", "stu", "vwx", "yz" }, actual);
        }

        [Fact]
        public void MonoLensTest()
        {
            var s = new List<string> { "abc", "def", "ghi", "jkl", "mno", "pqr", "stu", "vwx", "yz" };

            var listLens = L.Make.Lens<List<string>, string>(xs => xs[1], (xs, ys) => { xs[1] = ys; return xs; });
            var stringLens = L.Make.Lens<string, char>(ss => ss[0], (ss, c) => new string(ss.SelectAt(0, _ => c).ToArray()));

            Assert.Equal("def", L.Get(listLens, s));
            Assert.Equal('x', L.Get(stringLens, "xyz"));

            var lens = listLens.Then(stringLens);

            Assert.Equal('d', L.Get(lens, s));

            L.Set(listLens, s, "XYZ");

            Assert.Equal(new string[] { "abc", "XYZ", "ghi", "jkl", "mno", "pqr", "stu", "vwx", "yz" }, s);
            Assert.Equal("XYZ", L.Get(listLens, s));

            L.Set(lens, s, 'u');

            Assert.Equal(new string[] { "abc", "uYZ", "ghi", "jkl", "mno", "pqr", "stu", "vwx", "yz" }, s);
            Assert.Equal("uYZ", L.Get(listLens, s));
        }

        [Fact]
        public void MappedTest()
        {
            var s = new FuncList<string> { "abc", "def", "ghi", "jkl", "mno", "pqr", "stu", "vwx", "yz" };

            var mapLens = L.Make.MappedSetter<FuncList<string>, FuncList<int>, string, int>();
            var intLens = L.Make.Setter<string, int, string, int>(f => x => f(x));

            var actual = L.Update(mapLens, s, ss => ss.Length);

            Assert.Equal(new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 2 }, actual);

            actual = L.Update(mapLens.Then(intLens), s, x => 2 * x.Length);

            Assert.Equal(new int [] {6, 6, 6, 6, 6, 6, 6, 6, 4}, actual);
        }

        [Fact]
        public void LensAndGetTest()
        {
            var s = new List<string> { "abc", "def", "ghi", "jkl", "mno", "pqr", "stu", "vwx", "yz" };

            var listLens = L.Make.Lens<List<string>, string>(xs => xs[1], (xs, ys) => { xs[1] = ys; return xs; });
            var stringGetter = L.Make.Getter<string, char>(s => s[0]);

            var getter = listLens.Then(stringGetter);

            Assert.Equal('d', L.Get(getter, s));
        }

        [Fact]
        public void MappedIndexedTest()
        {
            var s = new FuncList<string> { "abc", "def", "ghi", "jkl", "mno", "pqr", "stu", "vwx", "yz" };

            var listSetter = L.Make.KeyedMappedSetter<FuncList<string>, FuncList<int>, string, int, int>();
            var actual = L.Update(listSetter, s, x => x.Item1.Length + x.Item2);

            Assert.Equal(new int[] { 3, 4, 5, 6, 7, 8, 9, 10, 10 }, actual);
        }

        [Fact]
        public void ListIndexTest()
        {
            var s = new List<string> { "abc", "def", "ghi", "jkl", "mno", "pqr", "stu", "vwx", "yz" };

            var listSetter = L.Make.IndexSetter<List<string>, string>();

            L.Update(listSetter, (s,2), x => x.value + "XYZ");

            Assert.Equal(new string[] { "abc", "def", "ghiXYZ", "jkl", "mno", "pqr", "stu", "vwx", "yz" }, s);

            L.Update(listSetter, (s, 99), x => x + "ggg");

            Assert.Equal(new string[] { "abc", "def", "ghiXYZ", "jkl", "mno", "pqr", "stu", "vwx", "yz" }, s);
        }

        [Fact]
        public void DictionaryIndexTest()
        {
            var s = new List<string> { "abc", "def", "ghi", "jkl", "mno", "pqr", "stu", "vwx", "yz" }.ToDictionary(s => s, s => s.Length);

            var dictSetter = L.Make.IndexSetter<Dictionary<string, int>, int, string>();

            L.Update(dictSetter, (s, "mno"), x => x.value + x.key.Length);

            Assert.Equal(new string[] { "abc", "def", "ghi", "jkl", "mno", "pqr", "stu", "vwx", "yz" }.ToDictionary(s => s, s => s == "mno" ? s.Length*2 : s.Length), s);

            L.Update(dictSetter, (s, "nonExistent"), x => x.value * 100);

            Assert.Equal(new string[] { "abc", "def", "ghi", "jkl", "mno", "pqr", "stu", "vwx", "yz" }.ToDictionary(s => s, s => s == "mno" ? s.Length * 2 : s.Length), s);
        }

        [Fact]
        public void ListAtTest()
        {
            var s = new List<string> { "abc", "def", "ghi", "jkl", "mno", "pqr", "stu", "vwx", "yz" };

            var listIndexSetter = L.Make.AtSetter<List<string>, string>(3);

            //Update existing element
            L.Update(listIndexSetter, s, x => from m in x select m + "aaa");

            Assert.Equal(new string[] { "abc", "def", "ghi", "jklaaa", "mno", "pqr", "stu", "vwx", "yz" }, s);

            //Remove existing elements
            L.Update(listIndexSetter, s, x => Maybe.Nothing<string>());
            Assert.Equal(new string[] { "abc", "def", "ghi", "mno", "pqr", "stu", "vwx", "yz" }, s);
            L.Update(listIndexSetter, s, x => Maybe.Nothing<string>());
            Assert.Equal(new string[] { "abc", "def", "ghi", "pqr", "stu", "vwx", "yz" }, s);
            L.Update(listIndexSetter, s, x => Maybe.Nothing<string>());
            Assert.Equal(new string[] { "abc", "def", "ghi", "stu", "vwx", "yz" }, s);
            L.Update(listIndexSetter, s, x => Maybe.Nothing<string>());
            Assert.Equal(new string[] { "abc", "def", "ghi", "vwx", "yz" }, s);
            L.Update(listIndexSetter, s, x => Maybe.Nothing<string>());
            Assert.Equal(new string[] { "abc", "def", "ghi", "yz" }, s);

            //Remove non-existing element
            L.Update(listIndexSetter, s, x => Maybe.Nothing<string>());
            Assert.Equal(new string[] { "abc", "def", "ghi" }, s);
            L.Update(listIndexSetter, s, x => Maybe.Nothing<string>());
            Assert.Equal(new string[] { "abc", "def", "ghi" }, s);
            L.Update(listIndexSetter, s, x => x);
            Assert.Equal(new string[] { "abc", "def", "ghi" }, s);

            //Insert element
            L.Update(listIndexSetter, s, x => Maybe.Just("yyy"));
            Assert.Equal(new string[] { "abc", "def", "ghi", "yyy" }, s);

            s = new List<string>();
            L.Update(listIndexSetter, s, x => Maybe.Just("yyy"));
            Assert.Equal(new string[] { "yyy" }, s);
            L.Update(listIndexSetter, s, x => Maybe.Just("yyy"));
            Assert.Equal(new string[] { "yyy", "yyy" }, s);
            L.Update(listIndexSetter, s, x => Maybe.Just("yyy"));
            Assert.Equal(new string[] { "yyy", "yyy", "yyy" }, s);
            L.Update(listIndexSetter, s, x => Maybe.Just("yyy"));
            Assert.Equal(new string[] { "yyy", "yyy", "yyy", "yyy" }, s);
            L.Update(listIndexSetter, s, x => Maybe.Just("yyy"));
            Assert.Equal(new string[] { "yyy", "yyy", "yyy", "yyy" }, s);
        }

        [Fact]
        public void DictionaryAtTest()
        {
            var s = new List<string> { "abc", "def", "ghi", "jkl", "mno", "pqr", "stu", "vwx", "yz" }.ToDictionary(s => s, s => s.Length);

            var dictIndexSetter = L.Make.AtSetter<Dictionary<string, int>, int, string>("jkl");

            //Update existing element
            L.Update(dictIndexSetter, s, x => from m in x select 3*m);
            Assert.Equal(new Dictionary<string, int> { { "abc", 3 }, { "def", 3 }, { "ghi", 3 }, { "jkl", 9 }, { "mno", 3 }, { "pqr", 3 }, { "stu", 3 }, { "vwx", 3 }, { "yz", 2 } }, s);

            //Remove existing elements
            L.Update(dictIndexSetter, s, x => Maybe.Nothing<int>());
            Assert.Equal(new Dictionary<string, int> { { "abc", 3 }, { "def", 3 }, { "ghi", 3 }, { "mno", 3 }, { "pqr", 3 }, { "stu", 3 }, { "vwx", 3 }, { "yz", 2 } }, s);

            //Remove non-existing element
            L.Update(dictIndexSetter, s, x => Maybe.Nothing<int>());
            Assert.Equal(new Dictionary<string, int> { { "abc", 3 }, { "def", 3 }, { "ghi", 3 }, { "mno", 3 }, { "pqr", 3 }, { "stu", 3 }, { "vwx", 3 }, { "yz", 2 } }, s);

            //Insert element
            L.Update(dictIndexSetter, s, x => Maybe.Just(100));
            Assert.Equal(new Dictionary<string, int> { { "abc", 3 }, { "def", 3 }, { "ghi", 3 }, { "jkl", 100 }, { "mno", 3 }, { "pqr", 3 }, { "stu", 3 }, { "vwx", 3 }, { "yz", 2 } }, s);
        }

        [Fact]
        public void PrismJustTest()
        {
            var m = Maybe.Just(5);
            var lens = L.Make.Just<int, int>();

            var actual = L.TryGet(lens, m);

            Assert.True(actual.HasValue);
            Assert.Equal(5, actual.Value());

            m = Maybe.Nothing<int>();
            actual = L.TryGet(lens, m);

            Assert.False(actual.HasValue);
        }

        [Fact]
        public void PrismEitherTest()
        {
            var left = L.Make.First<string, int, string>();
            var right = L.Make.Second<string, int, int>();

            Assert.Equal("xyz", L.TryGet(left, Either.EitherWith<string, int>(Either.One("xyz"))).Value());
            Assert.False(L.TryGet(right, Either.EitherWith<string, int>(Either.One("xyz"))).HasValue);

            Assert.Equal(25, L.TryGet(right, Either.EitherWith<string, int>(Either.Two(25))).Value());
            Assert.False(L.TryGet(left, Either.EitherWith<string, int>(Either.Two(25))).HasValue);
        }

        [Fact]
        public void PrismNestTest()
        {
            var left = L.Make.First<Either<string, bool>, Either<decimal, int>, Either<string, bool>>();
            var right = L.Make.Second<Either<string, bool>, Either<decimal, int>, Either<decimal, int>>();

            var left2 = L.Make.First<string, bool, string>();
            var right2 = L.Make.Second<string, bool, bool>();

            var leftLeft = left.Then(left2);

            Assert.Equal("xyz", L.TryGet(leftLeft, Either.EitherWith<Either<string, bool>, Either<decimal, int>>(Either.One(Either.EitherWith<string, bool>(Either.One("xyz"))))).Value());
            Assert.False(L.TryGet(leftLeft, Either.EitherWith<Either<string, bool>, Either<decimal, int>>(Either.One(Either.EitherWith<string, bool>(Either.Two(true))))).HasValue);
            Assert.False(L.TryGet(leftLeft, Either.EitherWith<Either<string, bool>, Either<decimal, int>>(Either.Two(Either.EitherWith<decimal, int>(Either.One(3.0M))))).HasValue);
            Assert.False(L.TryGet(leftLeft, Either.EitherWith<Either<string, bool>, Either<decimal, int>>(Either.Two(Either.EitherWith<decimal, int>(Either.Two(19))))).HasValue);
        }

        [Fact]
        public void PrismAssembleTest()
        {
            var expected = Either.EitherWith<string, bool>(Either.One("xyz"));

            var left = L.Make.First<string, bool, string>();
            var right = L.Make.Second<string, bool, bool>();

            var actual = L.Review(left, "abc");
            Assert.True(actual.IsLeft);
            Assert.Equal("abc", actual.Left());

            actual = L.Review(right, true);
            Assert.True(actual.IsRight);
            Assert.True(actual.Right());
        }
    }
}

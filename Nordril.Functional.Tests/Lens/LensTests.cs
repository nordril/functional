using Nordril.Functional.Data;
using Nordril.Functional.Lens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Nordril.Functional.Tests.Lens
{
    public sealed class LensTests
    {
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
            L.Update(dictIndexSetter, s, x => from m in x select 3 * m);
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
    }
}

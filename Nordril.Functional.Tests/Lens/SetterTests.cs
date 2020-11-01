using Nordril.Functional.Data;
using Nordril.Functional.Lens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Nordril.Functional.Tests.Lens
{
    public sealed class SetterTests
    {
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
        public void MappedTest()
        {
            var s = new FuncList<string> { "abc", "def", "ghi", "jkl", "mno", "pqr", "stu", "vwx", "yz" };

            var mapLens = L.Make.MappedSetter<FuncList<string>, FuncList<int>, string, int>();
            var intLens = L.Make.Setter<string, int, string, int>(f => x => f(x));

            var actual = L.Update(mapLens, s, ss => ss.Length);

            Assert.Equal(new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 2 }, actual);

            actual = L.Update(mapLens.Then(intLens), s, x => 2 * x.Length);

            Assert.Equal(new int[] { 6, 6, 6, 6, 6, 6, 6, 6, 4 }, actual);
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

            L.Update(listSetter, (s, 2), x => x.value + "XYZ");

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

            Assert.Equal(new string[] { "abc", "def", "ghi", "jkl", "mno", "pqr", "stu", "vwx", "yz" }.ToDictionary(s => s, s => s == "mno" ? s.Length * 2 : s.Length), s);

            L.Update(dictSetter, (s, "nonExistent"), x => x.value * 100);

            Assert.Equal(new string[] { "abc", "def", "ghi", "jkl", "mno", "pqr", "stu", "vwx", "yz" }.ToDictionary(s => s, s => s == "mno" ? s.Length * 2 : s.Length), s);
        }
    }
}

using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Nordril.Functional.Tests.Data
{
    public class FuncDictionaryTests
    {
        [Theory]
        [InlineData(null, null, 3, "c")]
        [InlineData(new int[0], new string[0], 3, "c")]
        [InlineData(new int[] { 2 }, new string[] { "b" }, 3, "c")]
        [InlineData(new int[] { 1, 2, 3 }, new string[] { "a", "b", "c" }, 4, "d")]
        [InlineData(new int[] { 5, 4, 6, 7, 8 }, new string[] { "e", "d", "f", "g", "h" }, 47, "xx")]
        public static void FuncDictionaryAdd(int[] xs, string[] ys, int key, string value)
        {
            var intComp = new FuncComparer<int>((x, y) => x.CompareTo(y), x => x.GetHashCode());

            var fd = new FuncDictionary<int, string>(intComp, (xs ?? new int[0]).Zip(ys ?? new string[0]));
            var origLength = fd.Count;
            fd.Add(key, value);

            var dict = new FuncDictionary<int, string>(intComp, (xs ?? new int[0]).Zip(ys ?? new string[0]))
            {
                { key, value }
            };

            Assert.Equal(dict.OrderBy(kv => kv.Key), fd.OrderBy(kv => kv.Key));
            Assert.Equal(origLength + 1, fd.Count);
        }

        [Theory]
        [InlineData(true, new int[] { }, new string[] { }, new int[] { }, new string[] { })]
        [InlineData(true, new int[] { 1 }, new string[] { "a" }, new int[] { 1 }, new string[] { "a" })]
        [InlineData(true, new int[] { 1, 2, 3 }, new string[] { "a", "b", "c" }, new int[] { 1, 2, 3 }, new string[] { "a", "b", "c" })]
        [InlineData(true, new int[] { 5, 4, 2, 8, 3, 0 }, new string[] { "e", "d", "b", "h", "c", "0" }, new int[] { 5, 4, 2, 8, 3, 0 }, new string[] { "e", "d", "b", "h", "c", "0" })]
        [InlineData(false, new int[] { 4 }, new string[] { "d" }, new int[] { }, new string[] { })]
        [InlineData(false, new int[] { }, new string[] { }, new int[] { 7 }, new string[] { "g" })]
        [InlineData(false, new int[] { 1, 5 }, new string[] { "a", "e" }, new int[] { 1 }, new string[] { "a" })]
        [InlineData(false, new int[] { 1, 2, 3 }, new string[] { "a", "b", "c" }, new int[] { 1, 2, 3, 4 }, new string[] { "a", "b", "c", "d" })]
        [InlineData(true, new int[] { 1, 2, 3 }, new string[] { "a", "b", "c" }, new int[] { 3, 2, 1 }, new string[] { "c", "b", "a" })]
        [InlineData(false, new int[] { 1, 2, 3 }, new string[] { "a", "b", "c" }, new int[] { 1, 2, 3 }, new string[] { "a", "b", "x" })]
        public static void EqualTest(bool shouldBeEqual, int[] xs, string[] xv, int[] ys, string[] yv)
        {
            var expected = FuncDictionary.Make(xs.Zip(xv));
            var actual = FuncDictionary.Make(ys.Zip(yv));

            if (shouldBeEqual)
                Assert.True(actual.Equals(expected));
            else
                Assert.False(actual.Equals(expected));
        }
    }
}

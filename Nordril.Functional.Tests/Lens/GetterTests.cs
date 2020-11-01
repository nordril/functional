using Nordril.Functional.Lens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Nordril.Functional.Tests.Lens
{
    public sealed class GetterTests
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
    }
}

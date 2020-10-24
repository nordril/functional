using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Nordril.Functional.Tests
{
    public sealed class FuncComparerTests
    {
        [Fact]
        public void CompareTest()
        {
            var comp = FuncComparer.Make<int>();

            Assert.True(comp.Compare(4, 4) == 0);
            Assert.True(comp.Compare(7, 4) == 1);
            Assert.True(comp.Compare(4, 7) == -1);
            Assert.Equal(50.GetHashCode(), comp.GetHashCode(50));
            Assert.True(comp.Equals(5, 5));
            Assert.False(comp.Equals(8, 5));

            comp = FuncComparer.Make<int>((x, y) => x.CompareTo(y), x => x.GetHashCode());

            Assert.True(comp.Compare(4, 4) == 0);
            Assert.True(comp.Compare(7, 4) == 1);
            Assert.True(comp.Compare(4, 7) == -1);
            Assert.Equal(50.GetHashCode(), comp.GetHashCode(50));
            Assert.NotEqual(comp.GetHashCode(50), comp.GetHashCode(60));
            Assert.True(comp.Equals(5, 5));
            Assert.False(comp.Equals(8, 5));
        }

        [Fact]
        public void EqualsTest()
        {
            var eq = FuncEqualityComparer.Make<int>();

            Assert.Equal(50.GetHashCode(), eq.GetHashCode(50));
            Assert.NotEqual(50.GetHashCode(), eq.GetHashCode(60));
            Assert.True(eq.Equals(5, 5));
            Assert.False(eq.Equals(8, 5));

            eq = FuncEqualityComparer.Make<int>((x, y) => x.Equals(y));

            Assert.Equal(50.GetHashCode(), eq.GetHashCode(50));
            Assert.NotEqual(50.GetHashCode(), eq.GetHashCode(60));
            Assert.True(eq.Equals(5, 5));
            Assert.False(eq.Equals(8, 5));

            eq = FuncEqualityComparer.Make<int>((x, y) => x.Equals(y), x => 2*x);

            Assert.Equal(100, eq.GetHashCode(50));
            Assert.NotEqual(eq.GetHashCode(50), eq.GetHashCode(60));
            Assert.True(eq.Equals(5, 5));
            Assert.False(eq.Equals(8, 5));
        }
    }
}

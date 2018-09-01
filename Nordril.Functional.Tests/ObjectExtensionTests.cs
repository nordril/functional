using Nordril.Functional;
using Nordril.Functional.Tests.DiffNs;
using System;
using System.Collections.Generic;
using Xunit;

namespace Nordril.Functional.Tests.DiffNs
{
    public class X5
    {
        public string F1 { get; set; }
        public int F2 { get; set; }

        public override int GetHashCode() => this.DefaultHash(F1, F2);
    }
}

namespace Nordril.Functional.Tests
{
    public static class ObjectExtensionsTests
    {
        private struct X1
        {
            public override int GetHashCode() => this.DefaultHash();
        }

        private class X2
        {
            public override int GetHashCode() => this.DefaultHash();
        }

        private class X3
        {
            public string F1 { get; set; }
            public int F2 { get; set; }
            public X1 F3 { get; set; }

            public override int GetHashCode() => this.DefaultHash(F1, F2, F3);
        }

        private class X4
        {
            public string F1 { get; set; }
            public int F2 { get; set; }
            public X1 F3 { get; set; }
            public int Irrelevant { get; set; }

            public override int GetHashCode() => this.DefaultHash(F1, F2, F3);
        }

        public class X6
        {
            public string F1 { get; set; }
            public int F2 { get; set; }

            public override int GetHashCode() => this.DefaultHash(F1, F2);
        }

        [Fact]
        public static void IdenticalObjectsHavSameHashStruct()
        {
            var x = new X1();

            Assert.Equal(x.GetHashCode(), x.GetHashCode());
        }

        [Fact]
        public static void IdenticalObjectsHavSameHashClass()
        {
            var x = new X2();

            Assert.Equal(x.GetHashCode(), x.GetHashCode());
        }

        [Fact]
        public static void SameObjectsHaveSameHashes()
        {
            var x = new X3
            {
                F1 = "some string",
                F2 = 17,
                F3 = new X1()
            };

            var y = new X3
            {
                F1 = "some string",
                F2 = 17,
                F3 = new X1()
            };

            Assert.Equal(x.GetHashCode(), y.GetHashCode());
        }

        [Fact]
        public static void DifferentObjectTypesHaveDifferentHashes()
        {
            var x = new X1();
            var y = new X2();

            Assert.NotEqual(x.GetHashCode(), y.GetHashCode());
        }

        [Fact]
        public static void DifferentObjectsHaveDifferentHashes()
        {
            var x = new X3
            {
                F1 = "some string",
                F2 = 17,
                F3 = new X1()
            };

            var y = new X3
            {
                F1 = "some string xxx",
                F2 = 17,
                F3 = new X1()
            };

            Assert.NotEqual(x.GetHashCode(), y.GetHashCode());
        }

        [Fact]
        public static void DifferentObjectsHavSameHashesIfOnlyIrrelevantFieldsDiffer()
        {
            var x = new X4
            {
                F1 = "some string",
                F2 = 17,
                F3 = new X1(),
                Irrelevant = 999
            };

            var y = new X4
            {
                F1 = "some string",
                F2 = 17,
                F3 = new X1(),
                Irrelevant = 1000
            };

            Assert.Equal(x.GetHashCode(), y.GetHashCode());
        }

        [Fact]
        public static void DifferentObjectsHaveDifferentHashesIfDifferentNamespaces()
        {
            var x = new X5
            {
                F1 = "some string",
                F2 = 17,
            };

            var y = new X6
            {
                F1 = "some string xxx",
                F2 = 17,
            };

            Assert.NotEqual(x.GetHashCode(), y.GetHashCode());
        }

        [Theory]
        [InlineData(typeof(int), false, "Int32")]
        [InlineData(typeof(int), true, "System.Int32")]
        [InlineData(typeof(List<int>), false, "List<Int32>")]
        [InlineData(typeof(List<int>), true, "System.Collections.Generic.List<System.Int32>")]
        [InlineData(typeof(List<List<(int, bool)>>), false, "List<List<ValueTuple<Int32, Boolean>>>")]
        [InlineData(typeof(List<List<(int, bool)>>), true, "System.Collections.Generic.List<System.Collections.Generic.List<System.ValueTuple<System.Int32, System.Boolean>>>")]
        public static void GetGenericNameTest(Type t, bool useFullName, string expected)
        {
            var actual = t.GetGenericName(useFullName);

            Assert.Equal(actual, expected);
        }
    }
}

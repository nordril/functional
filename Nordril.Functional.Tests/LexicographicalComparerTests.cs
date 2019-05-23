using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Nordril.Functional.Tests
{
    public class LexicographicalComparerTests
    {
        public static IEnumerable<object[]> CompareData()
        {
            yield return new object[]
            {
                new List<int>(),
                new List<int>(),
                0
            };

            yield return new object[]
            {
                new List<int>{0},
                new List<int>(),
                1
            };

            yield return new object[]
            {
                new List<int>(),
                new List<int>{0},
                -1
            };

            yield return new object[]
            {
                new List<int>{6 },
                new List<int>{6 },
                0
            };

            yield return new object[]
            {
                new List<int>{-5, 7 },
                new List<int>(),
                1
            };

            yield return new object[]
            {
                new List<int>{ },
                new List<int>{-5, 7},
                -1
            };

            yield return new object[]
            {
                new List<int>{ -5 },
                new List<int>{-5, 7},
                -1
            };

            yield return new object[]
            {
                new List<int>{ 3 },
                new List<int>{-5, 7},
                1
            };

            yield return new object[]
            {
                new List<int>{ -5, 7, 5},
                new List<int>{-5, 7},
                1
            };

            yield return new object[]
            {
                new List<int>{ -5, 6},
                new List<int>{-5, 7},
                -1
            };

            yield return new object[]
            {
                new List<int>{ 1, 2,1, 7, 1},
                new List<int>{1, 2,1, 3, 1},
                1
            };
        }

        [Theory]
        [MemberData(nameof(CompareData))]
        public static void CompareTest(IEnumerable<int> left, IEnumerable<int> right, int expected)
        {
            var comparer = new LexicographicalComparer<int>((x, y) => x.CompareTo(y));

            var actual = comparer.Compare(left, right);

            Assert.Equal(expected, actual);
        }
    }
}

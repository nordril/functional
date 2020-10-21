using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Nordril.Functional.Tests
{
    public class PatternTests
    {
        public static IEnumerable<object[]> MultiplePatternsReturnResultTestData()
        {
            yield return new object[]
            {
                100,
                50,
                Pattern
                    .Match((int x) => x == 50, x => x*2)
                    .Match(x => x == 40, x => x*3)
            };

            yield return new object[]
            {
                120,
                40,
                Pattern
                    .Match((int x) => x == 50, x => x*2)
                    .Match(x => x == 40, x => x*3)
            };

            yield return new object[]
            {
                150,
                30,
                Pattern
                    .Match((int x) => x == 50, x => x*2)
                    .Match(x => x == 40, x => x*3)
                    .Match(x => x == 30, x => x*5)
            };
        }

        [Fact]
        public void EmptyPatternThrowsExceptionTest()
        {
            var p = Pattern.MatchMany(new (Func<int, bool>, Func<int, int>)[0]);

            Assert.Throws<PatternMatchException>(() => p.Run(4));
        }

        [Fact]
        public void SinglePatternThrowsExceptionTest()
        {
            var p = Pattern.Match<int, int>(x => false, x => 54);

            Assert.Throws<PatternMatchException>(() => p.Run(5));
            Assert.Throws<PatternMatchException>(() => p.Run(54));
        }

        [Fact]
        public void SinglePatternReturnsResultTest()
        {
            var p = Pattern.Match<int, int>(x => x % 2 == 0, x => x*3);

            Assert.Equal(12, p.Run(4));
        }

        [Theory]
        [MemberData(nameof(MultiplePatternsReturnResultTestData))]
        public void MultiplePatternsReturnResultTest(int expected, int input, Pattern<int, int> p)
        {
            Assert.Equal(expected, p.Run(input));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(4)]
        [InlineData(15)]
        [InlineData(16)]
        [InlineData(147)]
        [InlineData(2127)]
        public void RecursivePatternReturnsResultTest(int input)
        {
            Func<int, int> go = null;

            //Collatz-algorithm without tail-recursion
            var p = Pattern
                .Match((int x) => x <= 1, x => 1)
                .Match(x => x % 2 == 0, x => go(x / 2))
                .WithDefault(x => go(x * 3 + 1));

            go = x => p.Run(x);

            var task = Task.Run(() => go(input));

            bool isCompleted = Task.WaitAll(new[] { task }, 30000);

            if (!isCompleted)
                throw new TimeoutException();

            Assert.Equal(1, task.Result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(4)]
        [InlineData(15)]
        [InlineData(16)]
        [InlineData(147)]
        [InlineData(2127)]
        [InlineData(361285)]
        public void TailRecursivePatternReturnsResultTest(int input)
        {
            //Collatz-algorithm
            var p = Pattern
                .Match((int x) => x <= 1, x => 1)
                .MatchTailRec(x => x % 2 == 0, x => x / 2)
                .MatchTailRec(x => x % 2 == 1, x => x * 3 + 1);

            var task = Task.Run(() => p.Run(input));

            bool isCompleted = Task.WaitAll(new[] { task }, 30000);

            if (!isCompleted)
                throw new TimeoutException();

            Assert.Equal(1, task.Result);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(24, 4)]
        [InlineData(120, 5)]
        [InlineData(1307674368000, 15)]
        public void TailRecursivePatternReturnsResultTest2(long expected, int input)
        {
            //Factorial
            var p = Pattern
                .Match(((int n, long sum) x) => x.n <= 1, x => x.sum)
                .MatchTailRec(x => x.n > 1, x => (x.n-1, (x.n * x.sum)));

            var task = Task.Run(() => p.Run((input, 1)));

            bool isCompleted = Task.WaitAll(new[] { task }, 30000);

            if (!isCompleted)
                throw new TimeoutException();

            Assert.Equal(expected, task.Result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(24)]
        [InlineData(120)]
        [InlineData(12785670)]
        public void TailRecursivePatternReturnsResultTest3(int expected)
        {
            //expected = Math.Max(0,input), since we add 1 at each iteration. This is only to test against a stack overflow
            var p = Pattern
                .Match(((int n, int sum) x) => x.n <= 0, x => x.sum)
                .MatchTailRec(x => x.n >= 1, x => (x.n - 1, (1 + x.sum)));

            var task = Task.Run(() => p.Run((expected, 0)));

            bool isCompleted = Task.WaitAll(new[] { task }, 30000);

            if (!isCompleted)
                throw new TimeoutException();

            Assert.Equal(expected, task.Result);
        }

        [Theory]
        [InlineData(new int[0], new int[0])]
        [InlineData(new int[] { 3, }, new int[] { 6 })]
        [InlineData(new int[] { 4, 3 }, new int[] { 8, 6 })]
        [InlineData(new int[] { 4, 3, 6, 8, 1, 0, 3 }, new int[] { 8, 6, 12, 16, 2, 0, 6 })]
        public void TailRecModuloConsMapTest(int[] xs, int[] expected)
        {
            var p = Pattern
                .Match((FuncList<int> zs) => zs.Count == 0, zs => zs)
                .MatchTailRecModuloCons(xs => xs.Count > 0, xs => 2*xs[0], xs => xs.GetRange(1, xs.Count - 1));

            var actual = p.Run(new FuncList<int>(xs));

            Assert.Equal(expected, actual);

            p = Pattern
                .Match((FuncList<int> zs) => zs.Count() < 0, zs => zs)
                .MatchTailRecModuloCons(xs => xs.Count > 0, xs => 2 * xs[0], xs => xs.GetRange(1, xs.Count - 1))
                .WithDefault(xs => xs);

            actual = p.Run(new FuncList<int>(xs));

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(100)]
        [InlineData(1_000)]
        [InlineData(10_000)]
        [InlineData(100_000)]
        public void TailRecModuloConsSumTest(int count)
        {
            var p = Pattern
                .Match((List<int> zs) => zs.Count == 0, zs => 0)
                .MatchTailRecModuloCons(zs => zs.Count > 0, zs => zs[0], zs => zs.GetRange(1, zs.Count - 1), x => x, (x, y) => x + y, (x, y) => x + y);

            var xs = Enumerable.Repeat(1, count).ToList();

            var actual = p.Run(xs);

            Assert.Equal(count, actual);
        }
    }
}

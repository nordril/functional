using Nordril.Functional.Category;
using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace Nordril.Functional.Tests.Data
{
    public class RandomTests
    {
        private readonly ITestOutputHelper outputHelper;

        public RandomTests(ITestOutputHelper outputHelper)
        {
            this.outputHelper = outputHelper;
        }

        [Fact]
        public void RandomLinqQuery()
        {

            var randSum =
                from _ in Rnd.Put(1234567)
                from a in Rnd.RandomInt<Random>(0, 10000)
                from b in Rnd.RandomInt<Random>(0, 10000)
                from c in Rnd.RandomInt<Random>(0, 10000)
                select a + b + c;

            var expected = 7791 + 7597 + 2744;

            var actual = randSum.RunForResult(Rng<Random>.FromRandom(0));

            var actual2 = randSum.RunForResult(Rng<Random>.FromRandom(7856));

            Assert.Equal(expected, actual);
            Assert.Equal(expected, actual2);
        }

        [Fact]
        public void RandomBoundedDoubleInRange()
        {
            var min = 52D;
            var max = 148D;

            var doubles = Enumerable
                .Repeat((min, max), 50)
                .SelectAp<(double, double), double, Random<Random, IEnumerable<double>>>(bounds => Rnd.RandomDouble<Random>(bounds.Item1, bounds.Item2)) as Random<Random, IEnumerable<double>>;

            var actual = (from _ in Rnd.Put(123897)
                          from xs in doubles
                          select xs.ToList()).RunForResult(Rng<Random>.FromRandom(0));

            Assert.All(actual, a => { Assert.True(a >= min && a < max, $"{a} is not in the interval ({min},{max}]"); });
        }

        [Fact]
        public void RandomBoundedIntInRange()
        {
            var min = 52;
            var max = 148;

            var ints = Enumerable
                .Repeat((min, max), 50)
                .SelectAp<(int, int), int, Random<Random, IEnumerable<int>>>(bounds => Rnd.RandomInt<Random>(bounds.Item1, bounds.Item2)) as Random<Random, IEnumerable<int>>;

            var actual = (from _ in Rnd.Put(123897)
                          from xs in ints
                          select xs.ToList()).RunForResult(Rng<Random>.FromRandom(0));

            Assert.All(actual, a => { Assert.True(a >= min && a < max, $"{a} is not in the interval ({min},{max}]"); });
        }

        /// <summary>
        /// The main thing to see here is that the test doesn't fail due to overflow.
        /// </summary>
        [Fact]
        public void LongChainedComputationDoesNotOverflow()
        {
            var min = 1;
            var max = 10;
            var numRuns = 10_000_000;

            var watch = new Stopwatch();
            watch.Start();

            var ints = Rnd.RandomInts<Random>(min, max, numRuns);

            var phase1 = watch.ElapsedMilliseconds;
            outputHelper.WriteLine($"phase1: {phase1}");

            try
            {
                var actual = (from _ in Rnd.Put(123897)
                              from xs in ints
                              select xs.ToList()).RunForResult(Rng<Random>.FromRandom(0)).Sum();

                var phase2 = watch.ElapsedMilliseconds;
                watch.Stop();
                outputHelper.WriteLine($"phase2: {phase2}");

                Assert.True(actual >= min * numRuns, $"{actual} is smaller than {min * numRuns}");
                Assert.True(actual <= max * numRuns, $"{actual} is smaller than {max * numRuns}");

            } catch (Exception ex)
            {
                outputHelper.WriteLine($"{ex}");
            }


        }
    }
}

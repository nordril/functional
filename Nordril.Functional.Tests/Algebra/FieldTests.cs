using Nordril.Functional.Algebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Nordril.Functional.Tests.Algebra
{
    public static class FieldTests
    {
        [Fact]
        public static void DoubleFieldTest()
        {
            var f = Field.Double;

            Assert.Equal(0D, f.Zero());
            Assert.Equal(1D, f.One());
            Assert.Equal(4D, f.Plus(1D, 3D), 5);
            Assert.Equal(3D, f.Minus(7D, 4D));
            Assert.Equal(6D, f.Mult(2D, 3D), 5);
            Assert.Equal(-2D, f.Negate(2D), 5);
            Assert.Equal(1D / 5D, f.Reciprocal(5D), 5);
            Assert.Equal(3D / 5D, f.Divide(3D, 5D), 5);
        }

        [Fact]
        public static void DecimalFieldTest()
        {
            var f = Field.Decimal;

            Assert.Equal(0M, f.Zero());
            Assert.Equal(1M, f.One());
            Assert.Equal(4M, f.Plus(1M, 3M), 5);
            Assert.Equal(3M, f.Minus(7M, 4M));
            Assert.Equal(6M, f.Mult(2M, 3M), 5);
            Assert.Equal(-2M, f.Negate(2M), 5);
            Assert.Equal(1M / 5M, f.Reciprocal(5M), 5);
            Assert.Equal(3M / 5M, f.Divide(3M, 5M), 5);
        }

        [Theory]
        [InlineData(new double[] { })]
        [InlineData(new double[] { 1D })]
        [InlineData(new double[] { 3.5D })]
        [InlineData(new double[] { 5, 7.5, 3, -45, 62.38 })]
        [InlineData(new double[] { 5, 7.5, 3, -45, 62.38, 34.7, 843.387, 1643, 28.32 })]
        public static void AverageTest(IEnumerable<double> xs)
        {
            var iterativeAvg = xs.AverageIterative();
            var fieldAvg = xs.Average(Field.Double);

            if (xs.Any())
            {
                var linqAvg = xs.Average();
                Assert.Equal(linqAvg, fieldAvg, 5);
            }
            Assert.Equal(iterativeAvg, fieldAvg, 5);
        }

        [Theory]
        [InlineData(new double[] { })]
        [InlineData(new double[] { 1D })]
        [InlineData(new double[] { 3.5D })]
        [InlineData(new double[] { 5, 7.5, 3, -45, 62.38 })]
        [InlineData(new double[] { 5, 7.5, 3, -45, 62.38, 34.7, 843.387, 1643, 28.32 })]
        public static void WeightedAverageTest(IEnumerable<double> xs)
        {
            var rand = new System.Random();
            var weightedXs = xs.Select(x => (x, (double)rand.Next(0, 100))).ToList();

            static double weightedAverage(IEnumerable<(double elem, double weight)> ys)
            {
                var num = ys.Select(y => y.elem * y.weight).Sum();
                var denom = ys.Select(y => y.weight).Sum();

                if (denom != 0D)
                    return num / denom;
                else
                    return 0D;
            }

            var avg = weightedAverage(weightedXs);
            var fieldAvg = weightedXs.WeightedAverage(Field.Double);

            Assert.Equal(avg, fieldAvg, 5);
        }

        [Fact]
        public static void FieldProductTest()
        {
            var f = Field.Double.AsProduct<double, Field.DoubleField, Group.DoubleAddGroup, Group.DoubleMultGroup>();

            Assert.Equal(0D, f.Zero<double, Group.DoubleAddGroup>());
            Assert.Equal(1D, f.One<double, Group.DoubleMultGroup>());
            Assert.Equal(4D, f.Plus(1D, 3D), 5);
            Assert.Equal(3D, f.Minus(7D, 4D));
            Assert.Equal(6D, f.Mult(2D, 3D), 5);
            Assert.Equal(-2D, f.Negate(2D), 5);
            Assert.Equal(1D / 5D, f.Reciprocal(5D), 5);
            Assert.Equal(3D / 5D, f.Divide(3D, 5D), 5);
        }
    }
}

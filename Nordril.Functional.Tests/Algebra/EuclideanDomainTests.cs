using Nordril.Functional.Algebra;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Nordril.Functional.Tests.Algebra
{
    public class EuclideanDomainTests
    {
        [Fact]
        public static void GcdTest()
        {
            var d = EuclideanDomain.Integers;

            Assert.Equal(0, d.Gcd(15, 0));
            Assert.Equal(0, d.Gcd(0, 5));
            Assert.Equal(5, d.Gcd(15, 5));

            Assert.Equal(1, d.Gcd(1, 1));
            Assert.Equal(2, d.Gcd(2, 2));
            Assert.Equal(3, d.Gcd(3, 3));
            Assert.Equal(15, d.Gcd(15, 15));
            Assert.Equal(23, d.Gcd(23, 23));

            Assert.Equal(12, d.Gcd(3612, 468));
            Assert.Equal(12, d.Gcd(468, 3612));
            Assert.Equal(1, d.Gcd(3612, 467));
            Assert.Equal(1, d.Gcd(467, 3612));

            var f = Field.Double;

            Assert.Equal(0, f.Gcd(15, 0));
            Assert.Equal(0, f.Gcd(0, 5));
            Assert.Equal(5, f.Gcd(15, 5));

            Assert.Equal(1, f.Gcd(1, 1));
            Assert.Equal(2, f.Gcd(2, 2));
            Assert.Equal(3, f.Gcd(3, 3));
            Assert.Equal(15, f.Gcd(15, 15));
            Assert.Equal(23, f.Gcd(23, 23));

            Assert.Equal(12, f.Gcd(3612, 468));
            Assert.Equal(12, f.Gcd(468, 3612));
            Assert.Equal(1, f.Gcd(3612, 467));
            Assert.Equal(1, f.Gcd(467, 3612));
        }

        [Fact]
        public static void LcmTest()
        {
            var d = EuclideanDomain.Integers;

            Assert.Throws<DivideByZeroException>(() => d.Lcm(15, 0));
            Assert.Throws<DivideByZeroException>(() => d.Lcm(0, 5));
            Assert.Equal(15, d.Lcm(15, 5));

            Assert.Equal(1, d.Lcm(1, 1));
            Assert.Equal(2, d.Lcm(2, 2));
            Assert.Equal(3, d.Lcm(3, 3));
            Assert.Equal(15, d.Lcm(15, 15));
            Assert.Equal(23, d.Lcm(23, 23));

            Assert.Equal(140868, d.Lcm(3612, 468));
            Assert.Equal(140868, d.Lcm(468, 3612));
            Assert.Equal(1686804, d.Lcm(3612, 467));
            Assert.Equal(1686804, d.Lcm(467, 3612));

            var f = Field.Double;

            Assert.Throws<DivideByZeroException>(() => f.Lcm(15, 0));
            Assert.Throws<DivideByZeroException>(() => f.Lcm(0, 5));
            Assert.Equal(15, f.Lcm(15, 5));

            Assert.Equal(1, f.Lcm(1, 1));
            Assert.Equal(2, f.Lcm(2, 2));
            Assert.Equal(3, f.Lcm(3, 3));
            Assert.Equal(15, f.Lcm(15, 15));
            Assert.Equal(23, f.Lcm(23, 23));

            Assert.Equal(140868, f.Lcm(3612, 468));
            Assert.Equal(140868, f.Lcm(468, 3612));
            Assert.Equal(1686804, f.Lcm(3612, 467));
            Assert.Equal(1686804, f.Lcm(467, 3612));
        }

        [Fact]
        public static void IsZeroTest()
        {
            var d = EuclideanDomain.Integers;

            Assert.True(d.IsZero(0));
            Assert.False(d.IsZero(1));

            var f = Field.Double;

            Assert.True(f.IsZero(0D));
            Assert.False(f.IsZero(0.5D));
            Assert.False(f.IsZero(1D));
        }

        [Fact]
        public static void ModTest()
        {
            var d = EuclideanDomain.Integers;

            Assert.Throws<DivideByZeroException>(() => d.EuclideanDivide(0, 0));
            Assert.Equal((0, 0), d.EuclideanDivide(0,4));
            Assert.Equal((0, 1), d.EuclideanDivide(1,4));
            Assert.Equal((1, 0), d.EuclideanDivide(4,4));
            Assert.Equal((2, 0), d.EuclideanDivide(8, 4));
            Assert.Equal((2, 2), d.EuclideanDivide(10, 4));
            Assert.Equal((-2, -2), d.EuclideanDivide(-10, 4));

            Assert.Equal(0, d.Mod(0, 4));
            Assert.Equal(1, d.Mod(1, 4));
            Assert.Equal(0, d.Mod(4, 4));
            Assert.Equal(0, d.Mod(8, 4));
            Assert.Equal(2, d.Mod(10, 4));
            Assert.Equal(-2, d.Mod(-10, 4));
        }
    }
}

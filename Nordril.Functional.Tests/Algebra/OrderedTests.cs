using Nordril.Functional.Algebra;
using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Nordril.Functional.Tests.Algebra
{
    using G = IPartiallyOrderedGrouplike<int, Group.IntAddGroup, IPartialOrder<int>>;
    public static class OrderedTests
    {
        [Fact]
        public static void PartiallyOrderedTest()
        {
            var order = Group.IntAdd.WithPartialOrder<int, Group.IntAddGroup, IPartialOrder<int>>(PartialOrder.FromEquatable<int>());

            Assert.Equal(0, order.Zero<int, Group.IntAddGroup>());
            Assert.Equal(7, order.Plus(3, 4));
            Assert.Equal(1, order.Minus(3, 2));
            Assert.Equal(-3, order.Negate(3));
            Assert.True(order.Leq(3, 3));
            Assert.False(order.Leq(5, 3));

            var order2 = EuclideanDomain.Integers.WithPartialOrder<int, EuclideanDomain.IntegerEuclideanDomain, Group.IntAddGroup, Monoid.IntMultMonoid, IPartialOrder<int>>(PartialOrder.Make<int>((x, y) => Maybe.Just(x <= y)));

            Assert.Equal(0, order2.Zero<int, Group.IntAddGroup>());
            Assert.Equal(1, order2.One<int, Monoid.IntMultMonoid>());
            Assert.Equal(7, order2.Plus(3, 4));
            Assert.Equal(1, order2.Minus(3, 2));
            Assert.Equal(12, order2.Mult(3, 4));
            Assert.Equal(-3, order2.Negate(3));
            Assert.Equal((2,1), order2.EuclideanDivide<int, EuclideanDomain.IntegerEuclideanDomain, Group.IntAddGroup, Monoid.IntMultMonoid>(5,2));
            Assert.Equal(1, order2.Mod<int, EuclideanDomain.IntegerEuclideanDomain, Group.IntAddGroup, Monoid.IntMultMonoid>(5,2));
            Assert.Equal(5, order2.Gcd<int, EuclideanDomain.IntegerEuclideanDomain, Group.IntAddGroup, Monoid.IntMultMonoid>(5,15));
            Assert.Equal(15, order2.Lcm<int, EuclideanDomain.IntegerEuclideanDomain, Group.IntAddGroup, Monoid.IntMultMonoid>(5,15));
            Assert.True(order2.Leq(3, 3));
            Assert.False(order2.Leq(5, 3));

            var order3 = Field.Double.WithPartialOrder<double, Field.DoubleField, Group.DoubleAddGroup, Group.DoubleMultGroup, IPartialOrder<double>>(PartialOrder.Make<double>((x, y) => Maybe.Just(x <= y)));

            Assert.Equal(0D, order3.Zero<double, Group.DoubleAddGroup>());
            Assert.Equal(1D, order3.One<double, Group.DoubleMultGroup>());
            Assert.Equal(7D, order3.Plus(3D, 4D));
            Assert.Equal(1.5D, order3.Minus(5D, 3.5D));
            Assert.Equal(12D, order3.Mult(3D, 4D));
            Assert.Equal(2D, order3.Divide(6D, 3D));
            Assert.Equal(-3D, order3.Negate(3D));
            Assert.Equal(1/3D, order3.Reciprocal(3D));
            Assert.Equal(2D, new double[] { 1, 2, 3 }.Average<double, Field.DoubleField, Group.DoubleAddGroup, Group.DoubleMultGroup>(order3));
            Assert.Equal(2D, new (double, double)[] { (1,1), (2,1), (3,1) }.WeightedAverage<double, Field.DoubleField, Group.DoubleAddGroup, Group.DoubleMultGroup>(order3));
            Assert.True(order2.Leq(3, 3));
            Assert.False(order2.Leq(5, 3));
        }

        [Fact]
        public static void TotallyOrderedTest()
        {
            var order = Group.IntAdd.WithTotalOrder<int, Group.IntAddGroup, ITotalOrder<int>>(TotalOrder.FromComparable<int>());

            Assert.Equal(0, order.Zero<int, Group.IntAddGroup>());
            Assert.Equal(7, order.Plus(3, 4));
            Assert.Equal(1, order.Minus(3, 2));
            Assert.Equal(-3, order.Negate(3));
            Assert.True(order.Leq(3, 3));
            Assert.False(order.Leq(5, 3));
            Assert.False(order.Le(5, 3));

            var order2 = EuclideanDomain.Integers.WithTotalOrder<int, EuclideanDomain.IntegerEuclideanDomain, Group.IntAddGroup, Monoid.IntMultMonoid, ITotalOrder<int>>(TotalOrder.Make<int>((x, y) => (short)x.CompareTo(y)));

            Assert.Equal(0, order2.Zero<int, Group.IntAddGroup>());
            Assert.Equal(1, order2.One<int, Monoid.IntMultMonoid>());
            Assert.Equal(7, order2.Plus(3, 4));
            Assert.Equal(1, order2.Minus(3, 2));
            Assert.Equal(12, order2.Mult(3, 4));
            Assert.Equal(-3, order2.Negate(3));
            Assert.True(order2.Leq(3, 3));
            Assert.False(order2.Leq(5, 3));

            var order3 = Field.Double.WithTotalOrder<double, Field.DoubleField, Group.DoubleAddGroup, Group.DoubleMultGroup, ITotalOrder<double>>(TotalOrder.Make<double>((x, y) => (short)x.CompareTo(y)));

            Assert.Equal(0D, order3.Zero<double, Group.DoubleAddGroup>());
            Assert.Equal(1D, order3.One<double, Group.DoubleMultGroup>());
            Assert.Equal(7D, order3.Plus(3D, 4D));
            Assert.Equal(1.5D, order3.Minus(5D, 3.5D));
            Assert.Equal(12D, order3.Mult(3D, 4D));
            Assert.Equal(2D, order3.Divide(6D, 3D));
            Assert.Equal(-3D, order3.Negate(3D));
            Assert.Equal(1 / 3D, order3.Reciprocal(3D));

            Assert.True(order2.Le(2, 3));
            Assert.False(order2.Le(3, 3));
            Assert.False(order2.Le(4, 3));

            Assert.True(order2.Leq(2, 3));
            Assert.True(order2.Leq(3, 3));
            Assert.False(order2.Leq(4, 3));

            Assert.False(order2.Eq(2, 3));
            Assert.True(order2.Eq(3, 3));
            Assert.False(order2.Eq(4, 3));

            Assert.True(order2.Neq(2, 3));
            Assert.False(order2.Neq(3, 3));
            Assert.True(order2.Neq(4, 3));

            Assert.False(order2.Geq(2, 3));
            Assert.True(order2.Geq(3, 3));
            Assert.True(order2.Geq(4, 3));

            Assert.False(order2.Ge(2, 3));
            Assert.False(order2.Ge(3, 3));
            Assert.True(order2.Ge(4, 3));
        }
    }
}

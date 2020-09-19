using Nordril.Functional.Algebra;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Xunit;

namespace Nordril.Functional.Tests.Algebra
{
    public class GroupTests
    {
        [Fact]
        public static void FromGroupInstanceTest()
        {
            var g = Group.FromGroupInstance<IntWithAddGroup>();

            Assert.Equal(new IntWithAddGroup(5), g.Op(g.Neutral, new IntWithAddGroup(5)));
            Assert.Equal(new IntWithAddGroup(5), g.Op(new IntWithAddGroup(5), g.Neutral));
            Assert.Equal(new IntWithAddGroup(8), g.Op(new IntWithAddGroup(3), new IntWithAddGroup(5)));
            Assert.Equal(new IntWithAddGroup(-7), g.Inverse(new IntWithAddGroup(7)));
        }

        private class IntWithAddGroup : IHasGroup<IntWithAddGroup>, IEquatable<IntWithAddGroup>
        {
            public int Value { get; }

            public IntWithAddGroup Neutral => new IntWithAddGroup(0);

            public IntWithAddGroup(int value)
            {
                Value = value;
            }

            public IntWithAddGroup Op(IntWithAddGroup x, IntWithAddGroup y)
            {
                return new IntWithAddGroup(x.Value + y.Value);
            }

            public IntWithAddGroup Inverse(IntWithAddGroup x)
                => new IntWithAddGroup(x.Value * -1);

            public bool Equals([AllowNull] IntWithAddGroup other)
            {
                return Value == other.Value;
            }
        }

        [Fact]
        public static void IntGroupTest()
        {
            var g = new Group<int>(0, (x, y) => x + y, x => x * -1);

            Assert.Equal(5, g.Op(g.Neutral, 5));
            Assert.Equal(5, g.Op(5, g.Neutral));
            Assert.Equal(8, g.Op(3, 5));
            Assert.Equal(-7, g.Inverse(7));
        }

        [Fact]
        public static void IntAddGroupTest()
        {
            var g = Group.IntAdd;

            Assert.Equal(5, g.Op(g.Neutral, 5));
            Assert.Equal(5, g.Op(5, g.Neutral));
            Assert.Equal(8, g.Op(3, 5));
            Assert.Equal(-7, g.Inverse(7));
        }

        [Fact]
        public static void LongAddGroupTest()
        {
            var g = Group.LongAdd;

            Assert.Equal(5L, g.Op(g.Neutral, 5L));
            Assert.Equal(5L, g.Op(5, g.Neutral));
            Assert.Equal(8L, g.Op(3L, 5L));
            Assert.Equal(-7L, g.Inverse(7L));
        }

        [Fact]
        public static void FloatAddGroupTest()
        {
            var g = Group.FloatAdd;

            Assert.Equal(5.1F, g.Op(g.Neutral, 5.1F));
            Assert.Equal(5.1F, g.Op(5.1F, g.Neutral));
            Assert.Equal(8.6F, g.Op(3F, 5.6F));
            Assert.Equal(-7.3F, g.Inverse(7.3F));
        }

        [Fact]
        public static void DoubleAddGroupTest()
        {
            var g = Group.DoubleAdd;

            Assert.Equal(5.1D, g.Op(g.Neutral, 5.1D));
            Assert.Equal(5.1D, g.Op(5.1D, g.Neutral));
            Assert.Equal(8.6D, g.Op(3D, 5.6D));
            Assert.Equal(-7.3D, g.Inverse(7.3D));
        }

        [Fact]
        public static void DecimalAddGroupTest()
        {
            var g = Group.DecimalAdd;

            Assert.Equal(5.1M, g.Op(g.Neutral, 5.1M));
            Assert.Equal(5.1M, g.Op(5.1M, g.Neutral));
            Assert.Equal(8.6M, g.Op(3M, 5.6M));
            Assert.Equal(-7.3M, g.Inverse(7.3M));
        }

        [Fact]
        public static void FloatMultGroupTest()
        {
            var g = Group.FloatMult;

            Assert.Equal(5.1F, g.Op(g.Neutral, 5.1F));
            Assert.Equal(5.1F, g.Op(5.1F, g.Neutral));
            Assert.Equal(3F*5.6F, g.Op(3F, 5.6F));
            Assert.Equal(1F/7.3F, g.Inverse(7.3F));
        }

        [Fact]
        public static void DoubleMultGroupTest()
        {
            var g = Group.DoubleMult;

            Assert.Equal(5.1D, g.Op(g.Neutral, 5.1D), 5);
            Assert.Equal(5.1D, g.Op(5.1F, g.Neutral), 5);
            Assert.Equal(3D * 5.6D, g.Op(3D, 5.6D), 5);
            Assert.Equal(1D / 7.3D, g.Inverse(7.3D), 5);
        }

        [Fact]
        public static void DecimalMultGroupTest()
        {
            var g = Group.DecimalMult;

            Assert.Equal(5.1M, g.Op(g.Neutral, 5.1M));
            Assert.Equal(5.1M, g.Op(5.1M, g.Neutral));
            Assert.Equal(3M * 5.6M, g.Op(3M, 5.6M));
            Assert.Equal(1M / 7.3M, g.Inverse(7.3M));
        }

        [Fact]
        public static void BoolXorGroupTest()
        {
            var g = Group.BoolXor;

            Assert.False(g.Op(false, g.Neutral));
            Assert.False(g.Op(g.Neutral, false));
            Assert.True(g.Op(true, g.Neutral));
            Assert.True(g.Op(g.Neutral, true));

            Assert.False(g.Op(false, false));
            Assert.False(g.Op(true, true));
            Assert.True(g.Op(true, false));
            Assert.True(g.Op(false, true));

            Assert.Equal(g.Neutral, g.Op(false, g.Inverse(false)));
            Assert.Equal(g.Neutral, g.Op(true, g.Inverse(true)));
        }

        [Fact]
        public static void GroupNamedOperationsTest()
        {
            var g = Group.IntAdd.AsProduct<int, Group.IntAddGroup>();

            Assert.Equal(0, g.Zero<int, Group.IntAddGroup>());
            Assert.Equal(9, g.Plus(4, 5));

            var g2 = new Group<int>(0, (x, y) => x + y, x => -x).AsProduct<int, Group<int>>();

            Assert.Equal(0, g2.Zero<int, Group<int>>());
            Assert.Equal(9, g2.Plus(4, 5));
        }
    }
}

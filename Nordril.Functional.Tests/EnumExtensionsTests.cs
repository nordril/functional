using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Xunit;

namespace Nordril.Functional.Tests
{
    public static class EnumExtensionsTests
    {
        [Fact]
        public static void ToGetAllColors()
        {
            var range = KnownColor.ActiveBorder.To(KnownColor.MenuHighlight).ToList();

            Assert.Equal(174, range.Count);

            for (int i = 1; i <= 174; i++)
                Assert.Equal(i, (int)range[i-1]);
        }

        [Fact]
        public static void ToGetSomeColors()
        {
            var range = ((KnownColor)40).To((KnownColor)67).ToList();

            Assert.Equal(28, range.Count);

            for (int i = 40; i <= 67; i++)
                Assert.Equal(i, (int)range[i-40]);
        }

        [Fact]
        public static void ToGetSomeColorsOutsideOfRange()
        {
            var range = ((KnownColor)160).To((KnownColor)190).ToList();

            Assert.Equal(15, range.Count);

            for (int i = 160; i <= 174; i++)
                Assert.Equal(i, (int)range[i-160]);
        }

        [Fact]
        public static void ToGetNoColorsWithNegativeRange()
        {
            var range = ((KnownColor)160).To((KnownColor)150).ToList();

            Assert.Empty(range);
        }
    }
}

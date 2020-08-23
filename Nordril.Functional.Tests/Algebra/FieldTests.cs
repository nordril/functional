using Nordril.Functional.Algebra;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Nordril.Functional.Tests.Algebra
{
    public static class FieldTests
    {
        [Fact]
        public static void FieldTest()
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
    }
}

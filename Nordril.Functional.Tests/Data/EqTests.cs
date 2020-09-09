using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Nordril.Functional.Tests.Data
{
    public static class EqTests
    {
        [Fact]
        public static void EqualsTests()
        {
            Func<Type, Type, bool> eq = (x, y) => x.Equals(y);
            Func<Type, Eq<Type>> mk = t => Eq.Make(t, eq);

            Assert.Equal(mk(typeof(string)), mk(typeof(string)));
            Assert.Equal(mk(null), mk(null));
            Assert.NotEqual(mk(typeof(int)), mk(typeof(string)));
            Assert.NotEqual(mk(null), mk(typeof(string)));
            Assert.NotEqual(mk(typeof(int)), mk(null));
        }
    }
}

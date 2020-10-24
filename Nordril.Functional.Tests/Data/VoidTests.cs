using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Nordril.Functional.Tests.Data
{
    public sealed class VoidTests
    {
        [Fact]
        public void CallVoid()
        {
            static Functional.Data.Void f(int _) => throw new NotImplementedException();

            Assert.Throws<NotImplementedException>(() => f(5));
        }
    }
}

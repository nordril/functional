using Nordril.Functional.Category;
using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Nordril.Functional.Tests.Data
{
    public class StateTests
    {
        [Fact]
        public void LinqQueryTest()
        {
            var res = from x in State.Get<int>()
                      from _ in State.Modify<int>(state => state + 5)
                      from y in State.Get<int>()
                      from __ in State.Put(x-1)
                      select x + y;

            var (a, s) = res.Run(3);

            Assert.Equal(2, s);
            Assert.Equal(11, a);
        }
    }
}

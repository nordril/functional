using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Nordril.Functional.Tests.Data
{
    public sealed class ConstTests
    {
        [Fact]
        public async Task ConstLinqTest()
        {
            //We just check whether the query executes.
            var res =
                from x in new Const<string>()
                select x[0];

            var res2 =
                from x in new Const<float, string>(3.4f)
                select x + "a";

            Assert.Equal(3.4f, res2.RealValue);

            var res3 = await
                from x in Task.FromResult(new Const<string>())
                select x[0];

            var res4 = await
                from x in Task.FromResult(new Const<float, string>(3.5f))
                select x + "a";

            Assert.Equal(3.5f, res4.RealValue);
        }
    }
}

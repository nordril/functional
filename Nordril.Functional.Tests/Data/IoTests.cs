using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Nordril.Functional.Tests.Data
{
    public class IoTests
    {
        [Fact]
        public async Task BindEffectsTest()
        {
            var output = new List<string>();

            Func<string, int, Io<int>> f1 = (msg, i) => new Io<int>(() => { output.Add($"out: {msg}, {i}"); return i + 1; });

            var computation =
                f1("first", 5)
                .Bind(i => f1("second", i + 3))
                .Bind(i => f1("third", i + 4))
                .ToIo();

            Assert.Empty(output);

            var actual = computation.Run();

            Assert.Equal(new List<string> {
                "out: first, 5",
                "out: second, 9",
                "out: third, 14"
            }, output);
            Assert.Equal(15, await actual);
        }
    }
}

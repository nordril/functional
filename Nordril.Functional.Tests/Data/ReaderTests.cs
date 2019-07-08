using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Nordril.Functional.Tests.Data
{
    public class ReaderTests
    {
        [Fact]
        public static void LinqSelectQueryTest()
        {
            var res =
                from c1 in Reader.Get<int>()
                select c1 * 2;

            var actual = res.Run(7);

            Assert.Equal(14, actual);
        }

        [Fact]
        public static void LinqSelectManyQueryTest()
        {
            var res =
                from c1 in Reader.Get<int>()
                from c2 in Reader.With<int, string>(i => i+"")
                select (c1 * 2) + c2;

            var actual = res.Run(7);

            Assert.Equal("147", actual);
        }

        [Fact]
        public static void BindTest()
        {
            var actual = Reader.Get<int>()
                .Bind(i => Reader.With<int, string>(j => (i + 1) + "" + j))
                .ToReader<int, string>()
                .Run(6);

            Assert.Equal("76", actual);
        }
    }
}

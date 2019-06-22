using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Nordril.Functional.Tests.Data
{
    public class MemoTests
    {
        [Fact]
        public void MemoLinqQueryTest()
        {
            Memo<Dictionary<int, int>, int, int> fib(int x)
            {
                if (x == 0)
                    return Memo.Pure<Dictionary<int, int>, int, int>(0);
                else if (x == 1)
                    return Memo.Pure<Dictionary<int, int>, int, int>(1);
                else
                    return from n1 in Memo.Memoized(x - 1, fib)
                           from n2 in Memo.Memoized(x - 2, fib)
                           select n1 + n2;
            }

            var expected = 1836311903;
            var actual = fib(46).Run(new Dictionary<int, int>()).result;

            Assert.Equal(expected, actual);
        }
    }
}

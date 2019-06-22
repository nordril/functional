using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
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
            var actual = fib(46).RunForResult(new Dictionary<int, int>());

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void RandomLinqQueryTest()
        {

            var randSum =
                from _ in Rnd.Put(1234567)
                from a in Rnd.RandomInt<Random>(0, 10000)
                from b in Rnd.RandomInt<Random>(0, 10000)
                from c in Rnd.RandomInt<Random>(0, 10000)
                select a + b + c;

            var expected = 7791 + 7597 + 2744;

            var actual = randSum.RunForResult(Rng<Random>.FromRandom(0));

            var actual2 = randSum.RunForResult(Rng<Random>.FromRandom(7856));

            Assert.Equal(expected, actual);
            Assert.Equal(expected, actual2);
        }
    }
}

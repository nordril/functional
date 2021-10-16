using Nordril.Functional.Category;
using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Nordril.Functional.Tests.Category
{
    public sealed class MonadTests
    {
        [Fact]
        public static void AggregateMTest()
        {
            var xs1 = new List<int>();
            var res1 = xs1.AggregateM(5, (x, acc) => Maybe.Nothing<int>());

            Assert.True(res1.HasValue);
            Assert.Equal(5, res1.Value());

            var xs2 = new List<int> { 4 };
            var res2 = xs2.AggregateM(5, (x, acc) => Maybe.Nothing<int>());

            Assert.False(res2.HasValue);

            var xs3 = new List<int> { 4 };
            var res3 = xs3.AggregateM(5, (x, acc) => Maybe.Just(x + acc));

            Assert.True(res3.HasValue);
            Assert.Equal(9, res3.Value());

            var xs4 = new List<int> { 4, 2, 7 };
            var res4 = xs4.AggregateM(5, (x, acc) => Maybe.Just(x + acc));

            Assert.True(res4.HasValue);
            Assert.Equal(5 + 4 + 2 + 7, res4.Value());

            var xs5 = new List<int> { 4, 2, 99, 7 };
            var res5 = xs5.AggregateM(5, (x, acc) => Maybe.JustIf(x != 99, () => x + acc));

            Assert.False(res5.HasValue);
        }

        [Fact]
        public static void UnfoldMTest()
        {
            var seed =
                from counter in State.Get<Maybe<int>>()
                from _ in State.Put(from c in counter from _ in Maybe.JustIf(c > 0, () => new Unit()) select c - 1)
                select counter;

            var computation = seed.UnfoldM().ToState<IEnumerable<int>, Maybe<int>>();

            var (result1, finalState1) = computation.Run(Maybe.Nothing<int>());
            Assert.Equal(Array.Empty<int>(), result1);
            Assert.True(finalState1.IsNothing);

            var (result2, finalState2) = computation.Run(Maybe.Just(1));
            Assert.Equal(new int[] { 1, 0 }, result2);
            Assert.True(finalState2.IsNothing);

            var (result3, finalState3) = computation.Run(Maybe.Just(5));
            Assert.Equal(new int[] { 5, 4, 3, 2, 1, 0 }, result3);
            Assert.True(finalState3.IsNothing);

            var (result4, finalState4) = computation.Run(Maybe.Just(100));
            var expected = Enumerable.Range(0, 101).Reverse().ToList();
            Assert.Equal(expected, result4);
            Assert.True(finalState4.IsNothing);
        }

        [Fact]
        public static void WhileMTest()
        {
            var seed =
                from counter in State.Get<Maybe<int>>()
                from _ in State.Put(from c in counter from _ in Maybe.JustIf(c > 0, () => new Unit()) select c - 1)
                select counter.HasValue;

            var body =
                from counter in State.Get<Maybe<int>>()
                select counter.ValueOr(i => (i+1)*10, 0) + "_x";

            var computation = seed.WhileM(body).ToState<IEnumerable<string>, Maybe<int>>();

            var (result1, finalState1) = computation.Run(Maybe.Nothing<int>());
            Assert.Equal(Array.Empty<string>(), result1);
            Assert.True(finalState1.IsNothing);

            var (result2, finalState2) = computation.Run(Maybe.Just(1));
            Assert.Equal(new int[] { 10, 0 }.Select(i => i+"_x"), result2);
            Assert.True(finalState2.IsNothing);

            var (result3, finalState3) = computation.Run(Maybe.Just(5));
            Assert.Equal(new int[] { 50, 40, 30, 20, 10, 0 }.Select(i => i + "_x"), result3);
            Assert.True(finalState3.IsNothing);

            var (result4, finalState4) = computation.Run(Maybe.Just(100));
            var expected = Enumerable.Range(0, 101).Reverse().ToList().Select(i => (i*10) + "_x");
            Assert.Equal(expected, result4);
            Assert.True(finalState4.IsNothing);
        }
    }
}

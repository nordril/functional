using Nordril.Functional.Algebra;
using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Nordril.Functional.Tests.Data
{
    public static class MaybeT_TransformerTests
    {
        [Fact]
        public static void LinqFuncConstTest()
        {
            //We just check whether it runs through, as Const has no value.
            var init = 5;
            var computation =
                from x in MaybeT.Lift(new Const<string>())
                from y in MaybeT.Lift(new Const<bool>())
                from _ in MaybeT.HoistConst(Maybe.JustIf(init > 0, () => new Unit()))
                from z in MaybeT.Lift(new Const<float>())
                select z;

            var value = computation.Run;
        }

        [Fact]
        public static void LinqFuncEitherTest()
        {
            var cxt = new EitherCxt<string>();

            var init = 5;
            var computation =
                from x in MaybeT.Lift(cxt.FromRight(45))
                from y in MaybeT.Lift(cxt.FromRight(90))
                from _ in MaybeT.HoistEither(cxt, Maybe.JustIf(init > 0, () => new Unit()))
                from z in MaybeT.Lift(cxt.FromRight(100))
                select x + y + z;

            var value = computation.Run;

            Assert.True(value.IsRight);
            Assert.True(value.Right().HasValue);
            Assert.Equal(45+90+100, value.Right().Value());

            init = 0;
            computation =
                from x in MaybeT.Lift(cxt.FromRight(45))
                from y in MaybeT.Lift(cxt.FromRight(90))
                from _ in MaybeT.HoistEither(cxt, Maybe.JustIf(init > 0, () => new Unit()))
                from z in MaybeT.Lift(cxt.FromRight(100))
                select x + y + z;

            value = computation.Run;

            Assert.False(value.IsLeft);
        }

        [Fact]
        public static void LinqFuncListTest()
        {
            var init = 5;
            var computation =
                from x in MaybeT.Lift(FuncList.Make("a", "b", "c"))
                from y in MaybeT.Lift(FuncList.Make("c", "d", "e"))
                from _ in MaybeT.HoistFuncList(Maybe.JustIf(init > 0, () => new Unit()))
                from z in MaybeT.Lift(FuncList.Make("f"))
                select x + y + z;

            var value = computation.Run;

            var expected = new[] { "acf", "adf", "aef", "bcf", "bdf", "bef", "ccf", "cdf", "cef" }.Select(x => Maybe.Just(x)).MakeFuncList();
            Assert.Equal(expected, value);

            init = 0;
            computation =
                from x in MaybeT.Lift(FuncList.Make("a", "b", "c"))
                from y in MaybeT.Lift(FuncList.Make("c", "d", "e"))
                from _ in MaybeT.HoistFuncList(Maybe.JustIf(init > 0, () => new Unit()))
                from z in MaybeT.Lift(FuncList.Make("f"))
                select x + y + z;

            value = computation.Run;

            expected = Enumerable.Repeat(Maybe.Nothing<string>(), 9).MakeFuncList();
            Assert.Equal(expected, value);
        }

        [Fact]
        public static void LinqFuncSetTest()
        {
            var init = 5;
            var computation =
                from x in MaybeT.Lift(FuncSet.Make("a","b","c"))
                from y in MaybeT.Lift(FuncSet.Make("c","d","e"))
                from _ in MaybeT.HoistFuncSet(Maybe.JustIf(init > 0, () => new Unit()))
                from z in MaybeT.Lift(FuncSet.Make("f"))
                select x + y + z;

            var value = computation.Run;

            var expected = new[] { "acf", "adf", "aef", "bcf", "bdf", "bef", "ccf", "cdf", "cef" }.Select(x => Maybe.Just(x)).MakeFuncSet();
            Assert.Equal(expected, value);

            init = 0;
            computation =
                from x in MaybeT.Lift(FuncSet.Make("a", "b", "c"))
                from y in MaybeT.Lift(FuncSet.Make("c", "d", "e"))
                from _ in MaybeT.HoistFuncSet(Maybe.JustIf(init > 0, () => new Unit()))
                from z in MaybeT.Lift(FuncSet.Make("f"))
                select x + y + z;

            value = computation.Run;

            expected = FuncSet.Make(Maybe.Nothing<string>());
            Assert.Equal(expected, value);
        }

        [Fact]
        public static void LinqIdentityTest()
        {
            var init = 5;
            var computation =
                from x in MaybeT.Lift(Identity.Make("abc"))
                from y in MaybeT.Lift(Identity.Make("def"))
                from _ in MaybeT.HoistIdentity(Maybe.JustIf(init > 0, () => new Unit()))
                from z in MaybeT.Lift(Identity.Make("ghi"))
                select x + y + z;

            var value = computation.Run.Value;

            Assert.True(value.HasValue);
            Assert.Equal("abcdefghi", value.Value());

            init = 0;
            computation =
                from x in MaybeT.Lift(Identity.Make("abc"))
                from y in MaybeT.Lift(Identity.Make("def"))
                from _ in MaybeT.HoistIdentity(Maybe.JustIf(init > 0, () => new Unit()))
                from z in MaybeT.Lift(Identity.Make("ghi"))
                select x + y + z;

            value = computation.Run.Value;

            Assert.False(value.HasValue);
        }

        [Fact]
        public static async Task LinqIoTest()
        {
            var init = 5;
            var console = new List<string>();
            var computation =
                from x in MaybeT.Lift(new Io<int>(() => Task.FromResult(1)))
                from y in MaybeT.Lift(Io.AsIo(() => Task.FromResult(2)))
                from _ in MaybeT.HoistIo(Maybe.JustIf(init > 0, () => new Unit()))
                from z in MaybeT.Lift(Io.AsIo(() => { console.Add("func run"); return Task.FromResult(init); }))
                select x+y+z;

            var value = await computation.Run.Run();

            Assert.True(value.HasValue);
            Assert.Equal(1+2+init, value.Value());
            Assert.Single(console);

            init = 0;
            console = new List<string>();

            value = await computation.Run.Run();

            Assert.False(value.HasValue);
            Assert.Empty(console);
        }

        [Fact]
        public static async Task LinqRandomTest()
        {
            var cxt = new RandomCxt<Random>();

            var init = 5;
            var computation =
                from x in MaybeT.Lift(cxt.RandomInt())
                from y in MaybeT.Lift(cxt.RandomInt(20, 30))
                from z in MaybeT.Lift(cxt.RandomInts(30, 100, 40))
                from u in MaybeT.Lift(cxt.Random32Bits())
                from v in MaybeT.Lift(cxt.Random64Bits())
                let bs = new byte[16]
                from w in MaybeT.Lift(cxt.RandomBytes(bs))
                from t in MaybeT.Lift(cxt.RandomDouble())
                from s in MaybeT.Lift(cxt.RandomDouble(4.5, 8.9))
                from r in MaybeT.Lift(cxt.RandomDoubles(4.5, 8.9, 20))
                from q in MaybeT.Lift(cxt.RandomGaussianDouble(50, 2))
                from p in MaybeT.Lift(cxt.RandomGaussianDoubles(50, 2, 1000))
                from _ in MaybeT.HoistRandom(cxt, Maybe.JustIf(init >= 3, () => new Unit()))
                from o in MaybeT.Lift(cxt.RandomInt())
                select new { x, y, z, u, v, w, t, s, r, q, p, o };

            var value = await computation.Run.RunRandom().Run();

            Assert.True(value.HasValue);

            var res = value.Value();
            Assert.True(res.y >= 20 && res.y < 30);
            Assert.Equal(40, res.z.Count);
            Assert.All(res.z, z => { Assert.True(z >= 30 && z < 100); });
            Assert.True(res.s >= 4.5 && res.s < 8.9);
            Assert.Equal(20, res.r.Count);
            Assert.All(res.r, r => { Assert.True(r >= 4.5 && r < 8.9); });

            //This was tested by hand, but only visually, not via a frequentist test.
            var gaussian = string.Join(";", res.p);

            init = 0;
            value = await computation.Run.RunRandom().Run();

            Assert.False(value.HasValue);
        }

        [Fact]
        public static void LinqReaderTest()
        {
            var cxt = new ReaderCxt<Dictionary<int, int>>();

            var computation =
                from x in MaybeT.Lift(cxt.Get())
                from y in MaybeT.Lift(cxt.With(env => env[4]))
                from z in MaybeT.Lift(cxt.Local(dict => {
                    var d2 = new Dictionary<int, int>(dict)
                    {
                        [99] = 150
                    }; return d2; }, from u in cxt.Get() select u[99]))
                from _ in MaybeT.HoistReader(cxt, Maybe.JustIf(x.ContainsKey(120), () => new Unit()))
                select x[1] + y + z;

            var value = computation.Run.Run(new Dictionary<int, int> { { 1, 2 }, { 4, 8 }, { 16, 32 }, { 120, 0 } });

            Assert.True(value.HasValue);
            Assert.Equal(2 + 8 + 150, value.Value());

            value = computation.Run.Run(new Dictionary<int, int> { { 1, 2 }, { 4, 8 }, { 16, 32 } });

            Assert.False(value.HasValue);
        }

        [Fact]
        public static void LinqRwsTest()
        {
            var cxt = new RwsCxt<Dictionary<int, int>, IList<string>, Monoid.ListAppendImmutableMonoid<string>, int>();

            var computation =
                from x in MaybeT.Lift(cxt.Get())
                from _1 in MaybeT.Lift(cxt.Tell(FuncList.Make("function call 1")))
                from _2 in MaybeT.Lift(cxt.Put(x + 3))
                from env in MaybeT.Lift(cxt.GetEnvironment())
                from _3 in MaybeT.Lift(cxt.Tell(FuncList.Make("function call 2")))
                from twice in MaybeT.HoistRws(cxt, Maybe.JustIf(env.ContainsKey(x), () => env[x]))
                from _4 in MaybeT.Lift(cxt.Tell(FuncList.Make("function call 3")))
                from _5 in MaybeT.Lift(cxt.Put(x + 10))
                from y in MaybeT.Lift(cxt.Get())
                from _6 in MaybeT.Lift(cxt.Put(y + x))
                from goodKey in MaybeT.HoistRws(cxt, Maybe.Just(1))
                select x + twice;

            var (value, state, output) = computation.Run.Run(new Dictionary<int, int> { { 1, 2 }, { 4, 8 }, { 16, 32 } }, 16);

            Assert.True(value.HasValue);
            Assert.Equal(16+32, value.Value());
            Assert.Equal(16+26, state);
            Assert.Equal(new[] { "function call 1", "function call 2", "function call 3" }, (IEnumerable<string>)output);

            (value, state, output) = computation.Run.Run(new Dictionary<int, int> { { 1, 2 }, { 4, 8 }, { 16, 32 } }, 17);

            Assert.False(value.HasValue);
            Assert.Equal(20, state);
            Assert.Equal(new[] { "function call 1", "function call 2" }, (IEnumerable<string>)output);
        }

        [Fact]
        public static void LinqStateTest()
        {
            var cxt = new StateCxt<string>();

            var computation =
                from x in MaybeT.Lift(cxt.Get())
                from y in MaybeT.Lift(cxt.Put("xyz"))
                from z in MaybeT.Lift(cxt.Modify(s => x + "___"))
                from u in MaybeT.Lift(cxt.Get())
                from _ in MaybeT.HoistState(cxt, Maybe.JustIf(x.Length >= 3, () => 1))
                from v in MaybeT.Lift(cxt.Put("gef"))
                select u;

            var (value, state) = computation.Run.Run("abc");

            Assert.True(value.HasValue);
            Assert.Equal("abc___", value.Value());
            Assert.Equal("gef", state);

            (value, state) = computation.Run.Run("ab");

            Assert.False(value.HasValue);
            Assert.Equal("ab___", state);
        }

        [Fact]
        public static void LinqWriterTest()
        {
            var cxt = new WriterCxt<IList<string>, Monoid.ListAppendImmutableMonoid<string>>();

            bool m = true;

            var computation = 
                from _1 in MaybeT.Lift(cxt.Tell(FuncList.Make("function call 1")))
                from _2 in MaybeT.Lift(cxt.Tell(FuncList.Make("function call 2")))
                from _3 in MaybeT.HoistWriter(cxt, Maybe.JustIf(m, () => new Unit()))
                from _4 in MaybeT.Lift(cxt.Tell(FuncList.Make("function call 3")))
                select 5;

            var value = computation.Run;

            Assert.True(value.Result.HasValue);
            Assert.Equal(5, value.Result.Value());
            Assert.Equal(new[] { "function call 1", "function call 2", "function call 3" }, (IEnumerable<string>)value.State);

            m = false;
            computation =
                from _1 in MaybeT.Lift(cxt.Tell(FuncList.Make("function call 1")))
                from _2 in MaybeT.Lift(cxt.Tell(FuncList.Make("function call 2")))
                from _3 in MaybeT.HoistWriter(cxt, Maybe.JustIf(m, () => new Unit()))
                from _4 in MaybeT.Lift(cxt.Tell(FuncList.Make("function call 3")))
                select 5;

            value = computation.Run;

            Assert.False(value.Result.HasValue);
            Assert.Equal(new[] { "function call 1", "function call 2" }, (IEnumerable<string>)value.State);
        }
    }
}

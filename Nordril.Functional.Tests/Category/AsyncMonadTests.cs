using Nordril.Functional.Category;
using Nordril.Functional.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Nordril.Functional.Tests.Category
{
    public sealed class AsyncMonadTests
    {
        [Fact]
        public static async Task MapAsyncTest()
        {
            var xs = Task.FromResult(new DelayedList<int>());
            var res = await xs.MapAsync((int x) => Task.FromResult(x * 2));
            Assert.Empty(res.Down());

            xs = Task.FromResult(new DelayedList<int>(1, 2, 3, 4));
            res = await xs.MapAsync((int x) => Task.FromResult(x * 2));
            Assert.Equal(new int[] { 2, 4, 6, 8 }, res.Down());
        }

        [Fact]
        public static async Task MapAsyncCancelTest()
        {
            var cts = new CancellationTokenSource();
            var xs = Task.FromResult(new DelayedList<int>());
            cts.Cancel();
            var res = await xs.MapAsync((int x) => Task.FromResult(x * 2), cts.Token);
            Assert.Empty(res.Down());

            cts = new CancellationTokenSource();
            cts.CancelAfter(1250);
            xs = Task.FromResult(new DelayedList<int>(1, 2, 3, 4));
            res = await xs.MapAsync((int x) => Task.FromResult(x * 2), cts.Token);
            Assert.Equal(new int[] { 2, 4 }, res.Down());
        }

        [Fact]
        public static async Task BindAsyncTest()
        {
            var xs = Task.FromResult(new DelayedList<int>());
            var res = await xs.BindAsync((int x) => Task.FromResult(new DelayedList<int>(x, x * -1).ToAsyncMonad()));
            Assert.Empty(res.Down());

            xs = Task.FromResult(new DelayedList<int>(1, 2, 3, 4));
            res = await xs.BindAsync((int x) => Task.FromResult(new DelayedList<int>(x, x * -1).ToAsyncMonad()));
            Assert.Equal(new int[] { 1, -1, 2, -2, 3, -3, 4, -4 }, res.Down());
        }

        [Fact]
        public static async Task BindAsyncCancelTest()
        {
            var cts = new CancellationTokenSource();
            var xs = Task.FromResult(new DelayedList<int>());
            var res = await xs.BindAsync((int x) => Task.FromResult(new DelayedList<int>(x, x * -1).ToAsyncMonad()), cts.Token);
            Assert.Empty(res.Down());

            cts = new CancellationTokenSource();
            xs = Task.FromResult(new DelayedList<int>(1, 2, 3, 4));
            cts.CancelAfter(1250);
            res = await xs.BindAsync((int x) => Task.FromResult(new DelayedList<int>(x, x * -1).ToAsyncMonad()), cts.Token);
            Assert.Equal(new int[] { 1, -1, 2, -2 }, res.Down());
        }

        [Fact]
        public static async Task WhileMAsync1Test()
        {
            var xs = Task.FromResult(new Finite<bool>(false).ToAsyncMonad());
            var ret = (await xs.WhileMAsync(() => Task.FromResult(new Finite<int>(2).ToAsyncMonad()))).DownF();

            Assert.Empty(ret.Value);

            var body = new Finite<int>(2, 4);
            xs = Task.FromResult(new Finite<bool>(true, 2).ToAsyncMonad());
            ret = (await xs.WhileMAsync(() => Task.FromResult(body.ToAsyncMonad()))).DownF();

            Assert.Equal(new int[] { 2, 2 }, ret.Value);

            body = new Finite<int>(2, 1);
            xs = Task.FromResult(new Finite<bool>(true, 3).ToAsyncMonad());
            ret = (await xs.WhileMAsync(() => Task.FromResult(body.ToAsyncMonad()))).DownF();

            Assert.Equal(new int[] { 2, 0, 0 }, ret.Value);
        }

        [Fact]
        public static async Task WhileMAsync1CancelTest()
        {
            var cts = new CancellationTokenSource();
            cts.Cancel();
            var xs = Task.FromResult(new Finite<bool>(false).ToAsyncMonad());
            var ret = (await xs.WhileMAsync(() => Task.FromResult(new Finite<int>(2).ToAsyncMonad()), cts.Token)).DownF();

            Assert.Empty(ret.Value);

            cts = new CancellationTokenSource();
            cts.CancelAfter(1250);
            var body = new Finite<int>(2, 4);
            xs = Task.FromResult(new Finite<bool>(true, 2).ToAsyncMonad());
            ret = (await xs.WhileMAsync(() => Task.FromResult(body.ToAsyncMonad()), cts.Token)).DownF();

            Assert.Equal(new int[] { 2, 2 }, ret.Value);

            cts = new CancellationTokenSource();
            cts.CancelAfter(1250);
            body = new Finite<int>(2, 1);
            xs = Task.FromResult(new Finite<bool>(true, 3).ToAsyncMonad());
            ret = (await xs.WhileMAsync(() => Task.FromResult(body.ToAsyncMonad()), cts.Token)).DownF();

            Assert.Equal(new int[] { 2, 0 }, ret.Value);
        }

        [Fact]
        public static async Task WhileMAsync2Test()
        {
            var xs = Task.FromResult(new Finite<int>(0).ToAsyncMonad());
            var ret = (await xs.WhileMAsync(c => c > 0, c => Task.FromResult(new Finite<int>(c + 2).ToAsyncMonad()))).DownF();

            Assert.Empty(ret.Value);

            var body = new Finite<int>(2, 4);
            xs = Task.FromResult(new Finite<int>(15, 2).ToAsyncMonad());
            ret = (await xs.WhileMAsync(c => c > 0, c => Task.FromResult(body.Mutate(d => d + c).ToAsyncMonad()))).DownF();

            Assert.Equal(new int[] { 17, 32 }, ret.Value);

            body = new Finite<int>(2, 1);
            xs = Task.FromResult(new Finite<int>(15, 3).ToAsyncMonad());
            ret = (await xs.WhileMAsync(c => c > 0, c => Task.FromResult(body.Mutate(d => d + c).ToAsyncMonad()))).DownF();

            Assert.Equal(new int[] { 17, 0, 0 }, ret.Value);
        }

        [Fact]
        public static async Task WhileMAsync2CancelTest()
        {
            var cts = new CancellationTokenSource();
            cts.Cancel();
            var xs = Task.FromResult(new Finite<int>(0).ToAsyncMonad());
            var ret = (await xs.WhileMAsync(c => c > 0, c => Task.FromResult(new Finite<int>(c + 2).ToAsyncMonad()), cts.Token)).DownF();

            Assert.Empty(ret.Value);

            cts = new CancellationTokenSource();
            cts.CancelAfter(750);
            var body = new Finite<int>(2, 4);
            xs = Task.FromResult(new Finite<int>(15, 2).ToAsyncMonad());
            ret = (await xs.WhileMAsync(c => c > 0, c => Task.FromResult(body.Mutate(d => d + c).ToAsyncMonad()), cts.Token)).DownF();

            Assert.Equal(new int[] { 17 }, ret.Value);

            cts = new CancellationTokenSource();
            cts.CancelAfter(1250);
            body = new Finite<int>(2, 1);
            xs = Task.FromResult(new Finite<int>(15, 3).ToAsyncMonad());
            ret = (await xs.WhileMAsync(c => c > 0, c => Task.FromResult(body.Mutate(d => d + c).ToAsyncMonad()), cts.Token)).DownF();

            Assert.Equal(new int[] { 17, 0 }, ret.Value);
        }

        [Fact]
        public static async Task AggregateMAsyncTest()
        {
            var xs = await (new int[] { }.AggregateMAsync("", async (x, acc) =>
            {
                await Task.Delay(500);
                return Maybe.JustIf(acc.Length < 5, () => acc + x);
            }));

            Assert.True(xs.HasValue);
            Assert.Equal("", xs.Value());

            xs = await (new int[] { 1, 2 }.AggregateMAsync("", async (x, acc) =>
             {
                 await Task.Delay(500);
                 return Maybe.JustIf(acc.Length < 5, () => acc + x);
             }));

            Assert.True(xs.HasValue);
            Assert.Equal("12", xs.Value());

            xs = await (new int[] { 1, 2,3,4,5, 6 }.AggregateMAsync("", async (x, acc) =>
            {
                await Task.Delay(500);
                return Maybe.JustIf(acc.Length < 5, () => acc + x);
            }));

            Assert.False(xs.HasValue);
        }

        [Fact]
        public static async Task AggregateMAsyncCancelTest()
        {
            var cts = new CancellationTokenSource();
            cts.Cancel();
            var xs = await(new int[] { }.AggregateMAsync("", async (x, acc) =>
            {
                await Task.Delay(500);
                return Maybe.JustIf(acc.Length < 5, () => acc + x);
            }, cts.Token));

            Assert.True(xs.HasValue);
            Assert.Equal("", xs.Value());

            cts = new CancellationTokenSource();
            cts.CancelAfter(250);
            xs = await(new int[] { 1, 2,3,4 }.AggregateMAsync("", async (x, acc) =>
            {
                await Task.Delay(500);
                return Maybe.JustIf(acc.Length < 5, () => acc + x);
            }, cts.Token));

            Assert.True(xs.HasValue);
            Assert.Equal("1", xs.Value());

            cts = new CancellationTokenSource();
            cts.CancelAfter(1750);
            xs = await(new int[] { 1, 2, 3, 4, 5, 6 }.AggregateMAsync("", async (x, acc) =>
            {
                await Task.Delay(500);
                return Maybe.JustIf(acc.Length < 5, () => acc + x);
            }, cts.Token));

            Assert.True(xs.HasValue);
            Assert.Equal("1234", xs.Value());
        }
    }

    public static class Exts
    {
        public static DelayedList<T> Down<T>(this IFunctor<T> f) => (DelayedList<T>)f;

        public static Finite<T> DownF<T>(this IFunctor<T> f) => (Finite<T>)f;
    }

    /// <summary>
    /// An identity-monad which only returns its value N times, during each subsequent retrieval, returns the default value of the type of its value.
    /// </summary>
    public class Finite<T> : IAsyncMonad<T>
    {
        private int remaining;
        private T inner;
        private readonly int delay = 500;
        public T Value
        {
            get
            {
                if (remaining-- > 0)
                {
                    return inner;
                }
                else
                    return default;
            }
        }

        public Finite(T value)
        {
            inner = value;
            remaining = 1;
        }

        public Finite(T value, int remaining)
        {
            inner = value;
            this.remaining = remaining;
        }

        public Finite<T> Mutate(Func<T, T> update)
        {
            inner = update(inner);
            return this;
        }

        public async Task<IAsyncMonad<TResult>> BindAsync<TResult>(Func<T, Task<IAsyncMonad<TResult>>> f)
        {
            await Task.Delay(delay);
            return await f(Value);
        }

        public IMonad<TResult> Bind<TResult>(Func<T, IMonad<TResult>> f)
        {
            return f(Value);
        }

        public async Task<IApplicative<TResult>> PureAsync<TResult>(Func<Task<TResult>> x)
        {
            await Task.Delay(delay);
            return new Finite<TResult>(await x());
        }

        public async Task<IAsyncApplicative<TResult>> ApAsync<TResult>(IApplicative<Func<T, Task<TResult>>> f)
        {
            var r = await (f as Finite<Func<T, Task<TResult>>>)
                .BindAsync(async ff => new Finite<TResult>(ff is null ? default : await ff(Value)).ToAsyncMonad());
            return r;
        }

        public IApplicative<TResult> Pure<TResult>(TResult x)
        {
            return new Finite<TResult>(x);
        }

        public IApplicative<TResult> Ap<TResult>(IApplicative<Func<T, TResult>> f)
        {
            return (f as Finite<Func<T, TResult>>)
                .Bind(ff => new Finite<TResult>(ff is null ? default : ff(Value)));
        }

        public async Task<IAsyncFunctor<TResult>> MapAsync<TResult>(Func<T, Task<TResult>> f)
        {
            await Task.Delay(delay);
            return new Finite<TResult>(await f(Value));
        }

        public IFunctor<TResult> Map<TResult>(Func<T, TResult> f)
        {
            return new Finite<TResult>(f(Value));
        }
    }

    public class DelayedList<T> : IAsyncMonad<T>, IEnumerable<T>
    {
        private readonly int delay = 500;
        private readonly List<T> inner;

        public DelayedList(params T[] elems)
        {
            inner = new List<T>(elems);
        }

        public DelayedList(IEnumerable<T> elems)
        {
            inner = elems is null ? new List<T>() : elems.ToList();
        }

        public IApplicative<TResult> Ap<TResult>(IApplicative<Func<T, TResult>> f)
        {
            var functions = (DelayedList<Func<T, TResult>>)f;
            var ret = new List<TResult>();

            foreach (var fx in functions.inner)
            {
                foreach (var x in inner)
                {
                    var y = fx(x);
                    ret.Add(y);
                }
            }

            return new DelayedList<TResult>(ret);
        }

        public async Task<IAsyncApplicative<TResult>> ApAsync<TResult>(IApplicative<Func<T, Task<TResult>>> f)
        {
            var functions = (DelayedList<Func<T, Task<TResult>>>)f;
            var ret = new List<TResult>();

            foreach (var fx in functions.inner)
            {
                foreach (var x in inner)
                {
                    await Task.Delay(delay);
                    var y = await fx(x);
                    ret.Add(y);
                }
            }

            return new DelayedList<TResult>(ret);
        }

        public IMonad<TResult> Bind<TResult>(Func<T, IMonad<TResult>> f)
        {
            return new DelayedList<TResult>(inner.SelectMany(x => ((DelayedList<TResult>)f(x)).inner));
        }

        public Task<IAsyncMonad<TResult>> BindAsync<TResult>(Func<T, Task<IAsyncMonad<TResult>>> f)
            => BindAsync(f, default);

        public async Task<IAsyncMonad<TResult>> BindAsync<TResult>(Func<T, Task<IAsyncMonad<TResult>>> f, CancellationToken token)
        {
            var ys = new List<TResult>(inner.Count);
            foreach (var x in inner)
            {
                await Task.Delay(delay);
                if (token.IsCancellationRequested)
                    return new DelayedList<TResult>(ys);
                var newYs = (DelayedList<TResult>)(await f(x));
                ys.AddRange(newYs.inner);
            }
            return new DelayedList<TResult>(ys);
        }

        public IEnumerator<T> GetEnumerator() => inner.GetEnumerator();

        public IFunctor<TResult> Map<TResult>(Func<T, TResult> f)
        {
            return new DelayedList<TResult>(inner.Select(f));
        }

        public Task<IAsyncFunctor<TResult>> MapAsync<TResult>(Func<T, Task<TResult>> f)
            => MapAsync(f, default);

        public async Task<IAsyncFunctor<TResult>> MapAsync<TResult>(Func<T, Task<TResult>> f, CancellationToken token)
        {
            var ys = new List<TResult>(inner.Count);
            foreach (var x in inner)
            {
                await Task.Delay(delay);
                if (token.IsCancellationRequested)
                    return new DelayedList<TResult>(ys);
                ys.Add(await f(x));
            }
            return new DelayedList<TResult>(ys);
        }

        public IApplicative<TResult> Pure<TResult>(TResult x)
        {
            return new DelayedList<TResult>(new TResult[] { x });
        }

        public async Task<IApplicative<TResult>> PureAsync<TResult>(Func<Task<TResult>> x)
        {
            await Task.Delay(delay);
            return new DelayedList<TResult>(new TResult[] { await x() });
        }

        IEnumerator IEnumerable.GetEnumerator() => inner.GetEnumerator();
    }
}

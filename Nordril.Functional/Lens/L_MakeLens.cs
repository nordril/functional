using System;
using System.Collections.Generic;
using System.Text;
using Nordril.Functional.Data;

namespace Nordril.Functional.Lens
{
    public static partial class L
    {
        public static partial class Make
        {
            /// <summary>
            /// Creates a lens (a combined getter/setter) out of a pair of accessing- and setting-functions.
            /// </summary>
            /// <typeparam name="S">The type of the container.</typeparam>
            /// <typeparam name="A">The type of the result.</typeparam>
            /// <param name="get">The accessing-function.</param>
            /// <param name="set">The setting-function.</param>
            public static IMonoLens<S, A> Lens<S, A>(Func<S, A> get, Func<S, A, S> set)
                => new MonoLens<S, A>(t => g => s => g(get(s)).Map(x => set(s, x)));



            /// <summary>
            /// Create a mutating lens for a given index <paramref name="index"/> which updates, removes, or adds an element in a list.
            /// </summary>
            /// <typeparam name="S">The type of the list-container.</typeparam>
            /// <typeparam name="A">The type of the value.</typeparam>
            public static IMonoLens<S, Maybe<A>> AtSetter<S, A>(int index)
                where S : IList<A>
                => new MonoLens<S, Maybe<A>>(t => g => s =>
                {
                    var x = Maybe.JustIf(s.Count > index, () => s[index]);

                    return g(x).Map(mx =>
                    {
                        if (x.HasValue && mx.IsNothing)
                            s.RemoveAt(index);
                        else if (x.IsNothing && mx.HasValue)
                            s.Add(mx.Value());
                        else if (x.HasValue && mx.HasValue)
                            s[index] = mx.Value();

                        return s;
                    });
                });

            /// <summary>
            /// Create a mutating lens for a given key <paramref name="key"/> which updates, removes, or adds an element in a dictionary.
            /// </summary>
            /// <typeparam name="S">The type of the dictionary-container.</typeparam>
            /// <typeparam name="A">The type of the value.</typeparam>
            /// <typeparam name="TKey">The type of the key.</typeparam>
            public static IMonoLens<S, Maybe<A>> AtSetter<S, A, TKey>(TKey key)
                where S : IDictionary<TKey, A>
                => new MonoLens<S, Maybe<A>>(t => g => s =>
                {
                    var x = Maybe.JustIf(s.TryGetValue(key, out var v), () => v);

                    return g(x).Map(mx =>
                    {
                        if (x.HasValue && mx.IsNothing)
                            s.Remove(key);
                        else if (x.IsNothing && mx.HasValue)
                            s.Add(key, mx.Value());
                        else if (x.HasValue && mx.HasValue)
                            s[key] = mx.Value();

                        return s;
                    });
                });

            /// <summary>
            /// The identity lens.
            /// </summary>
            /// <typeparam name="S">The type of the data.</typeparam>
            public static IMonoLens<S, S> Id<S>() => Lens<S, S>(x => x, (x, _) => x);

            /// <summary>
            /// Turns a function into a lens.
            /// </summary>
            /// <typeparam name="S">The type of the input.</typeparam>
            /// <typeparam name="T">The type of the output.</typeparam>
            /// <param name="f">The function to get the output.</param>
            /// <param name="g">The function to get the input from the output.</param>
            /// <returns></returns>
            public static IMonoLens<S, T> Func<S, T>(Func<S, T> f, Func<T, S> g) => Lens(f, (_, t) => g(t));
        }
    }
}

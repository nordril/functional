using System;
using System.Collections.Generic;
using System.Text;
using Nordril.Functional.Category;
using Nordril.Functional.Data;

namespace Nordril.Functional.Lens
{
    public static partial class L
    {
        public partial class Make
        {
            /// <summary>
            /// Creates a setter out of a setting-function.
            /// If you do not need to change the type of the value and the container, see <see cref="Setter{S, A}(Func{Func{A, A}, Func{S, S}})"/>.
            /// </summary>
            /// <typeparam name="S">The type of the input container.</typeparam>
            /// <typeparam name="T">The type of the output container.</typeparam>
            /// <typeparam name="A">The type of the value to set.</typeparam>
            /// <typeparam name="B">The type of the result of the setting.</typeparam>
            /// <param name="set">The function which takes a function that modifies a value of type <typeparamref name="A"/>, an input-container of type <typeparamref name="S"/>, and which returns an output-container of type <typeparamref name="T"/>.</param>
            public static ISetter<S, T, A, B> Setter<S, T, A, B>(Func<Func<A, B>, Func<S, T>> set)
            => new Setter<S, T, A, B>(g => s => new Identity<T>(set(a => g(a).Value)(s)));

            /// <summary>
            /// Creates a setter out of a setting-function which does not change the type of the value or its container.
            /// </summary>
            /// <typeparam name="S">The type of the input container.</typeparam>
            /// <typeparam name="A">The type of the value to set.</typeparam>
            /// <param name="set">The function which takes a function that modifies a value of type <typeparamref name="A"/>, an input-container of type <typeparamref name="S"/>, and which returns an output-container of type <typeparamref name="S"/>.</param>
            public static ISetter<S, S, A, A> Setter<S, A>(Func<Func<A, A>, Func<S, S>> set)
                => new Setter<S, S, A, A>(g => s => new Identity<S>(set(a => g(a).Value)(s)));

            /// <summary>
            /// Creates a setter which applies a function to each element of an <see cref="IFunctor{TSource}"/>.
            /// </summary>
            /// <typeparam name="S">The type of the input container.</typeparam>
            /// <typeparam name="T">The type of the output container.</typeparam>
            /// <typeparam name="A">The type of the value to set.</typeparam>
            /// <typeparam name="B">The type of the result of the setting.</typeparam>
            public static ISetter<S, T, A, B> MappedSetter<S, T, A, B>()
                where S : IFunctor<A>
                where T : IFunctor<B>
                => Setter<S, T, A, B>(g => s => (T)s.Map(g));

            /// <summary>
            /// Creates a setter which applies a function to each element of an <see cref="IKeyedFunctor{TKey, TSource}"/>, also accessing the element's index.
            /// </summary>
            public static ISetter<S, T, (A, TKey), B> KeyedMappedSetter<S, T, A, B, TKey>()
                where S : IKeyedFunctor<TKey, A>
                where T : IFunctor<B>
                => Setter<S, T, (A, TKey), B>(g => s => (T)s.MapWithKey((k, a) => g((a, k))));

            /// <summary>
            /// Creates a mutating setter which sets an element in a collection with a given index.
            /// If the index does not fall into the list, nothing is done.
            /// </summary>
            /// <typeparam name="A">The type of the value.</typeparam>
            /// <typeparam name="S">The type of the list.</typeparam>
            public static ISetter<(S input, int index), S, (A value, int index), A> IndexSetter<S, A>()
                where S : IList<A>
                => new Setter<(S input, int index), S, (A value, int index), A>(g => s =>
                {
                    if (s.input.Count <= s.index || 0 > s.index)
                        return new Identity<S>(s.input);

                    s.input[s.index] = g((s.input[s.index], s.index)).Value;
                    return new Identity<S>(s.input);
                });

            /// <summary>
            /// Creates a mutating setter which sets an element in a collection with a given index.
            /// </summary>
            /// <typeparam name="TKey">The type of the key.</typeparam>
            /// <typeparam name="A">The type of the value.</typeparam>
            /// <typeparam name="S">The type of the dictionary.</typeparam>
            public static ISetter<(S input, TKey key), S, (A value, TKey key), A> IndexSetter<S, A, TKey>()
                where S : IDictionary<TKey, A>
                => new Setter<(S input, TKey key), S, (A value, TKey key), A>(g => s =>
                {
                    if (s.input.TryGetValue(s.key, out var x))
                        s.input[s.key] = g((x, s.key)).Value;

                    return new Identity<S>(s.input);
                });
        }
    }
}

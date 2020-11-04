using Nordril.Functional.Category;
using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional.Lens
{
    public static partial class L
    {
        public static partial class Make
        {
            /// <summary>
            /// Creates an <see cref="IWithering{S, T, A, B}"/> for a type <typeparamref name="S"/> which is <see cref="IWitherable{TSource}"/>.
            /// </summary>
            /// <typeparam name="S">The type of the <see cref="IWitherable{TSource}"/>.</typeparam>
            /// <typeparam name="T">The type of the resulting <see cref="IWitherable{TSource}"/>.</typeparam>
            /// <typeparam name="B">The type of the result of the inner function.</typeparam>
            /// <typeparam name="A">The type of the contained elements.</typeparam>
            public static IWithering<S, T, A, B> Withering<S, T, A, B>()
                where S : IWitherable<A>
            {
                return new Withering<S, T, A, B>(t => g => s => (IAlternative<T>)
                s.TraverseMaybe<B>(t,
                g.Then(Alternative.Optional)).Map(x => (T)x));
            }

            /// <summary>
            /// Creates an <see cref="IWithering{S, T, A, B}"/> for a type <typeparamref name="S"/> which is <see cref="IWitherable{TSource}"/>.
            /// </summary>
            /// <typeparam name="S">The type of the <see cref="IWitherable{TSource}"/>.</typeparam>
            /// <typeparam name="A">The type of the contained elements.</typeparam>
            public static IWithering<S, S, A, A> Withering<S, A>()
                where S : IWitherable<A>
            {
                return new Withering<S, S, A, A>(t => g => s =>
                (IAlternative<S>)s.TraverseMaybe<A>(t, g.Then(Alternative.Optional)).Map(x => (S)x));
            }
        }
    }
}

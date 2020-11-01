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
            /// Creates an <see cref="ITraversal{S, T,A,B}"/> for a type <typeparamref name="S"/> which is <see cref="ITraversable{TSource}"/>.
            /// </summary>
            /// <typeparam name="S">The type of the <see cref="ITraversable{TSource}"/>.</typeparam>
            /// <typeparam name="T">The type of the resulting <see cref="ITraversable{TSource}"/>.</typeparam>
            /// <typeparam name="B">The type of the result of the inner function.</typeparam>
            /// <typeparam name="A">The type of the contained elements.</typeparam>
            public static ITraversal<S, T, A, B> Traversal<S, T, A, B>()
                where S : ITraversable<A>
            {
                return new Traversal<S, T, A, B>(t => g => s => (IApplicative<T>)s.Traverse<B>(t, g).Map(x => (T)x));
            }

            /// <summary>
            /// Creates an <see cref="IMonoTraversal{S, A}"/> for a type <typeparamref name="S"/> which is <see cref="ITraversable{TSource}"/>.
            /// </summary>
            /// <typeparam name="S">The type of the <see cref="ITraversable{TSource}"/>.</typeparam>
            /// <typeparam name="A">The type of the contained elements.</typeparam>
            public static IMonoTraversal<S, A> Traversal<S, A>()
                where S : ITraversable<A>
            {
                return new MonoTraversal<S, A>(t => g => s => (IApplicative<S>)s.Traverse<A>(t, g).Map(x => (S)x));
            }
        }
    }
}

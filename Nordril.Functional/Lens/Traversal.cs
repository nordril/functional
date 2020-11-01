using Nordril.Functional.Algebra;
using Nordril.Functional.Category;
using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional.Lens
{
    /// <summary>
    /// A setter which can read/update multiple fields.
    /// </summary>
    /// <typeparam name="S">The type of the input data structure.</typeparam>
    /// <typeparam name="T">The type of the output data structure.</typeparam>
    /// <typeparam name="A">The type of the input-object in <typeparamref name="S"/>.</typeparam>
    /// <typeparam name="B">The type of the result-object in <typeparamref name="T"/>.</typeparam>
    internal class Traversal<S, T, A, B> : ITraversal<S, T, A, B>
    {
        /// <inheritdoc />
        public Func<Func<A, Identity<B>>, Func<S, Identity<T>>> SetFunc { get; }

        protected Func<Type, Func<Func<A, IApplicative<B>>, Func<S, IApplicative<T>>>> Func { get; }

        /// <summary>
        /// Creates a traversal.
        /// </summary>
        /// <param name="f">The traversal-function. This uses the applicative returned by its first input-function, though the compiler can't check this.</param>
        public Traversal(Func<Type, Func<Func<A, IApplicative<B>>, Func<S, IApplicative<T>>>> f)
        {
            Func = f;
            var sf = TraversalFunc<Identity<B>, Identity<T>>();
            SetFunc = g => s => sf(g)(s);
        }

        /// <inheritdoc />
        public Func<Func<A, FB>, Func<S, FT>> TraversalFunc<FB, FT>()
            where FB : IApplicative<B>
            where FT : IApplicative<T>
        {
            return g => s => (FT)Func(typeof(FT))(x => g(x))(s);
        }

        /// <inheritdoc />
        public Func<Func<A, IApplicative<B>>, Func<S, IApplicative<T>>> TraversalFunc(Type t)
        {
            return g => Func(t)(g);
        }
    }
}

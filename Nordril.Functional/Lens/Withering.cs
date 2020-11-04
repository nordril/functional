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
    internal class Withering<S, T, A, B> : IWithering<S, T, A, B>
    {
        protected Func<Type, Func<Func<A, IAlternative<B>>, Func<S, IAlternative<T>>>> Func { get; }

        /// <summary>
        /// Creates a traversal.
        /// </summary>
        /// <param name="f">The traversal-function. This uses the applicative returned by its first input-function, though the compiler can't check this.</param>
        public Withering(Func<Type, Func<Func<A, IAlternative<B>>, Func<S, IAlternative<T>>>> f)
        {
            Func = f;
        }

        /// <inheritdoc />
        public Func<Func<A, FB>, Func<S, FT>> WitherFunc<FB, FT>()
            where FB : IAlternative<B>
            where FT : IAlternative<T>
        {
            return g => s => (FT)Func(typeof(FT))(x => g(x))(s);
        }

        /// <inheritdoc />
        public Func<Func<A, IAlternative<B>>, Func<S, IAlternative<T>>> WitherFunc(Type t)
        {
            return g => Func(t)(g);
        }
    }
}

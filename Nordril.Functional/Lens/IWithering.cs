using Nordril.Functional.Category;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional.Lens
{
    /// <summary>
    /// A "filter" on values. Even though <see cref="IWitherable{TSource}"/> is a subtype of <see cref="ITraversable{TSource}"/>, an <see cref="IWithering{S, T, A, B}"/> is not, in general, a valid traversal or setter.
    /// </summary>
    /// <typeparam name="S">The type of the input data structure.</typeparam>
    /// <typeparam name="T">The type of the output data structure.</typeparam>
    /// <typeparam name="A">The type of the input-object in <typeparamref name="S"/>.</typeparam>
    /// <typeparam name="B">The type of the result-object in <typeparamref name="T"/>.</typeparam>
    public interface IWithering<S, T, A, B>
    {
        /// <summary>
        /// Returns the wither-function, which must work with any <see cref="IAlternative{TSource}"/> for <typeparamref name="B"/> and <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="FB">The concrete functor for <typeparamref name="B"/>.</typeparam>
        /// <typeparam name="FT">The concrete functor for <typeparamref name="T"/>.</typeparam>
        Func<Func<A, FB>, Func<S, FT>> WitherFunc<FB, FT>()
            where FB : IAlternative<B>
            where FT : IAlternative<T>;

        /// <summary>
        /// Returns the wither-function, which must work with any <see cref="IAlternative{TSource}"/> for <typeparamref name="B"/> and <typeparamref name="T"/>.
        /// </summary>
        Func<Func<A, IAlternative<B>>, Func<S, IAlternative<T>>> WitherFunc(Type t);
    }
}

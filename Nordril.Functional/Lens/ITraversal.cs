using Nordril.Functional.Category;
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
    public interface ITraversal<S, T, A, B> : ISetter<S, T, A, B>
    {
        /// <summary>
        /// Returns the traversal-function, which must work with any <see cref="IApplicative{TSource}"/> for <typeparamref name="B"/> and <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="FB">The concrete functor for <typeparamref name="B"/>.</typeparam>
        /// <typeparam name="FT">The concrete functor for <typeparamref name="T"/>.</typeparam>
        Func<Func<A, FB>, Func<S, FT>> TraversalFunc<FB, FT>()
            where FB : IApplicative<B>
            where FT : IApplicative<T>;

        /// <summary>
        /// Returns the traversal-function, which must work with any <see cref="IApplicative{TSource}"/> for <typeparamref name="B"/> and <typeparamref name="T"/>.
        /// </summary>
        Func<Func<A, IApplicative<B>>, Func<S, IApplicative<T>>> TraversalFunc(Type t);
    }
}

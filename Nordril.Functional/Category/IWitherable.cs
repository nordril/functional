using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional.Category
{
    /// <summary>
    /// A traversable data structure which also supports the removal of elements during the traversal.
    /// Also known as <em>Witherable</em>.
    /// </summary>
    /// <typeparam name="TSource">The type of elements in the container.</typeparam>
    public interface IWitherable<out TSource> : ITraversable<TSource>, IFilterable<TSource>
    {
        /// <summary>
        /// Maps each element of to an <see cref="IApplicative{TSource}"/> and collects the results.
        /// The following must hold for implementations:
        /// <code>
        /// X.TraverseMaybe(x => f(x).Map(y => Maybe.Just(y)) == X.Traverse(f) (conservation)<br />
        /// new Compose(X.TraverseMaybe(g).Map(y => y.TraverseMaybe(f))) == X.TraverseMaybe(x => new Compose(g(x).Then(y => y.Map(z => z.TraverseMaybe(f)))) (composition)<br />
        /// </code>
        /// </summary>
        /// <typeparam name="TApplicative">The type of the resulting applicative.</typeparam>
        /// <typeparam name="TResult">The type of the resulting elements.</typeparam>
        /// <param name="f">The mapping-function.</param>
        public IApplicative<IWitherable<TResult>> TraverseMaybe<TApplicative, TResult>(Func<TSource, TApplicative> f)
            where TApplicative : IApplicative<Maybe<TResult>>;

        /// <summary>
        /// Maps each element of to an <see cref="IApplicative{TSource}"/> and collects the results.
        /// The following must hold for implementations:
        /// <code>
        /// X.Traverse(F).Then(T) == X.Traverse(F.Then(T)) (naturality)<br />
        /// X.Traverse(x => new Identity&lt;T&gt;) == new Identity&lt;T&lt;Source&gt;&gt;(X) where X is T&lt;Source&gt; (identity)<br />
        /// X.Traverse(y => new Compose(f(x).Map(g))) == new Compose(X.Traverse(f).Map(y => y.Traverse(g)) (composition)<br />
        /// </code>
        /// </summary>
        /// <param name="applicative">The type of the resulting applicative. Must be of type <c>IApplicative&lt;TResult&gt;</c></param>
        /// <typeparam name="TResult">The type of the resulting elements.</typeparam>
        /// <param name="f">The mapping-function.</param>
        public IApplicative<IWitherable<TResult>> TraverseMaybe<TResult>(Type applicative, Func<TSource, IApplicative<Maybe<TResult>>> f);
    }
}

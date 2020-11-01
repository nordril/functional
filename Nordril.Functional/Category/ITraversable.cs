using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional.Category
{
    /// <summary>
    /// Data structures which can be traversed left to right.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the container.</typeparam>
    public interface ITraversable<out TSource> : IFoldable<TSource>
    {
        /// <summary>
        /// Maps each element of to an <see cref="IApplicative{TSource}"/> and collects the results.
        /// The following must hold for implementations:
        /// <code>
        /// X.Traverse(F).Then(T) == X.Traverse(F.Then(T)) (naturality)<br />
        /// X.Traverse(x => new Identity&lt;T&gt;) == new Identity&lt;T&lt;Source&gt;&gt;(X) where X is T&lt;Source&gt; (identity)<br />
        /// X.Traverse(y => new Compose(f(x).Map(g))) == new Compose(X.Traverse(f).Map(y => y.Traverse(g)) (composition)<br />
        /// </code>
        /// </summary>
        /// <typeparam name="TApplicative">The type of the resulting applicative.</typeparam>
        /// <typeparam name="TResult">The type of the resulting elements.</typeparam>
        /// <param name="f">The mapping-function.</param>
        public IApplicative<ITraversable<TResult>> Traverse<TApplicative, TResult>(Func<TSource, TApplicative> f)
            where TApplicative : IApplicative<TResult>;

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
        public IApplicative<ITraversable<TResult>> Traverse<TResult>(Type applicative, Func<TSource, IApplicative<TResult>> f);
    }

    /// <summary>
    /// Extension methods for <see cref="ITraversable{TSource}"/>.
    /// </summary>
    public static class Traversable
    {
        /// <summary>
        /// Iterates through an <see cref="ITraversable{TSource}"/> containing <see cref="IApplicative{TSource}"/>-elements and collects the results.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements.</typeparam>
        /// <typeparam name="TApplicative">The type of the applicative to which to map the contained elements.</typeparam>
        /// <param name="x">The <see cref="ITraversable{TSource}"/>.</param>
        public static IApplicative<ITraversable<TSource>> Traverse<TApplicative, TSource>(this ITraversable<TApplicative> x)
            where TApplicative : IApplicative<TSource>
            => x.Traverse<TApplicative, TSource>(x => x);
    }
}

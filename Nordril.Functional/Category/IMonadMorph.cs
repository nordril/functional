using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional.Category
{
    /// <summary>
    /// A functor in the category of monads which offers the counterpart to <see cref="IMonadTransformer{TUnlifted, TLifted, TInner, TSource}.Lift(TUnlifted)"/>.
    /// </summary>
    /// <remarks>
    /// <see cref="IMonadTransformer{TUnlifted, TLifted, TInner, TSource}.Lift(TUnlifted)"/> and <see cref="Hoist(TInner)"/> are two ways of arriving at the same destination. If we have a value of type <c>A</c>, a monad <c>M&lt;A&gt;</c>, and a monad-transformer <c>T&lt;I, M,A&gt;</c>, we can go from <c>A</c> to <c>T&lt;I, M,A&gt;</c> over two "paths", as illustrated by this ASCII-diagram:
    /// <code>
    ///    A -----[M.Pure]----&gt; M&lt;A&gt;<br />
    ///    |                     |<br />
    /// [I.Pure]             [T.Lift]<br />
    ///    |                     |<br />
    ///    v                     v<br />
    ///   I&lt;A&gt; --[T.Hoist]--&gt; T&lt;M,A&gt;<br />
    /// </code>
    /// That is, if we have, say, a value of type <see cref="int"/>, a monad <see cref="Io{T}"/>, and a transformer <see cref="MaybeT{TUnlifted, TLifted, TInner, TSource}"/> (which is the transformer-version of <see cref="Maybe{T}"/>), then we can either
    /// <list type="number">
    ///     <item>wrap the <see cref="int"/> into <see cref="Io{T}"/> and then into <see cref="MaybeT{TUnlifted, TLifted, TInner, TSource}"/> via <see cref="IMonadTransformer{TUnlifted, TLifted, TInner, TSource}"/>, or</item>
    ///     <item>wrap the <see cref="int"/> into <see cref="Maybe{T}"/>, and then into <see cref="MaybeT{TUnlifted, TLifted, TInner, TSource}"/> via <see cref="IMonadMorph{TUnlifted, TLifted, TInner, TSource}.Hoist(TInner)"/>.</item>
    /// </list>
    /// <c>lift</c> is thus used to lift "unrelated" computations into a monad transformer (like <see cref="Io{T}"/> into <see cref="MaybeT{TUnlifted, TLifted, TInner, TSource}"/>), whereas <c>hoist</c> is used to lift the "basic" version of computation into its transformer-form (like <see cref="Maybe{T}"/> into <see cref="MaybeT{TUnlifted, TLifted, TInner, TSource}"/>).
    /// <br />
    /// Formally, this interface is one morphism in a functor in the category of monads.
    /// </remarks>
    /// <typeparam name="TUnlifted">The unlifted monadic value (e.g. <c>IO&lt;int&gt;</c>). This is used only by <see cref="IMonadTransformer{TUnlifted, TLifted, TInner, TSource}.Lift(TUnlifted)"/>.</typeparam>
    /// <typeparam name="TLifted">The type of the overall monadic value, e.g. <c>IO&lt;Maybe&lt;int&gt;&gt;</c>.</typeparam>
    /// <typeparam name="TInner">The inner monadic value, e.g. <c>Maybe&lt;int&gt;</c>.</typeparam>
    /// <typeparam name="TSource">The innermost source-type.</typeparam>
    public interface IMonadMorph<in TUnlifted, out TLifted, TInner, TSource> : IMonadTransformer<TUnlifted, TLifted, TInner, TSource>
        where TUnlifted : IMonad<TSource>
        where TLifted : IMonad<TInner>
        where TInner : IMonad<TSource>
    {
        /// <summary>
        /// Hoist a value from the base form of the monad into its transformer-form, wrapping it, effectively, in a second monad.
        /// </summary>
        /// <param name="inner">The value in the inner monad to hoist.</param>
        IMonadTransformer<TUnlifted, TLifted, TInner, TSource> Hoist(TInner inner);
    }
}

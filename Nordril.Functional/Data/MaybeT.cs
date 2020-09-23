using Nordril.Functional.Category;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional.Data
{
    /// <summary>
    /// The <see cref="Maybe{T}"/> monad transformer which models a <see cref="Maybe{T}"/>-value being contained in a monad, e.g. <c>IO&lt;Maybe&lt;T&gt;&gt;</c>.
    /// </summary>
    /// <typeparam name="TUnlifted">The unlifted monadic value (e.g. <c>IO&lt;T&gt;</c>).</typeparam>
    /// <typeparam name="TLifted">The monadic type of the inner monad, containing, in turn, a <see cref="Maybe{T}"/>-value.</typeparam>
    /// <typeparam name="TInner">The <see cref="Maybe{T}"/>-type.</typeparam>
    /// <typeparam name="TSource">The innermost source-type.</typeparam>
    public struct MaybeT<TUnlifted, TLifted, TInner, TSource> : IMonadTransformer<TUnlifted, TLifted, TInner, TSource>
        where TUnlifted : IMonad<TSource>
        where TLifted : IMonad<TInner>
        where TInner : IMonad<TSource>
    {
        /// <summary>
        /// The inner value of type <c>m (Maybe a)</c>.
        /// </summary>
        public TLifted Run { get; }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="run">The contained monadic value with the <see cref="Maybe{T}"/>-instance as the base value.</param>
        public MaybeT(TLifted run)
        {
            Run = run;
        }

        /// <summary>
        /// Creates a Nothing-instance of <see cref="MaybeT"/>.
        /// </summary>
        public static MaybeT<TUnlifted, TLifted, TInner, TSource> Nothing()
        {
            var z = ((TInner)(object)Maybe.Nothing<TSource>()).PureUnsafe<TInner, TLifted>();
            return new MaybeT<TUnlifted, TLifted, TInner, TSource>(z);
        }

        /// <inheritdoc />
        public IMonadTransformer<TUnliftedResult, TLiftedResult, TInnerResult, TResult> BindT<TUnliftedResult, TLiftedResult, TInnerResult, TResult>(Func<TSource, IMonadTransformer<TUnliftedResult, TLiftedResult, TInnerResult, TResult>> f)
            where TUnliftedResult : IMonad<TResult>
            where TLiftedResult : IMonad<TInnerResult>
            where TInnerResult : IMonad<TResult>
        {
            var mnb = (TLiftedResult)Run.Bind(v => ((Maybe<TSource>)(object)v)
                .ValueOrLazy(
                    x => ((MaybeT<TUnliftedResult, TLiftedResult, TInnerResult, TResult>)f(x)).Run,
                    () => ((TInnerResult)(object)Maybe.Nothing<TResult>()).PureUnsafe<TInnerResult, TLiftedResult>()));

            return new MaybeT<TUnliftedResult, TLiftedResult, TInnerResult, TResult>(mnb);
        }

        /// <inheritdoc />
        public IMonadTransformer<TUnliftedResult, TLiftedResult, TInnerResult, TResult> ApT<TUnliftedFunc, TLiftedFunc, TInnerFunc, TUnliftedResult, TLiftedResult, TInnerResult, TResult>(IMonadTransformer<TUnliftedFunc, TLiftedFunc, TInnerFunc, Func<TSource, TResult>> f)
            where TUnliftedFunc : IMonad<Func<TSource, TResult>>
            where TLiftedFunc : IMonad<TInnerFunc>
            where TInnerFunc : IMonad<Func<TSource, TResult>>
            where TUnliftedResult : IMonad<TResult>
            where TLiftedResult : IMonad<TInnerResult>
            where TInnerResult : IMonad<TResult>
        {
            var m_f = (MaybeT<TUnliftedFunc, TLiftedFunc, TInnerFunc, Func<TSource, TResult>>)f;
            var m_x = this;

            return m_f.BindT(y =>
            {
                return m_x.MapT<TUnliftedResult, TLiftedResult, TInnerResult, TResult>(x =>
                {
                    return y(x);
                });
            });
        }

        /// <inheritdoc />
        public IMonadTransformer<TUnlifted, TLifted, TInner, TSource> Lift(TUnlifted x)
        {
            var z = (TLifted)x.Bind(y => ((TInner)(object)Maybe.Just(y)).PureUnsafe<TInner, TLifted>());
            return new MaybeT<TUnlifted, TLifted, TInner, TSource>(z);
        }

        /// <inheritdoc />
        public IMonadTransformer<TUnliftedResult, TLiftedResult, TInnerResult, TResult> PureT<TUnliftedResult, TLiftedResult, TInnerResult, TResult>(TResult x)
            where TUnliftedResult : IMonad<TResult>
            where TLiftedResult : IMonad<TInnerResult>
            where TInnerResult : IMonad<TResult>
        {
            var z = ((TInnerResult)(object)Maybe.Just(x)).PureUnsafe<TInnerResult, TLiftedResult>();
            return new MaybeT<TUnliftedResult, TLiftedResult, TInnerResult, TResult>(z);
        }

        /// <inheritdoc />
        public IMonadTransformer<TUnliftedResult, TLiftedResult, TInnerResult, TResult> MapT<TUnliftedResult, TLiftedResult, TInnerResult, TResult>(Func<TSource, TResult> f)
            where TUnliftedResult : IMonad<TResult>
            where TLiftedResult : IMonad<TInnerResult>
            where TInnerResult : IMonad<TResult>
        {
            var mnb = (TLiftedResult)Run.Map(m => (TInnerResult)m.Map(f));

            return new MaybeT<TUnliftedResult, TLiftedResult, TInnerResult, TResult>(mnb);
        }

        /// <inheritdoc />
        public IFunctor<TResult> Map<TResult>(Func<TSource, TResult> f)
        {
            var map = this.MakeMap<TUnlifted, TLifted, TInner, TSource, TResult>();
            return map(f, this);
        }

        /// <inheritdoc />
        public IMonad<TResult> Bind<TResult>(Func<TSource, IMonad<TResult>> f)
        {
            var bind = this.MakeBind<TUnlifted, TLifted, TInner, TSource, TResult>();
            return bind(this, f);
        }

        /// <inheritdoc />
        public IApplicative<TResult> Pure<TResult>(TResult x)
        {
            var pure = this.MakePure<TUnlifted, TLifted, TInner, TSource, TResult>();
            return pure(this, x);
        }

        /// <inheritdoc />
        public IApplicative<TResult> Ap<TResult>(IApplicative<Func<TSource, TResult>> f)
        {
            var pure = this.MakeAp<TUnlifted, TLifted, TInner, TSource, TResult>();
            return pure(this, f);
        }
    }

    /// <summary>
    /// Extension methods for <see cref="MaybeT{TUnlifted, TLifted, TInner, TSource}"/>.
    /// </summary>
    public static class MaybeT
    {
        /// <summary>
        /// Replaces an occurrence of a generic argument <paramref name="original"/> in a (generic or non-generic) type with <paramref name="replacement"/> and returns the result.
        /// </summary>
        /// <param name="t">The type in which to replace the argument.</param>
        /// <param name="original">The argument to search for (which can also be <paramref name="t"/> itself).</param>
        /// <param name="replacement">The argument with which to replace <paramref name="replacement"/>.</param>
        /// <returns></returns>
        public static Type ReplaceTypeVariable(this Type t, Type original, Type replacement)
        {
            if (t == original)
                return replacement;

            if (!t.IsConstructedGenericType)
                return t;

            var args = t.GetGenericArguments();
            var replacedArgs = new Type[args.Length];

            for (int i = 0;i<args.Length; i++)
            {
                replacedArgs[i] = args[i].ReplaceTypeVariable(original, replacement);
            }

            var typeDef = t.GetGenericTypeDefinition();
            var replaced = typeDef.MakeGenericType(replacedArgs);

            return replaced;

        }

        /// <summary>
        /// Creates a <see cref="MaybeT{TUnlifted, TLifted, TInner, TSource}"/> out of a pure value.
        /// </summary>
        /// <typeparam name="TUnlifted">The unlifted monadic value (e.g. <c>IO&lt;T&gt;</c>).</typeparam>
        /// <typeparam name="TLifted">The monadic type of the inner monad, containing, in turn, a <see cref="Maybe{T}"/>-value.</typeparam>
        /// <typeparam name="TSource">The innermost source-type.</typeparam>
        /// <param name="x">The value to wrap.</param>
        public static MaybeT<TUnlifted, TLifted, Maybe<TSource>, TSource> Just<TUnlifted, TLifted, TSource>(TSource x)
            where TUnlifted : IMonad<TSource> //m a
            where TLifted : IMonad<Maybe<TSource>> //m (Maybe a)
            => (MaybeT<TUnlifted, TLifted, Maybe<TSource>, TSource>)new MaybeT<TUnlifted, TLifted, Maybe<TSource>, TSource>().PureT<TUnlifted, TLifted, Maybe<TSource>, TSource>(x);

        /// <summary>
        /// Creates a <see cref="MaybeT{TUnlifted, TLifted, TInner, TSource}"/> out of a outer monadic value (e.g. <see cref="Io{T}"/>).
        /// </summary>
        /// <typeparam name="TUnlifted">The unlifted monadic value (e.g. <c>IO&lt;T&gt;</c>).</typeparam>
        /// <typeparam name="TLifted">The monadic type of the inner monad, containing, in turn, a <see cref="Maybe{T}"/>-value.</typeparam>
        /// <typeparam name="TSource">The innermost source-type.</typeparam>
        /// <param name="x">The value to wrap.</param>
        public static MaybeT<TUnlifted, TLifted, Maybe<TSource>, TSource> Just<TUnlifted, TLifted, TSource>(TUnlifted x)
            where TUnlifted : IMonad<TSource> //m a
            where TLifted : IMonad<Maybe<TSource>> //m (Maybe a)
            => (MaybeT<TUnlifted, TLifted, Maybe<TSource>, TSource>)new MaybeT<TUnlifted, TLifted, Maybe<TSource>, TSource>().Lift(x);

        /// <summary>
        /// Flattens the structure of a <see cref="MaybeT{TUnlifted, TLifted, TInner, TSource}"/> by erasing the innermost <see cref="Maybe{T}"/>-value.
        /// </summary>
        /// <typeparam name="TUnlifted">The unlifted monadic value (e.g. <c>IO&lt;T&gt;</c>).</typeparam>
        /// <typeparam name="TLifted">The monadic type of the inner monad, containing, in turn, a <see cref="Maybe{T}"/>-value.</typeparam>
        /// <typeparam name="TSource">The innermost source-type.</typeparam>
        /// <typeparam name="TResult">The innermost result-type.</typeparam>
        /// <param name="maybe">The <see cref="MaybeT{TUnlifted, TLifted, TInner, TSource}"/> to flatten.</param>
        /// <param name="f">The function to apply to the innermost value, if present.</param>
        /// <param name="alternative">The function which produces an alternative innermost alternative value, if the value is missing.</param>
        public static TUnlifted ValueOr<TUnlifted, TLifted, TSource, TResult>(
            this MaybeT<TUnlifted, TLifted, Maybe<TSource>, TSource> maybe,
            Func<TSource, TResult> f,
            Func<TResult> alternative)
            where TUnlifted : IMonad<TSource> //m a
            where TLifted : IMonad<Maybe<TSource>> //m (Maybe a)
        {
            return (TUnlifted)maybe.Run.Map(m => m.ValueOrLazy(f, alternative));
        }
    }
}

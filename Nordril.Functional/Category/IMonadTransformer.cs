using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Nordril.Functional.Category
{
    /// <summary>
    /// A monad transformer which wraps an inner monad (<typeparamref name="TInner"/>) in an outer monad (<typeparamref name="TLifted"/>), combining their semantics.
    /// Monad transformers are, here, also monads, enabling the usage of <see cref="IFunctor{TSource}.Map{TResult}(Func{TSource, TResult})"/>, <see cref="IApplicative{TSource}.Ap{TResult}(IApplicative{Func{TSource, TResult}})"/>, and <see cref="IMonad{TSource}.Bind{TResult}(Func{TSource, IMonad{TResult}})"/>, though they also provide monad-transformer-versions of these functions suffixed with "T". The only unique method of a monad transformer is <see cref="Lift(TUnlifted)"/>, which takes a value from the unlifted monad and lifts it.
    /// </summary>
    /// <remarks>
    /// Monad transformers allow one to combine the functionality of a whole list/stack of monads, e.g. <see cref="Reader{TEnvironment, TValue}"/>, <see cref="Writer{TOutput, TValue, TMonoid}"/>, and <see cref="State{TState, TValue}"/> in a single computation, though, in general, the order of wrapping matters, meaning <c>OuterT&lt;InnerT&lt;A&gt;&gt;</c> might have different semantics than <c>InnerT&lt;OuterT&lt;A&gt;&gt;</c>. Since monad transformers require higher-kinded types, the type parameters one has to specify are somewhat cumbersome due to CLR-limitations.
    /// </remarks>
    /// <typeparam name="TUnlifted">The unlifted monadic value (e.g. <c>IO&lt;int&gt;</c>). This is used only by <see cref="Lift(TUnlifted)"/>.</typeparam>
    /// <typeparam name="TLifted">The type of the overall monadic value, e.g. <c>IO&lt;Maybe&lt;int&gt;&gt;</c>.</typeparam>
    /// <typeparam name="TInner">The inner monadic value, e.g. <c>Maybe&lt;int&gt;</c>.</typeparam>
    /// <typeparam name="TSource">The innermost source-type.</typeparam>
    public interface IMonadTransformer<in TUnlifted, out TLifted, out TInner, TSource> : IMonad<TSource>
        where TUnlifted : IMonad<TSource>
        where TLifted : IMonad<TInner>
        where TInner : IMonad<TSource>
    {
        /// <summary>
        /// The monad-transformer-version of <see cref="IApplicative{TSource}.Pure{TResult}(TResult)"/>.
        /// </summary>
        /// <typeparam name="TUnliftedResult">The unlifted type of the result.</typeparam>
        /// <typeparam name="TLiftedResult">The lifted type of the result.</typeparam>
        /// <typeparam name="TInnerResult">The inner type of the result.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="x">The value to wrap.</param>
        IMonadTransformer<TUnliftedResult, TLiftedResult, TInnerResult, TResult> PureT<TUnliftedResult, TLiftedResult, TInnerResult, TResult>(TResult x)
            where TUnliftedResult : IMonad<TResult>
            where TLiftedResult : IMonad<TInnerResult>
            where TInnerResult : IMonad<TResult>;

        /// <summary>
        /// The monad-transformer-version of <see cref="IFunctor{TSource}.Map{TResult}(Func{TSource, TResult})"/>.
        /// </summary>
        /// <typeparam name="TUnliftedResult">The unlifted type of the result.</typeparam>
        /// <typeparam name="TLiftedResult">The lifted type of the result.</typeparam>
        /// <typeparam name="TInnerResult">The inner type of the result.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="f">The pure function to apply.</param>
        IMonadTransformer<TUnliftedResult, TLiftedResult, TInnerResult, TResult>
            MapT<TUnliftedResult, TLiftedResult, TInnerResult, TResult>(Func<TSource, TResult> f)
            where TUnliftedResult : IMonad<TResult>
            where TLiftedResult : IMonad<TInnerResult>
            where TInnerResult : IMonad<TResult>;

        /// <summary>
        /// The monad-transformer-version of <see cref="IApplicative{TSource}.Ap{TResult}(IApplicative{Func{TSource, TResult}})"/>.
        /// </summary>
        /// <typeparam name="TUnliftedFunc">The unlifted type of the function.</typeparam>
        /// <typeparam name="TLiftedFunc">The lifted type of the function.</typeparam>
        /// <typeparam name="TInnerFunc">The inner type of the function.</typeparam>
        /// <typeparam name="TUnliftedResult">The unlifted type of the result.</typeparam>
        /// <typeparam name="TLiftedResult">The lifted type of the result.</typeparam>
        /// <typeparam name="TInnerResult">The inner type of the result.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="f">The monadic value containing the function to apply to this monadic value.</param>
        IMonadTransformer<TUnliftedResult, TLiftedResult, TInnerResult, TResult> ApT<TUnliftedFunc, TLiftedFunc, TInnerFunc, TUnliftedResult, TLiftedResult, TInnerResult, TResult>(IMonadTransformer<TUnliftedFunc, TLiftedFunc, TInnerFunc, Func<TSource, TResult>> f)
            where TUnliftedFunc : IMonad<Func<TSource, TResult>>
            where TLiftedFunc : IMonad<TInnerFunc>
            where TInnerFunc : IMonad<Func<TSource, TResult>>
            where TUnliftedResult : IMonad<TResult>
            where TLiftedResult : IMonad<TInnerResult>
            where TInnerResult : IMonad<TResult>;

        /// <summary>
        /// The monad-transformer-version of <see cref="IMonad{TSource}.Bind{TResult}(Func{TSource, IMonad{TResult}})"/>.
        /// </summary>
        /// <typeparam name="TUnliftedResult">The unlifted type of the result.</typeparam>
        /// <typeparam name="TLiftedResult">The lifted type of the result.</typeparam>
        /// <typeparam name="TInnerResult">The inner type of the result.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="f">The monadic function to apply.</param>
        IMonadTransformer<TUnliftedResult, TLiftedResult, TInnerResult, TResult>
            BindT<TUnliftedResult, TLiftedResult, TInnerResult, TResult>(Func<TSource, IMonadTransformer<TUnliftedResult, TLiftedResult, TInnerResult, TResult>> f)
            where TUnliftedResult : IMonad<TResult>
            where TLiftedResult : IMonad<TInnerResult>
            where TInnerResult : IMonad<TResult>;

        /// <summary>
        /// Lifts a value from an inner monad into this monad transformer, wrapping it, effectively, in a second monad.
        /// </summary>
        /// <param name="x">The value to wrap.</param>
        IMonadTransformer<TUnlifted, TLifted, TInner, TSource> Lift(TUnlifted x);
    }

    /// <summary>
    /// Extension methods for <see cref="IMonadTransformer{TUnlifted, TLifted, TInner, TSource}"/>.
    /// </summary>
    public static class MonadTransformer
    {
        /// <summary>
        /// Creates a <see cref="Func{T1, T2, TResult}"/> which runs <see cref="IMonadTransformer{TUnlifted, TLifted, TInner, TSource}.MapT{TUnliftedResult, TLiftedResult, TInnerResult, TResult}(Func{TSource, TResult})"/>, doing the type variable replacement at runtime.
        /// </summary>
        /// <typeparam name="TUnlifted">The unlifted value.</typeparam>
        /// <typeparam name="TLifted">The lifted value, containing the inner value.</typeparam>
        /// <typeparam name="TInner">The inner value.</typeparam>
        /// <typeparam name="TSource">The raw value.</typeparam>
        /// <typeparam name="TResult">The result-type.</typeparam>
        /// <param name="transformer">The transformer whose <see cref="IMonadTransformer{TUnlifted, TLifted, TInner, TSource}.MapT{TUnliftedResult, TLiftedResult, TInnerResult, TResult}(Func{TSource, TResult})"/>-function to use.</param>
        public static Func<Func<TSource, TResult>, IFunctor<TSource>, IFunctor<TResult>> MakeMap<TUnlifted, TLifted, TInner, TSource, TResult>(this IMonadTransformer<TUnlifted, TLifted, TInner, TSource> transformer)
            where TUnlifted : IMonad<TSource>
            where TLifted : IMonad<TInner>
            where TInner : IMonad<TSource>
        {
            var method = MakeMonadTransMethod<TUnlifted, TLifted, TInner, TSource, TResult>(transformer, "MapT").Item1;

            var pF = Expression.Parameter(typeof(Func<TSource, TResult>), "f");
            var pX = Expression.Parameter(typeof(IFunctor<TSource>), "x");

            var map = Expression.Lambda<Func<Func<TSource, TResult>, IFunctor<TSource>, IFunctor<TResult>>>(
                Expression.Call(
                    Expression.Convert(pX,transformer.GetType()),
                    method, pF), pF, pX).Compile();

            return map;
        }

        /// <summary>
        /// Creates a <see cref="Func{T1, T2, TResult}"/> which runs <see cref="IMonadTransformer{TUnlifted, TLifted, TInner, TSource}.PureT{TUnliftedResult, TLiftedResult, TInnerResult, TResult}(TResult)"/>, doing the type variable replacement at runtime.
        /// </summary>
        /// <typeparam name="TUnlifted">The unlifted value.</typeparam>
        /// <typeparam name="TLifted">The lifted value, containing the inner value.</typeparam>
        /// <typeparam name="TInner">The inner value.</typeparam>
        /// <typeparam name="TSource">The raw value.</typeparam>
        /// <typeparam name="TResult">The result-type.</typeparam>
        /// <param name="transformer">The transformer whose <see cref="IMonadTransformer{TUnlifted, TLifted, TInner, TSource}.PureT{TUnliftedResult, TLiftedResult, TInnerResult, TResult}(TResult)"/>-function to use.</param>
        public static Func<IApplicative<TSource>, TResult, IApplicative<TResult>> MakePure<TUnlifted, TLifted, TInner, TSource, TResult>(this IMonadTransformer<TUnlifted, TLifted, TInner, TSource> transformer)
            where TUnlifted : IMonad<TSource>
            where TLifted : IMonad<TInner>
            where TInner : IMonad<TSource>
        {
            var method = MakeMonadTransMethod<TUnlifted, TLifted, TInner, TSource, TResult>(transformer, "PureT").Item1;

            var pY = Expression.Parameter(typeof(TResult), "y");
            var pX = Expression.Parameter(typeof(IApplicative<TSource>), "x");

            var pure = Expression.Lambda<Func<IApplicative<TSource>, TResult, IApplicative<TResult>>>(
                Expression.Call(
                    Expression.Convert(pX, transformer.GetType()), 
                    method, pY), pX, pY).Compile();

            return pure;
        }

        /// <summary>
        /// Creates a <see cref="Func{T1, T2, TResult}"/> which runs <see cref="IMonadTransformer{TUnlifted, TLifted, TInner, TSource}.ApT{TUnliftedFunc, TLiftedFunc, TInnerFunc, TUnliftedResult, TLiftedResult, TInnerResult, TResult}(IMonadTransformer{TUnliftedFunc, TLiftedFunc, TInnerFunc, Func{TSource, TResult}})"/>, doing the type variable replacement at runtime.
        /// </summary>
        /// <typeparam name="TUnlifted">The unlifted value.</typeparam>
        /// <typeparam name="TLifted">The lifted value, containing the inner value.</typeparam>
        /// <typeparam name="TInner">The inner value.</typeparam>
        /// <typeparam name="TSource">The raw value.</typeparam>
        /// <typeparam name="TResult">The result-type.</typeparam>
        /// <param name="transformer">The transformer whose <see cref="IMonadTransformer{TUnlifted, TLifted, TInner, TSource}.ApT{TUnliftedFunc, TLiftedFunc, TInnerFunc, TUnliftedResult, TLiftedResult, TInnerResult, TResult}(IMonadTransformer{TUnliftedFunc, TLiftedFunc, TInnerFunc, Func{TSource, TResult}})"/>-function to use.</param>
        public static Func<IApplicative<TSource>, IApplicative<Func<TSource, TResult>>, IApplicative<TResult>> MakeAp<TUnlifted, TLifted, TInner, TSource, TResult>(this IMonadTransformer<TUnlifted, TLifted, TInner, TSource> transformer)
            where TUnlifted : IMonad<TSource>
            where TLifted : IMonad<TInner>
            where TInner : IMonad<TSource>
        {
            var unliftedFunc = typeof(TUnlifted).ReplaceTypeVariable(typeof(TSource), typeof(Func<TSource, TResult>));
            var liftedFunc = typeof(TLifted).ReplaceTypeVariable(typeof(TSource), typeof(Func<TSource, TResult>));
            var innerFunc = typeof(TInner).ReplaceTypeVariable(typeof(TSource), typeof(Func<TSource, TResult>));

            var unliftedResult = typeof(TUnlifted).ReplaceTypeVariable(typeof(TSource), typeof(TResult));
            var liftedResult = typeof(TLifted).ReplaceTypeVariable(typeof(TSource), typeof(TResult));
            var innerResult = typeof(TInner).ReplaceTypeVariable(typeof(TSource), typeof(TResult));

            var interfaceType = typeof(IMonadTransformer<TUnlifted, TLifted, TInner, TSource>);
            var interfaceMethod = interfaceType.GetMethod("ApT");
            var im = transformer.GetType().GetInterfaceMap(interfaceType);
            var index = Array.IndexOf(im.InterfaceMethods, interfaceMethod);
            var mi = im.TargetMethods[index];

            var method = mi.MakeGenericMethod(unliftedFunc, liftedFunc, innerFunc, unliftedResult, liftedResult, innerResult, typeof(TResult));

            var fType =
                typeof(IMonadTransformer<,,,>)
                    .MakeGenericType(
                        unliftedFunc,
                        liftedFunc,
                        innerFunc,
                        typeof(Func<TSource, TResult>));

            var pF = Expression.Parameter(typeof(IApplicative<Func<TSource, TResult>>), "f");
            var pX = Expression.Parameter(typeof(IApplicative<TSource>), "x");

            var ap = Expression.Lambda<Func<IApplicative<TSource>, IApplicative<Func<TSource, TResult>>, IApplicative<TResult>>>(
                Expression.Call(
                    Expression.Convert(pX, transformer.GetType()),
                    method, Expression.Convert(pF, fType)), pX, pF);

            return ap.Compile();
        }

        /// <summary>
        /// Creates a <see cref="Func{T1, T2, TResult}"/> which runs <see cref="IMonadTransformer{TUnlifted, TLifted, TInner, TSource}.BindT{TUnliftedResult, TLiftedResult, TInnerResult, TResult}(Func{TSource, IMonadTransformer{TUnliftedResult, TLiftedResult, TInnerResult, TResult}})"/>, doing the type variable replacement at runtime.
        /// </summary>
        /// <typeparam name="TUnlifted">The unlifted value.</typeparam>
        /// <typeparam name="TLifted">The lifted value, containing the inner value.</typeparam>
        /// <typeparam name="TInner">The inner value.</typeparam>
        /// <typeparam name="TSource">The raw value.</typeparam>
        /// <typeparam name="TResult">The result-type.</typeparam>
        /// <param name="transformer">The transformer whose <see cref="IMonadTransformer{TUnlifted, TLifted, TInner, TSource}.BindT{TUnliftedResult, TLiftedResult, TInnerResult, TResult}(Func{TSource, IMonadTransformer{TUnliftedResult, TLiftedResult, TInnerResult, TResult}})"/>-function to use.</param>
        public static Func<IMonad<TSource>, Func<TSource, IMonad<TResult>>, IMonad<TResult>> MakeBind<TUnlifted, TLifted, TInner, TSource, TResult>(this IMonadTransformer<TUnlifted, TLifted, TInner, TSource> transformer)
            where TUnlifted : IMonad<TSource>
            where TLifted : IMonad<TInner>
            where TInner : IMonad<TSource>
        {
            var (method, unlifted, lifted, inner) = MakeMonadTransMethod<TUnlifted, TLifted, TInner, TSource, TResult>(transformer, "BindT");
            var resultRetType = 
                typeof(IMonadTransformer<,,,>)
                    .MakeGenericType(
                        unlifted,
                        lifted,
                        inner,
                        typeof(TResult));

            var pF = Expression.Parameter(typeof(Func<TSource, IMonad<TResult>>), "f");
            var pY = Expression.Parameter(typeof(TSource), "y");
            var pX = Expression.Parameter(typeof(IMonad<TSource>), "x");

            var bind = Expression.Lambda<Func<IMonad<TSource>, Func<TSource, IMonad<TResult>>, IMonad<TResult>>>(
                Expression.Call(
                    Expression.Convert(pX, transformer.GetType()),
                    method,
                    Expression.Lambda(
                        Expression.Convert(Expression.Invoke(pF, pY) , resultRetType)    
                    , pY)), pX, pF);

            return bind.Compile();
        }

        private static (MethodInfo, Type, Type, Type) MakeMonadTransMethod<TUnlifted, TLifted, TInner, TSource, TResult>(this IMonadTransformer<TUnlifted, TLifted, TInner, TSource> transformer, string methodName)
            where TUnlifted : IMonad<TSource>
            where TLifted : IMonad<TInner>
            where TInner : IMonad<TSource>
        {
            var unliftedResult = typeof(TUnlifted).ReplaceTypeVariable(typeof(TSource), typeof(TResult));
            var liftedResult = typeof(TLifted).ReplaceTypeVariable(typeof(TSource), typeof(TResult));
            var innerResult = typeof(TInner).ReplaceTypeVariable(typeof(TSource), typeof(TResult));

            var interfaceType = typeof(IMonadTransformer<TUnlifted, TLifted, TInner, TSource>);
            var interfaceMethod = interfaceType.GetMethod(methodName);
            var im = transformer.GetType().GetInterfaceMap(interfaceType);
            var index = Array.IndexOf(im.InterfaceMethods, interfaceMethod);
            var mi = im.TargetMethods[index];

            var concrete = mi.MakeGenericMethod(unliftedResult, liftedResult, innerResult, typeof(TResult));

            return (concrete, unliftedResult, liftedResult, innerResult);
        }
    }
}

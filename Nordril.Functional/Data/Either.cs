using Nordril.Functional.Category;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading.Tasks;

namespace Nordril.Functional.Data
{
    /// <summary>
    /// Static methods for <see cref="Either{TLeft, TRight}"/>.
    /// </summary>
    public static class Either
    {
        /// <summary>
        /// Equivalent to <see cref="IFunctor{TSource}.Map{TResult}(Func{TSource, TResult})"/>, but restricted to <see cref="Either{TLeft, TRight}"/>. Offers LINQ query support with one <c>from</c>-clause.
        /// </summary>
        /// <typeparam name="TLeft">The type of the either's left-value.</typeparam>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        public static Either<TLeft, TResult> Select<TLeft, TSource, TResult>(this Either<TLeft, TSource> source, Func<TSource, TResult> f)
            => (Either<TLeft, TResult>)source.Map(f);

        /// <summary>
        /// Equivalent to <see cref="IFunctor{TSource}.Map{TResult}(Func{TSource, TResult})"/>, but restricted to <see cref="Either{T1, T2, T3}"/>. Offers LINQ query support with one <c>from</c>-clause.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        public static Either<T1, T2, TResult> Select<T1, T2, TSource, TResult>(this Either<T1, T2, TSource> source, Func<TSource, TResult> f)
            => (Either<T1, T2, TResult>)source.Map(f);

        /// <summary>
        /// Equivalent to <see cref="IFunctor{TSource}.Map{TResult}(Func{TSource, TResult})"/>, but restricted to <see cref="Either{T1, T2, T3, T4}"/>. Offers LINQ query support with one <c>from</c>-clause.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="T3">The type of the third value.</typeparam>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        public static Either<T1, T2, T3, TResult> Select<T1, T2, T3, TSource, TResult>(this Either<T1, T2, T3, TSource> source, Func<TSource, TResult> f)
            => (Either<T1, T2, T3, TResult>)source.Map(f);

        /// <summary>
        /// Equivalent to <see cref="IFunctor{TSource}.Map{TResult}(Func{TSource, TResult})"/>, but restricted to <see cref="Either{T1, T2, T3, T4, T5}"/>. Offers LINQ query support with one <c>from</c>-clause.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="T3">The type of the third value.</typeparam>
        /// <typeparam name="T4">The type of the fourth value.</typeparam>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        public static Either<T1, T2, T3, T4, TResult> Select<T1, T2, T3, T4, TSource, TResult>(this Either<T1, T2, T3, T4, TSource> source, Func<TSource, TResult> f)
            => (Either<T1, T2, T3, T4, TResult>)source.Map(f);

        /// <summary>
        /// Equivalent to <see cref="IFunctor{TSource}.Map{TResult}(Func{TSource, TResult})"/>, but restricted to <see cref="Either{T1, T2, T3, T4, T5, T6}"/>. Offers LINQ query support with one <c>from</c>-clause.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="T3">The type of the third value.</typeparam>
        /// <typeparam name="T4">The type of the fourth value.</typeparam>
        /// <typeparam name="T5">The type of the fifth value.</typeparam>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        public static Either<T1, T2, T3, T4, T5, TResult> Select<T1, T2, T3, T4, T5, TSource, TResult>(this Either<T1, T2, T3, T4, T5, TSource> source, Func<TSource, TResult> f)
            => (Either<T1, T2, T3, T4, T5, TResult>)source.Map(f);

        /// <summary>
        /// Equivalent to <see cref="IFunctor{TSource}.Map{TResult}(Func{TSource, TResult})"/>, but restricted to <see cref="Either{T1, T2, T3, T4, T5, T6, T7}"/>. Offers LINQ query support with one <c>from</c>-clause.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="T3">The type of the third value.</typeparam>
        /// <typeparam name="T4">The type of the fourth value.</typeparam>
        /// <typeparam name="T5">The type of the fifth value.</typeparam>
        /// <typeparam name="T6">The type of the sixth value.</typeparam>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        public static Either<T1, T2, T3, T4, T5, T6, TResult> Select<T1, T2, T3, T4, T5, T6, TSource, TResult>(this Either<T1, T2, T3, T4, T5, T6, TSource> source, Func<TSource, TResult> f)
            => (Either<T1, T2, T3, T4, T5, T6, TResult>)source.Map(f);

        /// <summary>
        /// Equivalent to <see cref="IFunctor{TSource}.Map{TResult}(Func{TSource, TResult})"/>, but restricted to <see cref="Either{T1, T2, T3, T4, T5, T6, T7, T8}"/>. Offers LINQ query support with one <c>from</c>-clause.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="T3">The type of the third value.</typeparam>
        /// <typeparam name="T4">The type of the fourth value.</typeparam>
        /// <typeparam name="T5">The type of the fifth value.</typeparam>
        /// <typeparam name="T6">The type of the sixth value.</typeparam>
        /// <typeparam name="T7">The type of the seventh value.</typeparam>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        public static Either<T1, T2, T3, T4, T5, T6, T7, TResult> Select<T1, T2, T3, T4, T5, T6, T7, TSource, TResult>(this Either<T1, T2, T3, T4, T5, T6, T7, TSource> source, Func<TSource, TResult> f)
            => (Either<T1, T2, T3, T4, T5, T6, T7, TResult>)source.Map(f);

        /// <summary>
        /// Equivalent to <see cref="IMonad{TSource}"/>, but restricted to <see cref="Either{TLeft, TRight}"/>. Offers LINQ query support with multiple <c>from</c>-clauses.
        /// </summary>
        /// <typeparam name="TLeft">The type of the either's left-value.</typeparam>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TMiddle">The type of the selector's result.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        /// <param name="resultSelector">The result-selector.</param>
        public static Either<TLeft, TResult> SelectMany<TLeft, TSource, TMiddle, TResult>
            (this Either<TLeft, TSource> source,
             Func<TSource, Either<TLeft, TMiddle>> f,
             Func<TSource, TMiddle, TResult> resultSelector)
            => (Either<TLeft, TResult>)source.Bind(x => (IMonad<TResult>)f(x).Map(y => resultSelector(x, y)));

        /// <summary>
        /// Equivalent to <see cref="IMonad{TSource}"/>, but restricted to <see cref="Either{T1, T2, T3}"/>. Offers LINQ query support with multiple <c>from</c>-clauses.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TMiddle">The type of the selector's result.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        /// <param name="resultSelector">The result-selector.</param>
        public static Either<T1, T2, TResult> SelectMany<T1, T2, TSource, TMiddle, TResult>
            (this Either<T1, T2, TSource> source,
             Func<TSource, Either<T1, T2, TMiddle>> f,
             Func<TSource, TMiddle, TResult> resultSelector)
            => (Either<T1, T2, TResult>)source.Bind(x => (IMonad<TResult>)f(x).Map(y => resultSelector(x, y)));

        /// <summary>
        /// Equivalent to <see cref="IMonad{TSource}"/>, but restricted to <see cref="Either{T1, T2, T3, T4}"/>. Offers LINQ query support with multiple <c>from</c>-clauses.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="T3">The type of the third value.</typeparam>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TMiddle">The type of the selector's result.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        /// <param name="resultSelector">The result-selector.</param>
        public static Either<T1, T2, T3, TResult> SelectMany<T1, T2, T3, TSource, TMiddle, TResult>
            (this Either<T1, T2, T3, TSource> source,
             Func<TSource, Either<T1, T2, T3, TMiddle>> f,
             Func<TSource, TMiddle, TResult> resultSelector)
            => (Either<T1, T2, T3, TResult>)source.Bind(x => (IMonad<TResult>)f(x).Map(y => resultSelector(x, y)));

        /// <summary>
        /// Equivalent to <see cref="IMonad{TSource}"/>, but restricted to <see cref="Either{T1, T2, T3, T4, T5}"/>. Offers LINQ query support with multiple <c>from</c>-clauses.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="T3">The type of the third value.</typeparam>
        /// <typeparam name="T4">The type of the fourth value.</typeparam>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TMiddle">The type of the selector's result.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        /// <param name="resultSelector">The result-selector.</param>
        public static Either<T1, T2, T3, T4, TResult> SelectMany<T1, T2, T3, T4, TSource, TMiddle, TResult>
            (this Either<T1, T2, T3, T4, TSource> source,
             Func<TSource, Either<T1, T2, T3, T4, TMiddle>> f,
             Func<TSource, TMiddle, TResult> resultSelector)
            => (Either<T1, T2, T3, T4, TResult>)source.Bind(x => (IMonad<TResult>)f(x).Map(y => resultSelector(x, y)));

        /// <summary>
        /// Equivalent to <see cref="IMonad{TSource}"/>, but restricted to <see cref="Either{T1, T2, T3, T4, T5, T6}"/>. Offers LINQ query support with multiple <c>from</c>-clauses.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="T3">The type of the third value.</typeparam>
        /// <typeparam name="T4">The type of the fourth value.</typeparam>
        /// <typeparam name="T5">The type of the fifth value.</typeparam>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TMiddle">The type of the selector's result.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        /// <param name="resultSelector">The result-selector.</param>
        public static Either<T1, T2, T3, T4, T5, TResult> SelectMany<T1, T2, T3, T4, T5, TSource, TMiddle, TResult>
            (this Either<T1, T2, T3, T4, T5, TSource> source,
             Func<TSource, Either<T1, T2, T3, T4, T5, TMiddle>> f,
             Func<TSource, TMiddle, TResult> resultSelector)
            => (Either<T1, T2, T3, T4, T5, TResult>)source.Bind(x => (IMonad<TResult>)f(x).Map(y => resultSelector(x, y)));

        /// <summary>
        /// Equivalent to <see cref="IMonad{TSource}"/>, but restricted to <see cref="Either{T1, T2, T3, T4, T5, T6, T7}"/>. Offers LINQ query support with multiple <c>from</c>-clauses.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="T3">The type of the third value.</typeparam>
        /// <typeparam name="T4">The type of the fourth value.</typeparam>
        /// <typeparam name="T5">The type of the fifth value.</typeparam>
        /// <typeparam name="T6">The type of the sixth value.</typeparam>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TMiddle">The type of the selector's result.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        /// <param name="resultSelector">The result-selector.</param>
        public static Either<T1, T2, T3, T4, T5, T6, TResult> SelectMany<T1, T2, T3, T4, T5, T6, TSource, TMiddle, TResult>
            (this Either<T1, T2, T3, T4, T5, T6, TSource> source,
             Func<TSource, Either<T1, T2, T3, T4, T5, T6, TMiddle>> f,
             Func<TSource, TMiddle, TResult> resultSelector)
            => (Either<T1, T2, T3, T4, T5, T6, TResult>)source.Bind(x => (IMonad<TResult>)f(x).Map(y => resultSelector(x, y)));

        /// <summary>
        /// Equivalent to <see cref="IMonad{TSource}"/>, but restricted to <see cref="Either{T1, T2, T3, T4, T5, T6, T7, T8}"/>. Offers LINQ query support with multiple <c>from</c>-clauses.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="T3">The type of the third value.</typeparam>
        /// <typeparam name="T4">The type of the fourth value.</typeparam>
        /// <typeparam name="T5">The type of the fifth value.</typeparam>
        /// <typeparam name="T6">The type of the sixth value.</typeparam>
        /// <typeparam name="T7">The type of the seventh value.</typeparam>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TMiddle">The type of the selector's result.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        /// <param name="resultSelector">The result-selector.</param>
        public static Either<T1, T2, T3, T4, T5, T6, T7, TResult> SelectMany<T1, T2, T3, T4, T5, T6, T7, TSource, TMiddle, TResult>
            (this Either<T1, T2, T3, T4, T5, T6, T7, TSource> source,
             Func<TSource, Either<T1, T2, T3, T4, T5, T6, T7, TMiddle>> f,
             Func<TSource, TMiddle, TResult> resultSelector)
            => (Either<T1, T2, T3, T4, T5, T6, T7, TResult>)source.Bind(x => (IMonad<TResult>)f(x).Map(y => resultSelector(x, y)));

        /// <summary>
        /// Equivalent to <see cref="IFunctor{TSource}.Map{TResult}(Func{TSource, TResult})"/>, but restricted to asynchronous <see cref="Either{TLeft, TRight}"/>. Offers LINQ query support with one <c>from</c>-clause.
        /// </summary>
        /// <typeparam name="TLeft">The type of the either's left-value.</typeparam>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        public static async Task<Either<TLeft, TResult>> Select<TLeft, TSource, TResult>(
            this Task<Either<TLeft, TSource>> source, Func<TSource, TResult> f)
            => Select(await source, f);

        /// <summary>
        /// Equivalent to <see cref="IFunctor{TSource}.Map{TResult}(Func{TSource, TResult})"/>, but restricted to asynchronous <see cref="Either{T1, T2, T3}"/>. Offers LINQ query support with one <c>from</c>-clause.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        public static async Task<Either<T1, T2, TResult>> Select<T1, T2, TSource, TResult>(
            this Task<Either<T1, T2, TSource>> source, Func<TSource, TResult> f)
            => Select(await source, f);

        /// <summary>
        /// Equivalent to <see cref="IFunctor{TSource}.Map{TResult}(Func{TSource, TResult})"/>, but restricted to asynchronous <see cref="Either{T1, T2, T3, T4}"/>. Offers LINQ query support with one <c>from</c>-clause.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="T3">The type of the third value.</typeparam>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        public static async Task<Either<T1, T2, T3, TResult>> Select<T1, T2, T3, TSource, TResult>(
            this Task<Either<T1, T2, T3, TSource>> source, Func<TSource, TResult> f)
            => Select(await source, f);

        /// <summary>
        /// Equivalent to <see cref="IFunctor{TSource}.Map{TResult}(Func{TSource, TResult})"/>, but restricted to asynchronous <see cref="Either{T1, T2, T3, T4, T5}"/>. Offers LINQ query support with one <c>from</c>-clause.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="T3">The type of the third value.</typeparam>
        /// <typeparam name="T4">The type of the fourth value.</typeparam>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        public static async Task<Either<T1, T2, T3, T4, TResult>> Select<T1, T2, T3, T4, TSource, TResult>(
            this Task<Either<T1, T2, T3, T4, TSource>> source, Func<TSource, TResult> f)
            => Select(await source, f);

        /// <summary>
        /// Equivalent to <see cref="IFunctor{TSource}.Map{TResult}(Func{TSource, TResult})"/>, but restricted to asynchronous <see cref="Either{T1, T2, T3, T4, T5, T6}"/>. Offers LINQ query support with one <c>from</c>-clause.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="T3">The type of the third value.</typeparam>
        /// <typeparam name="T4">The type of the fourth value.</typeparam>
        /// <typeparam name="T5">The type of the fifth value.</typeparam>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        public static async Task<Either<T1, T2, T3, T4, T5, TResult>> Select<T1, T2, T3, T4, T5, TSource, TResult>(
            this Task<Either<T1, T2, T3, T4, T5, TSource>> source, Func<TSource, TResult> f)
            => Select(await source, f);

        /// <summary>
        /// Equivalent to <see cref="IFunctor{TSource}.Map{TResult}(Func{TSource, TResult})"/>, but restricted to asynchronous <see cref="Either{T1, T2, T3, T4, T5, T6, T7}"/>. Offers LINQ query support with one <c>from</c>-clause.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="T3">The type of the third value.</typeparam>
        /// <typeparam name="T4">The type of the fourth value.</typeparam>
        /// <typeparam name="T5">The type of the fifth value.</typeparam>
        /// <typeparam name="T6">The type of the sixth value.</typeparam>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        public static async Task<Either<T1, T2, T3, T4, T5, T6, TResult>> Select<T1, T2, T3, T4, T5, T6, TSource, TResult>(
            this Task<Either<T1, T2, T3, T4, T5, T6, TSource>> source, Func<TSource, TResult> f)
            => Select(await source, f);

        /// <summary>
        /// Equivalent to <see cref="IFunctor{TSource}.Map{TResult}(Func{TSource, TResult})"/>, but restricted to asynchronous <see cref="Either{T1, T2, T3, T4, T5, T6, T7, T8}"/>. Offers LINQ query support with one <c>from</c>-clause.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="T3">The type of the third value.</typeparam>
        /// <typeparam name="T4">The type of the fourth value.</typeparam>
        /// <typeparam name="T5">The type of the fifth value.</typeparam>
        /// <typeparam name="T6">The type of the sixth value.</typeparam>
        /// <typeparam name="T7">The type of the seventh value.</typeparam>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        public static async Task<Either<T1, T2, T3, T4, T5, T6, T7, TResult>> Select<T1, T2, T3, T4, T5, T6, T7, TSource, TResult>(
            this Task<Either<T1, T2, T3, T4, T5, T6, T7, TSource>> source, Func<TSource, TResult> f)
            => Select(await source, f);

        /// <summary>
        /// Equivalent to <see cref="IMonad{TSource}"/>, but restricted to asynchronous <see cref="Either{TLeft, TRight}"/>. Offers LINQ query support with multiple <c>from</c>-clauses.
        /// </summary>
        /// <typeparam name="TLeft">The type of the either's left-value.</typeparam>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TMiddle">The type of the selector's result.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        /// <param name="resultSelector">The result-selector.</param>
        public static async Task<Either<TLeft, TResult>> SelectMany<TLeft, TSource, TMiddle, TResult>
            (this Task<Either<TLeft, TSource>> source,
             Func<TSource, Task<Either<TLeft, TMiddle>>> f,
             Func<TSource, TMiddle, TResult> resultSelector)
            => (Either<TLeft, TResult>)(await (await source).BindAsync(async x => (IAsyncMonad<TResult>)(await f(x)).Map(y => resultSelector(x, y))));

        /// <summary>
        /// Equivalent to <see cref="IMonad{TSource}"/>, but restricted to asynchronous <see cref="Either{T1, T2, T3}"/>. Offers LINQ query support with multiple <c>from</c>-clauses.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TMiddle">The type of the selector's result.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        /// <param name="resultSelector">The result-selector.</param>
        public static async Task<Either<T1, T2, TResult>> SelectMany<T1, T2, TSource, TMiddle, TResult>
            (this Task<Either<T1, T2, TSource>> source,
             Func<TSource, Task<Either<T1, T2, TMiddle>>> f,
             Func<TSource, TMiddle, TResult> resultSelector)
            => (Either<T1, T2, TResult>)(await (await source).BindAsync(async x => (IAsyncMonad<TResult>)(await f(x)).Map(y => resultSelector(x, y))));

        /// <summary>
        /// Equivalent to <see cref="IMonad{TSource}"/>, but restricted to asynchronous <see cref="Either{T1, T2, T3, T4}"/>. Offers LINQ query support with multiple <c>from</c>-clauses.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="T3">The type of the third value.</typeparam>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TMiddle">The type of the selector's result.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        /// <param name="resultSelector">The result-selector.</param>
        public static async Task<Either<T1, T2, T3, TResult>> SelectMany<T1, T2, T3, TSource, TMiddle, TResult>
            (this Task<Either<T1, T2, T3, TSource>> source,
             Func<TSource, Task<Either<T1, T2, T3, TMiddle>>> f,
             Func<TSource, TMiddle, TResult> resultSelector)
            => (Either<T1, T2, T3, TResult>)(await (await source).BindAsync(async x => (IAsyncMonad<TResult>)(await f(x)).Map(y => resultSelector(x, y))));

        /// <summary>
        /// Equivalent to <see cref="IMonad{TSource}"/>, but restricted to asynchronous <see cref="Either{T1, T2, T3, T4, T5}"/>. Offers LINQ query support with multiple <c>from</c>-clauses.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="T3">The type of the third value.</typeparam>
        /// <typeparam name="T4">The type of the fourth value.</typeparam>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TMiddle">The type of the selector's result.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        /// <param name="resultSelector">The result-selector.</param>
        public static async Task<Either<T1, T2, T3, T4, TResult>> SelectMany<T1, T2, T3, T4, TSource, TMiddle, TResult>
            (this Task<Either<T1, T2, T3, T4, TSource>> source,
             Func<TSource, Task<Either<T1, T2, T3, T4, TMiddle>>> f,
             Func<TSource, TMiddle, TResult> resultSelector)
            => (Either<T1, T2, T3, T4, TResult>)(await (await source).BindAsync(async x => (IAsyncMonad<TResult>)(await f(x)).Map(y => resultSelector(x, y))));

        /// <summary>
        /// Equivalent to <see cref="IMonad{TSource}"/>, but restricted to asynchronous <see cref="Either{T1, T2, T3, T4, T5, T6}"/>. Offers LINQ query support with multiple <c>from</c>-clauses.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="T3">The type of the third value.</typeparam>
        /// <typeparam name="T4">The type of the fourth value.</typeparam>
        /// <typeparam name="T5">The type of the fifth value.</typeparam>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TMiddle">The type of the selector's result.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        /// <param name="resultSelector">The result-selector.</param>
        public static async Task<Either<T1, T2, T3, T4, T5, TResult>> SelectMany<T1, T2, T3, T4, T5, TSource, TMiddle, TResult>
            (this Task<Either<T1, T2, T3, T4, T5, TSource>> source,
             Func<TSource, Task<Either<T1, T2, T3, T4, T5, TMiddle>>> f,
             Func<TSource, TMiddle, TResult> resultSelector)
            => (Either<T1, T2, T3, T4, T5, TResult>)(await (await source).BindAsync(async x => (IAsyncMonad<TResult>)(await f(x)).Map(y => resultSelector(x, y))));

        /// <summary>
        /// Equivalent to <see cref="IMonad{TSource}"/>, but restricted to asynchronous <see cref="Either{T1, T2, T3, T4, T5, T6, T7}"/>. Offers LINQ query support with multiple <c>from</c>-clauses.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="T3">The type of the third value.</typeparam>
        /// <typeparam name="T4">The type of the fourth value.</typeparam>
        /// <typeparam name="T5">The type of the fifth value.</typeparam>
        /// <typeparam name="T6">The type of the sixth value.</typeparam>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TMiddle">The type of the selector's result.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        /// <param name="resultSelector">The result-selector.</param>
        public static async Task<Either<T1, T2, T3, T4, T5, T6, TResult>> SelectMany<T1, T2, T3, T4, T5, T6, TSource, TMiddle, TResult>
            (this Task<Either<T1, T2, T3, T4, T5, T6, TSource>> source,
             Func<TSource, Task<Either<T1, T2, T3, T4, T5, T6, TMiddle>>> f,
             Func<TSource, TMiddle, TResult> resultSelector)
            => (Either<T1, T2, T3, T4, T5, T6, TResult>)(await (await source).BindAsync(async x => (IAsyncMonad<TResult>)(await f(x)).Map(y => resultSelector(x, y))));

        /// <summary>
        /// Equivalent to <see cref="IMonad{TSource}"/>, but restricted to asynchronous <see cref="Either{T1, T2, T3, T4, T5, T6, T7, T8}"/>. Offers LINQ query support with multiple <c>from</c>-clauses.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="T3">The type of the third value.</typeparam>
        /// <typeparam name="T4">The type of the fourth value.</typeparam>
        /// <typeparam name="T5">The type of the fifth value.</typeparam>
        /// <typeparam name="T6">The type of the sixth value.</typeparam>
        /// <typeparam name="T7">The type of the seventh value.</typeparam>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TMiddle">The type of the selector's result.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        /// <param name="resultSelector">The result-selector.</param>
        public static async Task<Either<T1, T2, T3, T4, T5, T6, T7, TResult>> SelectMany<T1, T2, T3, T4, T5, T6, T7, TSource, TMiddle, TResult>
            (this Task<Either<T1, T2, T3, T4, T5, T6, T7, TSource>> source,
             Func<TSource, Task<Either<T1, T2, T3, T4, T5, T6, T7, TMiddle>>> f,
             Func<TSource, TMiddle, TResult> resultSelector)
            => (Either<T1, T2, T3, T4, T5, T6, T7, TResult>)(await (await source).BindAsync(async x => (IAsyncMonad<TResult>)(await f(x)).Map(y => resultSelector(x, y))));

        /// <summary>
        /// Creates a left-either from a value.
        /// </summary>
        /// <typeparam name="TLeft">The type of the left-value.</typeparam>
        /// <typeparam name="TRight">The type of the right-value.</typeparam>
        /// <param name="value">The value to store in the either.</param>
        public static Either<TLeft, TRight> FromLeft<TLeft, TRight>(TLeft value) => Either<TLeft, TRight>.FromLeft(value);

        /// <summary>
        /// Creates a right-either from a value.
        /// </summary>
        /// <typeparam name="TLeft">The type of the left-value.</typeparam>
        /// <typeparam name="TRight">The type of the right-value.</typeparam>
        /// <param name="value">The value to store in the either.</param>
        public static Either<TLeft, TRight> FromRight<TLeft, TRight>(TRight value) => Either<TLeft, TRight>.FromRight(value);

        /// <summary>
        /// Creates a right-either from a value.
        /// This is a convenience-method which does not require explicitly specifying the type arguments.
        /// </summary>
        /// <typeparam name="TLeft">The type of the left-value.</typeparam>
        /// <typeparam name="TRight">The type of the right-value.</typeparam>
        /// <param name="_cxt">The context to fix the type variables.</param>
        /// <param name="value">The value to store in the either.</param>
        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "type tag")]
        public static Either<TLeft, TRight> FromRight<TLeft, TRight>(this EitherCxt<TLeft> _cxt, TRight value) => Either<TLeft, TRight>.FromRight(value);

        /// <summary>
        /// Creates a right-either from a value.
        /// </summary>
        /// <typeparam name="TLeft">The type of the left-value.</typeparam>
        /// <typeparam name="TRight">The type of the right-value.</typeparam>
        /// <param name="isRight">If true, an <see cref="Either.FromRight{TLeft, TRight}(TRight)"/> will be created, and if false, an <see cref="Either.FromLeft{TLeft, TRight}(TLeft)"/>.</param>
        /// <param name="leftFactory">The left-value to store in the either.</param>
        /// <param name="rightFactory">The right-value to store in the either.</param>
        public static Either<TLeft, TRight> EitherIf<TLeft, TRight>(
            bool isRight,
            Func<TLeft> leftFactory,
            Func<TRight> rightFactory) => isRight ?
                Either<TLeft, TRight>.FromRight(rightFactory())
                : Either<TLeft, TRight>.FromLeft(leftFactory());

        public static Either1<T> One<T>(T x) => new Either1<T>(x);
        public static Either2<T> Two<T>(T x) => new Either2<T>(x);
        public static Either3<T> Three<T>(T x) => new Either3<T>(x);
        public static Either4<T> Four<T>(T x) => new Either4<T>(x);
        public static Either5<T> Five<T>(T x) => new Either5<T>(x);
        public static Either6<T> Six<T>(T x) => new Either6<T>(x);
        public static Either7<T> Seven<T>(T x) => new Either7<T>(x);
        public static Either8<T> Eight<T>(T x) => new Either8<T>(x);


        #region First
        public static Identity<T1> EitherWith<T1>(Either1<T1> x)
            => new Identity<T1>(x.Value);

        public static Either<T1, T2> EitherWith<T1, T2>(Either1<T1> x)
            => new Either<T1, T2>(x.Value, TagLeft.Value);

        public static Either<T1, T2, T3> EitherWith<T1, T2, T3>(Either1<T1> x)
            => new Either<T1, T2, T3>(x);

        public static Either<T1, T2, T3, T4> EitherWith<T1, T2, T3, T4>(Either1<T1> x)
            => new Either<T1, T2, T3, T4>(x);

        public static Either<T1, T2, T3, T4, T5> EitherWith<T1, T2, T3, T4, T5>(Either1<T1> x)
            => new Either<T1, T2, T3, T4, T5>(x);

        public static Either<T1, T2, T3, T4, T5, T6> EitherWith<T1, T2, T3, T4, T5, T6>(Either1<T1> x)
            => new Either<T1, T2, T3, T4, T5, T6>(x);

        public static Either<T1, T2, T3, T4, T5, T6, T7> EitherWith<T1, T2, T3, T4, T5, T6, T7>(Either1<T1> x)
            => new Either<T1, T2, T3, T4, T5, T6, T7>(x);

        public static Either<T1, T2, T3, T4, T5, T6, T7, T8> EitherWith<T1, T2, T3, T4, T5, T6, T7, T8>(Either1<T1> x)
            => new Either<T1, T2, T3, T4, T5, T6, T7, T8>(x);
        #endregion

        #region Second
        public static Either<T1, T2> EitherWith<T1, T2>(Either2<T2> x)
            => new Either<T1, T2>(x.Value, TagRight.Value);

        public static Either<T1, T2, T3> EitherWith<T1, T2, T3>(Either2<T2> x)
            => new Either<T1, T2, T3>(x);

        public static Either<T1, T2, T3, T4> EitherWith<T1, T2, T3, T4>(Either2<T2> x)
            => new Either<T1, T2, T3, T4>(x);

        public static Either<T1, T2, T3, T4, T5> EitherWith<T1, T2, T3, T4, T5>(Either2<T2> x)
            => new Either<T1, T2, T3, T4, T5>(x);

        public static Either<T1, T2, T3, T4, T5, T6> EitherWith<T1, T2, T3, T4, T5, T6>(Either2<T2> x)
            => new Either<T1, T2, T3, T4, T5, T6>(x);

        public static Either<T1, T2, T3, T4, T5, T6, T7> EitherWith<T1, T2, T3, T4, T5, T6, T7>(Either2<T2> x)
            => new Either<T1, T2, T3, T4, T5, T6, T7>(x);

        public static Either<T1, T2, T3, T4, T5, T6, T7, T8> EitherWith<T1, T2, T3, T4, T5, T6, T7, T8>(Either2<T2> x)
            => new Either<T1, T2, T3, T4, T5, T6, T7, T8>(x);
        #endregion

        #region Third
        public static Either<T1, T2, T3> EitherWith<T1, T2, T3>(Either3<T3> x)
            => new Either<T1, T2, T3>(x);

        public static Either<T1, T2, T3, T4> EitherWith<T1, T2, T3, T4>(Either3<T3> x)
            => new Either<T1, T2, T3, T4>(x);

        public static Either<T1, T2, T3, T4, T5> EitherWith<T1, T2, T3, T4, T5>(Either3<T3> x)
            => new Either<T1, T2, T3, T4, T5>(x);

        public static Either<T1, T2, T3, T4, T5, T6> EitherWith<T1, T2, T3, T4, T5, T6>(Either3<T3> x)
            => new Either<T1, T2, T3, T4, T5, T6>(x);

        public static Either<T1, T2, T3, T4, T5, T6, T7> EitherWith<T1, T2, T3, T4, T5, T6, T7>(Either3<T3> x)
            => new Either<T1, T2, T3, T4, T5, T6, T7>(x);

        public static Either<T1, T2, T3, T4, T5, T6, T7, T8> EitherWith<T1, T2, T3, T4, T5, T6, T7, T8>(Either3<T3> x)
            => new Either<T1, T2, T3, T4, T5, T6, T7, T8>(x);
        #endregion

        #region Fourth
        public static Either<T1, T2, T3, T4> EitherWith<T1, T2, T3, T4>(Either4<T4> x)
            => new Either<T1, T2, T3, T4>(x);

        public static Either<T1, T2, T3, T4, T5> EitherWith<T1, T2, T3, T4, T5>(Either4<T4> x)
            => new Either<T1, T2, T3, T4, T5>(x);

        public static Either<T1, T2, T3, T4, T5, T6> EitherWith<T1, T2, T3, T4, T5, T6>(Either4<T4> x)
            => new Either<T1, T2, T3, T4, T5, T6>(x);

        public static Either<T1, T2, T3, T4, T5, T6, T7> EitherWith<T1, T2, T3, T4, T5, T6, T7>(Either4<T4> x)
            => new Either<T1, T2, T3, T4, T5, T6, T7>(x);

        public static Either<T1, T2, T3, T4, T5, T6, T7, T8> EitherWith<T1, T2, T3, T4, T5, T6, T7, T8>(Either4<T4> x)
            => new Either<T1, T2, T3, T4, T5, T6, T7, T8>(x);
        #endregion

        #region Fifth
        public static Either<T1, T2, T3, T4, T5> EitherWith<T1, T2, T3, T4, T5>(Either5<T5> x)
            => new Either<T1, T2, T3, T4, T5>(x);

        public static Either<T1, T2, T3, T4, T5, T6> EitherWith<T1, T2, T3, T4, T5, T6>(Either5<T5> x)
            => new Either<T1, T2, T3, T4, T5, T6>(x);

        public static Either<T1, T2, T3, T4, T5, T6, T7> EitherWith<T1, T2, T3, T4, T5, T6, T7>(Either5<T5> x)
            => new Either<T1, T2, T3, T4, T5, T6, T7>(x);

        public static Either<T1, T2, T3, T4, T5, T6, T7, T8> EitherWith<T1, T2, T3, T4, T5, T6, T7, T8>(Either5<T5> x)
            => new Either<T1, T2, T3, T4, T5, T6, T7, T8>(x);
        #endregion

        #region Sixth
        public static Either<T1, T2, T3, T4, T5, T6> EitherWith<T1, T2, T3, T4, T5, T6>(Either6<T6> x)
            => new Either<T1, T2, T3, T4, T5, T6>(x);

        public static Either<T1, T2, T3, T4, T5, T6, T7> EitherWith<T1, T2, T3, T4, T5, T6, T7>(Either6<T6> x)
            => new Either<T1, T2, T3, T4, T5, T6, T7>(x);

        public static Either<T1, T2, T3, T4, T5, T6, T7, T8> EitherWith<T1, T2, T3, T4, T5, T6, T7, T8>(Either6<T6> x)
            => new Either<T1, T2, T3, T4, T5, T6, T7, T8>(x);
        #endregion

        #region Seventh
        public static Either<T1, T2, T3, T4, T5, T6, T7> EitherWith<T1, T2, T3, T4, T5, T6, T7>(Either7<T7> x)
            => new Either<T1, T2, T3, T4, T5, T6, T7>(x);

        public static Either<T1, T2, T3, T4, T5, T6, T7, T8> EitherWith<T1, T2, T3, T4, T5, T6, T7, T8>(Either7<T7> x)
            => new Either<T1, T2, T3, T4, T5, T6, T7, T8>(x);
        #endregion

        #region Eigth
        public static Either<T1, T2, T3, T4, T5, T6, T7, T8> EitherWith<T1, T2, T3, T4, T5, T6, T7, T8>(Either8<T8> x)
            => new Either<T1, T2, T3, T4, T5, T6, T7, T8>(x);
        #endregion

        /// <summary>
        /// Tries to cast a generic bifunctor to an either via an explicit cast. Provided for convenience.
        /// </summary>
        /// <typeparam name="TLeft">The type of the left-value.</typeparam>
        /// <typeparam name="TRight">The type of the right-value.</typeparam>
        /// <param name="f">The bifunctor.</param>
        public static Either<TLeft, TRight> ToEither<TLeft, TRight>(this IBifunctor<TLeft, TRight> f) => (Either<TLeft, TRight>)f;
    }
}

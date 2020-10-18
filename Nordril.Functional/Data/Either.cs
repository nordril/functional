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

        /// <summary>
        /// Coalesces an <see cref="Either{T1, T2, T3}"/> into a single value.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="T3">The type of the third value.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="coproduct">The Either to coalesce.</param>
        /// <param name="one">The function to apply if the Either contains a first value.</param>
        /// <param name="two">The function to apply if the Either contains a second value.</param>
        /// <param name="three">The function to apply if the Either contains a third value.</param>
        public static TResult Coalesce<T1, T2, T3, TResult>(
            this Either<T1, T2, T3> coproduct,
            Func<T1, TResult> one,
            Func<T2, TResult> two,
            Func<T3, TResult> three)
        {
            return coproduct.First.ValueOrLazy(one,
                () => coproduct.Second.ValueOrLazy(two,
                () => three(coproduct.Third.Value())));
        }

        /// <summary>
        /// Coalesces an <see cref="Either{T1, T2, T3, T4}"/> into a single value.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="T3">The type of the third value.</typeparam>
        /// <typeparam name="T4">The type of the fourth value.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="coproduct">The Either to coalesce.</param>
        /// <param name="one">The function to apply if the Either contains a first value.</param>
        /// <param name="two">The function to apply if the Either contains a second value.</param>
        /// <param name="three">The function to apply if the Either contains a third value.</param>
        /// <param name="four">The function to apply if the Either contains a fourth value.</param>
        public static TResult Coalesce<T1, T2, T3, T4, TResult>(
            this Either<T1, T2, T3, T4> coproduct,
            Func<T1, TResult> one,
            Func<T2, TResult> two,
            Func<T3, TResult> three,
            Func<T4, TResult> four)
        {
            return coproduct.First.ValueOrLazy(one,
                () => coproduct.Second.ValueOrLazy(two,
                () => coproduct.Third.ValueOrLazy(three,
                () => four(coproduct.Fourth.Value()))));
        }

        /// <summary>
        /// Coalesces an <see cref="Either{T1, T2, T3, T4, T5}"/> into a single value.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="T3">The type of the third value.</typeparam>
        /// <typeparam name="T4">The type of the fourth value.</typeparam>
        /// <typeparam name="T5">The type of the fifth value.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="coproduct">The Either to coalesce.</param>
        /// <param name="one">The function to apply if the Either contains a first value.</param>
        /// <param name="two">The function to apply if the Either contains a second value.</param>
        /// <param name="three">The function to apply if the Either contains a third value.</param>
        /// <param name="four">The function to apply if the Either contains a fourth value.</param>
        /// <param name="five">The function to apply if the Either contains a fifth value.</param>
        public static TResult Coalesce<T1, T2, T3, T4, T5, TResult>(
            this Either<T1, T2, T3, T4, T5> coproduct,
            Func<T1, TResult> one,
            Func<T2, TResult> two,
            Func<T3, TResult> three,
            Func<T4, TResult> four,
            Func<T5, TResult> five)
        {
            return coproduct.First.ValueOrLazy(one,
                () => coproduct.Second.ValueOrLazy(two,
                () => coproduct.Third.ValueOrLazy(three,
                () => coproduct.Fourth.ValueOrLazy(four,
                () => five(coproduct.Fifth.Value())))));
        }

        /// <summary>
        /// Coalesces an <see cref="Either{T1, T2, T3, T4, T5, T6}"/> into a single value.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="T3">The type of the third value.</typeparam>
        /// <typeparam name="T4">The type of the fourth value.</typeparam>
        /// <typeparam name="T5">The type of the fifth value.</typeparam>
        /// <typeparam name="T6">The type of the sixth value.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="coproduct">The Either to coalesce.</param>
        /// <param name="one">The function to apply if the Either contains a first value.</param>
        /// <param name="two">The function to apply if the Either contains a second value.</param>
        /// <param name="three">The function to apply if the Either contains a third value.</param>
        /// <param name="four">The function to apply if the Either contains a fourth value.</param>
        /// <param name="five">The function to apply if the Either contains a fifth value.</param>
        /// <param name="six">The function to apply if the Either contains a sixth value.</param>
        public static TResult Coalesce<T1, T2, T3, T4, T5, T6, TResult>(
            this Either<T1, T2, T3, T4, T5, T6> coproduct,
            Func<T1, TResult> one,
            Func<T2, TResult> two,
            Func<T3, TResult> three,
            Func<T4, TResult> four,
            Func<T5, TResult> five,
            Func<T6, TResult> six)
        {
            return coproduct.First.ValueOrLazy(one,
                () => coproduct.Second.ValueOrLazy(two,
                () => coproduct.Third.ValueOrLazy(three,
                () => coproduct.Fourth.ValueOrLazy(four,
                () => coproduct.Fifth.ValueOrLazy(five,
                () => six(coproduct.Sixth.Value()))))));
        }

        /// <summary>
        /// Coalesces an <see cref="Either{T1, T2, T3, T4, T5, T6, T7}"/> into a single value.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="T3">The type of the third value.</typeparam>
        /// <typeparam name="T4">The type of the fourth value.</typeparam>
        /// <typeparam name="T5">The type of the fifth value.</typeparam>
        /// <typeparam name="T6">The type of the sixth value.</typeparam>
        /// <typeparam name="T7">The type of the seventh value.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="coproduct">The Either to coalesce.</param>
        /// <param name="one">The function to apply if the Either contains a first value.</param>
        /// <param name="two">The function to apply if the Either contains a second value.</param>
        /// <param name="three">The function to apply if the Either contains a third value.</param>
        /// <param name="four">The function to apply if the Either contains a fourth value.</param>
        /// <param name="five">The function to apply if the Either contains a fifth value.</param>
        /// <param name="six">The function to apply if the Either contains a sixth value.</param>
        /// <param name="seven">The function to apply if the Either contains a seventh value.</param>
        public static TResult Coalesce<T1, T2, T3, T4, T5, T6, T7, TResult>(
            this Either<T1, T2, T3, T4, T5, T6, T7> coproduct,
            Func<T1, TResult> one,
            Func<T2, TResult> two,
            Func<T3, TResult> three,
            Func<T4, TResult> four,
            Func<T5, TResult> five,
            Func<T6, TResult> six,
            Func<T7, TResult> seven)
        {
            return coproduct.First.ValueOrLazy(one,
                () => coproduct.Second.ValueOrLazy(two,
                () => coproduct.Third.ValueOrLazy(three,
                () => coproduct.Fourth.ValueOrLazy(four,
                () => coproduct.Fifth.ValueOrLazy(five,
                () => coproduct.Sixth.ValueOrLazy(six,
                () => seven(coproduct.Seventh.Value())))))));
        }

        /// <summary>
        /// Coalesces an <see cref="Either{T1, T2, T3, T4, T5, T6, T7, T8}"/> into a single value.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="T3">The type of the third value.</typeparam>
        /// <typeparam name="T4">The type of the fourth value.</typeparam>
        /// <typeparam name="T5">The type of the fifth value.</typeparam>
        /// <typeparam name="T6">The type of the sixth value.</typeparam>
        /// <typeparam name="T7">The type of the seventh value.</typeparam>
        /// <typeparam name="T8">The type of the eigthvalue.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="coproduct">The Either to coalesce.</param>
        /// <param name="one">The function to apply if the Either contains a first value.</param>
        /// <param name="two">The function to apply if the Either contains a second value.</param>
        /// <param name="three">The function to apply if the Either contains a third value.</param>
        /// <param name="four">The function to apply if the Either contains a fourth value.</param>
        /// <param name="five">The function to apply if the Either contains a fifth value.</param>
        /// <param name="six">The function to apply if the Either contains a sixth value.</param>
        /// <param name="seven">The function to apply if the Either contains a seventh value.</param>
        /// <param name="eight">The function to apply if the Either contains an eigth value.</param>
        public static TResult Coalesce<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(
            this Either<T1, T2, T3, T4, T5, T6, T7, T8> coproduct,
            Func<T1, TResult> one,
            Func<T2, TResult> two,
            Func<T3, TResult> three,
            Func<T4, TResult> four,
            Func<T5, TResult> five,
            Func<T6, TResult> six,
            Func<T7, TResult> seven,
            Func<T8, TResult> eight)
        {
            return coproduct.First.ValueOrLazy(one,
                () => coproduct.Second.ValueOrLazy(two,
                () => coproduct.Third.ValueOrLazy(three,
                () => coproduct.Fourth.ValueOrLazy(four,
                () => coproduct.Fifth.ValueOrLazy(five,
                () => coproduct.Sixth.ValueOrLazy(six,
                () => coproduct.Seventh.ValueOrLazy(seven,
                () => eight(coproduct.Eigth.Value()))))))));
        }

        #region Single constructors
        /// <summary>
        /// Creates the <see cref="Either1{T1}"/>-consturctor. See the <see cref="EitherWith{T1, T2, T3, T4, T5, T6, T7, T8}(Either1{T1})"/>-family of functions.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="x">The value to wrap.</param>
        public static Either1<T> One<T>(T x) => new Either1<T>(x);

        /// <summary>
        /// Creates the <see cref="Either2{T2}"/>-consturctor. See the <see cref="EitherWith{T1, T2, T3, T4, T5, T6, T7, T8}(Either2{T2})"/>-family of functions.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="x">The value to wrap.</param>
        public static Either2<T> Two<T>(T x) => new Either2<T>(x);

        /// <summary>
        /// Creates the <see cref="Either1{T3}"/>-consturctor. See the <see cref="EitherWith{T1, T2, T3, T4, T5, T6, T7, T8}(Either3{T3})"/>-family of functions.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="x">The value to wrap.</param>
        public static Either3<T> Three<T>(T x) => new Either3<T>(x);

        /// <summary>
        /// Creates the <see cref="Either1{T4}"/>-consturctor. See the <see cref="EitherWith{T1, T2, T3, T4, T5, T6, T7, T8}(Either4{T4})"/>-family of functions.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="x">The value to wrap.</param>
        public static Either4<T> Four<T>(T x) => new Either4<T>(x);

        /// <summary>
        /// Creates the <see cref="Either5{T5}"/>-consturctor. See the <see cref="EitherWith{T1, T2, T3, T4, T5, T6, T7, T8}(Either5{T5})"/>-family of functions.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="x">The value to wrap.</param>
        public static Either5<T> Five<T>(T x) => new Either5<T>(x);

        /// <summary>
        /// Creates the <see cref="Either6{T6}"/>-consturctor. See the <see cref="EitherWith{T1, T2, T3, T4, T5, T6, T7, T8}(Either6{T6})"/>-family of functions.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="x">The value to wrap.</param>
        public static Either6<T> Six<T>(T x) => new Either6<T>(x);

        /// <summary>
        /// Creates the <see cref="Either7{T7}"/>-consturctor. See the <see cref="EitherWith{T1, T2, T3, T4, T5, T6, T7, T8}(Either7{T7})"/>-family of functions.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="x">The value to wrap.</param>
        public static Either7<T> Seven<T>(T x) => new Either7<T>(x);

        /// <summary>
        /// Creates the <see cref="Either8{T8}"/>-consturctor. See the <see cref="EitherWith{T1, T2, T3, T4, T5, T6, T7, T8}(Either8{T8})"/>-family of functions.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="x">The value to wrap.</param>
        public static Either8<T> Eight<T>(T x) => new Either8<T>(x);
        #endregion

        #region First
        /// <summary>
        /// Creates a 1-element coproduct based on the <see cref="Either1{T1}"/>-constructor.
        /// </summary>
        /// <typeparam name="T1">The type of the value.</typeparam>
        /// <param name="x">The value to wrap.</param>
        public static Identity<T1> EitherWith<T1>(Either1<T1> x)
            => new Identity<T1>(x.Value);

        /// <summary>
        /// Creates an <see cref="Either{TLeft, TRight}"/> containing the first value.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <param name="x">The value to wrap.</param>
        public static Either<T1, T2> EitherWith<T1, T2>(Either1<T1> x)
            => new Either<T1, T2>(x.Value, TagLeft.Value);

        /// <summary>
        /// Creates an <see cref="Either{T1, T2, T3}"/> containing the first value.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="T3">The type of the third value.</typeparam>
        /// <param name="x">The value to wrap.</param>
        public static Either<T1, T2, T3> EitherWith<T1, T2, T3>(Either1<T1> x)
            => new Either<T1, T2, T3>(x);

        /// <summary>
        /// Creates an <see cref="Either{T1, T2, T3, T4}"/> containing the first value.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="T3">The type of the third value.</typeparam>
        /// <typeparam name="T4">The type of the fourth value.</typeparam>
        /// <param name="x">The value to wrap.</param>
        public static Either<T1, T2, T3, T4> EitherWith<T1, T2, T3, T4>(Either1<T1> x)
            => new Either<T1, T2, T3, T4>(x);

        /// <summary>
        /// Creates an <see cref="Either{T1, T2, T3, T4, T5}"/> containing the first value.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="T3">The type of the third value.</typeparam>
        /// <typeparam name="T4">The type of the fourth value.</typeparam>
        /// <typeparam name="T5">The type of the fifth value.</typeparam>
        /// <param name="x">The value to wrap.</param>
        public static Either<T1, T2, T3, T4, T5> EitherWith<T1, T2, T3, T4, T5>(Either1<T1> x)
            => new Either<T1, T2, T3, T4, T5>(x);

        /// <summary>
        /// Creates an <see cref="Either{T1, T2, T3, T4, T5, T6}"/> containing the first value.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="T3">The type of the third value.</typeparam>
        /// <typeparam name="T4">The type of the fourth value.</typeparam>
        /// <typeparam name="T5">The type of the fifth value.</typeparam>
        /// <typeparam name="T6">The type of the sixth value.</typeparam>
        /// <param name="x">The value to wrap.</param>
        public static Either<T1, T2, T3, T4, T5, T6> EitherWith<T1, T2, T3, T4, T5, T6>(Either1<T1> x)
            => new Either<T1, T2, T3, T4, T5, T6>(x);

        /// <summary>
        /// Creates an <see cref="Either{T1, T2, T3, T4, T5, T6, T7}"/> containing the first value.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="T3">The type of the third value.</typeparam>
        /// <typeparam name="T4">The type of the fourth value.</typeparam>
        /// <typeparam name="T5">The type of the fifth value.</typeparam>
        /// <typeparam name="T6">The type of the sixth value.</typeparam>
        /// <typeparam name="T7">The type of the seventh value.</typeparam>
        /// <param name="x">The value to wrap.</param>
        public static Either<T1, T2, T3, T4, T5, T6, T7> EitherWith<T1, T2, T3, T4, T5, T6, T7>(Either1<T1> x)
            => new Either<T1, T2, T3, T4, T5, T6, T7>(x);

        /// <summary>
        /// Creates an <see cref="Either{T1, T2, T3, T4, T5, T6, T7, T8}"/> containing the first value.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="T3">The type of the third value.</typeparam>
        /// <typeparam name="T4">The type of the fourth value.</typeparam>
        /// <typeparam name="T5">The type of the fifth value.</typeparam>
        /// <typeparam name="T6">The type of the sixth value.</typeparam>
        /// <typeparam name="T7">The type of the seventh value.</typeparam>
        /// <typeparam name="T8">The type of the eigth value.</typeparam>
        /// <param name="x">The value to wrap.</param>
        public static Either<T1, T2, T3, T4, T5, T6, T7, T8> EitherWith<T1, T2, T3, T4, T5, T6, T7, T8>(Either1<T1> x)
            => new Either<T1, T2, T3, T4, T5, T6, T7, T8>(x);
        #endregion

        #region Second
        /// <summary>
        /// Creates an <see cref="Either{TLeft, TRight}"/> containing the second value.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <param name="x">The value to wrap.</param>
        public static Either<T1, T2> EitherWith<T1, T2>(Either2<T2> x)
            => new Either<T1, T2>(x.Value, TagRight.Value);

        /// <summary>
        /// Creates an <see cref="Either{T1, T2, T3}"/> containing the second value.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="T3">The type of the third value.</typeparam>
        /// <param name="x">The value to wrap.</param>
        public static Either<T1, T2, T3> EitherWith<T1, T2, T3>(Either2<T2> x)
            => new Either<T1, T2, T3>(x);

        /// <summary>
        /// Creates an <see cref="Either{T1, T2, T3, T4}"/> containing the second value.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="T3">The type of the third value.</typeparam>
        /// <typeparam name="T4">The type of the fourth value.</typeparam>
        /// <param name="x">The value to wrap.</param>
        public static Either<T1, T2, T3, T4> EitherWith<T1, T2, T3, T4>(Either2<T2> x)
            => new Either<T1, T2, T3, T4>(x);

        /// <summary>
        /// Creates an <see cref="Either{T1, T2, T3, T4, T5}"/> containing the second value.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="T3">The type of the third value.</typeparam>
        /// <typeparam name="T4">The type of the fourth value.</typeparam>
        /// <typeparam name="T5">The type of the fifth value.</typeparam>
        /// <param name="x">The value to wrap.</param>
        public static Either<T1, T2, T3, T4, T5> EitherWith<T1, T2, T3, T4, T5>(Either2<T2> x)
            => new Either<T1, T2, T3, T4, T5>(x);

        /// <summary>
        /// Creates an <see cref="Either{T1, T2, T3, T4, T5, T6}"/> containing the second value.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="T3">The type of the third value.</typeparam>
        /// <typeparam name="T4">The type of the fourth value.</typeparam>
        /// <typeparam name="T5">The type of the fifth value.</typeparam>
        /// <typeparam name="T6">The type of the sixth value.</typeparam>
        /// <param name="x">The value to wrap.</param>
        public static Either<T1, T2, T3, T4, T5, T6> EitherWith<T1, T2, T3, T4, T5, T6>(Either2<T2> x)
            => new Either<T1, T2, T3, T4, T5, T6>(x);

        /// <summary>
        /// Creates an <see cref="Either{T1, T2, T3, T4, T5, T6, T7}"/> containing the second value.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="T3">The type of the third value.</typeparam>
        /// <typeparam name="T4">The type of the fourth value.</typeparam>
        /// <typeparam name="T5">The type of the fifth value.</typeparam>
        /// <typeparam name="T6">The type of the sixth value.</typeparam>
        /// <typeparam name="T7">The type of the seventh value.</typeparam>
        /// <param name="x">The value to wrap.</param>
        public static Either<T1, T2, T3, T4, T5, T6, T7> EitherWith<T1, T2, T3, T4, T5, T6, T7>(Either2<T2> x)
            => new Either<T1, T2, T3, T4, T5, T6, T7>(x);

        /// <summary>
        /// Creates an <see cref="Either{T1, T2, T3, T4, T5, T6, T7, T8}"/> containing the second value.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="T3">The type of the third value.</typeparam>
        /// <typeparam name="T4">The type of the fourth value.</typeparam>
        /// <typeparam name="T5">The type of the fifth value.</typeparam>
        /// <typeparam name="T6">The type of the sixth value.</typeparam>
        /// <typeparam name="T7">The type of the seventh value.</typeparam>
        /// <typeparam name="T8">The type of the eigth value.</typeparam>
        /// <param name="x">The value to wrap.</param>
        public static Either<T1, T2, T3, T4, T5, T6, T7, T8> EitherWith<T1, T2, T3, T4, T5, T6, T7, T8>(Either2<T2> x)
            => new Either<T1, T2, T3, T4, T5, T6, T7, T8>(x);
        #endregion

        #region Third
        /// <summary>
        /// Creates an <see cref="Either{T1, T2, T3}"/> containing the third value.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="T3">The type of the third value.</typeparam>
        /// <param name="x">The value to wrap.</param>
        public static Either<T1, T2, T3> EitherWith<T1, T2, T3>(Either3<T3> x)
            => new Either<T1, T2, T3>(x);

        /// <summary>
        /// Creates an <see cref="Either{T1, T2, T3, T4}"/> containing the third value.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="T3">The type of the third value.</typeparam>
        /// <typeparam name="T4">The type of the fourth value.</typeparam>
        /// <param name="x">The value to wrap.</param>
        public static Either<T1, T2, T3, T4> EitherWith<T1, T2, T3, T4>(Either3<T3> x)
            => new Either<T1, T2, T3, T4>(x);

        /// <summary>
        /// Creates an <see cref="Either{T1, T2, T3, T4, T5}"/> containing the third value.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="T3">The type of the third value.</typeparam>
        /// <typeparam name="T4">The type of the fourth value.</typeparam>
        /// <typeparam name="T5">The type of the fifth value.</typeparam>
        /// <param name="x">The value to wrap.</param>
        public static Either<T1, T2, T3, T4, T5> EitherWith<T1, T2, T3, T4, T5>(Either3<T3> x)
            => new Either<T1, T2, T3, T4, T5>(x);

        /// <summary>
        /// Creates an <see cref="Either{T1, T2, T3, T4, T5, T6}"/> containing the third value.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="T3">The type of the third value.</typeparam>
        /// <typeparam name="T4">The type of the fourth value.</typeparam>
        /// <typeparam name="T5">The type of the fifth value.</typeparam>
        /// <typeparam name="T6">The type of the sixth value.</typeparam>
        /// <param name="x">The value to wrap.</param>
        public static Either<T1, T2, T3, T4, T5, T6> EitherWith<T1, T2, T3, T4, T5, T6>(Either3<T3> x)
            => new Either<T1, T2, T3, T4, T5, T6>(x);

        /// <summary>
        /// Creates an <see cref="Either{T1, T2, T3, T4, T5, T6, T7}"/> containing the third value.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="T3">The type of the third value.</typeparam>
        /// <typeparam name="T4">The type of the fourth value.</typeparam>
        /// <typeparam name="T5">The type of the fifth value.</typeparam>
        /// <typeparam name="T6">The type of the sixth value.</typeparam>
        /// <typeparam name="T7">The type of the seventh value.</typeparam>
        /// <param name="x">The value to wrap.</param>
        public static Either<T1, T2, T3, T4, T5, T6, T7> EitherWith<T1, T2, T3, T4, T5, T6, T7>(Either3<T3> x)
            => new Either<T1, T2, T3, T4, T5, T6, T7>(x);

        /// <summary>
        /// Creates an <see cref="Either{T1, T2, T3, T4, T5, T6, T7, T8}"/> containing the third value.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="T3">The type of the third value.</typeparam>
        /// <typeparam name="T4">The type of the fourth value.</typeparam>
        /// <typeparam name="T5">The type of the fifth value.</typeparam>
        /// <typeparam name="T6">The type of the sixth value.</typeparam>
        /// <typeparam name="T7">The type of the seventh value.</typeparam>
        /// <typeparam name="T8">The type of the eigth value.</typeparam>
        /// <param name="x">The value to wrap.</param>
        public static Either<T1, T2, T3, T4, T5, T6, T7, T8> EitherWith<T1, T2, T3, T4, T5, T6, T7, T8>(Either3<T3> x)
            => new Either<T1, T2, T3, T4, T5, T6, T7, T8>(x);
        #endregion

        #region Fourth
        /// <summary>
        /// Creates an <see cref="Either{T1, T2, T3, T4}"/> containing the fourth value.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="T3">The type of the third value.</typeparam>
        /// <typeparam name="T4">The type of the fourth value.</typeparam>
        /// <param name="x">The value to wrap.</param>
        public static Either<T1, T2, T3, T4> EitherWith<T1, T2, T3, T4>(Either4<T4> x)
            => new Either<T1, T2, T3, T4>(x);

        /// <summary>
        /// Creates an <see cref="Either{T1, T2, T3, T4, T5}"/> containing the fourth value.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="T3">The type of the third value.</typeparam>
        /// <typeparam name="T4">The type of the fourth value.</typeparam>
        /// <typeparam name="T5">The type of the fifth value.</typeparam>
        /// <param name="x">The value to wrap.</param>
        public static Either<T1, T2, T3, T4, T5> EitherWith<T1, T2, T3, T4, T5>(Either4<T4> x)
            => new Either<T1, T2, T3, T4, T5>(x);

        /// <summary>
        /// Creates an <see cref="Either{T1, T2, T3, T4, T5, T6}"/> containing the fourth value.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="T3">The type of the third value.</typeparam>
        /// <typeparam name="T4">The type of the fourth value.</typeparam>
        /// <typeparam name="T5">The type of the fifth value.</typeparam>
        /// <typeparam name="T6">The type of the sixth value.</typeparam>
        /// <param name="x">The value to wrap.</param>
        public static Either<T1, T2, T3, T4, T5, T6> EitherWith<T1, T2, T3, T4, T5, T6>(Either4<T4> x)
            => new Either<T1, T2, T3, T4, T5, T6>(x);

        /// <summary>
        /// Creates an <see cref="Either{T1, T2, T3, T4, T5, T6, T7}"/> containing the fourth value.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="T3">The type of the third value.</typeparam>
        /// <typeparam name="T4">The type of the fourth value.</typeparam>
        /// <typeparam name="T5">The type of the fifth value.</typeparam>
        /// <typeparam name="T6">The type of the sixth value.</typeparam>
        /// <typeparam name="T7">The type of the seventh value.</typeparam>
        /// <param name="x">The value to wrap.</param>
        public static Either<T1, T2, T3, T4, T5, T6, T7> EitherWith<T1, T2, T3, T4, T5, T6, T7>(Either4<T4> x)
            => new Either<T1, T2, T3, T4, T5, T6, T7>(x);

        /// <summary>
        /// Creates an <see cref="Either{T1, T2, T3, T4, T5, T6, T7, T8}"/> containing the fourth value.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="T3">The type of the third value.</typeparam>
        /// <typeparam name="T4">The type of the fourth value.</typeparam>
        /// <typeparam name="T5">The type of the fifth value.</typeparam>
        /// <typeparam name="T6">The type of the sixth value.</typeparam>
        /// <typeparam name="T7">The type of the seventh value.</typeparam>
        /// <typeparam name="T8">The type of the eigth value.</typeparam>
        /// <param name="x">The value to wrap.</param>
        public static Either<T1, T2, T3, T4, T5, T6, T7, T8> EitherWith<T1, T2, T3, T4, T5, T6, T7, T8>(Either4<T4> x)
            => new Either<T1, T2, T3, T4, T5, T6, T7, T8>(x);
        #endregion

        #region Fifth
        /// <summary>
        /// Creates an <see cref="Either{T1, T2, T3, T4, T5}"/> containing the fifth value.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="T3">The type of the third value.</typeparam>
        /// <typeparam name="T4">The type of the fourth value.</typeparam>
        /// <typeparam name="T5">The type of the fifth value.</typeparam>
        /// <param name="x">The value to wrap.</param>
        public static Either<T1, T2, T3, T4, T5> EitherWith<T1, T2, T3, T4, T5>(Either5<T5> x)
            => new Either<T1, T2, T3, T4, T5>(x);

        /// <summary>
        /// Creates an <see cref="Either{T1, T2, T3, T4, T5, T6}"/> containing the fifth value.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="T3">The type of the third value.</typeparam>
        /// <typeparam name="T4">The type of the fourth value.</typeparam>
        /// <typeparam name="T5">The type of the fifth value.</typeparam>
        /// <typeparam name="T6">The type of the sixth value.</typeparam>
        /// <param name="x">The value to wrap.</param>
        public static Either<T1, T2, T3, T4, T5, T6> EitherWith<T1, T2, T3, T4, T5, T6>(Either5<T5> x)
            => new Either<T1, T2, T3, T4, T5, T6>(x);

        /// <summary>
        /// Creates an <see cref="Either{T1, T2, T3, T4, T5, T6, T7}"/> containing the fifth value.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="T3">The type of the third value.</typeparam>
        /// <typeparam name="T4">The type of the fourth value.</typeparam>
        /// <typeparam name="T5">The type of the fifth value.</typeparam>
        /// <typeparam name="T6">The type of the sixth value.</typeparam>
        /// <typeparam name="T7">The type of the seventh value.</typeparam>
        /// <param name="x">The value to wrap.</param>
        public static Either<T1, T2, T3, T4, T5, T6, T7> EitherWith<T1, T2, T3, T4, T5, T6, T7>(Either5<T5> x)
            => new Either<T1, T2, T3, T4, T5, T6, T7>(x);

        /// <summary>
        /// Creates an <see cref="Either{T1, T2, T3, T4, T5, T6, T7, T8}"/> containing the fifth value.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="T3">The type of the third value.</typeparam>
        /// <typeparam name="T4">The type of the fourth value.</typeparam>
        /// <typeparam name="T5">The type of the fifth value.</typeparam>
        /// <typeparam name="T6">The type of the sixth value.</typeparam>
        /// <typeparam name="T7">The type of the seventh value.</typeparam>
        /// <typeparam name="T8">The type of the eigth value.</typeparam>
        /// <param name="x">The value to wrap.</param>
        public static Either<T1, T2, T3, T4, T5, T6, T7, T8> EitherWith<T1, T2, T3, T4, T5, T6, T7, T8>(Either5<T5> x)
            => new Either<T1, T2, T3, T4, T5, T6, T7, T8>(x);
        #endregion

        #region Sixth
        /// <summary>
        /// Creates an <see cref="Either{T1, T2, T3, T4, T5, T6}"/> containing the sixth value.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="T3">The type of the third value.</typeparam>
        /// <typeparam name="T4">The type of the fourth value.</typeparam>
        /// <typeparam name="T5">The type of the fifth value.</typeparam>
        /// <typeparam name="T6">The type of the sixth value.</typeparam>
        /// <param name="x">The value to wrap.</param>
        public static Either<T1, T2, T3, T4, T5, T6> EitherWith<T1, T2, T3, T4, T5, T6>(Either6<T6> x)
            => new Either<T1, T2, T3, T4, T5, T6>(x);

        /// <summary>
        /// Creates an <see cref="Either{T1, T2, T3, T4, T5, T6}"/> containing the sixth value.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="T3">The type of the third value.</typeparam>
        /// <typeparam name="T4">The type of the fourth value.</typeparam>
        /// <typeparam name="T5">The type of the fifth value.</typeparam>
        /// <typeparam name="T6">The type of the sixth value.</typeparam>
        /// <typeparam name="T7">The type of the seventh value.</typeparam>
        /// <param name="x">The value to wrap.</param>
        public static Either<T1, T2, T3, T4, T5, T6, T7> EitherWith<T1, T2, T3, T4, T5, T6, T7>(Either6<T6> x)
            => new Either<T1, T2, T3, T4, T5, T6, T7>(x);

        /// <summary>
        /// Creates an <see cref="Either{T1, T2, T3, T4, T5, T6, T7, T8}"/> containing the sixth value.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="T3">The type of the third value.</typeparam>
        /// <typeparam name="T4">The type of the fourth value.</typeparam>
        /// <typeparam name="T5">The type of the fifth value.</typeparam>
        /// <typeparam name="T6">The type of the sixth value.</typeparam>
        /// <typeparam name="T7">The type of the seventh value.</typeparam>
        /// <typeparam name="T8">The type of the eigth value.</typeparam>
        /// <param name="x">The value to wrap.</param>
        public static Either<T1, T2, T3, T4, T5, T6, T7, T8> EitherWith<T1, T2, T3, T4, T5, T6, T7, T8>(Either6<T6> x)
            => new Either<T1, T2, T3, T4, T5, T6, T7, T8>(x);
        #endregion

        #region Seventh
        /// <summary>
        /// Creates an <see cref="Either{T1, T2, T3, T4, T5, T6, T7}"/> containing the seventh value.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="T3">The type of the third value.</typeparam>
        /// <typeparam name="T4">The type of the fourth value.</typeparam>
        /// <typeparam name="T5">The type of the fifth value.</typeparam>
        /// <typeparam name="T6">The type of the sixth value.</typeparam>
        /// <typeparam name="T7">The type of the seventh value.</typeparam>
        /// <param name="x">The value to wrap.</param>
        public static Either<T1, T2, T3, T4, T5, T6, T7> EitherWith<T1, T2, T3, T4, T5, T6, T7>(Either7<T7> x)
            => new Either<T1, T2, T3, T4, T5, T6, T7>(x);

        /// <summary>
        /// Creates an <see cref="Either{T1, T2, T3, T4, T5, T6, T7, T8}"/> containing the seventh value.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="T3">The type of the third value.</typeparam>
        /// <typeparam name="T4">The type of the fourth value.</typeparam>
        /// <typeparam name="T5">The type of the fifth value.</typeparam>
        /// <typeparam name="T6">The type of the sixth value.</typeparam>
        /// <typeparam name="T7">The type of the seventh value.</typeparam>
        /// <typeparam name="T8">The type of the eigth value.</typeparam>
        /// <param name="x">The value to wrap.</param>
        public static Either<T1, T2, T3, T4, T5, T6, T7, T8> EitherWith<T1, T2, T3, T4, T5, T6, T7, T8>(Either7<T7> x)
            => new Either<T1, T2, T3, T4, T5, T6, T7, T8>(x);
        #endregion

        #region Eigth
        /// <summary>
        /// Creates an <see cref="Either{T1, T2, T3, T4, T5, T6, T7, T8}"/> containing the eigth value.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="T3">The type of the third value.</typeparam>
        /// <typeparam name="T4">The type of the fourth value.</typeparam>
        /// <typeparam name="T5">The type of the fifth value.</typeparam>
        /// <typeparam name="T6">The type of the sixth value.</typeparam>
        /// <typeparam name="T7">The type of the seventh value.</typeparam>
        /// <typeparam name="T8">The type of the eigth value.</typeparam>
        /// <param name="x">The value to wrap.</param>
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

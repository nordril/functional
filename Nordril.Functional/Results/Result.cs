using Nordril.Functional.Category;
using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace Nordril.Functional.Results
{
    /// <summary>
    /// The result of a service call; a container for <see cref="Either{TLeft, TRight}"/>, containing either a list of errors or a result <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the result, if the call was successful.</typeparam>
    [DebuggerDisplay("IsOk = {IsOk}")]
    public struct Result<T> : IEquatable<Result<T>>, IAsyncMonad<T>, IMonoFunctor<Result<T>, T>
    {
        /// <summary>
        /// The underlying either.
        /// </summary>
        public Either<IList<Error>, T> InnerResult { get; set; }

        /// <summary>
        /// Gets or sets the general class of result, which allows a rough categorization.
        /// </summary>
        public ResultClass ResultClass { get; set; }

        /// <summary>
        /// Returns true iff there is a result, i.e. if the underlying <see cref="Either{TLeft, TRight}.IsRight"/> is true.
        /// </summary>
        public bool IsOk => InnerResult.IsRight;

        /// <summary>
        /// Returns the value of the result, if present. If there's no result, a <see cref="PatternMatchException"/> is thrown.
        /// </summary>
        /// <exception cref="PatternMatchException">If there's no result and only errors.</exception>
        public T Value() => InnerResult.Right();

        /// <summary>
        /// Returns the errors, if present. If there are no errors, a <see cref="PatternMatchException"/> is thrown.
        /// </summary>
        /// <exception cref="PatternMatchException">If there are no errors.</exception>
        public IList<Error> Errors() => InnerResult.Left();

        /// <summary>
        /// Returns true if <see cref="IsOk"/> is false and if <see cref="Errors"/> contains an error with an inner exception of type <typeparamref name="TError"/>. This is a useful analogue for <c>catch</c>.
        /// </summary>
        /// <typeparam name="TError">The type of the exception to search for.</typeparam>
        /// <param name="error">The first occurrence of an <see cref="Error"/> which has an inner exception of type <typeparamref name="TError"/>.</param>
        public bool HasError<TError>(out Error error)
        {
            error = default;

            if (IsOk)
                return false;
            else
            {
                error = Errors()
                    .FirstMaybe(err => err.InnerException.ValueOr(ex => ex is TError, false))
                    .ValueOr(default);
                return true;
            }
        }

        /// <summary>
        /// Returns true if <see cref="IsOk"/> is false and if <see cref="Errors"/> contains an error with error code <paramref name="code"/>. This is a useful analogue for <c>catch</c>.
        /// </summary>
        /// <typeparam name="TCode">The type of the error code enumeration.</typeparam>
        /// <param name="code">The error code to search for.</param>
        /// <param name="error">The first occurrence of an <see cref="Error"/> which has an error code <paramref name="code"/>.</param>
        public bool HasErrorCode<TCode>(TCode code, out Error error)
            where TCode : Enum
            => HasErrorCode((Convert.ToInt32(code)).ToString(CultureInfo.InvariantCulture), out error);

        /// <summary>
        /// Returns true if <see cref="IsOk"/> is false and if <see cref="Errors"/> contains an error with error code <paramref name="code"/>. This is a useful analogue for <c>catch</c>.
        /// </summary>
        /// <param name="code">The error code to search for.</param>
        /// <param name="error">The first occurrence of an <see cref="Error"/> which has an error code <paramref name="code"/>.</param>
        public bool HasErrorCode(string code, out Error error)
        {
            error = default;

            if (IsOk)
                return false;
            else
            {
                error = Errors().FirstMaybe(err => err.Code == code).ValueOr(default);
                return true;
            }
        }

        /// <summary>
        /// Creates a new <see cref="Result{T}"/> from an <see cref="Either{TLeft, TRight}"/>.
        /// If <paramref name="resultClass"/> is <see cref="ResultClass.Ok"/>, <paramref name="innerResult"/> MUST contain a right-value, otherwise, an exception is thrown.
        /// </summary>
        /// <param name="innerResult">The underlying data.</param>
        /// <param name="resultClass">The result class.</param>
        /// <exception cref="ArgumentException">If <paramref name="resultClass"/> is <see cref="ResultClass.Ok"/>, but <paramref name="innerResult"/> contains no value.</exception>
        public Result(Either<IList<Error>, T> innerResult, ResultClass resultClass = ResultClass.Unspecified)
        {
            InnerResult = innerResult;
            ResultClass = resultClass;

            if (resultClass == ResultClass.Ok && innerResult.IsLeft)
                throw new ArgumentException($"Cannot create an instance of {nameof(Result<object>)} with {nameof(resultClass)}={nameof(ResultClass)}.{nameof(ResultClass.Ok)} and no value.", nameof(innerResult));
        }

        /// <summary>
        /// Creates an OK-result from a value.
        /// </summary>
        /// <param name="result">The value.</param>
        public static Result<T> Ok(T result)
            => new Result<T>(Either<IList<Error>, T>.FromRight(result), ResultClass.Ok);

        /// <summary>
        /// Creates an error-result from a list of errors.
        /// </summary>
        /// <param name="errors">The list of errors.</param>
        /// <param name="resultClass">The class of the result.</param>
        public static Result<T> WithErrors(IEnumerable<Error> errors, ResultClass resultClass)
            => new Result<T>(Either<IList<Error>, T>.FromLeft(new List<Error>(errors)), resultClass);

        /// <summary>
        /// Creates an error-result from an error.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <param name="resultClass">The class of the result.</param>
        public static Result<T> WithError(Error error, ResultClass resultClass)
            => new Result<T>(Either<IList<Error>, T>.FromLeft(new List<Error> { error }), resultClass);

        /// <summary>
        /// Creates a <see cref="Result.Ok{T}(T)"/> if <paramref name="isOk"/> is true, using <paramref name="factory"/>,
        /// and <see cref="Result.WithErrors{T}(IEnumerable{Error}, ResultClass)"/> otherwise.
        /// </summary>
        /// <param name="isOk">Whether the result is OK.</param>
        /// <param name="factory">The value-factory for the return-value if <paramref name="isOk"/> is true.</param>
        /// <param name="errors">The list of errors if <paramref name="isOk"/> is false.</param>
        /// <param name="resultClassIfError">The <see cref="ResultClass"/> if <paramref name="isOk"/> is false.</param>
        public static Result<T> OkIf(bool isOk, Func<T> factory, IEnumerable<Error> errors, ResultClass resultClassIfError)
            => isOk ? Result.Ok(factory()) : new Result<T>(Either<IList<Error>, T>.FromLeft(new List<Error>(errors)), resultClassIfError);

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Result<T>))
                return false;

            var thatResult = (Result<T>)obj;

            return (InnerResult.Equals(thatResult.InnerResult)
                && ResultClass == thatResult.ResultClass);
        }

        /// <inheritdoc />
        public override int GetHashCode() => this.DefaultHash(InnerResult, ResultClass);

        /// <inheritdoc />
        public static bool operator ==(Result<T> left, Result<T> right) => left.Equals(right);

        /// <inheritdoc />
        public static bool operator !=(Result<T> left, Result<T> right) => !(left == right);

        /// <inheritdoc />
        public bool Equals(Result<T> other) => Equals((object)other);

        /// <inheritdoc />
        public IFunctor<TResult> Map<TResult>(Func<T, TResult> f)
            => new Result<TResult>((Either<IList<Error>, TResult>)InnerResult.Map(f), ResultClass);

        /// <inheritdoc />
        public Result<T> MonoMap(Func<T, T> f)
            => new Result<T>((Either<IList<Error>, T>)InnerResult.Map(f), ResultClass);

        /// <inheritdoc />
        public IMonad<TResult> Bind<TResult>(Func<T, IMonad<TResult>> f)
        {
            if (IsOk)
                return f(InnerResult.Right());
            else
                return Result.WithErrors<TResult>(Errors(), ResultClass);
        }

        /// <inheritdoc />
        public IApplicative<TResult> Pure<TResult>(TResult x) => Result.Ok(x);

        /// <inheritdoc />
        public IApplicative<TResult> Ap<TResult>(IApplicative<Func<T, TResult>> f)
        {
            if (f == null || !(f is Result<Func<T, TResult>>))
                throw new InvalidCastException();

            var fResult = (Result<Func<T, TResult>>)f;

            if (IsOk && fResult.IsOk)
                return new Result<TResult>((Either<IList<Error>, TResult>)InnerResult.Ap(fResult.InnerResult), fResult.ResultClass);
            else if (!fResult.IsOk)
                return new Result<TResult>(Either.FromLeft<IList<Error>, TResult>(fResult.InnerResult.Left()), fResult.ResultClass);
            else
                return new Result<TResult>(Either.FromLeft<IList<Error>, TResult>(InnerResult.Left()), ResultClass);
        }

        /// <inheritdoc />
        public async Task<IAsyncMonad<TResult>> BindAsync<TResult>(Func<T, Task<IAsyncMonad<TResult>>> f)
        {
            if (IsOk)
                return await f(InnerResult.Right());
            else
                return Result.WithErrors<TResult>(Errors(), ResultClass);
        }

        /// <inheritdoc />
        public async Task<IApplicative<TResult>> PureAsync<TResult>(Func<Task<TResult>> x) => Result.Ok(await x());

        /// <inheritdoc />
        public async Task<IAsyncApplicative<TResult>> ApAsync<TResult>(IApplicative<Func<T, Task<TResult>>> f)
        {
            if (f == null || !(f is Result<Func<T, Task<TResult>>>))
                throw new InvalidCastException();

            var fResult = (Result<Func<T, Task<TResult>>>)f;

            if (IsOk && fResult.IsOk)
                return new Result<TResult>((Either<IList<Error>, TResult>)await InnerResult.ApAsync(fResult.InnerResult), fResult.ResultClass);
            else if (!fResult.IsOk)
                return new Result<TResult>(Either.FromLeft<IList<Error>, TResult>(fResult.InnerResult.Left()), fResult.ResultClass);
            else
                return new Result<TResult>(Either.FromLeft<IList<Error>, TResult>(InnerResult.Left()), ResultClass);
        }

        /// <inheritdoc />
        public async Task<IAsyncFunctor<TResult>> MapAsync<TResult>(Func<T, Task<TResult>> f)
            => new Result<TResult>((Either<IList<Error>, TResult>)(await InnerResult.MapAsync(f)), ResultClass);
    }

    /// <summary>
    /// Extension methods for <see cref="Result{T}"/>.
    /// </summary>
    public static class Result
    {
        /// <summary>
        /// Equivalent to <see cref="IFunctor{TSource}.Map{TResult}(Func{TSource, TResult})"/>, but restricted to <see cref="Result{T}"/>. Offers LINQ query support with one <c>from</c>-clause.
        /// </summary>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        public static Result<TResult> Select<TSource, TResult>(this Result<TSource> source, Func<TSource, TResult> f)
            => (Result<TResult>)source.Map(f);

        /// <summary>
        /// Equivalent to <see cref="IMonad{TSource}"/>, but restricted to <see cref="Result{T}"/>. Offers LINQ query support with multiple <c>from</c>-clauses.
        /// </summary>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TMiddle">The type of the selector's result.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        /// <param name="resultSelector">The result-selector.</param>
        public static Result<TResult> SelectMany<TSource, TMiddle, TResult>
            (this Result<TSource> source,
             Func<TSource, Result<TMiddle>> f,
             Func<TSource, TMiddle, TResult> resultSelector)
            => (Result<TResult>)source.Bind(x => (Result<TResult>)f(x).Map(m => resultSelector(x, m)));

        /// <summary>
        /// Equivalent to <see cref="IFunctor{TSource}.Map{TResult}(Func{TSource, TResult})"/>, but restricted to asynchronous <see cref="Result{T}"/>. Offers LINQ query support with one <c>from</c>-clause.
        /// </summary>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        public static async Task<Result<TResult>> Select<TSource, TResult>(
            this Task<Result<TSource>> source, Func<TSource, TResult> f)
            => Select(await source, f);

        /// <summary>
        /// Equivalent to <see cref="IMonad{TSource}"/>, but restricted to asynchronous <see cref="Result{T}"/>. Offers LINQ query support with multiple <c>from</c>-clauses.
        /// </summary>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TMiddle">The type of the selector's result.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        /// <param name="resultSelector">The result-selector.</param>
        public static async Task<Result<TResult>> SelectMany<TSource, TMiddle, TResult>
            (this Task<Result<TSource>> source,
             Func<TSource, Task<Result<TMiddle>>> f,
             Func<TSource, TMiddle, TResult> resultSelector)
            => (Result<TResult>)(await (await source).BindAsync(async x => (IAsyncMonad<TResult>)(await f(x)).Map(y => resultSelector(x, y))));

        /// <summary>
        /// Creates an OK-result from a value.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="result">The result.</param>
        public static Result<T> Ok<T>(T result) => Result<T>.Ok(result);

        /// <summary>
        /// Creates an OK-result, invoking the parameterless constructor of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        public static Result<T> Ok<T>()
            where T : new()
            => new Result<T>(new Either<IList<Error>, T>(new T(), TagRight.Value));

        /// <summary>
        /// Creates an error-result from a list of errors.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="errors">The list of errors.</param>
        /// <param name="resultClass">The result class.</param>
        public static Result<T> WithErrors<T>(IEnumerable<Error> errors, ResultClass resultClass) => Result<T>.WithErrors(errors, resultClass);

        /// <summary>
        /// Creates an error-result from an error.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="error">The error.</param>
        /// <param name="resultClass">The class of the result.</param>
        public static Result<T> WithError<T>(Error error, ResultClass resultClass)
            => new Result<T>(Either<IList<Error>, T>.FromLeft(new List<Error> { error }), resultClass);

        /// <summary>
        /// Creates a <see cref="Result.Ok{T}(T)"/> if <paramref name="isOk"/> is true, using <paramref name="factory"/>,
        /// and <see cref="Result.WithErrors{T}(IEnumerable{Error}, ResultClass)"/> otherwise.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="isOk">Whether the result is OK.</param>
        /// <param name="factory">The value-factory for the return-value if <paramref name="isOk"/> is true.</param>
        /// <param name="errors">The list of errors if <paramref name="isOk"/> is false.</param>
        /// <param name="resultClassIfError">The <see cref="ResultClass"/> if <paramref name="isOk"/> is false.</param>
        public static Result<T> OkIf<T>(bool isOk, Func<T> factory, IEnumerable<Error> errors, ResultClass resultClassIfError)
            => isOk ? Result.Ok(factory()) : new Result<T>(Either<IList<Error>, T>.FromLeft(new List<Error>(errors)), resultClassIfError);

        /// <summary>
        /// Tries to cast a <see cref="IFunctor{TSource}"/> to a <see cref="Result{T}"/> via an explicit cast.
        /// Convenience method.
        /// </summary>
        /// <typeparam name="T">The type of the value contained in the functor.</typeparam>
        /// <param name="f">The functor to cast to a maybe.</param>
        public static Result<T> ToResult<T>(this IFunctor<T> f) => (Result<T>)f;
    }
}

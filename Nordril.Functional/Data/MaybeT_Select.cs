using Nordril.Functional.Algebra;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional.Data
{
#pragma warning disable 1591
    public static partial class MaybeT
    {
        #region Monad transformer Select/SelectMany-methods
        //Const{T}
        /*public static MaybeT<Const<TResult>, Const<Maybe<TResult>>, Maybe<TResult>, TResult> Select<TSource, TResult>(
            this MaybeT<Const<TSource>, Const<Maybe<TSource>>, Maybe<TSource>, TSource> source, Func<TSource, TResult> f)
            => (MaybeT<Const<TResult>, Const<Maybe<TResult>>, Maybe<TResult>, TResult>)source.MapT<Const<TResult>, Const<Maybe<TResult>>, Maybe<TResult>, TResult>(f);*/

        //Either
        public static MaybeT<Either<TLeft, TResult>, Either<TLeft, Maybe<TResult>>, Maybe<TResult>, TResult> Select<TLeft, TSource, TResult>(
            this MaybeT<Either<TLeft, TSource>, Either<TLeft, Maybe<TSource>>, Maybe<TSource>, TSource> source, Func<TSource, TResult> f)
            => (MaybeT<Either<TLeft, TResult>, Either<TLeft, Maybe<TResult>>, Maybe<TResult>, TResult>)source.MapT<Either<TLeft, TResult>, Either<TLeft, Maybe<TResult>>, Maybe<TResult>, TResult>(f);

        //FuncList
        public static MaybeT<FuncList<TResult>, FuncList<Maybe<TResult>>, Maybe<TResult>, TResult> Select<TSource, TResult>(
            this MaybeT<FuncList<TSource>, FuncList<Maybe<TSource>>, Maybe<TSource>, TSource> source, Func<TSource, TResult> f)
            => (MaybeT<FuncList<TResult>, FuncList<Maybe<TResult>>, Maybe<TResult>, TResult>)source.MapT<FuncList<TResult>, FuncList<Maybe<TResult>>, Maybe<TResult>, TResult>(f);

        //FuncSet
        public static MaybeT<FuncSet<TResult>, FuncSet<Maybe<TResult>>, Maybe<TResult>, TResult> Select<TSource, TResult>(
            this MaybeT<FuncSet<TSource>, FuncSet<Maybe<TSource>>, Maybe<TSource>, TSource> source, Func<TSource, TResult> f)
            => (MaybeT<FuncSet<TResult>, FuncSet<Maybe<TResult>>, Maybe<TResult>, TResult>)source.MapT<FuncSet<TResult>, FuncSet<Maybe<TResult>>, Maybe<TResult>, TResult>(f);

        //Identity
        public static MaybeT<Identity<TResult>, Identity<Maybe<TResult>>, Maybe<TResult>, TResult> Select<TSource, TResult>(
            this MaybeT<Identity<TSource>, Identity<Maybe<TSource>>, Maybe<TSource>, TSource> source, Func<TSource, TResult> f)
            => (MaybeT<Identity<TResult>, Identity<Maybe<TResult>>, Maybe<TResult>, TResult>)source.MapT<Identity<TResult>, Identity<Maybe<TResult>>, Maybe<TResult>, TResult>(f);


        //Io
        public static MaybeT<Io<TResult>, Io<Maybe<TResult>>, Maybe<TResult>, TResult> Select<TSource, TResult>(
            this MaybeT<Io<TSource>, Io<Maybe<TSource>>, Maybe<TSource>, TSource> source, Func<TSource, TResult> f)
            => (MaybeT<Io<TResult>, Io<Maybe<TResult>>, Maybe<TResult>, TResult>)source.MapT<Io<TResult>, Io<Maybe<TResult>>, Maybe<TResult>, TResult>(f);

        //Random
        public static MaybeT<Random<TRng, TResult>, Random<TRng, Maybe<TResult>>, Maybe<TResult>, TResult> Select<TRng, TSource, TResult>(
            this MaybeT<Random<TRng, TSource>, Random<TRng, Maybe<TSource>>, Maybe<TSource>, TSource> source, Func<TSource, TResult> f)
            => (MaybeT<Random<TRng, TResult>, Random<TRng, Maybe<TResult>>, Maybe<TResult>, TResult>)source.MapT<Random<TRng, TResult>, Random<TRng, Maybe<TResult>>, Maybe<TResult>, TResult>(f);

        //Reader
        public static MaybeT<Reader<TEnvironment, TResult>, Reader<TEnvironment, Maybe<TResult>>, Maybe<TResult>, TResult> Select<TEnvironment, TSource, TResult>(
            this MaybeT<Reader<TEnvironment, TSource>, Reader<TEnvironment, Maybe<TSource>>, Maybe<TSource>, TSource> source, Func<TSource, TResult> f)
            => (MaybeT<Reader<TEnvironment, TResult>, Reader<TEnvironment, Maybe<TResult>>, Maybe<TResult>, TResult>)source.MapT<Reader<TEnvironment, TResult>, Reader<TEnvironment, Maybe<TResult>>, Maybe<TResult>, TResult>(f);

        //Rws
        public static MaybeT<Rws<TEnvironment, TOutput, TMonoid, TState, TResult>, Rws<TEnvironment, TOutput, TMonoid, TState, Maybe<TResult>>, Maybe<TResult>, TResult> Select<TEnvironment, TOutput, TMonoid, TState, TSource, TResult>(
            this MaybeT<Rws<TEnvironment, TOutput, TMonoid, TState, TSource>, Rws<TEnvironment, TOutput, TMonoid, TState, Maybe<TSource>>, Maybe<TSource>, TSource> source, Func<TSource, TResult> f)
            where TMonoid : IMonoid<TOutput>
            => (MaybeT<Rws<TEnvironment, TOutput, TMonoid, TState, TResult>, Rws<TEnvironment, TOutput, TMonoid, TState, Maybe<TResult>>, Maybe<TResult>, TResult>)source.MapT<Rws<TEnvironment, TOutput, TMonoid, TState, TResult>, Rws<TEnvironment, TOutput, TMonoid, TState, Maybe<TResult>>, Maybe<TResult>, TResult>(f);

        //State
        public static MaybeT<State<TState, TResult>, State<TState, Maybe<TResult>>, Maybe<TResult>, TResult> Select<TState, TSource, TResult>(
            this MaybeT<State<TState, TSource>, State<TState, Maybe<TSource>>, Maybe<TSource>, TSource> source, Func<TSource, TResult> f)
            => (MaybeT<State<TState, TResult>, State<TState, Maybe<TResult>>, Maybe<TResult>, TResult>)source.MapT<State<TState, TResult>, State<TState, Maybe<TResult>>, Maybe<TResult>, TResult>(f);

        //Writer
        public static MaybeT<Writer<TOutput, TResult, TMonoid>, Writer<TOutput, Maybe<TResult>, TMonoid>, Maybe<TResult>, TResult> Select<TOutput, TMonoid, TSource, TResult>(
            this MaybeT<Writer<TOutput, TSource, TMonoid>, Writer<TOutput, Maybe<TSource>, TMonoid>, Maybe<TSource>, TSource> source, Func<TSource, TResult> f)
            where TMonoid : IMonoid<TOutput>
            => (MaybeT<Writer<TOutput, TResult, TMonoid>, Writer<TOutput, Maybe<TResult>, TMonoid>, Maybe<TResult>, TResult>)source.MapT<Writer<TOutput, TResult, TMonoid>, Writer<TOutput, Maybe<TResult>, TMonoid>, Maybe<TResult>, TResult>(f);
        #endregion
    }
#pragma warning restore 1591
}

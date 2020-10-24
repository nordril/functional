using Nordril.Functional.Algebra;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace Nordril.Functional.Data
{
#pragma warning disable 1591
    public static partial class MaybeT
    {
        //Const{T}
        /*public static MaybeT<Const<TValue>, Const<Maybe<TValue>>, Maybe<TValue>, TValue>
            Lift<TValue>(Const<TValue> value)
        {
            var m = new MaybeT<Const<TValue>, Const<Maybe<TValue>>, Maybe<TValue>, TValue>();
            return (MaybeT<Const<TValue>, Const<Maybe<TValue>>, Maybe<TValue>, TValue>)m.Lift(value);
        }*/

        //Either
        public static MaybeT<Either<TLeft, TValue>, Either<TLeft, Maybe<TValue>>, Maybe<TValue>, TValue>
            Lift<TLeft, TValue>(Either<TLeft, TValue> value)
        {
            var m = new MaybeT<Either<TLeft, TValue>, Either<TLeft, Maybe<TValue>>, Maybe<TValue>, TValue>();
            return (MaybeT<Either<TLeft, TValue>, Either<TLeft, Maybe<TValue>>, Maybe<TValue>, TValue>)m.Lift(value);
        }

        //FuncList
        public static MaybeT<FuncList<TValue>, FuncList<Maybe<TValue>>, Maybe<TValue>, TValue>
            Lift<TValue>(FuncList<TValue> value)
        {
            var m = new MaybeT<FuncList<TValue>, FuncList<Maybe<TValue>>, Maybe<TValue>, TValue>();
            return (MaybeT<FuncList<TValue>, FuncList<Maybe<TValue>>, Maybe<TValue>, TValue>)m.Lift(value);
        }

        //FuncSet
        public static MaybeT<FuncSet<TValue>, FuncSet<Maybe<TValue>>, Maybe<TValue>, TValue>
            Lift<TValue>(FuncSet<TValue> value)
        {
            var m = new MaybeT<FuncSet<TValue>, FuncSet<Maybe<TValue>>, Maybe<TValue>, TValue>();
            return (MaybeT<FuncSet<TValue>, FuncSet<Maybe<TValue>>, Maybe<TValue>, TValue>)m.Lift(value);
        }

        //Identity
        public static MaybeT<Identity<TValue>, Identity<Maybe<TValue>>, Maybe<TValue>, TValue>
            Lift<TValue>(Identity<TValue> value)
        {
            var m = new MaybeT<Identity<TValue>, Identity<Maybe<TValue>>, Maybe<TValue>, TValue>();
            return (MaybeT<Identity<TValue>, Identity<Maybe<TValue>>, Maybe<TValue>, TValue>)m.Lift(value);
        }

        //Io
        public static MaybeT<Io<TValue>, Io<Maybe<TValue>>, Maybe<TValue>, TValue>
            Lift<TValue>(Io<TValue> value)
        {
            var m = new MaybeT<Io<TValue>, Io<Maybe<TValue>>, Maybe<TValue>, TValue>();
            return (MaybeT<Io<TValue>, Io<Maybe<TValue>>, Maybe<TValue>, TValue>)m.Lift(value);
        }

        //Random
        public static MaybeT<Random<TRng, TValue>, Random<TRng, Maybe<TValue>>, Maybe<TValue>, TValue>
            Lift<TRng, TValue>(Random<TRng, TValue> value)
        {
            var m = new MaybeT<Random<TRng, TValue>, Random<TRng, Maybe<TValue>>, Maybe<TValue>, TValue>();
            return (MaybeT<Random<TRng, TValue>, Random<TRng, Maybe<TValue>>, Maybe<TValue>, TValue>)m.Lift(value);
        }

        //Reader
        public static MaybeT<Reader<TEnvironment, TValue>, Reader<TEnvironment, Maybe<TValue>>, Maybe<TValue>, TValue>
            Lift<TEnvironment, TValue>(Reader<TEnvironment, TValue> value)
        {
            var m = new MaybeT<Reader<TEnvironment, TValue>, Reader<TEnvironment, Maybe<TValue>>, Maybe<TValue>, TValue>();
            return (MaybeT<Reader<TEnvironment, TValue>, Reader<TEnvironment, Maybe<TValue>>, Maybe<TValue>, TValue>)m.Lift(value);
        }

        //RWS
        public static MaybeT<Rws<TEnvironment, TOutput, TMonoid, TState, TValue>, Rws<TEnvironment, TOutput, TMonoid, TState, Maybe<TValue>>, Maybe<TValue>, TValue>
            Lift<TEnvironment, TOutput, TMonoid, TState, TValue>(Rws<TEnvironment, TOutput, TMonoid, TState, TValue> value)
            where TMonoid : IMonoid<TOutput>
        {
            var m = new MaybeT<Rws<TEnvironment, TOutput, TMonoid, TState, TValue>, Rws<TEnvironment, TOutput, TMonoid, TState, Maybe<TValue>>, Maybe<TValue>, TValue>();
            return (MaybeT<Rws<TEnvironment, TOutput, TMonoid, TState, TValue>, Rws<TEnvironment, TOutput, TMonoid, TState, Maybe<TValue>>, Maybe<TValue>, TValue>)m.Lift(value);
        }

        //State
        public static MaybeT<State<TState, TValue>, State<TState, Maybe<TValue>>, Maybe<TValue>, TValue>
            Lift<TState, TValue>(State<TState, TValue> value)
        {
            var m = new MaybeT<State<TState, TValue>, State<TState, Maybe<TValue>>, Maybe<TValue>, TValue>();
            return (MaybeT<State<TState, TValue>, State<TState, Maybe<TValue>>, Maybe<TValue>, TValue>)m.Lift(value);
        }

        //Writer
        public static MaybeT<Writer<TOutput, TValue, TMonoid>, Writer<TOutput, Maybe<TValue>, TMonoid>, Maybe<TValue>, TValue>
            Lift<TOutput, TValue, TMonoid>(Writer<TOutput, TValue, TMonoid> value)
            where TMonoid : IMonoid<TOutput>
        {
            var m = new MaybeT<Writer<TOutput, TValue, TMonoid>, Writer<TOutput, Maybe<TValue>, TMonoid>, Maybe<TValue>, TValue>();
            return (MaybeT<Writer<TOutput, TValue, TMonoid>, Writer<TOutput, Maybe<TValue>, TMonoid>, Maybe<TValue>, TValue>)m.Lift(value);
        }
    }
#pragma warning restore 1591
}

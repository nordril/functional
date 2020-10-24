using Nordril.Functional.Algebra;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Nordril.Functional.Data
{
#pragma warning disable 1591
    public static partial class MaybeT
    {
        //Const{T}
        /*public static MaybeT<Const<TValue>, Const<Maybe<TValue>>, Maybe<TValue>, TValue>
            HoistConst<TValue>(
            Maybe<TValue> value)
        {
            var m = new MaybeT<Const<TValue>, Const<Maybe<TValue>>, Maybe<TValue>, TValue>();
            return (MaybeT<Const<TValue>, Const<Maybe<TValue>>, Maybe<TValue>, TValue>)m.Hoist(value);
        }*/

        //Either
        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "type tag")]
        public static MaybeT<Either<TLeft, TValue>, Either<TLeft, Maybe<TValue>>, Maybe<TValue>, TValue>
            HoistEither<TLeft, TValue>(
            EitherCxt<TLeft> _cxt,
            Maybe<TValue> value)
        {
            var m = new MaybeT<Either<TLeft, TValue>, Either<TLeft, Maybe<TValue>>, Maybe<TValue>, TValue>();
            return (MaybeT<Either<TLeft, TValue>, Either<TLeft, Maybe<TValue>>, Maybe<TValue>, TValue>)m.Hoist(value);
        }

        //FuncList
        public static MaybeT<FuncList<TValue>, FuncList<Maybe<TValue>>, Maybe<TValue>, TValue>
            HoistFuncList<TValue>(
            Maybe<TValue> value)
        {
            var m = new MaybeT<FuncList<TValue>, FuncList<Maybe<TValue>>, Maybe<TValue>, TValue>();
            return (MaybeT<FuncList<TValue>, FuncList<Maybe<TValue>>, Maybe<TValue>, TValue>)m.Hoist(value);
        }

        //FuncSet
        public static MaybeT<FuncSet<TValue>, FuncSet<Maybe<TValue>>, Maybe<TValue>, TValue>
            HoistFuncSet<TValue>(
            Maybe<TValue> value)
        {
            var m = new MaybeT<FuncSet<TValue>, FuncSet<Maybe<TValue>>, Maybe<TValue>, TValue>();
            return (MaybeT<FuncSet<TValue>, FuncSet<Maybe<TValue>>, Maybe<TValue>, TValue>)m.Hoist(value);
        }

        //Identity
        public static MaybeT<Identity<TValue>, Identity<Maybe<TValue>>, Maybe<TValue>, TValue>
            HoistIdentity<TValue>(
            Maybe<TValue> value)
        {
            var m = new MaybeT<Identity<TValue>, Identity<Maybe<TValue>>, Maybe<TValue>, TValue>();
            return (MaybeT<Identity<TValue>, Identity<Maybe<TValue>>, Maybe<TValue>, TValue>)m.Hoist(value);
        }

        //Io
        public static MaybeT<Io<TValue>, Io<Maybe<TValue>>, Maybe<TValue>, TValue>
            HoistIo<TValue>(
            Maybe<TValue> value)
        {
            var m = new MaybeT<Io<TValue>, Io<Maybe<TValue>>, Maybe<TValue>, TValue>();
            return (MaybeT<Io<TValue>, Io<Maybe<TValue>>, Maybe<TValue>, TValue>)m.Hoist(value);
        }

        //Random
        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "type tag")]
        public static MaybeT<Random<TRng, TValue>, Random<TRng, Maybe<TValue>>, Maybe<TValue>, TValue>
            HoistRandom<TRng, TValue>(
            RandomCxt<TRng> _cxt,
            Maybe<TValue> value)
        {
            var m = new MaybeT<Random<TRng, TValue>, Random<TRng, Maybe<TValue>>, Maybe<TValue>, TValue>();
            return (MaybeT<Random<TRng, TValue>, Random<TRng, Maybe<TValue>>, Maybe<TValue>, TValue>)m.Hoist(value);
        }

        //Reader
        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "type tag")]
        public static MaybeT<Reader<TEnvironment, TValue>, Reader<TEnvironment, Maybe<TValue>>, Maybe<TValue>, TValue>
            HoistReader<TEnvironment, TValue>(
            ReaderCxt<TEnvironment> _cxt,
            Maybe<TValue> value)
        {
            var m = new MaybeT<Reader<TEnvironment, TValue>, Reader<TEnvironment, Maybe<TValue>>, Maybe<TValue>, TValue>();
            return (MaybeT<Reader<TEnvironment, TValue>, Reader<TEnvironment, Maybe<TValue>>, Maybe<TValue>, TValue>)m.Hoist(value);
        }

        //RWS
        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "type tag")]
        public static MaybeT<Rws<TEnvironment, TOutput, TMonoid, TState, TValue>, Rws<TEnvironment, TOutput, TMonoid, TState, Maybe<TValue>>, Maybe<TValue>, TValue>
            HoistRws<TEnvironment, TOutput, TMonoid, TState, TValue>(
            RwsCxt<TEnvironment, TOutput, TMonoid, TState> _cxt,
            Maybe<TValue> value)
            where TMonoid : IMonoid<TOutput>
        {
            var m = new MaybeT<Rws<TEnvironment, TOutput, TMonoid, TState, TValue>, Rws<TEnvironment, TOutput, TMonoid, TState, Maybe<TValue>>, Maybe<TValue>, TValue>();
            return (MaybeT<Rws<TEnvironment, TOutput, TMonoid, TState, TValue>, Rws<TEnvironment, TOutput, TMonoid, TState, Maybe<TValue>>, Maybe<TValue>, TValue>)m.Hoist(value);
        }

        //State
        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "type tag")]
        public static MaybeT<State<TState, TValue>, State<TState, Maybe<TValue>>, Maybe<TValue>, TValue>
            HoistState<TState, TValue>(
            StateCxt<TState> _cxt,
            Maybe<TValue> value)
        {
            var m = new MaybeT<State<TState, TValue>, State<TState, Maybe<TValue>>, Maybe<TValue>, TValue>();
            return (MaybeT<State<TState, TValue>, State<TState, Maybe<TValue>>, Maybe<TValue>, TValue>)m.Hoist(value);
        }

        //Writer
        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "type tag")]
        public static MaybeT<Writer<TOutput, TValue, TMonoid>, Writer<TOutput, Maybe<TValue>, TMonoid>, Maybe<TValue>, TValue>
            HoistWriter<TOutput, TMonoid, TValue>(
            WriterCxt<TOutput, TMonoid> _cxt,
            Maybe<TValue> value)
            where TMonoid : IMonoid<TOutput>
        {
            var m = new MaybeT<Writer<TOutput, TValue, TMonoid>, Writer<TOutput, Maybe<TValue>, TMonoid>, Maybe<TValue>, TValue>();
            return (MaybeT<Writer<TOutput, TValue, TMonoid>, Writer<TOutput, Maybe<TValue>, TMonoid>, Maybe<TValue>, TValue>)m.Hoist(value);
        }
    }
#pragma warning restore 1591
}

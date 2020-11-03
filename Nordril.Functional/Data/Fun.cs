using Nordril.Functional.Category;
using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional.Data
{
    /// <summary>
    /// A profunctor-wrapper for <see cref="Func{T, TResult}"/>, needed for prisms.
    /// <see cref="Fun{TIn, TOut}"/> is ismorphic to <see cref="Func{T, TResult}"/>, but since it's a struct, it can implement
    /// interfaces, esp. <see cref="IProfunctor{TNeed, THave}"/> (allowing a function to be prepended to its input and another to be appended to its output)
    /// and <see cref="IChoice{TNeed, THave}"/> (allowing it to be lifted into <see cref="Either{TLeft, TRight}"/>).
    /// </summary>
    /// <typeparam name="TIn">The type of the input.</typeparam>
    /// <typeparam name="TOut">The type of the output.</typeparam>
    public struct Fun<TIn, TOut> : IArrowChoice<TIn, TOut>
    {
        /// <summary>
        /// The underlying function.
        /// </summary>
        public Func<TIn, TOut> Func { get; set; }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="func">The function to wrap.</param>
        public Fun(Func<TIn, TOut> func)
        {
            Func = func;
        }

        /// <inheritdoc />
        public IContravariant<TResult> ContraMap<TResult>(Func<TResult, TIn> f)
        {
            var func = Func;
            return new Fun<TResult, TOut>(x => func(f(x)));
        }

        /// <inheritdoc />
        public IFunctor<TResult> Map<TResult>(Func<TOut, TResult> f)
        {
            var func = Func;
            return new Fun<TIn, TResult>(x => f(func(x)));
        }

        /// <inheritdoc />
        public IProfunctor<TNeedResult, THaveResult> Promap<TNeedResult, THaveResult>(Func<TNeedResult, TIn> @in, Func<TOut, THaveResult> @out)
        {
            var func = Func;
            return new Fun<TNeedResult, THaveResult>(x => @out(func(@in(x))));
        }

        /// <inheritdoc />
        public IChoice<Either<TIn, TRight>, Either<TOut, TRight>> ChooseLeft<TRight>()
        {
            var func = Func;
            Func<Either<TIn, TRight>, Either<TOut, TRight>> f = e => e.BiMap(func, x => x).ToEither();
            return f.MakeFun();
        }

        /// <inheritdoc />
        public IChoice<Either<TLeft, TIn>, Either<TLeft, TOut>> ChooseRight<TLeft>()
        {
            var func = Func;
            Func<Either<TLeft, TIn>, Either<TLeft, TOut>> f = e => e.BiMap(x => x, func).ToEither();
            return f.MakeFun();
        }

        /// <inheritdoc />
        public IArrow<TNeedResult, THaveResult> Make<TNeedResult, THaveResult>(Func<TNeedResult, THaveResult> f)
        {
            return new Fun<TNeedResult, THaveResult>(f);
        }

        /// <inheritdoc />
        public IArrow<(TIn, TNeedRight), (TOut, THaveRight)> TogetherWith<TNeedRight, THaveRight>(IArrow<TNeedRight, THaveRight> that)
        {
            if (!(that is Fun<TNeedRight, THaveRight> thatF))
                throw new InvalidCastException();

            var func = Func;

            return new Fun<(TIn, TNeedRight), (TOut, THaveRight)>(xy => (func(xy.Item1), thatF.Func(xy.Item2)));
        }

        /// <inheritdoc />
        public ICategory<THaveResult, THaveResult> Id<THaveResult>()
        {
            return new Fun<THaveResult, THaveResult>(x => x);
        }

        /// <inheritdoc />
        public ICategory<TIn, THaveResult> Then<THaveResult>(ICategory<TOut, THaveResult> that)
        {
            if (!(that is Fun<TOut, THaveResult> thatF))
                throw new InvalidCastException();

            var func = Func;

            return new Fun<TIn, THaveResult>(x => thatF.Func(func(x)));
        }

        /// <inheritdoc />
        public IStrong<(TIn, TRight), (TOut, TRight)> LiftFirst<TRight>()
        {
            var func = Func;

            return new Fun<(TIn, TRight), (TOut, TRight)>(xy => (func(xy.Item1), xy.Item2));
        }

        /// <inheritdoc />
        public IStrong<(TLeft, TIn), (TLeft, TOut)> LiftSecond<TLeft>()
        {
            var func = Func;

            return new Fun<(TLeft, TIn), (TLeft, TOut)>(xy => (xy.Item1, func(xy.Item2)));
        }
    }
}

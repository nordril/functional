using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional.Category
{
    /// <summary>
    /// A profunctor-wrapper for <see cref="Func{T, TResult}"/>, needed for prisms.
    /// <see cref="ProfunctorFunc{TIn, TOut}"/> is ismorphic to <see cref="Func{T, TResult}"/>, but since it's a struct, it can implement
    /// interfaces, esp. <see cref="IProfunctor{TNeed, THave}"/> (allowing a function to be prepended to its input and another to be appended to its output)
    /// and <see cref="IChoice{TNeed, THave}"/> (allowing it to be lifted into <see cref="Either{TLeft, TRight}"/>).
    /// </summary>
    /// <typeparam name="TIn">The type of the input.</typeparam>
    /// <typeparam name="TOut">The type of the output.</typeparam>
    public struct ProfunctorFunc<TIn, TOut> : IChoice<TIn, TOut>
    {
        /// <summary>
        /// The underlying function.
        /// </summary>
        public Func<TIn, TOut> Func { get; set; }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="func">The function to wrap.</param>
        public ProfunctorFunc(Func<TIn, TOut> func)
        {
            Func = func;
        }

        /// <inheritdoc />
        public IContravariant<TResult> ContraMap<TResult>(Func<TResult, TIn> f)
        {
            var func = Func;
            return new ProfunctorFunc<TResult, TOut>(x => func(f(x)));
        }

        /// <inheritdoc />
        public IFunctor<TResult> Map<TResult>(Func<TOut, TResult> f)
        {
            var func = Func;
            return new ProfunctorFunc<TIn, TResult>(x => f(func(x)));
        }

        /// <inheritdoc />
        public IProfunctor<TNeedResult, THaveResult> Promap<TNeedResult, THaveResult>(Func<TNeedResult, TIn> @in, Func<TOut, THaveResult> @out)
        {
            var func = Func;
            return new ProfunctorFunc<TNeedResult, THaveResult>(x => @out(func(@in(x))));
        }

        /// <inheritdoc />
        public IChoice<Either<TIn, TRight>, Either<TOut, TRight>> ChooseLeft<TRight>()
        {
            var func = Func;
            Func<Either<TIn, TRight>, Either<TOut, TRight>> f = e => e.BiMap(func, x => x).ToEither();
            return f.FuncToProfunctor();
        }

        /// <inheritdoc />
        public IChoice<Either<TLeft, TIn>, Either<TLeft, TOut>> ChooseRight<TLeft>()
        {
            var func = Func;
            Func<Either<TLeft, TIn>, Either<TLeft, TOut>> f = e => e.BiMap(x => x, func).ToEither();
            return f.FuncToProfunctor();
        }
    }
}

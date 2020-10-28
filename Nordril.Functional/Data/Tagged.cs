using Nordril.Functional.Category;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional.Data
{
    /// <summary>
    /// Tagged is like <see cref="Const{TReal, TPhantom}"/>, but the first type-argument is the phantom one.
    /// You can think of Tagged as a "fake constant function", what takes a <typeparamref name="TPhantom"/> (on which it doesn't and can't depend) and returns
    /// an <typeparamref name="TReal"/>. <c>Tagged&lt;A,B&gt;</c> is thus the same as the function<c>(A _) =&gt; B</c>.
    /// It's basically only used for prisms, as a faked replacement for <see cref="Func{T, TResult}"/>.
    /// </summary>
    /// <typeparam name="TPhantom">The phantom-type</typeparam>
    /// <typeparam name="TReal">The real-type.</typeparam>
    public class Tagged<TPhantom, TReal>
        : IChoice<TPhantom, TReal>
    {
        /// <summary>
        /// Gets the real value.
        /// </summary>
        public TReal RealValue { get; }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="realValue">The real value to store.</param>
        public Tagged(TReal realValue)
        {
            RealValue = realValue;
        }

        /// <inheritdoc />
        public IChoice<Either<TPhantom, TRight>, Either<TReal, TRight>> ChooseLeft<TRight>()
        {
            return new Tagged<Either<TPhantom, TRight>, Either<TReal, TRight>>(Either.FromLeft<TReal, TRight>(RealValue));
        }

        /// <inheritdoc />
        public IChoice<Either<TLeft, TPhantom>, Either<TLeft, TReal>> ChooseRight<TLeft>()
        {
            return new Tagged<Either<TLeft, TPhantom>, Either<TLeft, TReal>>(Either.FromRight<TLeft, TReal>(RealValue));
        }

        /// <inheritdoc />
        public IContravariant<TResult> ContraMap<TResult>(Func<TResult, TPhantom> f)
        {
            return new Tagged<TResult, TReal>(RealValue);
        }

        /// <inheritdoc />
        public IFunctor<TResult> Map<TResult>(Func<TReal, TResult> f)
        {
            return new Tagged<TPhantom, TResult>(f(RealValue));
        }

        /// <inheritdoc />
        public IProfunctor<TNeedResult, THaveResult> Promap<TNeedResult, THaveResult>(Func<TNeedResult, TPhantom> @in, Func<TReal, THaveResult> @out)
        {
            return new Tagged<TNeedResult, THaveResult>(@out(RealValue));
        }
    }
}

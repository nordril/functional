using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional.Data
{
    /// <summary>
    /// A type-tag for the <see cref="Const{TReal, TPhantom}"/>-monad. This class contains no data; it just supplies the type-arguments which do not vary inside a single monadic computation and thus eliminates the need to explicitly supply the type arguments to methods. See <see cref="RwsCxt{TEnvironment, TOutput, TMonoid, TState}"/> for examples
    /// </summary>
    /// <typeparam name="TReal">The type of the real value.</typeparam>
    public sealed class ConstCxt<TReal>
    {
        /// <summary>
        /// Creates a new <see cref="Const{TReal, TPhantom}"/>.
        /// </summary>
        /// <typeparam name="TPhantom">The varying phantom-type.</typeparam>
        /// <param name="value">The real value.</param>
        public Const<TReal, TPhantom> Make<TPhantom>(TReal value)
            => new Const<TReal, TPhantom>(value);
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional.Data
{
    /// <summary>
    /// A type-tag for the <see cref="Random{TRng, TValue}"/>-monad. This class contains no data; it just supplies the type-arguments which do not vary inside a single monadic computation and thus eliminates the need to explicitly supply the type arguments to methods like <see cref="Random.Next(int, int)"/>. See <see cref="RwsCxt{TEnvironment, TOutput, TMonoid, TState}"/> for examples
    /// </summary>
    /// <typeparam name="TRng">The type of the RNG.</typeparam>
    public sealed class RandomCxt<TRng>
    {
    }
}

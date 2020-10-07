using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional.Data
{
    /// <summary>
    /// A type-tag for the <see cref="Writer{TOutput, TValue, TMonoid}"/>-monad. This class contains no data; it just supplies the type-arguments which do not vary inside a single monadic computation and thus eliminates the need to explicitly supply the type arguments to methods like <see cref="Writer.Tell{TOutput, TMonoid}(TOutput)"/>. See <see cref="RwsCxt{TEnvironment, TOutput, TMonoid, TState}"/> for examples.
    /// </summary>
    /// <typeparam name="TOutput">The type of the output.</typeparam>
    /// <typeparam name="TMonoid">The type of the monoid used to combine outputs.</typeparam>
    public sealed class WriterCxt<TOutput, TMonoid>
    {
    }
}

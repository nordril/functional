using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional.Data
{
    /// <summary>
    /// A type-tag for the <see cref="Reader{TEnvironment, TValue}"/>-monad. This class contains no data; it just supplies the type-arguments which do not vary inside a single monadic computation and thus eliminates the need to explicitly supply the type arguments to methods like <see cref="Reader.Local{TEnvironment, TResult}(Func{TEnvironment, TEnvironment}, Reader{TEnvironment, TResult})"/>. See <see cref="RwsCxt{TEnvironment, TOutput, TMonoid, TState}"/> for examples
    /// </summary>
    /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
    public sealed class ReaderCxt<TEnvironment>
    {
    }
}

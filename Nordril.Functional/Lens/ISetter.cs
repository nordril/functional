using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional.Lens
{
    /// <summary>
    /// A setter which applies a function to some part of its input.
    /// </summary>
    /// <typeparam name="S">The type of the input data structure.</typeparam>
    /// <typeparam name="T">The type of the output data structure.</typeparam>
    /// <typeparam name="A">The type of the input-object in <typeparamref name="S"/>.</typeparam>
    /// <typeparam name="B">The type of the result-object in <typeparamref name="T"/>.</typeparam>
    public interface ISetter<S, T, A, B>
    {
        /// <summary>
        /// Returns the setter in CPS-form.
        /// </summary>
        Func<Func<A, Identity<B>>, Func<S, Identity<T>>> SetFunc { get; }
    }
}

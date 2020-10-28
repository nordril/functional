using System;
using System.Collections.Generic;
using System.Text;
using Nordril.Functional.Data;

namespace Nordril.Functional.Lens
{
    /// <summary>
    /// A setter which applies a function to some part of its input.
    /// </summary>
    /// <typeparam name="S">The type of the input data structure.</typeparam>
    /// <typeparam name="T">The type of the output data structure.</typeparam>
    /// <typeparam name="A">The type of the input-object in <typeparamref name="S"/>.</typeparam>
    /// <typeparam name="B">The type of the result-object in <typeparamref name="T"/>.</typeparam>
    internal struct Setter<S, T, A, B> : ISetter<S, T, A, B>
    {
        /// <inheritdoc />
        public Func<Func<A, Identity<B>>, Func<S, Identity<T>>> SetFunc { get; }

        public Setter(Func<Func<A, Identity<B>>, Func<S, Identity<T>>> f)
        {
            SetFunc = f;
        }
    }
}

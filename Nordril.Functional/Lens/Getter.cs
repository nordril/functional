using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional.Lens
{
    /// <summary>
    /// A concrete getter.
    /// </summary>
    /// <typeparam name="S">The type of the input data structure.</typeparam>
    /// <typeparam name="A">The type of the object to get in <typeparamref name="S"/>.</typeparam>
    internal struct Getter<S, A> : IGetter<S, A>
    {
        /// <inheritdoc />
        public Func<Func<A, Const<object, A>>, Func<S, Const<object, S>>> GetFunc { get; }

        public Getter(Func<Func<A, Const<object, A>>, Func<S, Const<object, S>>> f)
        {
            GetFunc = f;
        }
    }
}

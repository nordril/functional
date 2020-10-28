using System;
using System.Collections.Generic;
using System.Text;
using Nordril.Functional.Data;

namespace Nordril.Functional.Lens
{
    /// <summary>
    /// A getter which does not modify its input and returns some part of it.
    /// </summary>
    /// <typeparam name="S">The type of the input data structure.</typeparam>
    /// <typeparam name="A">The type of the object to get in <typeparamref name="S"/>.</typeparam>
    public interface IGetter<S, A>
    {
        /// <summary>
        /// Returns the getter in CPS-form.
        /// </summary>
        Func<Func<A, Const<object, A>>, Func<S, Const<object, S>>> GetFunc { get; }
    }
}

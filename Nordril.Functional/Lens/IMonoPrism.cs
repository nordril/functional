using System;
using System.Collections.Generic;
using System.Text;
using Nordril.Functional.Category;
using Nordril.Functional.Data;

namespace Nordril.Functional.Lens
{
    /// <summary>
    /// A getter that "may work" for some types of input (e.g. on the left-case of <see cref="Either{TLeft, TRight}"/> only), and which can always be turned around (e.g. one can always turn a left-value into an see cref="Either{TLeft, TRight}"/>).
    /// This is a special case of <see cref="IPrism{S, T, A, B}"/> that does not change the types of its inputs.
    /// </summary>
    /// <typeparam name="S">The type of the input data structure.</typeparam>
    /// <typeparam name="A">The type of the input-object in <typeparamref name="S"/>.</typeparam>
    public interface IMonoPrism<S, A> : IPrism<S, S, A, A>
    {
    }
}

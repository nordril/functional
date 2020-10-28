using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional.Lens
{
    public static partial class L
    {
        public static partial class Make
        {
            /// <summary>
            /// Creates a getter out of an accessing-function.
            /// </summary>
            /// <typeparam name="S">The type of the container.</typeparam>
            /// <typeparam name="A">The type of the retrieved value.</typeparam>
            /// <param name="get">The accessing-function.</param>
            public static IGetter<S, A> Getter<S, A>(Func<S, A> get)
            => new Getter<S, A>(g => s => new Const<object, S>(g(get(s)).RealValue));
        }
    }
}

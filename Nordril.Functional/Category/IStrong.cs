using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional.Category
{
    /// <summary>
    /// A strong profunctor which supports lifting its input/output to a tuple-type.
    /// The stereotypical example of an instance is <see cref="Func{T, TResult}"/> (wrapped in <see cref="Fun{TIn, TOut}"/>), with the methods having the "obvious" implementations.
    /// </summary>
    /// <typeparam name="TNeed">The type of values the functor needs.</typeparam>
    /// <typeparam name="THave">The type of values the functor contains.</typeparam>
    public interface IStrong<TNeed, THave> : IProfunctor<TNeed, THave>
    {
        /// <summary>
        /// Lifts the profunctor into one which takes and produces a tuple, passing through <typeparamref name="TRight"/> unchanged. Implementors must obey the following:
        /// <code>
        /// X.LiftFirst() == X.LiftSecond().Promap((x,y) => (y,x), (x,y) => (y,x))<br />
        /// X.LiftFirst().LiftFirst() == X.LiftFirst().X.Promap(assoc, unassoc) where <br />
        /// assoc ((a, b), c) = (a, (b, c))<br />
        /// unassoc (a, (b, c)) == ((a, b), c)<br />
        /// </code>
        /// </summary>
        /// <typeparam name="TRight">The type of the second tuple-component.</typeparam>
        IStrong<(TNeed, TRight), (THave, TRight)> LiftFirst<TRight>();

        /// <summary>
        /// Lifts the profunctor into one which takes and produces a tuple, passing through <typeparamref name="TLeft"/> unchanged. Implementors must obey the following:
        /// <code>
        /// X.LiftSecond() == X.LiftFirst().Promap((x,y) => (y,x), (x,y) => (y,x))<br />
        /// </code>
        /// </summary>
        /// <typeparam name="TLeft">The type of the first tuple-component.</typeparam>
        IStrong<(TLeft, TNeed), (TLeft, THave)> LiftSecond<TLeft>();
    }
}

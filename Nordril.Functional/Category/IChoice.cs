using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional.Category
{
    /// <summary>
    /// A <see cref="IProfunctor{TNeed, THave}"/> which can be turned into a "left"- and "right"-choice, with the typical example
    /// being lifting a function <c>a -&gt; b</c> into a function <c>Either&lt;a, c&gt; -&gt; Either&lt;b, c&gt;</c> which runs the original function
    /// if the input-argument is a left-value and just passes a right-value along unchanged. The right-choice is analogous.
    /// Implementors must fulfill the following laws:
    /// <code>
    /// todo
    /// </code>
    /// </summary>
    /// <typeparam name="TNeed">The type of the input.</typeparam>
    /// <typeparam name="THave">The type of the output.</typeparam>
    public interface IChoice<TNeed, THave> : IProfunctor<TNeed, THave>
    {
        /// <summary>
        /// Lift the <see cref="IProfunctor{TNeed, THave}"/> into <see cref="Either{TLeft, TRight}"/> by mapping it over its left-value.
        /// </summary>
        /// <typeparam name="TRight">The unchanged right-value.</typeparam>
        IChoice<Either<TNeed, TRight>, Either<THave, TRight>> ChooseLeft<TRight>();

        /// <summary>
        /// Lift the <see cref="IProfunctor{TNeed, THave}"/> into <see cref="Either{TLeft, TRight}"/> by mapping it over its right-value.
        /// </summary>
        /// <typeparam name="TLeft">The unchanged left-value.</typeparam>
        IChoice<Either<TLeft, TNeed>, Either<TLeft, THave>> ChooseRight<TLeft>();
    }
}

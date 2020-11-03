using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional.Category
{
    /// <summary>
    /// A (morphism in a) category in the category-theoretical sense
    /// which goes from <typeparamref name="TNeed"/> to <typeparamref name="THave"/>.
    /// A category is a collection of objects (here: types) and morphisms between two objects (going from <typeparamref name="TNeed"/> to <typeparamref name="THave"/>).
    /// The stereotypical example of an instance is <see cref="Func{T, TResult}"/> (wrapped in <see cref="Fun{TIn, TOut}"/>), with the methods having the "obvious" implementations.
    /// </summary>
    /// <typeparam name="TNeed">The type of the source of the morphism.</typeparam>
    /// <typeparam name="THave">The type of the result of the morphism.</typeparam>
    public interface ICategory<in TNeed, out THave>
    {
        /// <summary>
        /// Creates the identity-morphism. Implementors MUST NOT access the this-pointer and must obey the following:
        /// <code>
        /// X.Id().Then(Y) == X (left-identity of Id)<br />
        /// X.Then(Y.Id()) == X (right-identity of Id)<br />
        /// </code>
        /// </summary>
        /// <typeparam name="THaveResult">The type of the source/target of the identity-morphism.</typeparam>
        ICategory<THaveResult, THaveResult> Id<THaveResult>();

        /// <summary>
        /// Concatenates a second morphism to this first and returns the resultant, combined morphism. Implementors must obey the following:
        /// <code>
        /// X.Then(Y).Then(Z) == X.Then(Y.Then(Z)) (associativity)<br />
        /// </code>
        /// </summary>
        /// <typeparam name="THaveResult">The type of the result of the second morphism.</typeparam>
        /// <param name="that">The second morphism.</param>
        ICategory<TNeed, THaveResult> Then<THaveResult>(ICategory<THave, THaveResult> that);
    }
}

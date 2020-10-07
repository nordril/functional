using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional.Data
{
    /// <summary>
    /// A type-tag for the <see cref="Rws{TEnvironment, TOutput, TMonoid, TState, TValue}"/>-monad. This class contains no data; it just supplies the type-arguments which do not vary inside a single monadic computation and thus eliminates the need to explicitly supply the type arguments to methods like <see cref="Rws.Get{TEnvironment, TOutput, TMonoid, TState}(RwsCxt{TEnvironment, TOutput, TMonoid, TState})"/>.
    /// </summary>
    /// <example>
    ///     <code>
    ///     //We only need to specify this long context once.
    ///     var cxt = new RwsCxt&lt;Dictionary&lt;string, int&gt;, IList&lt;string&gt; Monoid.ImmutableListAppendMonoid&lt;string&gt;, double&gt;();  
    ///     
    ///     var result =
    ///         //Instead of Rws.Get&lt;Dictionary&lt;string, int&gt;, IList&lt;string&gt; Monoid.ImmutableListAppendMonoid&lt;string&gt;, double&gt;()
    ///         from s in Rws.Get(cxt)
    ///         //Instead of Rws.Put&lt;Dictionary&lt;string, int&gt;, IList&lt;string&gt; Monoid.ImmutableListAppendMonoid&lt;string&gt;, double&gt;
    ///         from _ in Rws.Put(cxt, s*2D)
    ///         ...;
    /// </code>
    /// </example>
    /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
    /// <typeparam name="TOutput">The type of the output.</typeparam>
    /// <typeparam name="TMonoid">The type of the monoid used to combine outputs.</typeparam>
    /// <typeparam name="TState">The type of the state.</typeparam>
    public sealed class RwsCxt<TEnvironment, TOutput, TMonoid, TState>
    {
    }
}

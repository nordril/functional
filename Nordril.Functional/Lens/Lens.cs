using Nordril.Functional.Category;
using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional.Lens
{
    /// <summary>
    /// Lenses; first-class getters, settings, and updaters.
    /// See the <see cref="L.Make.Getter{S, A}(Func{S, A})"/>-family of functions in creating lenses and <see cref="Get{S, A}(IGetter{S, A}, S)"/>, <see cref="Set{S, T, A, B}(ISetter{S, T, A, B}, S, B)"/>, and <see cref="Update{S, T, A, B}(ISetter{S, T, A, B}, S, Func{A, B})"/> on how to execute them.
    /// See the remarks and examples.
    /// </summary>
    /// <remarks>
    /// A lens is basically a function of the type <c>(a -> f&lt;b&gt;) -> s -> f&lt;t&gt;</c>, where the arrow denotes a function. <c>s</c> is an input data structure, like a list, <c>a</c> is the part of <c>s</c> to modify, say, a string inside the list, <c>b</c> is the result of the modification, like an integer that is the length of the string, and <c>t</c> is the resulting structure, in this example a list of integers. <c>f</c>, lastly, is an <see cref="IFunctor{TSource}"/> which defined what the "lens" does, exactly, and <c>f</c> defines whether a lens can work as a getter, setter, both, a traversal, etc. - it is the "added feature" or the lens.
    /// 
    /// If we only want to create a setter, which can update a value but nothing else, we set <c>f</c> to <see cref="Identity{T}"/>. <see cref="Identity{T}"/> is just a featureless wrapper, meaning that we can use it to update the value <c>a</c>, but nothing else.
    /// If we want to create a getter, which can retrieve values, we set <c>f</c> to <see cref="Const{TReal, TPhantom}"/>. Let's look at the function <c>a -> f&lt;b&gt;</c> which "updates" <c>a</c> and replace <c>f</c> with <see cref="Const{TReal, TPhantom}"/>: <c>a -> Const&lt;a,a&gt;</c>. Here, we store the value of <c>a</c> in the real-part, and set the phantom-part (which is not present) to <c>a</c> as well. However, note that the whole lens becomes this: <c>(a -> Const&lt;a, b&gt;) -> s -> Const&lt;a, T&gt;</c>. The final <see cref="Const{TReal, TPhantom}"/> contains <c>t</c> as the phantom-part, meaning it doesn't actually have a value of type <c>t</c>, but <c>a</c> as its real-part. We thus use <see cref="Const{TReal, TPhantom}"/> to "smuggle out" the inner <c>a</c>.
    /// We can also create a lens which can work as both a getter and a setter via <see cref="IMonoLens{S, A}"/>, where <c>f</c> is any <see cref="IFunctor{TSource}"/>. Here, <c>f</c> must be <em>universally quantified</em>, meaning it must be able to use and produce any functor that the caller desires.
    /// <h1>Using this class</h1>
    /// <para>
    /// All methods in this class are static and thread-safe, the class has no internal state. You can create lenses, getters, setters, etc. via the methods of the <see cref="Make"/>-class and transform them using the methods of the <see cref="Do"/>-class. Methods to consume lenses are in this class directly.
    /// </para>
    /// <h1>Lens laws</h1>
    /// A lens must obey the following laws:
    /// <code>
    ///     //1. You get what you put in<br />
    ///     Lens.Get(L, Lens.Set(L, X, V)) == X<br />
    ///     //2. Putting back what you got doesn't change anything<br />
    ///     Lens.Set(L, X, Lens.Get(L, X)) == X<br />
    ///     //3. Setting twice is the same thing as setting once<br />
    ///     Lens.Set(L, Lens.Set(L, X, V2), V1) == Lens.Set(L, X, V2)<br />
    /// </code>
    /// <h1>Hierarchy of lenses</h1>
    /// <para>
    /// One basic type of lenslike is a <em>Getter</em>, which takes a structure <c>s</c> and retrieves some value <c>a</c> from it. Getters are consume via the <see cref="Get{S, A}(IGetter{S, A}, S)"/>-function.
    /// The counterpart is a <em>Setter</em>, which takes a structure <c>s</c>, replaces some part <c>a</c> in it with a part <c>b</c>, and returns the structure <c>t</c> (an example being a setter which replaces the strings in a string-list with integers, resulting in an integer-list).
    /// A combined getter and setter is called a <em>Lens</em>, which supports both getting and changing a specific part of a structure.
    /// A specialized setter is a <em>Prism</em>, which "may work": if the part <c>a</c> is present in the input <c>s</c>, it gets replaced with <c>b</c>, but if not, then the input remains unchanged. An example is a prism which sets the left-value of an <see cref="Either{TLeft, TRight}"/> but which does nothing if the <see cref="Either{TLeft, TRight}"/> contains a right-value.
    /// </para>
    /// 
    /// todo: fold, traversal
    /// 
    /// <h1>Creating lenses</h1>
    /// <para>todo</para>
    /// <h1>Composing lenses</h1>
    /// <para>Two lenses <c>L1</c> and <c>L2</c> can actually be composed using function composition, meaning <c>L1(L2(x))</c>. However, since the "raw" functions are not exposed for technical reasons, one can use the "Then"-methods of this class instead, which just perform function-composition in the background. To compose two getters into a single getter, where <c>L1</c> gets, for instance, a string from a list of strings, and where <c>L2</c> retrieves a substring from a string, one would write <c>var subStringFromStringList = L1.Then(L2)</c>.
    /// <h1>Mutating and well-behaved lenses</h1>
    /// Technically, lenses should not mutate their arguments but only return results that reflect changes, but as a matter of practicality, some lenses do. These are noted in this class as <em>mutating</em>, an example being <see cref="Make.IndexSetter{S, A, TKey}"/>, which mutates a dictionary in-place. We call lenses which obey the lens-laws and do not mutate their inputs <em>well-behaved</em>, and lenses which aren't explicitly said to be mutating in this class are well-behaved.
    /// </para>
    /// 
    /// <h1>Executing lenses</h1>
    /// <para>todo</para>
    /// <h1>History</h1>
    /// <para>Lenses developed over time and the result of the work of Jeremy Gibbons, Luke Palmer, Twan van Laarhoven, Rossel O'Connor, and Edward Kmett, as detailed in https://github.com/ekmett/lens/wiki/History-of-Lenses .<br />
    /// The structure of the library is taken (with simplifications) from Edward Kmett's lens-library: https://github.com/ekmett/lens.
    /// </para>
    /// </remarks>
    public static partial class L
    {
        /// <summary>
        /// Contains methods for creating getters, setters, lenses, prisms, etc.
        /// </summary>
        public static partial class Make
        {
        }

        /// <summary>
        /// Contains methods for transforming lenses.
        /// </summary>
        public static partial class Do
        {
        }
    }
}

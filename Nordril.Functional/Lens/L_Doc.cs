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
    /// The hierarchy of lenses is this:
    /// Fold &lt;- Getter, MonoTraversal<br />
    /// Setter &lt;- Traversal<br />
    /// Getter &lt;- MonoLens<br />
    /// Traversal &lt;- Lens, Prism<br />
    /// Lens &lt;- MonoLens<br />
    /// There are also Reviews, Isos and Equalities, but they are currently not implemented.
    /// </para>
    /// <para>
    /// <see cref="IFold{S, A}"/> can retrieve 0 to more values from a container. <see cref="IFold{S, A}"/>s mainly consume their inputs via TODO.
    /// A special case of <see cref="IFold{S, A}"/> is <see cref="IGetter{S, A}"/>, which retrieves exactly one value from a container.
    /// The opposite is an <see cref="ISetter{S, T, A, B}"/>, which replaces some part of its container with an updated value.
    /// The special case of an <see cref="ISetter{S, T, A, B}"/> is an <see cref="ITraversal{S, T, A, B}"/>, which can change multiple elements in its container, elementwise.
    /// An <see cref="ILens{S, T, A, B}"/> can serve as both an <see cref="ITraversal{S, T, A, B}"/> (and therefore an <see cref="ISetter{S, T, A, B}"/>) and, if it's an <see cref="IMonoLens{S, A}"/>, also an <see cref="IGetter{S, A}"/>. Thus, an <see cref="IMonoLens{S, A}"/> is a combined <see cref="IGetter{S, A}"/> and <see cref="ISetter{S, T, A, B}"/> which can, say, get and update a field in a class.
    /// We also have <see cref="IPrism{S, T, A, B}"/>, which is the "opposite" of an <see cref="ILens{S, T, A, B}"/>: whereas an <see cref="ILens{S, T, A, B}"/> can get a part from a whole, an <see cref="IPrism{S, T, A, B}"/> can assemble a part into a whole or, equivalently, try to get a part from the whole if it's the right type. An <see cref="IPrism{S, T, A, B}"/> is thus an <see cref="ILens{S, T, A, B}"/> which "may fail" in trying to get the part. To illustrate this, consider tuples and <see cref="Either{TLeft, TRight}"/>: we can write an <see cref="ILens{S, T, A, B}"/> for a tuple which gets and can change, say, the first element, but we can't write one for the left-case of <see cref="Either{TLeft, TRight}"/>, since the <see cref="Either{TLeft, TRight}"/> may contain a right-case. However, we can write an <see cref="IPrism{S, T, A, B}"/>, which tries to get the left-case (failing with <see cref="Maybe.Nothing{T}"/> if it's a right-case) and, in the opposite direction, can turn a value into an <see cref="Either{TLeft, TRight}"/> but creating a left-case.
    /// </para>
    /// <h1>Creating lenses</h1>
    /// <para>
    /// All lenses can be created via the methods of <see cref="L.Make"/>, with the most common cases being:
    /// <list type="bullet">
    ///     <item><see cref="L.Make.Getter{S, A}(Func{S, A})"/> to create a getter from a function which gets a part <c>A</c> from a hole <c>S</c>, </item>
    ///     <item><see cref="L.Make.Setter{S, A}(Func{Func{A, A}, Func{S, S}})"/> to create a setter from a function which takes an updater (<c>A to A</c>) and uses it to update the whole <c>S</c>, </item>
    ///     <item><see cref="L.Make.Lens{S, A}(Func{S, A}, Func{S, A, S})"/> to create a lens from a getting-function and an updating-function which takes a whole <c>S</c>, the new value <c>A</c>, and returns a new <c>S</c>, </item>
    ///     <item><see cref="L.Make.Prism{S, A, B}(Func{B, S}, Func{S, Maybe{A}})"/> to create a prism from an compose-function which can turn a <c>B</c> into a whole <c>S</c>, and an extraction-function which tries to get a part <c>A</c> from a whole <c>S</c>, </item>
    ///     <item><see cref="L.Make.Traversal{S, A}"/>, which create a traversal for a type which implements <see cref="ITraversable{TSource}"/> with contents <c>A</c>, </item>
    ///     <item><see cref="L.Make.Folding{TFoldable, S, A}(Func{S, TFoldable})"/> which creates a fold from a function which maps a whole <c>S</c> into an instance of <see cref="IFoldable{TSource}"/> (<see cref="IFoldable{TSource}"/> is basically the same as <see cref="IEnumerable{T}"/>).</item>
    /// </list>
    /// </para>
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
    /// <h1>Technical background and additional information</h1>
    /// <para>
    /// Formally, a lenslike object (including folds, traversals, getters, lenses, etc.) is just a function of the following type:
    /// <code>
    /// type Lenslike&lt;p,f,s,t,a,b&gt; = Func&lt;p&lt;a,f&lt;b&gt;&gt;,p&lt;s,f&lt;t&gt;&gt;
    /// </code>
    /// Depending on the choice of <c>p</c> and <c>f</c>, we get a fold, traversal, getter, etc. Of course, this looks quite opaque, so let's insert concrete types: for <c>p</c>, which has to be a profunctor (<see cref="Func{T, TResult}"/> is a profunctor, meaning you can prepend a function and append another to it), we insert <see cref="Func{T, TResult}"/>, and for <c>f</c>, we insert <see cref="Identity{T}"/>. This then becomes:
    /// <code>
    /// type Lenslike&lt;s,t,a,b&gt; = Func&lt;Func&lt;a, Identity&lt;b&gt;&gt;, Func&lt;s,Identity&lt;t&gt;&gt;&gt;
    /// </code>
    /// <see cref="Identity{T}"/> does nothing and just stores a value, so conceptually, we can omit it and we have:
    /// <code>
    /// type Lenslike = Func&lt;Func&lt;a, b&gt;, Func&lt;s,t&gt;&gt;
    /// </code>
    /// So now we have a function which takes an "updater" (a function which takes an <c>a</c> and returns a <c>b</c>) and then gives us a function which takes an <c>s</c> and returns a <c>t</c>. Let's further say that <c>s = t = Person</c> and <c>a = b = string</c> (the person's name):
    /// <code>
    /// type Lenslike = Func&lt;Func&lt;string, string&lt;, Func&lt;Person, Person&gt;&gt;
    /// </code>
    /// This is a concrete setter which updates a person's name: we take an updating-function (which can do any update on the name and is chosen by <see cref="L.Set{S, T, A, B}(ISetter{S, T, A, B}, S, B)"/>), a <c>Person</c>, and returns a new <c>Person</c>.
    /// If we had chosen <see cref="Const{TReal, TPhantom}"/> for <c>f</c> instead, we would have an <see cref="IGetter{S, A}"/>: with <c>Const&lt;A,B&gt;</c> and <c>Const&lt;A, T&gt;</c>we don't actually store a <c>b</c> or a <c>t</c>, but only have the <c>a</c> (the first type-variable) as a real value, which we "smuggle out" via the <see cref="Const{TReal, TPhantom}"/>-functor. In general, different choices for <c>p</c> and <c>f</c> give us different types of lenslike objects, and often, we don't even have concrete <c>p</c> and <c>f</c>, but only generic type variables with constraints on them like <c>f : IFunctor</c> but we obviously cannot write a delegate like <c>Lenslike</c> in C#, since it's a <em>universally quantified rank-2 type</em>, meaning that, in the case of <see cref="IMonoLens{S, A}"/>, say, the lens must be able to return any functor that the called requests, not just some <see cref="IFunctor{TSource}"/> of its choosing. Furthermore, the concrete <see cref="IFunctor{TSource}"/>-type is not part of the type-signature of <see cref="IMonoLens{S, A}"/> (hence "rank-2").
    /// </para>
    /// <para>
    /// Lenses come from Haskell and the main source of information is the https://github.com/ekmett/lens repo, which also includes addition information. There is also a tutorial-package at https://hackage.haskell.org/package/lens-tutorial-1.0.4/docs/Control-Lens-Tutorial.html by Gabriel Gonzalez.
    /// </para>
    /// </remarks>
    /// <example>
    /// Suppose we have a class <c>Person</c> which contains a name and a list of names of friends.
    /// <code>
    /// public class Person
    /// {
    ///    public string Name { get; set; }
    ///    public FuncList&lt;string&gt; Friends { get; set; }
    /// }
    /// </code>
    /// <para>
    /// We now create a lens for the person's name and a fold and traversal for the person's list of friends. We create a well-behaved lens which does not update the original person when run.
    /// <code>
    /// var bill = new Person { Name = "bill", Friends = FuncList.Make("adam", "tiffany", "zack") };
    /// 
    /// var nameLens = L.Make.Lens&lt;Person, string&gt;(p =&gt; p.Name, (p, n) =&gt; new Person { Name = n; Friends = Friends.MakeFunctList();
    /// var friendsFold = L.Make.Folding(p =&gt; p.Friends);
    /// var friendsTraversal = L.Make.Traversal&lt;FuncList&lt;string&gt;, int&gt;();
    /// //Usage:
    /// //We get the name of bill.
    /// Console.WriteLine(L.Get(nameLens, bill));
    /// 
    /// //We change  bill's name to sophie and get it (the original bill remains unchanged).
    /// var sophie = L.Set(nameLens, bill, "sophie");
    /// Console.WriteLine(L.Get(nameLens, sophie));
    /// 
    /// //We go through sophie's friends and fail if "big tom" is included, and we change each friend's name from "x" to "big x".
    /// var bigSophie = L.Traverse(friendsTraversal), f =&gt; Maybe.JustIf(f != "big tom", () =&gt; "big " + f), sophie).ToMaybe().Value();
    /// bigSophie.Friends.ForEach(f =&gt; Console.WriteLine(f));
    /// 
    /// //We now add "big tom" to bigSophie's friends (FuncList.Add is not a pure operation, but for example's sake).
    /// L.Update(friendsTraversal, bigSophie, fs =&gt; fs.Add("big tom"));
    /// 
    /// //We run the traversal again, but this time, we get no new person back, since it included "big tom":
    /// bigSophieFailed = L.Traverse(friendsTraversal), f =&gt; Maybe.JustIf(f != "big tom", () =&gt; "big " + f), sophie).ToMaybe();
    /// Console.WriteLine(bigSophieFailed.HasValue);
    /// 
    /// //Output:
    /// bill
    /// sophie
    /// big adam
    /// big tiffany
    /// big zack
    /// false
    /// 
    /// </code>
    /// </para>
    /// </example>
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

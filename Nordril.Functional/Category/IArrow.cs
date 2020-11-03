using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional.Category
{
    /// <summary>
    /// A monoid in the category of strong profunctor whose morphisms are strong natural transformations, meaning the following:
    /// <code>
    /// X.LiftFirst().Then(f) == f.Then(X.LiftFirst()) (commutativity)<br />
    /// </code>
    /// The stereotypical example of an instance is <see cref="Func{T, TResult}"/> (wrapped in <see cref="Fun{TIn, TOut}"/>), with the methods having the "obvious" implementations.
    /// </summary>
    /// <typeparam name="TNeed">The type of values the arrow needs.</typeparam>
    /// <typeparam name="THave">The type of values the arrow contains.</typeparam>
    public interface IArrow<TNeed, THave>
        : ICategory<TNeed, THave>
        , IStrong<TNeed, THave>
    {
        /// <summary>
        /// Turns a function into an arrow. Implementors MUST NOT access the this-pointer and must obey the following:
        /// <code>
        /// X.Make(f) == X.Id().Promap(x => x, f)<br />
        /// </code>
        /// </summary>
        /// <typeparam name="TNeedResult">The type of the input of <paramref name="f"/>.</typeparam>
        /// <typeparam name="THaveResult">The type of the output of <paramref name="f"/>.</typeparam>
        /// <param name="f">The function to turn into an arrow.</param>
        IArrow<TNeedResult, THaveResult> Make<TNeedResult, THaveResult>(Func<TNeedResult, THaveResult> f);

        /// <summary>
        /// Combines two arrows into an arrow which runs its first tuple-component through <c>this</c> and the second through <paramref name="that"/>. Also known as <c>(***)</c>. Implementors must obey the following:
        /// <code>
        /// X.TogetherWith(Y) == X.LiftFirst().Then(X.Make((x,y) => (y,x))).Then(Y.LiftFirst()).Then(X.Make((x,y) => (y,x)))
        /// </code>
        /// </summary>
        /// <typeparam name="TNeedRight">The type of values the second arrow needs.</typeparam>
        /// <typeparam name="THaveRight">The type of values the second arrow has.</typeparam>
        /// <param name="that"></param>
        /// <returns></returns>
        IArrow<(TNeed, TNeedRight), (THave, THaveRight)> TogetherWith<TNeedRight, THaveRight>(IArrow<TNeedRight, THaveRight> that);
    }

    /// <summary>
    /// Extension methods for <see cref="IArrow{TNeed, THave}"/>.
    /// </summary>
    public static class Arrow
    {
        /// <summary>
        /// Sends the same input to two arrows and returns the outputs of both as a tuple.
        /// Also known as <em>(&amp;&amp;&amp;)</em>
        /// </summary>
        /// <typeparam name="TNeed">The type of the input of the arrows.</typeparam>
        /// <typeparam name="THaveLeft">The type of the output of the first arrow.</typeparam>
        /// <typeparam name="THaveRight">The type of the output of the second arrow.</typeparam>
        /// <param name="arrow">The first arrow.</param>
        /// <param name="that">The second arrow.</param>
        public static IArrow<TNeed, (THaveLeft, THaveRight)> Fanout<TNeed, THaveLeft, THaveRight>(this IArrow<TNeed, THaveLeft> arrow, IArrow<TNeed, THaveRight> that)
            => arrow.Make<TNeed, (TNeed, TNeed)>(x => (x, x)).Then(arrow.TogetherWith(that)) as IArrow<TNeed, (THaveLeft, THaveRight)>;

        /// <summary>
        /// Combines two arrows into one which can take the input of either one. Also known as <em>(+++)</em>
        /// </summary>
        /// <typeparam name="TNeedLeft">The type of the input of the first arrow.</typeparam>
        /// <typeparam name="THaveLeft">The type of the output of the first arrow.</typeparam>
        /// <typeparam name="TNeedRight">The type of the input of the second arrow.</typeparam>
        /// <typeparam name="THaveRight">The type of the output of the second arrow.</typeparam>
        /// <param name="arrow">The first arrow.</param>
        /// <param name="that">The second arrow.</param>
        public static IArrowChoice<Either<TNeedLeft, TNeedRight>, Either<THaveLeft, THaveRight>> EitherOr<TNeedLeft, THaveLeft, TNeedRight, THaveRight>(this IArrowChoice<TNeedLeft, THaveLeft> arrow, IArrowChoice<TNeedRight, THaveRight> that)
            => (arrow.ChooseLeft<TNeedRight>().ToArrowChoice()).Then(that.ChooseRight<THaveLeft>().ToArrowChoice()).ToArrowChoice();

        /// <summary>
        /// Unsafely casts an <see cref="ICategory{TNeed, THave}"/> to an <see cref="IArrowChoice{TNeed, THave}"/>.
        /// </summary>
        /// <typeparam name="TNeed">The type of values the arrow needs.</typeparam>
        /// <typeparam name="THave">The type of values the arrow contains.</typeparam>
        /// <param name="arrow">The object to cast.</param>
        public static IArrowChoice<TNeed, THave> ToArrowChoice<TNeed, THave>(this ICategory<TNeed, THave> arrow) => (IArrowChoice<TNeed, THave>)arrow;

        /// <summary>
        /// Unsafely casts an <see cref="IChoice{TNeed, THave}"/> to an <see cref="IArrowChoice{TNeed, THave}"/>.
        /// </summary>
        /// <typeparam name="TNeed">The type of values the arrow needs.</typeparam>
        /// <typeparam name="THave">The type of values the arrow contains.</typeparam>
        /// <param name="arrow">The object to cast.</param>
        public static IArrowChoice<TNeed, THave> ToArrowChoice<TNeed, THave>(this IChoice<TNeed, THave> arrow) => (IArrowChoice<TNeed, THave>)arrow;

        /// <summary>
        /// Unsafely casts an <see cref="ICategory{TNeed, THave}"/> to an <see cref="IArrow{TNeed, THave}"/>.
        /// </summary>
        /// <typeparam name="TNeed">The type of values the arrow needs.</typeparam>
        /// <typeparam name="THave">The type of values the arrow contains.</typeparam>
        /// <param name="arrow">The object to cast.</param>
        public static IArrow<TNeed, THave> ToArrow<TNeed, THave>(this ICategory<TNeed, THave> arrow) => (IArrow<TNeed, THave>)arrow;

        /// <summary>
        /// Unsafely casts an <see cref="IProfunctor{TNeed, THave}"/> to an <see cref="IArrow{TNeed, THave}"/>.
        /// </summary>
        /// <typeparam name="TNeed">The type of values the arrow needs.</typeparam>
        /// <typeparam name="THave">The type of values the arrow contains.</typeparam>
        /// <param name="arrow">The object to cast.</param>
        public static IArrow<TNeed, THave> ToArrow<TNeed, THave>(this IProfunctor<TNeed, THave> arrow) => (IArrow<TNeed, THave>)arrow;

        /// <summary>
        /// Creates an arrow that accepts the input of either of two arrows and returns the output (with both arrows having the same output type).
        /// Also known as <em>(|||)</em>.
        /// </summary>
        /// <typeparam name="TNeedLeft">The type of the result of the first arrow.</typeparam>
        /// <typeparam name="TNeedRight">The type of the result of the second arrow.</typeparam>
        /// <typeparam name="THave">The type of the result.</typeparam>
        /// <param name="arrow">The first arrow.</param>
        /// <param name="that">The second arrow.</param>
        public static IArrowChoice<Either<TNeedLeft, TNeedRight>, THave> Fanin<TNeedLeft, TNeedRight, THave>(this IArrowChoice<TNeedLeft, THave> arrow, IArrowChoice<TNeedRight, THave> that)
            => arrow.EitherOr(that).Then(arrow.Make<Either<THave, THave>, THave>(e => e.Coalesce(x => x, x => x))).ToArrowChoice();
    }
}

using Nordril.Functional.Category;
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
            /// Makes a prism from a <paramref name="get"/>-function which tries to get the value <typeparamref name="A"/> from <typeparamref name="S"/>,
            /// and a <paramref name="compose"/>-function, which puts the result <typeparamref name="B"/> back into a <typeparamref name="T"/> if <paramref name="get"/> succeeded.
            /// </summary>
            /// <typeparam name="S">The type of the input container.</typeparam>
            /// <typeparam name="T">The type of the output container.</typeparam>
            /// <typeparam name="A">The type of the value to get.</typeparam>
            /// <typeparam name="B">The type of the result inside <typeparamref name="T"/>.</typeparam>
            /// <param name="compose">The function which puts the result back into a structure and returns the result if <paramref name="get"/> was successful.</param>
            /// <param name="get">The function which tries to get part of the structure.</param>
            public static IPrism<S, T, A, B> Prism<S, T, A, B>(Func<B, T> compose, Func<S, Either<T, A>> get)
            {
                /* Ok, so what this does:
                 * 1. It lifts the profunctor g from (A -> F<B>) into (Either<T,A> -> Either<T, IApplicative<B>>).
                 *    Note that "->" does not mean "function application", though you can think of it as that, since Func<,> is a profunctor.
                 * 2. Then it prepends the get-function (which takes the S and returns Either<T,A>) to it.
                 * 3. Then it appends the coalesce-function to it. We get an Either<T, IApplicative<B> as a "result", so we have two cases:
                 *    3.1. it's a left T and we're basically done. We just wrap it in the required applicative.Pure, which is of the required
                 *         concrete applicative-type, via PureUnsafe (this means we can deliver any applicative desired and the whole thing is universally
                 *         quantified).
                 *    3.2. it's a right IApplicative<B>, in which case we map it's contained B via the compose-function to get the T back.
                 *    
                 * So let's take the conceptually simpler case of Profunctor<S, Either<T,A>> ~ Func<S, Either<T,A>>:
                 * 1. We create the function (Either<T,A> -> Either<T, IApplicative<B>>) which runs the original g on the right and does nothing if it's a left.
                 * 2. Then we run get to try and extract the Either<T,A>> from S (let's say S ~ Either<int, bool> and get tries to get the int).
                 * 3.1. Then, if we a left T, get failed and we wrap that T into Applicative.Pure.
                 * 3.2. If we have right B, we run compose on it to get the T.
                 * 
                 * We can also have Profunctor ~ Tagged, though, we allows us to *get* Either<int, bool>.Left from an int.
                 * (Whatever g returns is the profunctor we use; see L.Review and L.TryGet).
                 * 1. We have again lift g from Tagged<A, IApplicative<B>> into Tagged<Either<T,A>, Either<T, IApplicative<B>>>.
                 *    HOWEVER, Tagged does not depend on Either<T,A>, as it's a kind of "fake constant function", and it always "returns" (really, "stores as a constant) the IApplicative<B>> - thus, when we "prepend" the get-function, it does not matter what it returns. We always get the "right-case" back). Thus, we basically fake get being successful.
                 * 2. Since the lifted g, prepended with get, has simulated that a successful retrieval, we always go into the right-case of calling compose.
                 * 
                 * We can therefore use a Prism as not just a "may fail"-getter, but also in reverse, as a smart constructor:
                 * We have the get-function to get Either.Left from Either<int, bool>, say, and a compose-function, which goes from int to Either<int, bool>.
                 * By using Tagged to *always* return the int and, in fact, to only "take" an input in form of a phantom-type, we go straight to compose
                 * to assemble the int back into Either<int, bool>.
                 */

                var prism = new Prism<S, T, A, B>(applicativeType =>
                    g => (IChoice<S, IApplicative<T>>)g
                        .ChooseRight<T>()
                        .Promap(get, e => e.Coalesce(l => l.PureUnsafe(applicativeType), r => (IApplicative<T>)r.Map(compose))));

                return prism;
            }

            /// <summary>
            /// Makes a prism from a <paramref name="get"/>-function which tries to get the value <typeparamref name="A"/> from <typeparamref name="S"/>,
            /// and a <paramref name="compose"/>-function, which puts the result <typeparamref name="B"/> back into a <typeparamref name="S"/> if <paramref name="get"/> succeeded.
            /// </summary>
            /// <typeparam name="S">The type of the container.</typeparam>
            /// <typeparam name="A">The type of the value to get.</typeparam>
            /// <typeparam name="B">The type of the result inside <typeparamref name="S"/>.</typeparam>
            /// <param name="compose">The function which puts the result back into a structure and returns the result if <paramref name="get"/> was successful.</param>
            /// <param name="get">The function which tries to get part of the structure.</param>
            public static IPrism<S, S, A, B> Prism<S, A, B>(Func<B, S> compose, Func<S, Maybe<A>> get)
                => Prism<S, S, A, B>(compose, s => get(s).ValueOrLazy(a => Either.FromRight<S, A>(a), () => Either.FromLeft<S, A>(s)));
        }
    }
}

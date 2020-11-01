using Nordril.Functional.Category;
using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nordril.Functional.Lens
{
    public static partial class L
    {
        public static partial class Make
        {
            /// <summary>
            /// Create an <see cref="IFold{S, A}"/> for a type which implements <see cref="IFoldable{TSource}"/>
            /// </summary>
            /// <typeparam name="TFoldable">The result of the inner function, which is foldable.</typeparam>
            /// <typeparam name="S">The type of the data structure to fold.</typeparam>
            /// <typeparam name="A">The type of the resulting elements.</typeparam>
            /// <param name="toFoldable">The getter which returns an <see cref="IFoldable{TSource}"/> from <typeparamref name="S"/>.</param>
            public static IFold<S, A> Folding<TFoldable, S, A>(Func<S, TFoldable> toFoldable)
                where TFoldable : IFoldable<A>
            {
                return new Fold<S, A>(t => 
                {
                    var unitT = Applicative.PureUnsafe<A>(default, t).Map(_ => new Unit()).GetType();
                   
                    //We can map to default(S) because Contrariant+Functor implies that S is only a phantom type.
                    return g => s => toFoldable(s).TraverseDiscard(unitT, g).Map(_ => default(S)) as IContravariantApplicative<S>;
                });
            }

            /// <summary>
            /// Create an <see cref="IFold{S, A}"/> for a type which implements <see cref="IFoldable{TSource}"/>
            /// </summary>
            /// <typeparam name="S">The type of the foldable data structure to fold.</typeparam>
            /// <typeparam name="A">The type of the resulting elements.</typeparam>
            public static IFold<S, A> Folding<S, A>()
                where S : IFoldable<A>
            {
                return new Fold<S, A>(t =>
                {
                    var unitT = Applicative.PureUnsafe<A>(default, t).Map(_ => new Unit()).GetType();

                    //We can map to default(S) because Contravariant+Functor implies that S is only a phantom type.
                    return g => s => s.TraverseDiscard(unitT, g).Map(_ => default(S)) as IContravariantApplicative<S>;
                });
            }
        }
    }
}

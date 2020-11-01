using Nordril.Functional.Algebra;
using Nordril.Functional.Category;
using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional.Lens
{
    /// <summary>
    /// A folding-operation which allows us to get multiple results (or 0) from a data structure.
    /// </summary>
    /// <typeparam name="S">The type of the input data structure.</typeparam>
    /// <typeparam name="A">The type of the objects to get in <typeparamref name="S"/>.</typeparam>
    public interface IFold<S, A>
    {
        /// <summary>
        /// Returns the fold in CPS-form, strongly tyed to <see cref="Const{TMonoid, TReal, TPhantom}"/>.
        /// </summary>
        Func<Func<A, Const<TMonoid, R, A>>, Func<S, Const<TMonoid, R, S>>> FoldFuncConst<TMonoid, R>()
            where TMonoid : IMonoid<R>;

        /// <summary>
        /// Returns the fold in CPS-form, strongly typed.
        /// </summary>
        Func<Func<A, FA>, Func<S, FS>> FoldFunc<FA, FS>()
            where FA : IContravariantApplicative<A>
            where FS : IContravariantApplicative<S>;

        /// <summary>
        /// Returns the fold in CPS-form.
        /// </summary>
        Func<Func<A, IContravariantApplicative<A>>, Func<S, IContravariantApplicative<S>>> FoldFunc(Type t);
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional.Lens
{
    //can't be done because (Applicative<Const>) requires a forall'd monoid-instance on the real value (rank 2 type)
    /*public interface IFold<S, A, FA, FS> : ILenslike<S, S, A, A, FA, FS>
        where FA : IContravariant<A>, IApplicative<A>
        where FS : IContravariant<S>, IApplicative<S>
    {
    }*/
}

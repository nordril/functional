using System;
using System.Collections.Generic;
using System.Text;

/*namespace Nordril.Functional.Lens
{
    //WIP
    class Lens
    {
        public static IGetter<S,A> Path<S,A>(params IGetter<S,A>[] xs)
        {
            throw new NotImplementedException();
        }

        public static ILens<S,T,A,B> Path<S,T,A,B>(params ILens<S,T,A,B>[] xs)
        {
            throw new NotImplementedException();
        }
    }

    interface IFold<S,A>
    {

    }

    interface IGetter<S,A>
    {
        A get(S x);
    }

    public struct Getter<S, A> : IGetter<S, A>
    {
        private readonly Func<S, A> f;

        public Getter(Func<S,A> f)
        {
            this.f = f;
        }

        public A get(S x)
        {
            throw new NotImplementedException();
        }
    }

    interface ISetter<S,T,A,B>
    {
        T over(Func<A, B> f, S x);
    }

    interface ITraversal<S,T,A,B>
    {

    }

    interface IMonoTraversal<S,A> : IFold<S,A>, ITraversal<S,S,A,A>
    {

    }

    interface ILens<S,T,A,B> : ITraversal<S,T,A,B>
    {

    }

    interface IMonoLens<S,A> : ILens<S,S,A,A>, IMonoTraversal<S,A>, IGetter<S,A>
    {

    }

    interface IReview<S,A>
    {

    }

    interface IPrism<S,T,A,B> : ITraversal<S,T,A,B>
    {

    }

    interface IMonoPrism<S,A> : IPrism<S,S,A,A>
    {

    }

    interface IIso<S,T,A,B> : IPrism<S,T,A,B>, ILens<S,T,A,B>
    {

    }

    interface IMonoIso<S,A> : IMonoLens<S,A>, IIso<S,S,A,A>, IMonoPrism<S,A>, IMonoTraversal<S,A>
    {

    }

    interface IEquality<S,T,A,B> : IIso<S,T,A,B>
    {

    }

    interface IMonoEquality<S,A> : IEquality<S,S,A,A>, IMonoPrism<S,A>, IMonoLens<S,A>
    {

    }
}*/

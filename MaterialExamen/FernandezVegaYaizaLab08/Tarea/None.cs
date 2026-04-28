using System;

namespace Tarea;

public class None<TData> : Maybe<TData>
{
    public void Match(Action<TData> some, Action none) => none();

    public TResult Match<TResult>(Func<TData, TResult> some, Func<TResult> none) 
        => none(); 

    public Maybe<TResult> AndThen<TResult>(Func<TData, TResult> f) 
        => new None<TResult>(); 

    public TData OrElse(TData defaultValue) => defaultValue; 
}

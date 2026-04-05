using System;

namespace Tarea;

public interface Maybe<TData>
{
    void Match(Action<TData> some, Action none);
    TResult Match<TResult>(Func<TData, TResult> some, Func<TResult> none);
    Maybe<TResult> AndThen<TResult>(Func<TData, TResult> f);
    TData OrElse(TData defaultValue);
}

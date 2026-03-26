public class Some<TData> : Maybe<TData>
{
    public TData Value { get; }

    public Some(TData value) => Value = value;

    public void Match(Action<TData> some, Action none) => some(Value);

    public TResult Match<TResult>(Func<TData, TResult> some, Func<TResult> none)
        => some(Value);

    public Maybe<TResult> AndThen<TResult>(Func<TData, TResult> f)
        => new Some<TResult>(f(Value));

    public TData OrElse(TData defaultValue) => Value;
}
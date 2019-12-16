using System;

namespace MondadsInCsharp
{
    /// <summary>
    /// Value representing a maybe result.
    /// </summary>
    /// <typeparam name="T">The type of the maybe value</typeparam>
    public abstract class Maybe<T> { }

    /// <summary>
    /// Result that contains no value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Nothing<T> : Maybe<T> { }

    /// <summary>
    /// Result that contains a value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Just<T> : Maybe<T>
    {
        public T Value { get; }

        public Just(T value)
        {
            Value = value;
        }
    }

    public static class MaybeExtensions
    {
        public static Maybe<TOut> fmap<TIn, TOut>(this Maybe<TIn> maybe, Func<TIn, TOut> func)
        {
            switch (maybe)
            {
                case Nothing<TIn> _: return new Nothing<TOut>();
                case Just<TIn> just: return new Just<TOut>(func(just.Value));
                default: throw new Exception("Do not derive from Maybe yourself!");
            }
        }

        public static Maybe<TOut> fmap<TIn, TOut>(this Func<TIn, TOut> func, Maybe<TIn> maybe) => maybe.fmap(func);

        public static Maybe<TOut> ab<TArg, TOut>(this Maybe<Func<TArg, TOut>> maybe, Maybe<TArg> maybe_arg)
        {
            switch (maybe)
            {
                case Nothing<Func<TArg, TOut>> _: return new Nothing<TOut>();
                case Just<Func<TArg, TOut>> just:
                    switch (maybe_arg)
                    {
                        case Nothing<TArg> _: return new Nothing<TOut>();
                        case Just<TArg> arg:
                            return new Just<TOut>(just.Value(arg.Value));
                        default: throw new Exception("Do not derive from Maybe yourself!");
                    }
                default: throw new Exception("Do not derive from Maybe yourself!");
            }
        }

        public static Maybe<TOut> bind<TIn, TOut>(this Maybe<TIn> maybe, Func<TIn, Maybe<TOut>> func)
        {
            switch (maybe)
            {
                case Nothing<TIn> _: return new Nothing<TOut>();
                case Just<TIn> just: return func(just.Value);
                default: throw new Exception("Do not derive from Maybe yourself!");
            }
        }

        public static Maybe<T> pure<T>(this T value)
        {
            return new Just<T>(value);
        }
    }
}
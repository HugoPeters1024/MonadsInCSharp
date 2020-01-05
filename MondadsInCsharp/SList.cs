using System;

namespace MondadsInCsharp
{
    public abstract class SList<T>
    {
        public static SList<T> operator +(SList<T> a, SList<T> b) => a.concat(b);

        public static Func<T, Func<SList<T>, SList<T>>> mkList() => h => tl => new Cons<T>(h, tl);
    }

    public class Empty<T> : SList<T>
    {
    }

    public class Cons<T> : SList<T>
    {
        public T Head { get; }
        public SList<T> Tail { get; } 
        
        public Cons(T head, SList<T> tail)
        {
            Head = head;
            Tail = tail;
        }
    }

    public static class SListExtensions
    {
        public static SList<T> concat<T>(this SList<T> one, SList<T> two)
        {
            switch ((one, two))
            {
                case (Empty<T> _, _): return two;
                case (_, Empty<T> _): return one;
                default:
                    var left = one as Cons<T>;
                    return new Cons<T>(left.Head, left.Tail.concat(two));
            }
        }

        public static SList<(T1, T2)> zip<T1, T2>(this SList<T1> one, SList<T2> two)
        {
            switch ((one, two))
            {
                case (Empty<T1> _, _): return new Empty<(T1, T2)>(); 
                case (_, Empty<T2> _): return new Empty<(T1, T2)>();
                default:
                    var left = one as Cons<T1>;
                    var right = two as Cons<T2>;
                    return new Cons<(T1, T2)>((left.Head, right.Head), left.Tail.zip<T1, T2>(right.Tail));
            }
        }

        public static SList<TOut> fmap<TIn, TOut>(this SList<TIn> list, Func<TIn, TOut> func)
        {
            switch (list)
            {
                case Empty<TIn> _: return new Empty<TOut>();
                case Cons<TIn> el: return new Cons<TOut>(func(el.Head), fmap(el.Tail, func));
                default: return null;
            }
        }

        public static SList<TOut> fmap<TIn, TOut>(this Func<TIn, TOut> func, SList<TIn> list)
            => list.fmap(func);

        public static SList<TOut> ab<TIn, TOut>(this SList<Func<TIn, TOut>> list, SList<TIn> arg) 
            => list.zip(arg).fmap(t => t.Item1(t.Item2));

        public static SList<TOut> bind<TIn, TOut>(this SList<TIn> list, Func<TIn, SList<TOut>> func)
        {
            switch (list)
            {
                case Empty<TIn> _: return new Empty<TOut>();
                case Cons<TIn> el: return func(el.Head).concat(el.Tail.bind(func));
                default: return null;
            }
        }

        public static SList<T> pureSList<T>(this T item)
        {
            return new Cons<T>(item, new Empty<T>());
        }
    }
}
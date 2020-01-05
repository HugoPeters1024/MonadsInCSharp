using System;
using System.Diagnostics;

namespace MondadsInCsharp.Demos
{
    public static class ListDemo
    {
        public static void PrintAll<T>(SList<T> list)
        {
            for (var prev = list; prev is Cons<T> el; prev = el.Tail)
            {
                Console.WriteLine(el.Head.ToString());
            }
        }

        public static SList<int> Double(SList<int> list)
        {
            return list.fmap(x => x + 1);
        }

        public static SList<string> ToWord(SList<int> list)
        {
            var words = new[] {"one", "two", "three", "four"};
            return list.fmap(x => x >= 0 && x < words.Length ? words[x] : "unknown number");
        }

        public static void CheckRightAssociativity()
        {
            var input = new Cons<int>(1, new Cons<int>(2, new Cons<int>(3, new Empty<int>())));
            var output = input.bind(x => x.pureSList());
            Debug.Assert(input == output);
        }

        public static SList<T> filter<T>(this SList<T> list, Func<T, bool> predicate)
            => list.bind(x => predicate(x) ? x.pureSList() : new Empty<T>());

        public static SList<(T,T)> cartesianProduct<T>(SList<T> one, SList<T> two)
        {
            return one.bind(x => two.bind(y => new Cons<(T, T)>((x, y), new Empty<(T, T)>())));
        }

        public static SList<TOut> listComprension<TIn, TOut>(this SList<TIn> list, Func<TIn, TOut> mapping,
            Func<TIn, bool> predicate)
            => list.bind(x =>
            {
                if (predicate(x))
                    return new Cons<TOut>(mapping(x), new Empty<TOut>()) as SList<TOut>;
                return new Empty<TOut>();
            });

        public static SList<int> sumLists(SList<int> one, SList<int> two)
        {
            Func<int, Func<int, int>> sumFunc = a => b => a + b;
            return sumFunc.fmap(one).ab(two);
        }
    }
}
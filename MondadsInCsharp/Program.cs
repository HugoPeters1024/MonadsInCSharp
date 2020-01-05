using System;
using MondadsInCsharp.Demos;

namespace MondadsInCsharp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var list = new Cons<int>(1, new Cons<int>(2, new Cons<int>(3, new Empty<int>())));
            var newlist = list.listComprension(x => 2 * x, x => x%2==0);
            ListDemo.PrintAll(newlist);
        }
    }
}
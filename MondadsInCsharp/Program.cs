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
            var newlist = ListDemo.sumLists(list, list); 
            ListDemo.PrintAll(newlist);
        }
    }
}
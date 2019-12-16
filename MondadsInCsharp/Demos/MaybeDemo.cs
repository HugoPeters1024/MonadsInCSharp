using System;

namespace MondadsInCsharp.Demos
{
    public static class PlusDemo
    {
        /// <summary>
        /// Returns a nested function, this way we can apply
        /// arguments one by one.
        /// </summary>
        public static Func<int, Func<int, int>> Plus()
        {
            return x => y => x + y;
        }

        public static Maybe<int> SometimesValue()
        {
            return new Just<int>(5);
        }

        public static Maybe<int> Demo()
        {
            return Plus().fmap(SometimesValue()).ab(SometimesValue());
        }
    }
    
    public static class DestroyDemo
    {
        public static Maybe<int> DestroyIfEven(Maybe<int> input)
        {
            return input.bind(x => x % 2 == 0 ? new Nothing<int>() : x.pure());
        }
    }

    public static class InputDemo
    {
        public class Person
        {
            public string FirstName { get; }
            public string LastName { get; }

            public Person(string firstName, string lastName)
            {
                FirstName = firstName;
                LastName = lastName;
            }

            public static Func<string, Func<string, Person>> Make()
            {
                return x => y => new Person(x, y);
            }
        }
        /// <summary>
        /// Asks a name from a user, names with less than 5 characters fail.
        /// </summary>
        /// <returns></returns>
        public static Maybe<string> GetName()
        {
            var input = Console.ReadLine();
            if (input.Length < 5)
                return new Nothing<string>();
            return new Just<string>(input);
        }

        /// <summary>
        /// Asks a user to name a person, fails if any of its components fail.
        /// </summary>
        /// <returns></returns>
        public static Maybe<Person> GetMaybePerson()
        {
            return Person.Make().fmap(GetName()).ab(GetName());
        }

        /// <summary>
        /// Asks a user repeatedly for a person until it succeeds.
        /// </summary>
        /// <returns></returns>
        public static Person GetPerson()
        {
            var maybePerson = GetMaybePerson();
            if (maybePerson is Just<Person> person)
                return person.Value;
            return GetPerson();
        }
    }
}
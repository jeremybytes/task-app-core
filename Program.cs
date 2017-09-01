using System;
using System.Threading;

namespace CoreTaskApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("One Moment Please");
            var repository = new PersonRepository();

            var peopleTask = repository.GetAsync(CancellationToken.None);

            peopleTask.ContinueWith(task =>
            {
                var people = task.Result;
                foreach(var person in people)
                    Console.WriteLine(person.ToString());
            });

            Console.ReadLine();
        }
    }
}

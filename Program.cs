using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CoreTaskApp
{
    class Program
    {
        private static CancellationTokenSource tokenSource;

        static void Main(string[] args)
        {
            tokenSource = new CancellationTokenSource();

            Console.WriteLine("One Moment Please (press 'x' to Cancel)");

            var repository = new PersonRepository();
            var peopleTask = repository.GetAsync(tokenSource.Token);

            peopleTask.ContinueWith(task =>
                {
                    var people = task.Result;
                    foreach(var person in people)
                        Console.WriteLine(person.ToString());
                    Environment.Exit(0);
                },
                TaskContinuationOptions.OnlyOnRanToCompletion);

            peopleTask.ContinueWith(
                HandleError,
                TaskContinuationOptions.OnlyOnFaulted);

            peopleTask.ContinueWith(
                HandleCancellation,
                TaskContinuationOptions.OnlyOnCanceled);

            HandleExit();
        }

        private static void HandleError(Task<List<Person>> task)
        {
            Console.WriteLine("\nThere was a problem retrieving data");
            Environment.Exit(1);
        }

        private static void HandleCancellation(Task<List<Person>> task)
        {
            Console.WriteLine("\nThe operation was canceled");
            Environment.Exit(0);
        }

        private static void HandleExit()
        {
            ExitLoop:
            if (Console.ReadKey().Key == ConsoleKey.X)
            {
                tokenSource.Cancel();
                goto ExitLoop;
            }
            else
            {
                Console.WriteLine("Waiting...");
                goto ExitLoop;
            }
        }
    }
}

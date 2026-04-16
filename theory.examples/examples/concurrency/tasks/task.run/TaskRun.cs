using System;
using System.Threading;
using System.Threading.Tasks;

namespace task.run
{
    public class Example
    {
        public static void Main()
        {
            // Define and run the task.
            Task firstTask = Task.Run(() => Console.WriteLine("Running first task (Thread {0})"
                , Thread.CurrentThread.ManagedThreadId));

            // Output a message from the calling thread.
            Console.WriteLine("Running main thread (Thread {0})",
                                Thread.CurrentThread.ManagedThreadId);
            //Wait for the task to finish
            firstTask.Wait();
        }
    }
}

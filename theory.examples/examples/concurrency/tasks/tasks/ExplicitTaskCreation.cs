using System;
using System.Threading;
using System.Threading.Tasks;

namespace tasks
{
    class ExplicitTaskCreation
    {
        static void Main(string[] args)
        {
            // Create a task and supply a user delegate by using a lambda expression. 
            Task firstTask = new Task(() => Console.WriteLine("Running first task (Thread {0})"
                    , Thread.CurrentThread.ManagedThreadId));
            // Start the task.
            firstTask.Start();

            // Output a message from the calling thread.
            Console.WriteLine("Running main thread (Thread {0})",
                                Thread.CurrentThread.ManagedThreadId);
            //Wait for the task to finish
            firstTask.Wait();
        }
    }
}

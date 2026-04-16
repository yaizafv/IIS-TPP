using System;
using System.Threading;
using System.Threading.Tasks;

namespace TPP.Concurrency.Delegates {

    /// <summary>
    /// Thread creation by means of Tasks.
    /// A callback task is used to know when the original task has finished its execution.
    /// An old version of this code was based on BeginInvoke/EndInvoke.
    /// Since the Asynchronous Programming Model (APM) (using IAsyncResult and BeginInvoke) is no longer the preferred method of making asynchronous calls
    /// (see https://devblogs.microsoft.com/dotnet/migrating-delegate-begininvoke-calls-for-net-core/ or MSDN documentation), we have updated this example to use 
    /// Tasks and Task Continuations. The second task presents a method to handle results of the first invocation
    /// </summary>
    class Program {

        static void Main(string[] args) {
            string url;
            Menu menu = new Menu();
            while (menu.Show(out url) != Option.Exit) {
                WebPage web = new WebPage(url);
                // * A lambda expresion passed during task initialization.
                // * Another lambda is passed as a parameter of the Task.ContinueWith, acting like a
                //   callback to be called when the first task ends
                Task<int> workTask = Task.Run(() => web.GetNumberOfImages());
                workTask.ContinueWith(prevTask => ThreadEnds(prevTask.Result, web.Url), TaskScheduler.Default);
            }
        }

        /// <summary>
        /// The method that will be called when the thread ends
        /// </summary>
        /// <param name="threadResult">result of first task</param>
        /// <param name="url">extra information used in console messages</param>
        public static void ThreadEnds(int threadResult, string url)
        {
            Console.WriteLine("\n(Thread {0}) The {1} Web page has {2} images.",
                Thread.CurrentThread.ManagedThreadId,
                url,
                threadResult
                );
        }

    }
}

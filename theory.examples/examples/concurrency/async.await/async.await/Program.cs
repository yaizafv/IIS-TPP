using System;
using System.Threading;
using System.Threading.Tasks;

namespace async.await
{
    class Program
    {
        public static void SynchronousCall()
        {
            Thread.Sleep(2000); //Simulates processing
            Console.WriteLine("SynchronousCall body");
            return; //optional
        }


        //Since it does not uses await, this method produces a compile-time warning. It's invocation is synchronous
        public static async void NoAsynchronousCall()
        {
            Thread.Sleep(2000);
            Console.WriteLine("NoAsynchronousCall body");
            return;
        }

        public static async void AsynchronousCall()
        {
            //This code is executed in parallel with main thread (main method). Unfortunately main thread does not wait for this
            //method to finish 
            await Task.Run(() => Thread.Sleep(20000));
            //This code is executed when previous task has ended
            Console.WriteLine("AsynchronousCall body"); 
            return;
        }
        public static async Task AsynchronousTask()
        {
            await Task.Run(() => Thread.Sleep(2000));
            Console.WriteLine("AsynchronousTask body");
            return;
        }

        public static async Task<int> AsynchronousInteger()
        {
            return await Task.Run(() => { Thread.Sleep(2000); Console.WriteLine("AsynchronousInteger body"); return 4; });
        }

        //This method DO NOT return a Task. No 'async' keyword is needed
        public static int SynchronousWrapper(int i)
        {
            //Asynchronous method invocation without await
            var r = AsynchronousInteger();
            if (i % 2 == 0)
                return r.Result; //Waiting for the result is similar to a synchronous call
            else
                return -1; //No wait on r is implemented. In this case parallel code is ignored/unused
        }

        static void Main(string[] args)
        {
            //Traditional synchronos method
            SynchronousCall();
            Console.WriteLine("SynchronousCall invocation has ended");

            //This is also syncrhonos (no await is used in method body)
            NoAsynchronousCall();
            Console.WriteLine("NoAsynchronousCall invocation has ended");

            //This is my first asynchronous invocation
            AsynchronousCall();
            //Since await expression returns control to caller, following Console message is printed.
            //Console message in method body would be 'lost'. Main method does not wait for that method to finish
            Console.WriteLine("AsynchronousCall invocation has ended");

            //This is very similar to previous invocation. In this case main thread waits for the Task to finish
            var t = AsynchronousTask();
            Thread.Sleep(100); //This simulates more computations no dependent on Task result
            Console.WriteLine("AsynchronousTask invocation has ended");
            t.Wait(); //This is a synchronization mechanism. To continue execution, AsynchronousTask body should end


            //In this case an integer is returned
            var tInteger = AsynchronousInteger();
            Console.WriteLine("AsynchronousInteger invocation has ended");
            Thread.Sleep(2000); //This simulates more computations no dependent on Task result
            //This blocks (or waits) for task to finish
            Console.WriteLine("AsynchronousInteger value is {0}", tInteger.Result);

            for (int i = 1; i < 10; i+=2)
            {
                SynchronousWrapper(i); //SyncronosWrapper of an asynchronous method
                Console.WriteLine("SynchronousWrapper invocation {0} has ended", i);
            }
                
        }
    }
}

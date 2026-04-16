using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace task.exception
{
    class TaskException
    {
        static void CaptureException()
        {
            var task = Task.Run(() => { throw new Exception("hello world"); });
            try
            {
                task.Wait();
            }
            catch (AggregateException e)
            {
                Console.WriteLine("Task throwed the following exception: " + e);
            }
            return;
        }

        static void ReThrowException()
        {
            var task = Task.Run(() => { throw new ArgumentNullException(); });
            try
            {
                task.Wait(); 
            }
            catch (AggregateException e)
            {
                Exception[] list = new Exception[] { e };
                throw new AggregateException("Exception rethrown as AggregateException", list);
            }
            return;
        }
        static void Main(string[] args)
        {
            CaptureException();
            try
            {
                ReThrowException();
            }
            catch (AggregateException e)
            {
                Console.WriteLine("Task throwed the following exception: " + e);
            }

            Console.WriteLine("NOTE:");
            Console.WriteLine("The program has mamaged all the exceptions and ends its execution normally");
        }
    }
}

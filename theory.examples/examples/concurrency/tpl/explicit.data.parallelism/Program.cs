using System;
using System.IO;
using System.Drawing;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;

namespace TPP.Concurrency.TaskExplicit
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime before = DateTime.Now;
            string[] fileNames = Directory.GetFiles(@"..\..\..\..\pics", "*.jpg");
            string newDirectory = @"..\..\..\..\pics\rotated";
            Directory.CreateDirectory(newDirectory);

            // The following tasks are executed in parallel.
            // The program creates POTENTIALLY as many tasks as elements in the enumeration.
            var tasks = new List<Task>();
            foreach (var s in fileNames)
            {
                tasks.Add(Task.Factory.StartNew((object? str) => 
                {
                    string file = (string)str;
                    string fileName = Path.GetFileName(file);
                    using (Bitmap bitmap = new Bitmap(file))
                    {
                        Console.WriteLine("Processing the \"{0}\" file with thread {1}.", fileName, Thread.CurrentThread.ManagedThreadId);
                        bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        bitmap.Save(Path.Combine(newDirectory, fileName));
                    }
                }, s));
            }
            //if WaitAll is missing, this code will be in race condition
            Task.WaitAll(tasks.ToArray()); 

            DateTime after = DateTime.Now;
            Console.WriteLine("Elapsed time: {0:N} milliseconds.", (after - before).Ticks / TimeSpan.TicksPerMillisecond);
        }
    }
}

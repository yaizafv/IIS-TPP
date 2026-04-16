using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;

namespace TPP.Concurrency.No.PLinq
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime before = DateTime.Now;
            
            string newDirectory = @"..\..\..\..\pics\rotated";
            Directory.CreateDirectory(newDirectory);

            //This code does not uses PLINQ (AsParallel method), but runs pararllel code!!!
            var results = Directory.GetFiles(@"..\..\..\..\pics", "*.jpg").Select(file_name =>
                Task.Factory.StartNew((object? str) =>
                {
                    string file = (string)str;
                    string fileName = Path.GetFileName(file);
                    using (Bitmap bitmap = new Bitmap(file))
                    {
                        Console.WriteLine("Processing the \"{0}\" file with thread {1}.", fileName, Thread.CurrentThread.ManagedThreadId);
                        bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        bitmap.Save(Path.Combine(newDirectory, fileName));
                    }
                }, file_name)
            );

            //In this case WaitAll is important because of two reasons: 1 - Race Condition, 2 - Forces lazy Linq to create an execute elements
            Task.WaitAll(results.ToArray()); 

            DateTime after = DateTime.Now;
            Console.WriteLine("Elapsed time: {0:N} milliseconds.", (after - before).Ticks / TimeSpan.TicksPerMillisecond);
        }
    }
}

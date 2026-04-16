using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task.composition
{
    class TaskComposition
    {
        static void OneAntecedent()
        {
            // Execute the antecedent.
            Task<DayOfWeek> taskA = Task.Run(() => DateTime.Today.DayOfWeek);

            // Execute the continuation when the antecedent finishes.
            taskA.ContinueWith(antecedent => Console.WriteLine("Today is {0}.", antecedent.Result));
            taskA.Wait();
        }

        static void MultipleAntecedent()
        {
            List<Task<int>> tasks = new List<Task<int>>();
            for (int ctr = 1; ctr <= 10; ctr++)
            {
                int baseValue = ctr;
                tasks.Add(Task.Factory.StartNew((b) =>
                {
                    Console.WriteLine("1");
                    int i = (int)b;
                    return i * i;
                }, baseValue));
            }
            var continuation = Task.WhenAll(tasks);
            Task.Delay(1000);
            long sum = 0;
            for (int ctr = 0; ctr <= continuation.Result.Length - 1; ctr++)
            {
                Console.Write("{0} {1} ", continuation.Result[ctr],
                                ctr == continuation.Result.Length - 1 ? "=" : "+");
                sum += continuation.Result[ctr];
            }
            Console.WriteLine(sum);
            Task.WaitAll(tasks.ToArray());
        }
        static void Main(string[] args)
        {
            OneAntecedent();
            MultipleAntecedent();
        }
    }
}

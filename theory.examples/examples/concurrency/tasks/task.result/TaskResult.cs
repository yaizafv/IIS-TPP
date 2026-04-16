using System;
using System.Threading.Tasks;

public class TaskResult
{
    public static void Main()
    {
        Task<Double>[] taskArray = { Task<double>.Factory.StartNew(() => Compute(2.0)),
                                    Task<double>.Factory.StartNew(() => Compute(300.0)),
                                    Task<double>.Factory.StartNew(() => Compute(4000.0)) };

        var results = new double[taskArray.Length];
        double sum = 0;

        for (int i = 0; i < taskArray.Length; i++)
        {
            results[i] = taskArray[i].Result;
            Console.Write("{0:N1} {1}", results[i],
                                i == taskArray.Length - 1 ? "= " : "+ ");
            sum += results[i];
        }
        Console.WriteLine("{0:N1}", sum);
    }

    private static double Compute(double start)
    {
        double sum = 0;
        for (var value = start; value <= start + 10; value += .1)
            sum += value;

        return sum;
    }
}

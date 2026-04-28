using System.Diagnostics;

namespace activity10;

class Program
{
    static void Main(string[] args)
    {
        var data = activity10.Utils.GetBitcoinData();
        foreach (var d in data)
            Console.WriteLine(d);
        double threshold = 7000.0;
        int maxThreads = 50;

        Console.WriteLine("Hilos;Repeticion;Ticks");

        for (int numHilos = 1; numHilos <= maxThreads; numHilos++)
        {
            for (int rep = 1; rep <= 15; rep++)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();

                BitcoinMaster master = new BitcoinMaster(data, numHilos, threshold);

                Stopwatch sw = Stopwatch.StartNew();
                master.ComputeCount();
                sw.Stop();

                Console.WriteLine($"{numHilos};{rep};{sw.ElapsedTicks}");
            }
        }
    }
}
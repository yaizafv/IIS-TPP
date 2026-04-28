using System.Diagnostics;

namespace activity10;

class Program
{
    static void Main(string[] args)
    {
        var data = activity10.Utils.GetBitcoinData();

        double threshold = 7000.0;
        int maxThreads = 50;

        Console.WriteLine("Hilos;Ejecucion;Ticks");

        for (int numHilos = 1; numHilos <= maxThreads; numHilos++)
        {
            long sumaTicks = 0;
            for (int ejecucion = 1; ejecucion <= 15; ejecucion++)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();

                BitcoinMaster master = new BitcoinMaster(data, numHilos, threshold);

                DateTime before = DateTime.Now;
                master.ComputeCount();
                DateTime after = DateTime.Now;

                sumaTicks += (after - before).Ticks;

                Console.WriteLine($"{numHilos};{ejecucion};{(after - before).Ticks}");      //MasterWorker.exe >> datos.csv
            }
            long media = sumaTicks / 15;
            Console.WriteLine("Media Ticks: ");
            Console.WriteLine($"{numHilos};{media}");
        }
    }
}
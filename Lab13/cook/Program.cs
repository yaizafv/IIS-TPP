using System.Diagnostics;

namespace tpp.lab13;

class Program
{
    // Quita el async Task y pon void
    static async Task Main()
    {
        Console.WriteLine("Desayuno síncrono");
        var syncStopwatch = Stopwatch.StartNew();

        Cook.MakeBreakfastSync();

        syncStopwatch.Stop();
        Console.WriteLine($"El desayuno síncrono tardó {syncStopwatch.ElapsedMilliseconds} ms\n");

        Console.WriteLine("Desayuno asíncrono");
        var asyncStopwatch = Stopwatch.StartNew();

        // ¿await Es como el .Wait()? ¿Qué opinas?
        await Cook.MakeBreakfastAsync();

        asyncStopwatch.Stop();
        Console.WriteLine($"El desayuno asíncrono tardó {asyncStopwatch.ElapsedMilliseconds} ms\n");
    }
}


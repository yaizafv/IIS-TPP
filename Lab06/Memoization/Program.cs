﻿using System.Diagnostics;
using System.Security.Cryptography;

namespace lab06;
class Program
{
    static void Main()
    {
        const int N = 40;
        var crono = new Stopwatch();

        crono.Start();
        var res1 = Fibonacci(N);
        crono.Stop();
        var ticks1 = crono.ElapsedTicks;
        Console.WriteLine($"Tiempo: {ticks1} ticks. Resultado: {res1}");

        crono.Restart();
        var res2 = Fibonacci(N);
        crono.Stop();
        var ticks2 = crono.ElapsedTicks;
        Console.WriteLine($"Tiempo: {ticks2} ticks. Resultado: {res2}");

        var memoizedFibonacci = ObtenerMemoizedFibonacci();

        crono.Restart();
        var res3 = memoizedFibonacci(N);
        crono.Stop();
        var ticks3 = crono.ElapsedTicks;
        Console.WriteLine($"Tiempo: {ticks3} ticks. Resultado: {res3}");

        crono.Restart();
        var res4 = memoizedFibonacci(N);
        crono.Stop();
        var ticks4 = crono.ElapsedTicks;
        Console.WriteLine($"Tiempo: {ticks4} ticks. Resultado: {res4}");
    }

    static int Fibonacci(int n)
    {
        return n <= 2 ? 1 : Fibonacci(n - 2) + Fibonacci(n - 1);
    }

    static Func<int, int> ObtenerMemoizedFibonacci()
    {
        var valores = new Dictionary<int, int>();
        int Fibonacci(int n)
        {
            if (valores.ContainsKey(n)) return valores[n];      //valores funciona como una cache

            var res = n <= 2 ? 1 : Fibonacci(n - 2) + Fibonacci(n - 1);

            valores[n] = res;
            return res;
        }
        return Fibonacci;
    }

    /***************************************************************************
     * ¿Tiene sentido memoizar estas funciones?
     ***/
    static int SimulacionCalculoCosteAlto(int x, int y, int z)
    {
        // Simulamos una operación de coste alto
        Thread.Sleep(1000);

        return Random.Shared.Next(1, x + y + z + 1);
    }

    static int counter = 0;
    static int SimulacionProcesoCalculoCosteAlto(int x, int y, int z)
    {
        // Simulamos una operación de coste alto
        Thread.Sleep(1000);

        counter += x + y + z;
        return x + y + z;
    }

    static int CalcularConLog(int x, int y, int z)
    {
        // Simulamos una operación de coste alto
        Thread.Sleep(1000);

        int r = x + y + z;
        File.AppendAllText("audit.log", $"Calculado {r} en {DateTime.Now:O}\n");
        return r;
    }

    static int FuncionDeterministaEntradaMutableCosteAlto(List<int> xs)
    {
        // Simulamos una operación de coste alto
        Thread.Sleep(1000);
        return xs.Sum();
    }

    static string HashFile(string path)
    {
        using var stream = File.OpenRead(path); 	    //el contenido del fichero no es siempre el mismo
        using var sha = SHA256.Create();
        return Convert.ToHexString(sha.ComputeHash(stream));
    }
}

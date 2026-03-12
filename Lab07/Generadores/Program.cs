using System.Collections;
using System.Diagnostics;

namespace Generadores;

class Program
{
    static uint[] expectedPrimes = [2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97];

    static void Main(string[] args)
    {
        EnumeratorPrimosEj();
        ClausuraGenPrimosEj();
        YieldGenPrimosEj();
    }

    private static void EnumeratorPrimosEj()
    {
        var primos = new List<uint>();
        Console.WriteLine("EnumeratorPrimos:");
        foreach (var primo in new PrimosEnumerable())
        {
            if (primo > 100) break;
            Console.Write($"{primo}, ");
            primos.Add(primo);
        }
        Console.WriteLine("\n");
        Debug.Assert(primos.SequenceEqual(expectedPrimes));
    }

    private static void ClausuraGenPrimosEj()
    {
        var primos = new List<uint>();
        Console.WriteLine("ClausuraGenPrimos:");
        var primeGenerator = ClausuraGeneradorPrimos();
        while (true)        //desventaja: menos legible
        {
            var primo = primeGenerator();
            if (primo > 100) break;
            Console.Write($"{primo}, ");
            primos.Add(primo);
        }
        ;
        Console.WriteLine("\n");
        Debug.Assert(primos.SequenceEqual(expectedPrimes));
    }

    private static void YieldGenPrimosEj()
    {
        var obtainedPrimes = new List<uint>();
        Console.WriteLine("YieldGenPrimos:");
        foreach (var primeYield in YieldGeneradorPrimos())
        {
            if (primeYield > 100) break;
            Console.Write($"{primeYield}, ");
            obtainedPrimes.Add(primeYield);
        }
        Console.WriteLine("\n");
        Debug.Assert(obtainedPrimes.SequenceEqual(expectedPrimes));
    }

    /***************************************************************************
     * EnumeratorPrimos
     ***/

    class PrimosEnumerable : IEnumerable<uint>
    {

        public IEnumerator<uint> GetEnumerator()
        {
            return new PrimosEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    class PrimosEnumerator : IEnumerator<uint>
    {
        private uint actual = 1;

        public uint Current => actual == 1 ? throw new InvalidOperationException() : actual;

        object? IEnumerator.Current => Current;

        public void Dispose() { }

        public bool MoveNext()
        {
            do
            {
                if (actual == uint.MaxValue) throw new OverflowException();
                actual++;
            } while (!EsPrimo(actual));
            return true;
        }

        public void Reset() => actual = 1;
    }

    /***************************************************************************
     * ClausuraGeneradorPrimos
     ***/

    static Func<uint> ClausuraGeneradorPrimos()
    {
        uint actual = 1;

        return () =>
        {
            uint posible = actual + 1;
            while (!EsPrimo(posible))
            {
                if (posible == uint.MaxValue) throw new OverflowException();
                posible++;
            }
            actual = posible;
            return posible;
        };
    }

    /***************************************************************************
     * PrimesYieldGenerator
     ***/

    static IEnumerable<uint> YieldGeneradorPrimos()
    {
        uint actual = 1;
        while (true)
        {
            uint posible = actual + 1;
            while (!EsPrimo(posible))
            {
                if (posible == uint.MaxValue) throw new OverflowException();
                posible++;
            }
            actual = posible;
            yield return posible;       //guarda el estado del método 
        }
    }

    static bool EsPrimo(uint x)
    {
        if (x < 2) return false;
        if (x == 2) return true;
        if (x % 2 == 0) return false;
        for (uint d = 3; d * d <= x; d += 2)
            if (x % d == 0) return false;
        return true;
    }
}

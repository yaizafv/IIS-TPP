using System.Text;

namespace BuscarPrimosFuncional;

class Program
{
    static void Main()
    {

        Console.WriteLine("Versión funcional de BuscadorPrimos:");
        int numBuscadores = 4;
        Thread[] hilos = new Thread[numBuscadores];

        for (int i = 0; i < numBuscadores; i++)
        {
            int inicio = i * 20 + 2;
            int fin = i * 20 + 21;
            int id = i + 1;
            hilos[i] = new Thread(() => BuscarPrimosFuncional(inicio, fin, id));
            hilos[i].Start();
        }

        for (int i = 0; i < hilos.Length; i++)
            hilos[i].Join();
    }

    public static void BuscarPrimosFuncional(int inicio, int fin, int id)
    {
        var sb = new System.Text.StringBuilder();
        for (int n = inicio; n <= fin; n++)
        {
            if (EsPrimo(n))
                sb.Append(n).Append(' ');
        }
        Console.WriteLine($"[{id}] Primos en [{inicio}, {fin}]: {sb}");
    }

    private static bool EsPrimo(int n)
    {
        if (n < 2) return false;
        for (int i = 2; i * i <= n; i++)
            if (n % i == 0) return false;
        return true;
    }
}


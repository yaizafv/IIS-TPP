using System.Text;

namespace HilosPOO;

class Program
{
    static void Main()
    {
        int numBuscadores = 4;
        BuscadorPrimos[] buscadores = new BuscadorPrimos[numBuscadores];

        for (int i = 0; i < numBuscadores; i++)
        {
            int inicio = i * 20 + 2;
            int fin = i * 20 + 21;

            // Cada objeto encapsula los datos necesarios para su tarea.
            buscadores[i] = new BuscadorPrimos(inicio, fin);
        }

        Thread[] hilos = new Thread[numBuscadores];
        for (int i = 0; i < numBuscadores; i++)
        {
            // Cada hilo ejecuta la tarea asociada (método Buscar) a un objeto concreto.
            hilos[i] = new Thread(buscadores[i].Buscar);
            hilos[i].Name = $"Hilo buscador {i + 1}";
        }

        for (int i = 0; i < hilos.Length; i++)
            hilos[i].Start();

        for (int i = 0; i < hilos.Length; i++)      //TIENEN QUE ESTAR EN FOR DISTINTOS. SI NO ES UN 0
            hilos[i].Join();
    }
}


class BuscadorPrimos
{
    private int _inicio;
    private int _fin;
    public BuscadorPrimos(int inicio, int fin)
    {
        _inicio = inicio;
        _fin = fin;
    }

    public void Buscar()
    {
        StringBuilder sb = new StringBuilder();
        for (int n = _inicio; n <= _fin; n++)
        {
            if (EsPrimo(n))
                sb.Append(n).Append(' ');
        }
        Console.WriteLine($"[{Thread.CurrentThread.Name}] Primos en [{_inicio}, {_fin}]: {sb}");
    }

    private static bool EsPrimo(int n)
    {
        if (n < 2)
            return false;

        for (int i = 2; i * i <= n; i++)
            if (n % i == 0)
                return false;

        return true;
    }
}


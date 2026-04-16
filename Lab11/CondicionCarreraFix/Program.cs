using System.Diagnostics;

namespace CondicionCarreraFix;

class Program
{

    static short[] vector = CrearVectorAleatorio(20000000, 0, 10);
    static int numHilos = 4;
    static void Main()
    {
        BusquedaSecuencial();
        BusquedaMultihiloLock();
        BusquedaMultihiloInterlocked();
        // EJERCICIO: Implementa la solución óptima.
        BusquedaResultadosParciales();
    }

    public static void BusquedaMultihiloLock()
    {

        object syncLock = new object();     // hay que declararlo fuera del for

        int recuentoMultihilo = 0;
        Thread[] hilos = new Thread[numHilos];
        for (int i = 0; i < hilos.Length; i++)
        {
            int inicio = i * vector.Length / hilos.Length;
            int fin = inicio + vector.Length / hilos.Length;
            if (i == hilos.Length - 1)
                fin = vector.Length;

            hilos[i] = new Thread(() =>
            {
                for (int i = inicio; i < fin; i++)
                {
                    if (vector[i] is 2 or 3)        //hacemos tantos bloqueos como 2 o 3 haya
                    {
                        // Sección crítica y exclusión mutua
                        // ¿Cómo funciona el lock?
                        // hay que pasarle una referencia (objeto). el lock debe aplicarse siempre en la zona crítica
                        lock (syncLock)
                        {
                            recuentoMultihilo++;
                        }

                    }
                }
            });
            hilos[i].Start();
        }

        foreach (var hilo in hilos)
            hilo.Join();

        Console.WriteLine($"[Multihilo (Lock)] Los números 2 y 3 aparecen {recuentoMultihilo} veces.");
    }

    public static void BusquedaMultihiloInterlocked()
    {
        // Interlocked (System.Threading) -> Operaciones atómicas primitivas.
        // Como norma general, es más eficiente que lock.
        // Métodos más utilizados: Increment, Decrement, Add y Exchange.
        // https://learn.microsoft.com/en-us/dotnet/api/system.threading.interlocked?view=net-9.0

        int recuentoMultihilo = 0;
        Thread[] hilos = new Thread[numHilos];
        for (int i = 0; i < hilos.Length; i++)
        {
            int inicio = i * vector.Length / hilos.Length;
            int fin = inicio + vector.Length / hilos.Length;
            if (i == hilos.Length - 1)
                fin = vector.Length;

            hilos[i] = new Thread(() =>
            {
                for (int i = inicio; i < fin; i++)
                {
                    if (vector[i] is 2 or 3)
                    {
                        Interlocked.Increment(ref recuentoMultihilo);    //el lock deja guardar bloques de codigo. aqui no. puntua mas que el lock (0,5) ya que es mas eficiente
                    }
                }
            });
            hilos[i].Start();
        }

        foreach (var hilo in hilos)
            hilo.Join();

        Console.WriteLine($"[Multihilo (Interlocked)] Los números 2 y 3 aparecen {recuentoMultihilo} veces.");
    }

    //la mejor solucion es con resultados parciales
    public static void BusquedaResultadosParciales()
    {
        int recuentoMultihilo = 0;
        Thread[] hilos = new Thread[numHilos];
        object obj = new object();
        int[] array = new int[numHilos];
        for (int i = 0; i < hilos.Length; i++)
        {
            int inicio = i * vector.Length / hilos.Length;
            int fin = inicio + vector.Length / hilos.Length;
            if (i == hilos.Length - 1)
                fin = vector.Length;

            hilos[i] = new Thread(() =>
            {
                int contador = 0;
                for (int i = inicio; i < fin; i++)
                {
                    if (vector[i] is 2 or 3)
                    {
                        contador++;
                    }
                }
                lock (obj)
                {
                    recuentoMultihilo += contador;
                }
            });
            hilos[i].Start();
        }

        foreach (var hilo in hilos)
            hilo.Join();

        Console.WriteLine($"[Multihilo (resultados parciales)] Los números 2 y 3 aparecen {recuentoMultihilo} veces.");
    }

    public static void BusquedaSecuencial()
    {
        int recuentoSecuencial = 0;
        for (int i = 0; i < vector.Length; i++)
        {
            if (vector[i] is 2 or 3)
                recuentoSecuencial++;
        }
        Console.WriteLine($"[Secuencial] Los números 2 y 3 aparecen {recuentoSecuencial} veces.");
    }

    public static short[] CrearVectorAleatorio(int numElementos, short menor, short mayor)
    {
        short[] vector = new short[numElementos];
        Random random = new Random();
        for (int i = 0; i < numElementos; i++)
            vector[i] = (short)random.Next(menor, mayor + 1);
        return vector;
    }


}

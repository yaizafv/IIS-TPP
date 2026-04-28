using System.Text;

namespace HilosFuncional;


class Program
{
    private static readonly string[] urls =
        {
            "https://www.uniovi.es",
            "https://ingenieriainformatica.uniovi.es/",
            "https://tailwindcss.com/"
        };


    static void Main()
    {
        EjemploHilosConDelegados();
        //EjemploHilosConLambdas();

        // Implementa una versión funcional del ejemplo de HilosPOO
        EjemploBuscarPrimosFuncional();
    }

    public static void EjemploHilosConDelegados()
    {
        Console.WriteLine("Lanzamiento de hilos con delegados:");
        Thread[] hilos = new Thread[urls.Length];
        for (int i = 0; i < hilos.Length; i++)
        {
            hilos[i] = new Thread(ObtenerDatos);
            hilos[i].Start(urls[i]);        //le pasa el parámetro que necesita
        }

        for (int i = 0; i < hilos.Length; i++)
            hilos[i].Join();

        Action<object?> obtenerDatos = valor =>
        {
            Console.WriteLine($"[ID={Thread.CurrentThread.ManagedThreadId}] Obteniendo datos del destino: {valor}");
            Thread.Sleep(2000);         //Simulamos carga de trabajo
            Console.WriteLine($"[ID={Thread.CurrentThread.ManagedThreadId}] Datos obtenidos y almacenados.");
        };

        // El constructor de Thread espera un ParameterizedThreadStart (o ThreadStart si no hay parámetro).
        // Un Action puede adaptarse explícitamente.
        Thread hiloB = new Thread(new ParameterizedThreadStart(obtenerDatos));

        hiloB.Start("https://htmx.org/");
    }

    public static void EjemploHilosConLambdas()
    {
        // ¿Qué ocurre con este código?
        Console.WriteLine("Lanzamiento de hilos con lambdas:");
        Thread[] hilos = new Thread[urls.Length];
        for (int i = 0; i < hilos.Length; i++)
        {
            int copia = i;      // crea una nueva posicion de memoria. si i nunca cambiase de valor no haria falta
            hilos[i] = new Thread(
                () =>
                {
                    Console.WriteLine($"[ID={Thread.CurrentThread.ManagedThreadId}] Obteniendo datos del destino: {urls[copia]}");
                    Thread.Sleep(2000);
                    Console.WriteLine($"[ID={Thread.CurrentThread.ManagedThreadId}] Datos obtenidos y almacenados.");
                }
            );
            hilos[i].Start();
        }

        for (int i = 0; i < hilos.Length; i++)
            hilos[i].Join();
    }


    public static void ObtenerDatos(object? valor)
    {
        Console.WriteLine($"[ID={Thread.CurrentThread.ManagedThreadId}] Obteniendo datos del destino: {valor}");
        Thread.Sleep(2000); //Simulamos carga de trabajo
        Console.WriteLine($"[ID={Thread.CurrentThread.ManagedThreadId}] Datos obtenidos y almacenados.");
    }

    public static void EjemploBuscarPrimosFuncional()
    {
        int numBuscadores = 4;
        Thread[] hilos = new Thread[numBuscadores];
        for (int i = 0; i < numBuscadores; i++)
        {
            int inicio = i * 20 + 2;
            int fin = i * 20 + 21;
            int id = i + 1;
            hilos[i] = new Thread(() => BuscarPrimosFuncional(inicio, fin, id));
        }

        for (int i = 0; i < hilos.Length; i++)
        {
            hilos[i].Start();
        }

        for (int i = 0; i < hilos.Length; i++)
        {
            hilos[i].Join();
        }
    }

    public static void BuscarPrimosFuncional(int inicio, int fin, int id)
    {
        StringBuilder sb = new StringBuilder();
        for (int n = inicio; n <= fin; n++)
        {
            if (EsPrimo(n))
                sb.Append(n).Append(' ');
        }
        Console.WriteLine($"[{id}] Primos en [{inicio}, {fin}]: {sb}");
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


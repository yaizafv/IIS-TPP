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
        //EjemploHilosConDelegados();
        EjemploHilosConLambdas();

        // Implementa una versión funcional del ejemplo de HilosPOO
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
            Thread.Sleep(2000); //Simulamos carga de trabajo
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
}


using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace tpp.lab13;

class Program
{
    static async Task Main()
    {
        Console.WriteLine("WhenAll\n");

        await DemostrarWhenAll();

        Console.WriteLine("WhenAny\n");

        await DemostrarWhenAny();

        Console.WriteLine("WhenAny con cancelación\n");

        await DemostrarCancelacionConWhenAny();
    }

    static async Task DemostrarWhenAll()
    {
        Console.WriteLine(">>> Escenario 1: Task.WhenAll");
        Console.WriteLine("Imagina que estamos preparando un menú completo: hamburguesa, patatas y bebida.");
        Console.WriteLine("El menú no se sirve hasta que todo está listo.\n");

        var sw = Stopwatch.StartNew();

        Task<string> hamburguesa = PrepararComidaAsync("Hamburguesa", 3000);
        Task<string> patatas = PrepararComidaAsync("Patatas fritas", 2000);
        Task<string> bebida = PrepararComidaAsync("Refresco", 1000);

        Console.WriteLine("[Main] Tareas lanzadas. Esperando a que todo el menú esté listo...");

        // Task.WhenAll devuelve una nueva tarea que termina cuando terminen todas las demás.
        // El resultado es un array que almacena los resultados de las tareas (string)
        string[] menuCompleto = await Task.WhenAll(hamburguesa, patatas, bebida);

        sw.Stop();

        Console.WriteLine($"\n[Main] Menú completo servido en {sw.ElapsedMilliseconds} ms.");
        Console.WriteLine("La bandeja contiene:");

        foreach (string item in menuCompleto)
        {
            Console.WriteLine($" - {item}");
        }
    }

    static async Task DemostrarWhenAny()
    {
        Console.WriteLine(">>> Escenario 2: Task.WhenAny");
        Console.WriteLine("Un cliente tiene mucha hambre y pide: 'Tráeme lo primero que salga de la cocina'.");
        Console.WriteLine("Hay varios platos preparándose, pero serviremos el primero que termine.\n");

        var sw = Stopwatch.StartNew();

        Task<string> plato1 = CocinarPlatoAsync("Pizza pepperoni", 4000);
        Task<string> plato2 = CocinarPlatoAsync("Perrito caliente", 1500);
        Task<string> plato3 = CocinarPlatoAsync("Pollo frito", 3000);

        Console.WriteLine("[Main] Platos encargados. Esperando el primero en salir...");

        // Task.WhenAny no devuelve directamente el resultado del plato.
        // Devuelve la tarea que ha terminado primero.
        Task<string> primerPlatoTerminado = await Task.WhenAny(plato1, plato2, plato3);

        // Como tenemos la tarea ganadora, ahora obtenemos su resultado.
        string resultadoGanador = await primerPlatoTerminado;

        sw.Stop();

        Console.WriteLine($"\n[Main] ¡Ding! Plato listo en {sw.ElapsedMilliseconds} ms.");
        Console.WriteLine($"El camarero sirve: {resultadoGanador}");

        Console.WriteLine("\n[Main] Nota: los otros platos siguen preparándose en segundo plano.");

        // Solo para que se vea en consola que las tareas perdedoras siguen terminando.
        await Task.Delay(3000);
    }

    static async Task DemostrarCancelacionConWhenAny()
    {
        Console.WriteLine(">>> Escenario 3: Task.WhenAny con cancelación");
        Console.WriteLine("Un cliente pide tres postres, pero solo quiere el primero que llegue.");
        Console.WriteLine("Para no desperdiciar comida, se cancelarán los demás postres.\n");

        var sw = Stopwatch.StartNew();

        using var cts = new CancellationTokenSource();
        CancellationToken token = cts.Token;

        Task<string> postre1 = PrepararPostreCancelableAsync("Tarta de queso al horno", 3500, token);
        Task<string> postre2 = PrepararPostreCancelableAsync("Helado de vainilla", 1000, token);
        Task<string> postre3 = PrepararPostreCancelableAsync("Coulant de chocolate", 2500, token);

        Console.WriteLine("[Main] Postres encargados. Esperando el más rápido...");

        Task<string> primerPostreTerminado = await Task.WhenAny(postre1, postre2, postre3);
        string resultadoGanador = await primerPostreTerminado;

        sw.Stop();

        Console.WriteLine($"\n[Main] ¡Ding! Postre listo en {sw.ElapsedMilliseconds} ms.");
        Console.WriteLine($"El camarero sirve: {resultadoGanador}");

        Console.WriteLine("\n[Main] Camarero grita a cocina: ¡Cancelad el resto de postres!");

        // WhenAny no cancela automáticamente las demás tareas.
        // La cancelación hay que pedirla explícitamente.
        cts.Cancel();

        // Solo para que se vea en consola cómo reaccionan las tareas canceladas.
        await Task.Delay(3000);
    }

    static async Task<string> PrepararComidaAsync(string nombre, int msEspera)
    {
        Console.WriteLine($"\t[{nombre}] Empezando a preparar...");
        await Task.Delay(msEspera);
        Console.WriteLine($"\t[{nombre}] Listo.");
        return nombre;
    }

    static async Task<string> CocinarPlatoAsync(string nombre, int msEspera)
    {
        Console.WriteLine($"\t[{nombre}] En los fogones...");
        await Task.Delay(msEspera);
        Console.WriteLine($"\t[{nombre}] Terminado y emplatado.");
        return nombre;
    }

    static async Task<string> PrepararPostreCancelableAsync(string nombre, int msEspera, CancellationToken token)
    {
        Console.WriteLine($"\t[{nombre}] Empezando a preparar el postre...");

        try
        {
            await Task.Delay(msEspera, token);

            Console.WriteLine($"\t[{nombre}] Postre listo.");
            return nombre;
        }
        catch (TaskCanceledException)
        {
            Console.WriteLine($"\t[{nombre}] [CANCELADO] Preparación cancelada por el camarero.");
            return string.Empty;
        }
    }
}



using System.Threading.Channels;


namespace tpp.lab13;
class Program
{
    static async Task Main()
    {
        Console.WriteLine("Bounded Channel (Básico)");
        
        // Creamos un canal con un LÍMITE de 3 mensajes.
        // Si la cola se llena, el productor tendrá que esperar (asíncronamente) 
        // a que el consumidor saque algún elemento.
        Channel<int> canal = Channel.CreateBounded<int>(3);

        Task tareaProductor = ProductorAsync(canal.Writer);
        Task tareaConsumidor = ConsumidorAsync(canal.Reader);

        // Esperamos a que ambos terminen
        await Task.WhenAll(tareaProductor, tareaConsumidor);
        
        Console.WriteLine("Fin");
    }

    // --- PRODUCTOR ---
    static async Task ProductorAsync(ChannelWriter<int> escritor)
    {
        Console.WriteLine("[Productor] Empezando a generar números...");

        // ESCENARIO A: Bucle Secuencial (1, 2, ..., 10)
        for (int i = 1; i <= 10; i++) 
        {
            Console.WriteLine($"[Productor] Intentando escribir el número {i}...");
            
            // VARIANTE A: Escribimos de forma síncrona
            if (escritor.TryWrite(i))
                Console.WriteLine($"[Productor] Número {i} escrito en el canal (Sync).");
            else
                Console.WriteLine($"[Productor] ERROR: Canal lleno. Número {i} descartado (Sync).");
            

            // VARIANTE B: Escribimos de forma asíncrona
            // WriteAsync escribirá el número y pausa el método (no el hilo) si está llena

            // await escritor.WriteAsync(i);
            // Console.WriteLine($"[Productor] Número {i} escrito en el canal (Async).");
        }

        // ESCENARIO B: Bucle Paralelo (TPL)
        // Descomenta esto y comenta el bucle 'for' de arriba para probarlo.
        // Observa el error que se produce y razona el porqué

        // Parallel.For(11, 20, async i => 
        // {
        //     Console.WriteLine($"[Productor TPL] Intentando escribir el número {i}...");
        //     await escritor.WriteAsync(i);
        //     Console.WriteLine($"[Productor TPL] Número {i} escrito en el canal.");
        // });




        Console.WriteLine("[Productor] Ya he enviado todos los números. Cierro el canal.");
        // Avisamos de que no enviaremos más datos
        escritor.Complete();
    }


    static async Task ConsumidorAsync(ChannelReader<int> lector)
    {
        Console.WriteLine("\t[Consumidor] Esperando a que lleguen datos...");
              
        // Pausamos hasta elemento o se cierre el canal.
        while (await lector.WaitToReadAsync())
        {
            // Extraemos todos los elementos disponibles en este momento
            while (lector.TryRead(out int numeroRecibido))
            {
                Console.WriteLine($"\t[Consumidor] He leído el número {numeroRecibido}. Procesando...");
                await Task.Delay(1000); 
            }
        }
        
        Console.WriteLine("\t[Consumidor] El canal se ha cerrado. No hay más datos.");
    }
}


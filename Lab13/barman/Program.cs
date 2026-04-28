namespace tpp.lab13;

class Program
{
    static void Main()
    {
        Console.WriteLine($"[Hilo principal {Thread.CurrentThread.ManagedThreadId}] Comenzando el pedido...");

        // Tarea que devuelve un valor (simula moler los granos de café)
        Task<string> grindTask = Task.Run(() =>
        {
            Console.WriteLine($"[Hilo {Thread.CurrentThread.ManagedThreadId}] Moliendo los granos de café...");
            Thread.Sleep(2000); // Simula el tiempo de molido
            return "café molido";
        });

        // Una tarea que representa una continuación de otra (encada una tarea con el resultado de la otra)
        Task<string> brewTask = grindTask.ContinueWith((previousTask) =>
        {
            string ingredient = previousTask.Result; // Obtiene "café molido"
            Console.WriteLine($"[Hilo {Thread.CurrentThread.ManagedThreadId}] Usando '{ingredient}' para preparar el espresso...");
            Thread.Sleep(3000);
            return "taza de espresso";
        });

        // 3. ContinueWith y luego Task. Encadena una tarea que no devuelve nada, solo sirve el café
        Task serveTask = brewTask.ContinueWith((previousTask) =>
        {
            string finalProduct = previousTask.Result; // ¿Qué ocurre si la anterior tarea falló?
            Console.WriteLine($"[Hilo {Thread.CurrentThread.ManagedThreadId}] Sirviendo al cliente: {finalProduct} listo.");
        });

        Console.WriteLine($"[Hilo principal {Thread.CurrentThread.ManagedThreadId}] Pedido en marcha. El hilo principal aún no está bloqueado y puede hacer otras cosas...");
    
        serveTask.Wait(); // ¿A qué se parece esto?
        
        Console.WriteLine($"[Hilo principal {Thread.CurrentThread.ManagedThreadId}] Pedido terminado y entregado.");
    }
}

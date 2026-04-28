namespace HilosBasico;

class Program
{
    static void Main()
    {
        Thread hilo = new Thread(Tarea);        //crea un hilo secundario
        hilo.Name = "Hilo 1";
        hilo.Priority = ThreadPriority.AboveNormal;
        hilo.IsBackground = true;       //aqui no se ejecuta Tarea porque cuando el hilo principal termina mata a todos los demons (si no hubiera join)

        Console.WriteLine($"Main [id={Thread.CurrentThread.ManagedThreadId}]");
        Console.WriteLine($"Hilo \"{hilo.Name}\" creado. Estado: {hilo.ThreadState}");

        hilo.Start();
        hilo.Join();        // lanza el hilo.       el hilo principal espera a que el secundario acabe
        Console.WriteLine("Hilo principal continúa ejecutándose...");
    }

    public static void Tarea()
    {
        Console.WriteLine($"Empieza la tarea del hilo [id={Thread.CurrentThread.ManagedThreadId}] \"{Thread.CurrentThread.Name}\"");

        for (int i = 3; i >= 1; i--)
        {
            Thread.Sleep(500); // Simulación de carga de trabajo.
            Console.WriteLine($"\"{Thread.CurrentThread.Name}\", valor: {i}");
        }

        Console.WriteLine($"Finaliza la tarea del hilo [id={Thread.CurrentThread.ManagedThreadId}] \"{Thread.CurrentThread.Name}\"");
    }
}

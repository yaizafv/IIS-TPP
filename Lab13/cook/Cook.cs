namespace tpp.lab13;

public class Cook
{
    /*
    //
    //  Métodos síncronos
    //
    */
    public static void MakeBreakfastSync()
    {
        ToastBreadSync();
        MakeCoffeeSync();
        FryEggSync();

        Console.WriteLine($"¡Desayuno listo! (Hilo: {Thread.CurrentThread.ManagedThreadId})");
    }

    private static void ToastBreadSync()
    {
        Console.WriteLine($"Empezando a tostar el pan... (Hilo: {Thread.CurrentThread.ManagedThreadId})");

        Thread.Sleep(3000); // Bloquea el hilo actual.

        Console.WriteLine($"Pan tostado. (Hilo: {Thread.CurrentThread.ManagedThreadId})");
    }

    private static void MakeCoffeeSync()
    {
        Console.WriteLine($"Empezando a preparar el café... (Hilo: {Thread.CurrentThread.ManagedThreadId})");

        Thread.Sleep(2000); // Bloquea el hilo actual.

        Console.WriteLine($"Café preparado. (Hilo: {Thread.CurrentThread.ManagedThreadId})");
    }

    private static void FryEggSync()
    {
        Console.WriteLine($"Empezando a freír el huevo... (Hilo: {Thread.CurrentThread.ManagedThreadId})");

        Thread.Sleep(4000); // Bloquea el hilo actual.

        Console.WriteLine($"Huevo frito. (Hilo: {Thread.CurrentThread.ManagedThreadId})");
    }

    /*
    //
    //  Métodos asíncronos
    //
    */
    public static async Task MakeBreakfastAsync()
    {
        // Se lanzan las tres tareas.
        Task toastTask = ToastBreadAsync();
        Task coffeeTask = MakeCoffeeAsync();
        Task eggTask = FryEggAsync();

        Console.WriteLine($"El cocinero puede hacer otras comprobaciones mientras tanto... (Hilo: {Thread.CurrentThread.ManagedThreadId})");

        // Esperamos a que acaben las tres tareas (mejorable, lo veremos).
        await toastTask;
        await coffeeTask;
        await eggTask;

        Console.WriteLine($"¡Desayuno listo! (Hilo: {Thread.CurrentThread.ManagedThreadId})");
    }


    private static async Task ToastBreadAsync()
    {
        Console.WriteLine($"Empezando a tostar el pan... (Hilo antes del await: {Thread.CurrentThread.ManagedThreadId})");

        await Task.Delay(3000); // ¿Es equivalente al Sleep? ¿Qué opinas?

        Console.WriteLine($"Pan tostado. (Hilo después del await: {Thread.CurrentThread.ManagedThreadId})");
    }


    private static async Task MakeCoffeeAsync()
    {
        Console.WriteLine($"Empezando a preparar el café... (Hilo antes del await: {Thread.CurrentThread.ManagedThreadId})");

        await Task.Delay(2000); 

        Console.WriteLine($"Café preparado. (Hilo después del await: {Thread.CurrentThread.ManagedThreadId})");
    }


    private static async Task FryEggAsync()
    {
        Console.WriteLine($"Empezando a freír el huevo... (Hilo antes del await: {Thread.CurrentThread.ManagedThreadId})");

        await Task.Delay(4000); // No bloquea el hilo actual.

        Console.WriteLine($"Huevo frito. (Hilo después del await: {Thread.CurrentThread.ManagedThreadId})");
    }
}
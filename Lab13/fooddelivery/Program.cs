using System.Threading.Channels;

// Clase que representa los datos que se pasan por los canales.
public class Order
{
    public int Id { get; set; }
    public string Menu { get; set; } = "";
    public int Kilometers { get; set; }
    public int CookingTimeMs { get; set; }
}

class Program
{
    // Canales para comunicar los distintos departamentos.
    static readonly Channel<Order> kitchenChannel = Channel.CreateUnbounded<Order>();
    static readonly Channel<Order> deliveryChannel = Channel.CreateUnbounded<Order>();
    
    static int orderCount = 0;

    static async Task Main(string[] args)
    {
        Console.WriteLine("=== BIENVENIDO A TPP DELIVERY ===");
        Console.WriteLine("Paradigma: compartir memoria mediante comunicación (Channels)\n");

        Task tKitchen1 = CookTask("Cocinero Mario");
        Task tKitchen2 = CookTask("Cocinero Luigi");
        Task tDelivery1 = DeliveryTask("Repartidor Pepe");
        Task tDelivery2 = DeliveryTask("Repartidora Marta");

        // Pedidos de ejemplo para probar el programa.
        //Console.WriteLine("[Sistema] Generando pedidos iniciales...");
        //await GenerateOrderAsync(option: 1, km: 8);
        //await GenerateOrderAsync(option: 3, km: 5);
        //await GenerateOrderAsync(option: 2, km: 6);
        //await GenerateOrderAsync(option: 3, km: 11);

        while (true)
        {
            ShowUIMenu();
            Console.Write("\nElige una opción (1-5, o 'q' para salir): ");
            string optionStr = Console.ReadLine();

            if (optionStr?.ToLower() == "q")
                break;

            if (int.TryParse(optionStr, out int option) && option >= 1 && option <= 5)
            {
                Console.Write("Distancia de reparto en kilómetros (1-20): ");

                if (int.TryParse(Console.ReadLine(), out int km) && km > 0)
                    await GenerateOrderAsync(option, km);
                else
                    Console.WriteLine("Distancia no válida.");
            }
            else
            {
                Console.WriteLine("Opción no válida.");
            }
        }

        Console.WriteLine("\n[Sistema] Cerrando el restaurante. Esperando pedidos pendientes...");
        
        kitchenChannel.Writer.Complete();
        await Task.WhenAll(tKitchen1, tKitchen2);
        
        deliveryChannel.Writer.Complete();
        await Task.WhenAll(tDelivery1, tDelivery2);

        Console.WriteLine("[Sistema] Todos los pedidos han sido entregados. Cierre completado.");
    }

    static void ShowUIMenu()
    {
        Console.WriteLine("\n--- NUEVO PEDIDO ---");
        Console.WriteLine("1. Pizza margarita (cocinado: 4 s)");
        Console.WriteLine("2. Hamburguesa completa (cocinado: 5 s)");
        Console.WriteLine("3. Ensalada César (cocinado: 3 s)");
        Console.WriteLine("4. Sushi variado (cocinado: 6 s)");
        Console.WriteLine("5. Kebab mixto (cocinado: 4 s)");
    }

    static async Task GenerateOrderAsync(int option, int km)
    {
        int id = Interlocked.Increment(ref orderCount);
        
        string menuStr;
        int cookingTime;

        switch (option)
        {
            case 1:
                menuStr = "Pizza margarita";
                cookingTime = 4000;
                break;

            case 2:
                menuStr = "Hamburguesa completa";
                cookingTime = 5000;
                break;

            case 3:
                menuStr = "Ensalada César";
                cookingTime = 3000;
                break;

            case 4:
                menuStr = "Sushi variado";
                cookingTime = 6000;
                break;

            case 5:
                menuStr = "Kebab mixto";
                cookingTime = 4000;
                break;

            default:
                menuStr = "Desconocido";
                cookingTime = 3000;
                break;
        }

        var newOrder = new Order
        {
            Id = id,
            Menu = menuStr,
            Kilometers = km,
            CookingTimeMs = cookingTime
        };

        Console.WriteLine($"[Registro] Pedido #{id} ({menuStr}, a {km} km) recibido.");
        
        // TODO: enviar el pedido al canal de cocina.
    }

    static async Task CookTask(string cookName)
    {
        // TODO:
        // Leer pedidos del canal de cocina.
        // Simular el tiempo de cocinado.
        // Enviar cada pedido cocinado al canal de reparto.
        // Terminar cuando el canal de cocina se cierre.
    }

    static async Task DeliveryTask(string deliveryName)
    {
        // TODO:
        // Leer pedidos del canal de reparto.
        // Simular el tiempo de entrega según la distancia.
        // Mostrar por consola que el pedido ha sido entregado.
        // Terminar cuando el canal de reparto se cierre.
    }
}
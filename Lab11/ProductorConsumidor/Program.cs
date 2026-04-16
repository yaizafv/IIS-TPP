namespace ProductorConsumidor;

public class Program
{
    // Arregla los errores en el siguiente código.
    static void Main()
    {
        int capacidadMaxima = 5;
        Queue<TareaImpresion> cola = new Queue<TareaImpresion>();
        // El productor envía (o queda a la espera)
        Productor productor = new Productor(cola, capacidadMaxima);
        // El consumidor extrae (o queda a la espera)
        Consumidor consumidor = new Consumidor(cola);

        new Thread(productor.Run).Start();
        Thread.Sleep(2000);
        new Thread(consumidor.Run).Start();
    }
}

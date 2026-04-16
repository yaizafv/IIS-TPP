namespace CondicionCarrera;

class Program
{

    static void Main(string[] args)
    {
        // 200 elementos con valores en [0, 10]
        short[] vector = CrearVectorAleatorio(200000, 0, 10);

        // Recuento de los números 2 y 3 en un vector.
        // Secuencial
 
        int recuentoSecuencial = 0;
        for(int i = 0; i < vector.Length; i++)
        {
            if (vector[i] is 2 or 3)
                recuentoSecuencial++;
        }
        Console.WriteLine($"[Secuencial] Los números 2 y 3 aparecen {recuentoSecuencial} veces.");
    
        
        // Multihilo
        
        int recuentoMultihilo = 0;

        Thread[] hilos = new Thread[4];
        for(int i = 0; i < hilos.Length; i++)
        {
            int inicio = i * vector.Length / hilos.Length;
            int fin = inicio + vector.Length / hilos.Length;
            if (i == hilos.Length - 1)
                fin = vector.Length;

            hilos[i] = new Thread(() =>
            {
                for(int i = inicio; i < fin; i++)
                {
                    if (vector[i] is 2 or 3)        //el vector no es recurso compartido porque solo se lee
                    {   
                        recuentoMultihilo++;        //recurso compartido
                    }
                }                
            });
            hilos[i].Start();
        }

        foreach (var hilo in hilos)
            hilo.Join();

        Console.WriteLine($"[Multihilo] Los números 2 y 3 aparecen {recuentoMultihilo} veces.");

        // Ejecuta el programa varias veces. Luego, incrementa drásticamente el tamaño del vector. ¿Qué ocurre?
        
        // El acceso concurrente a estado (o recursos) compartidos debe coordinarse
        // CUANDO PUEDA DAR lugar a condiciones de carrera
        // o a problemas de atomicidad y consistencia (repasa la teoría).
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

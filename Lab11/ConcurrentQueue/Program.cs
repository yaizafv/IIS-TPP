using System;

namespace ConcurrentQueue;

public class Program
{
    static void Main()
    {
        int numHilos = 10000;
        ConcurrentQueue<string> queue = new ConcurrentQueue<string>();
        Console.WriteLine("¿Esta vacia? " + queue.IsEmpty());
        queue.Enqueue("a");
        queue.Enqueue(null);
        queue.Enqueue("b");
        Console.WriteLine("Numero de elementos: " + queue.Count);
        Thread[] threads = new Thread[numHilos];

        for (int i = 0; i < numHilos; i++)
        {
            threads[i] = new Thread(() =>
            {
                for (int i = 0; i < 5; i++)
                {
                    string a = $"Elemento {i}";
                    queue.Enqueue(a);
                }
            });
            threads[i].Start();
        }

        foreach (Thread thread in threads)
        {
            thread.Join();
        }

        Console.WriteLine("¿Esta vacia? " + queue.IsEmpty());
        Console.WriteLine("Numero de elementos: " + queue.Count);
    }
}
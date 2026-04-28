using System;

namespace MasterWorkerEj;

public class Master
{

    private short[] vector1;
    private short[] vector2;
    private int numHilos;

    public Master(short[] vector1, short[] vector2, int numHilos)
    {
        if (numHilos > 30)
        {
            throw new InvalidDataException("el numero de hilos no puede ser superior a 30");
        }
        this.vector1 = vector1;
        this.vector2 = vector2;
        this.numHilos = numHilos;
    }

    public int Concurrences()
    {
        Worker[] workers = new Worker[numHilos];
        int elementsPerThread = this.vector1.Length / numHilos;
        for (int i = 0; i < numHilos; i++)
        {
            workers[i] = new Worker(this.vector1, this.vector2, i * elementsPerThread,
                        (i < this.numHilos - 1) ? (i + 1) * elementsPerThread - 1 : this.vector1.Length - 1);
        }

        Thread[] hilos = new Thread[numHilos];
        for (int i = 0; i < numHilos; i++)
        {
            hilos[i] = new Thread(workers[i].Coincidencias);
        }
        for (int i = 0; i < hilos.Length; i++)
        {
            hilos[i].Start();
        }
        for (int i = 0; i < hilos.Length; i++)
        {
            hilos[i].Join();
        }

        int total = 0;
        foreach (Worker worker in workers)
        {
            total += worker.Result;
        }
        return total;
    }



}

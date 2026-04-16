using System;

namespace activity10;

public class BitcoinMaster
{
    private BitcoinValueData[] data;
    private int numberOfThreads;
    private double threshold;

    public BitcoinMaster(BitcoinValueData[] data, int numberOfThreads, double threshold)
    {
        if (numberOfThreads < 1 || numberOfThreads > data.Length)
            throw new ArgumentException("Número de hilos no válido.");
        this.data = data;
        this.numberOfThreads = numberOfThreads;
        this.threshold = threshold;
    }

    public int ComputeCount()
    {
        BitcoinWorker[] workers = new BitcoinWorker[this.numberOfThreads];
        int elementsPerThread = this.data.Length / numberOfThreads;

        for (int i = 0; i < this.numberOfThreads; i++)
        {
            int from = i * elementsPerThread;
            int to = (i == this.numberOfThreads - 1) ? this.data.Length - 1 : (i + 1) * elementsPerThread - 1;
            workers[i] = new BitcoinWorker(this.data, from, to, this.threshold);
        }

        Thread[] threads = new Thread[workers.Length];
        for (int i = 0; i < workers.Length; i++)
        {
            threads[i] = new Thread(workers[i].Compute);
            threads[i].Start();
        }

        foreach (Thread thread in threads)
        {
            thread.Join();
        }

        int totalCount = 0;
        foreach (var worker in workers)
        {
            totalCount += worker.Result;
        }
        return totalCount;
    }
}

namespace ConcurrentQueueTest;
using ConcurrentQueue;
[TestClass]
public sealed class Test1
{
    [TestMethod]
    public void EmptyQueue()
    {
        ConcurrentQueue<int> queue = new ConcurrentQueue<int>();
        Assert.IsTrue(queue.IsEmpty());
        Assert.AreEqual(0, queue.Count);
    }

    [TestMethod]
    public void QueueNullElement()
    {
        ConcurrentQueue<string>  queue = new ConcurrentQueue<string>();
        queue.Enqueue(null); 
        Assert.AreEqual(1, queue.Count);
        Assert.IsNull(queue.Peek()); 
        Assert.IsNull(queue.Dequeue());
    }

    [TestMethod]
    public void QueueEnqueueThreads()
    {
        int numHilos = 10000;
        ConcurrentQueue<int> queue = new ConcurrentQueue<int>();
        Console.WriteLine("¿Esta vacia? " + queue.IsEmpty());
        queue.Enqueue(1);
        Console.WriteLine("Numero de elementos: " + queue.Count);
        Thread[] threads = new Thread[numHilos];

        for (int i = 0; i < numHilos; i++)
        {
            threads[i] = new Thread(() =>
            {
                for (int i = 0; i < 5; i++)
                {
                    queue.Enqueue(i);
                }
            });
            threads[i].Start();
        }

        foreach (Thread thread in threads)
        {
            thread.Join();
        }

        Assert.AreEqual(numHilos * 5 + 1, queue.Count);
    }

    [TestMethod]
    public void QueueEnqueueDequeueThreads()
    {
        int numHilos = 10000;
        ConcurrentQueue<int> queue = new ConcurrentQueue<int>();
        Console.WriteLine("¿Esta vacia? " + queue.IsEmpty());
        queue.Enqueue(1);
        Console.WriteLine("Numero de elementos: " + queue.Count);
        Thread[] threads = new Thread[numHilos];

        for (int i = 0; i < numHilos; i++)
        {
            threads[i] = new Thread(() =>
            {
                for (int i = 0; i < 5; i++)
                {
                    queue.Enqueue(i);
                    queue.Dequeue();
                }
            });
            threads[i].Start();
        }

        foreach (Thread thread in threads)
        {
            thread.Join();
        }

        Assert.AreEqual(1, queue.Count);
    }

    [TestMethod]
    public void DequeueEmptyQueue()
    {
        ConcurrentQueue<int> queue = new ConcurrentQueue<int>();
        Assert.ThrowsException<IndexOutOfRangeException>(() => queue.Dequeue());
    }
}

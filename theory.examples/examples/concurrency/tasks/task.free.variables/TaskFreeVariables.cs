using System;
using System.Threading;
using System.Threading.Tasks;

class TaskData
{
    public long CreationTime { get; set; }
    public int Name { get; set; }
    public int ThreadNum { get; set; }
}

public class TaskFreeVariables
{
    public static void Main()
    {
        // Create the task object by using an Action(Of Object) to pass in the loop
        // counter. This produces an unexpected result.
        Task[] taskArray = new Task[10];
        for (int i = 0; i < taskArray.Length; i++)
        {
            taskArray[i] = Task.Factory.StartNew((Object obj) =>
            {
                var data = new TaskData()
                {
                    Name = i,
                    CreationTime = DateTime.Now.Ticks,
                    ThreadNum = Thread.CurrentThread.ManagedThreadId
                };

                Console.WriteLine("Task #{0} created at {1} on thread #{2}.",
                                  data.Name, data.CreationTime, data.ThreadNum);
            }, i); // i is shared by all created tasks!
        }
        Task.WaitAll(taskArray);
    }
}




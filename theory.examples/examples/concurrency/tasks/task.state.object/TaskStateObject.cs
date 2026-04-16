using System;
using System.Threading;
using System.Threading.Tasks;

class TaskData
{
    public long CreationTime { get; set; }
    public int Name { get; set; }
    public int ThreadNum { get; set; }
}

public class TaskStateObject
{
    public static void Main()
    {
        // Create the task object by using an Action(Of Object) to pass in custom data
        // to the Task constructor. This is useful when you need to capture outer variables
        // from within a loop. 
        Task[] taskArray = new Task[10];
        for (int i = 0; i < taskArray.Length; i++)
        {
            taskArray[i] = Task.Factory.StartNew((Object obj) => {
                TaskData data = obj as TaskData;
                if (data == null)
                    return;

                data.ThreadNum = Thread.CurrentThread.ManagedThreadId;
                Console.WriteLine("Task #{0} created at {1} on thread #{2}.",
                                 data.Name, data.CreationTime, data.ThreadNum);
            }, new TaskData() { Name = i, CreationTime = DateTime.Now.Ticks });
        }
        Task.WaitAll(taskArray);
    }
}

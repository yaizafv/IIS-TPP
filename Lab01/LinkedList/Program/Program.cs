namespace Program;

using Library;

class Program
{
    static void Main(string[] args)
    {
        LinkedList list = new LinkedList();
        list.Add(21);
        list.Add("hola");
        list.Add(new Client("Yaiza"));
        list.Add(null);
        Console.WriteLine(list.Count);
        Console.WriteLine(list.Contains(21));
        list.Remove(null);
        list.RemoveAt(2);
        Console.WriteLine(list.Count);
        list.Clear();
        Console.WriteLine(list.Count);
        Console.WriteLine(list.Contains(21));
    }
}

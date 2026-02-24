namespace LinkedListIterableGenericaTest;

using LinkedListIterableGenerica;

[TestClass]
public sealed class Test1
{
    [TestMethod]
    public void testEnteros()
    {
        LinkedList<int> listaInt = new LinkedList<int>();
        listaInt.Add(1);
        listaInt.Add(2);
        listaInt.Add(3);

        foreach (int n in listaInt)
        {
            Console.WriteLine($"Valor: {n}");
        }
    }

    [TestMethod]
    public void testStrings()
    {
        LinkedList<String> listaStr = new LinkedList<string>();
        listaStr.Add("Hola");
        listaStr.Add("Mundo");

        Console.WriteLine("\n--- Test Strings ---");
        foreach (string s in listaStr)
        {
            Console.WriteLine($"Texto: {s}");
        }
    }
}

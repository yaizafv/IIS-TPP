namespace LinkedListIterableGenericaTest;
using LinkedListIterableGenerica;

[TestClass]
public sealed class Test1
{
    [TestMethod]
    public void TestCasosNormales()
    {
        // Test con Enteros
        LinkedList<int> listaInt = new LinkedList<int>();
        listaInt.Add(1);
        listaInt.Add(2);
        listaInt.Add(3);

        Console.WriteLine("--- Test Enteros ---");
        foreach (int n in listaInt)
        {
            Console.WriteLine($"Valor: {n}"); // Debería imprimir 1, 2, 3
        }

        // Test con Strings (Tipos de referencia)
        LinkedList<string> listaStr = new LinkedList<string>();
        listaStr.Add("Hola");
        listaStr.Add("Mundo");

        Console.WriteLine("\n--- Test Strings ---");
        foreach (string s in listaStr)
        {
            Console.WriteLine($"Texto: {s}");
        }
    }
}

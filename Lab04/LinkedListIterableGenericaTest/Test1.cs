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
        Assert.AreEqual(1, listaInt[0]);
        Assert.AreEqual(2, listaInt[1]);
        Assert.AreEqual(3, listaInt[2]);
    }

    [TestMethod]
    public void testStrings()
    {
        LinkedList<String> listaStr = new LinkedList<String>();
        listaStr.Add("Hola");
        listaStr.Add("Mundo");

        foreach (string s in listaStr)
        {
            Console.WriteLine($"Texto: {s}");
        }
        Assert.AreEqual("Hola", listaStr[0]);
        Assert.AreEqual("Hola", listaStr[1]);
    }

    public void testDouble()
    {
        LinkedList<double> listaInt = new LinkedList<double>();
        listaInt.Add(1.5);
        listaInt.Add(2.3);
        listaInt.Add(3.7);

        foreach (int n in listaInt)
        {
            Console.WriteLine($"Valor: {n}");
        }
    }
}

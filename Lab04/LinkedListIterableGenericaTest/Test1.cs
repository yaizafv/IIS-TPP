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
        Assert.AreEqual(3, listaInt.Count);
        Assert.AreEqual(1, listaInt.ElementAt(0));
        Assert.AreEqual(2, listaInt.ElementAt(1));
        Assert.AreEqual(3, listaInt.ElementAt(2));
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
        Assert.AreEqual(2, listaStr.Count);
        Assert.AreEqual("Hola", listaStr.ElementAt(0));
        Assert.AreEqual("Mundo", listaStr.ElementAt(1));
    }

    public void testDouble()
    {
        LinkedList<double> listaDouble = new LinkedList<double>();
        listaDouble.Add(1.5);
        listaDouble.Add(2.3);
        listaDouble.Add(3.7);

        foreach (int n in listaDouble)
        {
            Console.WriteLine($"Valor: {n}");
        }
        Assert.AreEqual(3, listaDouble.Count);
        Assert.AreEqual(1.5, listaDouble.ElementAt(0));
        Assert.AreEqual(2.3, listaDouble.ElementAt(1));
        Assert.AreEqual(3.7, listaDouble.ElementAt(2));
    }
}

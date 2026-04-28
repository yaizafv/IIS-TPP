namespace SortedListTest;

using SortedList;

[TestClass]
public class SortedListTest
{
    private SortedList lista;

    [TestInitialize]
    public void TestInitialize()
    {
        lista = new SortedList();
    }

    [TestMethod]
    public void Add_int()
    {
        lista.Add(5);
        lista.Add(1);
        lista.Add(3);

        Assert.AreEqual(3, lista.Count);
        Assert.AreEqual(1, lista.ElementAt(0));
        Assert.AreEqual(3, lista.ElementAt(1));
        Assert.AreEqual(5, lista.ElementAt(2)); 
    }

    [TestMethod]
    public void Add_Strings()
    {
        lista.Add("A");
        lista.Add("B");
        lista.Add("C");

        Assert.AreEqual(3, lista.Count);
        Assert.AreEqual("A", lista.ElementAt(0));
        Assert.AreEqual("B", lista.ElementAt(1));
        Assert.AreEqual("C", lista.ElementAt(2));
    }

    [TestMethod]
    public void Add_ConDuplicados_InsertaInmediatamenteDespues()
    {
        lista.Add(10);
        lista.Add(10);

        Assert.AreEqual(2, lista.Count);
        Assert.AreEqual(10, lista.ElementAt(0));
        Assert.AreEqual(10, lista.ElementAt(1));
    }

    [TestMethod]
    public void Add_ValorNulo()
    {
        lista.Add(null);

        Assert.AreEqual(1, lista.Count);
    }

    [TestMethod]
    public void Add_NullRepetidos()
    {
        lista.Add(null);
        lista.Add(1);
        lista.Add(null);

        Assert.AreEqual(3, lista.Count);
        Assert.AreEqual(null, lista.ElementAt(0));
        Assert.AreEqual(null, lista.ElementAt(1));
        Assert.AreEqual(1, lista.ElementAt(2));
    }

    [TestMethod]
    public void ElementAt_IndiceFueraDeRango_LanzaExcepcion()
    {
        Assert.ThrowsException<IndexOutOfRangeException>(() => lista.ElementAt(0));
        lista.Add(5);
        Assert.ThrowsException<IndexOutOfRangeException>(() => lista.ElementAt(5));
        Assert.ThrowsException<IndexOutOfRangeException>(() => lista.ElementAt(500)); 
    }

    [TestMethod]
    public void RemoveAt_IndiceFueraDeRango_LanzaExcepcion()
    {
        lista.Add(10);
        Assert.ThrowsException<IndexOutOfRangeException>(() => lista.RemoveAt(1));
        Assert.ThrowsException<IndexOutOfRangeException>(() => lista.RemoveAt(500)); 
    }

    [TestMethod]
    public void Contains_DevuelveTrueSiExisteYFalseSiNo()
    {
        lista.Add(100);
        lista.Add(200);

        Assert.IsTrue(lista.Contains(100));
        Assert.IsTrue(lista.Contains(200));
        Assert.IsFalse(lista.Contains(999));
        Assert.IsFalse(lista.Contains(null));
    }

    [TestMethod]
    public void Remove_EliminaElementoMantieneOrdenYDevuelveTrue()
    {
        lista.Add(50);
        lista.Add(10);
        lista.Add(30);

        bool resultado = lista.Remove(30);

        Assert.IsTrue(resultado);
        Assert.AreEqual(2, lista.Count);
        Assert.AreEqual(10, lista.ElementAt(0));
        Assert.AreEqual(50, lista.ElementAt(1));
    }

    [TestMethod]
    public void Remove_ElementoNoExistente_DevuelveFalse()
    {
        lista.Add(10);

        bool resultado = lista.Remove(99);

        Assert.IsFalse(resultado);
        Assert.AreEqual(1, lista.Count);
    }
    
    [TestMethod]
    public void Clear_VaciaLaListaPorCompleto()
    {
        lista.Add(1);
        lista.Add(2);
        lista.Add(3);

        lista.Clear();
        
        Assert.AreEqual(0, lista.Count);
        Assert.IsFalse(lista.Contains(1));
        Assert.ThrowsException<IndexOutOfRangeException>(() => lista.ElementAt(0));
    }
}

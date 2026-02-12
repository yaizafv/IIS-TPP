
namespace InmutableListTest;

using InmutableList;

[TestClass]
public class InmutableListTests
{
    [TestMethod]
    public void test_Add()
    {
        InmutableList listaOriginal = new InmutableList();
        InmutableList listaConUno = listaOriginal.Add(1);
        InmutableList listaConDos = listaConUno.Add(2);

        Assert.AreEqual(0, listaOriginal.Count);
        Assert.AreEqual(1, listaConUno.Count);
        Assert.AreEqual(2, listaConDos.Count);
        Assert.AreNotSame(listaConUno, listaConDos);
    }

    [TestMethod]
    public void test_Set()
    {
        InmutableList lista = new InmutableList().Add("Original");
        InmutableList listaModificada = lista.Set(0, "Modificado");

        Assert.AreEqual("Original", lista.ElementAt(0));
        Assert.AreEqual("Modificado", listaModificada.ElementAt(0));
    }

    [TestMethod]
    public void test_Remove()
    {
        InmutableList lista = new InmutableList().Add("Borrar");
        InmutableList listaPostBorrado = lista.Remove("Borrar");

        Assert.IsTrue(lista.Contains("Borrar"));
        Assert.AreEqual(0, listaPostBorrado.Count);
    }

    [TestMethod]
    public void test_ElementAt()
    {
        InmutableList lista = new InmutableList().Add("Dato");
        Assert.ThrowsException<IndexOutOfRangeException>(
            () => lista.ElementAt(10)
        );
    }

    [TestMethod]
    public void test_Clear()
    {
        InmutableList listaLlena = new InmutableList().Add(1);
        InmutableList listaVacia = listaLlena.Clear();

        Assert.AreEqual(1, listaLlena.Count);
        Assert.AreEqual(0, listaVacia.Count);
    }
}
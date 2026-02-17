
namespace InmutableListTest;

using InmutableList;

[TestClass]
public class InmutableListTests
{
    [TestMethod]
    public void Add_DeberiaRetornarNuevaListaYNoModificarOriginal()
    {
        InmutableList listaOriginal = new InmutableList(new object[] { "A", "B" });
        InmutableList nuevaLista = listaOriginal.Add("C");
        Assert.AreEqual(2, listaOriginal.Count);
        Assert.AreEqual(3, nuevaLista.Count);
        Assert.AreEqual("C", nuevaLista.ElementAt(2));
    }

    [TestMethod]
    public void Remove_DeberiaRetornarNuevaListaSinElElemento()
    {
        InmutableList listaOriginal = new InmutableList(new object[] { "A", "B", "C" });
        InmutableList nuevaLista = listaOriginal.Remove("B");
        Assert.AreEqual(3, listaOriginal.Count, "La original sigue teniendo 3");
        Assert.AreEqual(2, nuevaLista.Count, "La nueva debe tener 2");
        Assert.IsFalse(nuevaLista.Contains("B"), "La nueva lista no debe contener 'B'");
    }

    [TestMethod]
    public void Set_DeberiaReemplazarElementoEnNuevaLista()
    {
        InmutableList listaOriginal = new InmutableList(new object[] { "Viejo" });
        InmutableList nuevaLista = listaOriginal.Set(0, "Nuevo");
        Assert.AreEqual("Viejo", listaOriginal.ElementAt(0));
        Assert.AreEqual("Nuevo", nuevaLista.ElementAt(0));
    }

    [TestMethod]
    public void Insert_DeberiaDesplazarElementosEnNuevaLista()
    {
        InmutableList listaOriginal = new InmutableList(new object[] { 1, 3 });
        InmutableList nuevaLista = listaOriginal.Insert(1, 2);
        Assert.AreEqual(2, listaOriginal.Count);
        Assert.AreEqual(3, nuevaLista.Count);
        Assert.AreEqual(1, nuevaLista.ElementAt(0));
        Assert.AreEqual(2, nuevaLista.ElementAt(1));
        Assert.AreEqual(3, nuevaLista.ElementAt(2));
    }

    [TestMethod]
    public void Clear_DeberiaRetornarListaVaciaYNoAfectarOriginal()
    {
        InmutableList listaOriginal = new InmutableList(new object[] { 1, 2, 3 });
        InmutableList listaVacia = listaOriginal.Clear();
        Assert.AreEqual(3, listaOriginal.Count);
        Assert.AreEqual(0, listaVacia.Count);
    }

    [TestMethod]
    public void ElementAt_IndiceFueraDeRango_DeberiaLanzarExcepcion()
    {
        InmutableList lista = new InmutableList(new object[] { "A" });
        Assert.ThrowsException<IndexOutOfRangeException>(() => lista.ElementAt(5));
    }
}

namespace SortedListTest;
using Sorted;

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
    public void Add_MantieneElOrdenConEnteros()
    {
        // Arrange (Preparar)
        lista.Add(5);
        lista.Add(1);
        lista.Add(3);

        // Act & Assert (Actuar y Comprobar)
        Assert.AreEqual(3, lista.Count);
        Assert.AreEqual(1, lista.ElementAt(0));
        Assert.AreEqual(3, lista.ElementAt(1));
        Assert.AreEqual(5, lista.ElementAt(2)); // El orden debe ser 1, 3, 5
    }

    [TestMethod]
    public void Add_MantieneElOrdenConStrings()
    {
        // Prueba con un tipo de dato distinto (String)
        lista.Add("Zorro");
        lista.Add("Abeja");
        lista.Add("Mono");

        Assert.AreEqual(3, lista.Count);
        Assert.AreEqual("Abeja", lista.ElementAt(0));
        Assert.AreEqual("Mono", lista.ElementAt(1));
        Assert.AreEqual("Zorro", lista.ElementAt(2));
    }

    [TestMethod]
    public void Add_ConDuplicados_InsertaInmediatamenteDespues()
    {
        // Según el enunciado, los duplicados van inmediatamente después
        lista.Add(10);
        lista.Add(10); // Duplicado

        Assert.AreEqual(2, lista.Count);
        Assert.AreEqual(10, lista.ElementAt(0));
        Assert.AreEqual(10, lista.ElementAt(1));
    }

    // ==========================================
    // PRUEBAS DE CASOS LÍMITE (EDGE CASES)
    // ==========================================

    [TestMethod]
    public void Add_ValorNulo_NoFallaYNoLoAñade()
    {
        // Comprobación de caso límite: Valores nulos
        lista.Add(null);

        Assert.AreEqual(0, lista.Count);
    }

    [TestMethod]
    public void ElementAt_IndiceFueraDeRango_LanzaExcepcion()
    {
        // Comprobación de caso límite: Índice fuera de rango en lista vacía
        Assert.ThrowsException<IndexOutOfRangeException>(() => lista.ElementAt(0));

        // Comprobación con elementos pero índice negativo o excesivo
        lista.Add(5);
        Assert.ThrowsException<IndexOutOfRangeException>(() => lista.ElementAt(5)); // Solo hay 1 elemento
        Assert.ThrowsException<IndexOutOfRangeException>(() => lista.ElementAt(-1)); // Índice negativo
    }

    [TestMethod]
    public void RemoveAt_IndiceFueraDeRango_LanzaExcepcion()
    {
        lista.Add(10);

        // Intentar borrar en un índice que no existe
        Assert.ThrowsException<IndexOutOfRangeException>(() => lista.RemoveAt(1));
        Assert.ThrowsException<IndexOutOfRangeException>(() => lista.RemoveAt(-1));
    }

    // ==========================================
    // PRUEBAS DEL RESTO DE MÉTODOS
    // ==========================================

    [TestMethod]
    public void Contains_DevuelveTrueSiExisteYFalseSiNo()
    {
        lista.Add(100);
        lista.Add(200);

        Assert.IsTrue(lista.Contains(100));
        Assert.IsTrue(lista.Contains(200));
        Assert.IsFalse(lista.Contains(999)); // No existe
        Assert.IsFalse(lista.Contains(null)); // Buscar nulo en estructura vacía/llena
    }

    [TestMethod]
    public void Remove_EliminaElementoMantieneOrdenYDevuelveTrue()
    {
        lista.Add(50);
        lista.Add(10);
        lista.Add(30);

        // La lista es: 10, 30, 50
        bool resultado = lista.Remove(30);

        Assert.IsTrue(resultado); // Debe devolver true al encontrarlo
        Assert.AreEqual(2, lista.Count);
        Assert.AreEqual(10, lista.ElementAt(0));
        Assert.AreEqual(50, lista.ElementAt(1)); // 50 pasa a la posición 1
    }

    [TestMethod]
    public void Remove_ElementoNoExistente_DevuelveFalse()
    {
        lista.Add(10);

        bool resultado = lista.Remove(99); // 99 no está

        Assert.IsFalse(resultado);
        Assert.AreEqual(1, lista.Count); // El Count no debe cambiar
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
        // Aseguramos que lanzar ElementAt en 0 ahora falla porque está vacía
        Assert.ThrowsException<IndexOutOfRangeException>(() => lista.ElementAt(0));
    }
}

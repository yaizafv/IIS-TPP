namespace Test;

using Transacciones;

[TestClass]
public sealed class Test1
{
    [TestMethod]
    public void MapCuadrado()
    {
        IList<int> numeros = new List<int> { 2, 5, 9, 3 };
        IList<int> result = new List<int> { 4, 25, 81, 9 };
        IEnumerable<int> cuadrados = Program.Map(numeros, x => x * x);
        Assert.AreEqual(result, cuadrados);
    }

    [TestMethod]
    public void MapContarVocales()
    {
        IList<string> palabras = new List<string> { "hola", "adios", "mundo", "prueba" };
        IList<int> numeroVocales = new List<int> { 2, 3, 2, 3 };
        IEnumerable<int> conteoVocales = Program.Map(palabras, p => p.ToLower().Count(c => "aeiou".Contains(c)));
        Assert.AreEqual(conteoVocales, numeroVocales);
    }

    [TestMethod]
    public void FilterAllTrue()
    {
        IList<int> numeros = new List<int> { 2, 5, 9, 3 };
        IEnumerable<int> condicion = Program.Filter(numeros, numero => numero < 10);
        Assert.AreEqual(condicion, numeros);
    }

    [TestMethod]
    public void FilterNotAllTrue()
    {
        IList<int> numeros = new List<int> { 2, 5, 9, 3 };
        IList<int> filtrada = new List<int> {2, 3};
        IEnumerable<int> condicion = Program.Filter(numeros, numero => numero < 5);
        Assert.AreEqual(condicion, filtrada);
    }

    [TestMethod]
    public void FilterAllFalse()
    {
        IList<int> numeros = new List<int> { 2, 5, 9, 3 };
        IList<int> filtrada = new List<int> {};
        IEnumerable<int> condicion = Program.Filter(numeros, numero => numero < 2);
        Assert.AreEqual(condicion, filtrada);
    }

    [TestMethod]
    public void ReduceSameType()
    {
        IList<int> numeros = new List<int> { 2, 5, 9, 3 };
        int result = Program.Reduce(numeros, 0, (x, acc) => x + acc);
        Assert.AreEqual(result, 19);
    }

    [TestMethod]
    public void ReduceWithSeed()
    {
        IList<int> numeros = new List<int> { 2, 5, 9, 3 };
        int result = Program.Reduce(numeros, 0, (x, acc) => x + acc);
        Assert.AreEqual(result, 19);
    }

    [TestMethod]
    public void ReduceDifferentType()
    {
        
    }

    [TestMethod]
    public void ReduceDifferentTypeWithSeed()
    {
        
    }

    [TestMethod]
    public void ZipSameTypeAndLength()
    {
        IList<int> numeros1 = new List<int> { 2, 5, 9, 3 };
        IList<int> numeros2 = new List<int> { 1, 7, 6, 3 };
        var result = Program.Zip(numeros1, numeros2);
        Assert.AreEqual(4, result.Count());
    }

    [TestMethod]
    public void ZipDifferentTypeAndLength()
    {
        IList<int> numeros1 = new List<int> { 2, 5, 9, 3 };
        IList<string> palabras = new List<string> { "hola", "adios", "mundo", "prueba" };
        var result = Program.Zip(numeros1, palabras);
        Assert.AreEqual(4, result.Count());
    }

    [TestMethod]
    public void AllCombined()
    {
        var inputs = new[] { 1, 2, 3, 4 };
        var pas1 = Program.Map(inputs, x => x * 2);
        var pas2 = Program.Filter(pas1, x => x > 5);
        var res = Program.Reduce(pas2, 0, (x, acc) => x + acc); 
        Assert.AreEqual(14, res);
    }    
}
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
        CollectionAssert.AreEqual(cuadrados.ToList(), result.ToList());
    }

    [TestMethod]
    public void MapContarVocales()
    {
        IList<string> palabras = new List<string> { "hola", "adios", "mundo", "prueba" };
        IList<int> numeroVocales = new List<int> { 2, 3, 2, 3 };
        IEnumerable<int> conteoVocales = Program.Map(palabras, p => p.ToLower().Count(c => "aeiou".Contains(c)));
        CollectionAssert.AreEqual(conteoVocales.ToList(), numeroVocales.ToList());
    }

    [TestMethod]
    public void FilterAllTrue()
    {
        IList<int> numeros = new List<int> { 2, 5, 9, 3 };
        IEnumerable<int> condicion = Program.Filter(numeros, numero => numero < 10);
        CollectionAssert.AreEqual(condicion.ToList(), numeros.ToList());
    }

    [TestMethod]
    public void FilterNotAllTrue()
    {
        IList<int> numeros = new List<int> { 2, 5, 9, 3 };
        IList<int> filtrada = new List<int> {2, 3};
        IEnumerable<int> condicion = Program.Filter(numeros, numero => numero < 5);
        CollectionAssert.AreEqual(condicion.ToList(), filtrada.ToList());
    }

    [TestMethod]
    public void FilterAllFalse()
    {
        IList<int> numeros = new List<int> { 2, 5, 9, 3 };
        IList<int> filtrada = new List<int> {};
        IEnumerable<int> condicion = Program.Filter(numeros, numero => numero < 2);
        CollectionAssert.AreEqual(condicion.ToList(), filtrada.ToList());
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
        int minimo = Program.Reduce(numeros, int.MaxValue, (x, acc) => x < acc ? x : acc);
        Assert.AreEqual(2, minimo);
    }

    [TestMethod]
    public void ReduceDifferentType()
    {
        IList<string> palabras = new List<string> { "hello", "world" };
        int totalLongitud = Program.Reduce(palabras, 0, (palabra, acc) => acc + palabra.Length);
        Assert.AreEqual(10, totalLongitud);
    }

    [TestMethod]
    public void ReduceDifferentTypeWithSeed()
    {
        string word = "hello";
        var seed = new Dictionary<char, int> { {'a', 0}, {'e', 0}, {'i', 0}, {'o', 0}, {'u', 0} };

        var resultado = Program.Reduce(word.ToCharArray(), seed, (caracter, dict) => 
        {
            if (dict.ContainsKey(caracter))
            {
                dict[caracter]++;
            }
            return dict;
        });

        Assert.AreEqual(1, resultado['e']);
        Assert.AreEqual(1, resultado['o']);
        Assert.AreEqual(0, resultado['a']);        
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
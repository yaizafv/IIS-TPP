namespace ForLoopTest;
using For;
[TestClass]
public class ForLoopTest
{

    [TestMethod]
    public void TestStandardIteration()
    {
        int i = -1;
        int counter = 0;

        Action initialize = () => i = 0;
        Func<bool> condition = () => i < 5;
        Action iteration = () => i = i + 1;
        Action body = () => counter++;

        Program.ForLoop(initialize, condition, iteration, body);

        Assert.AreEqual(counter, 5);
        Assert.AreEqual(i, 5);
    }

    [TestMethod]
    public void TestEmptyLoop_ConditionInitiallyFalse()
    {
        int i = -1;
        int contador = 0;

        Program.ForLoop(
            () => i = 100, 
            () => i < 10, // Falso inmediatamente
            () => i++, 
            () => contador++
        );

        Assert.AreEqual(0, contador);
        Assert.AreEqual(100, i);
    }

    [TestMethod]
    public void TestDiferentesTiposDeDatos_Double()
    {
        double d = 0.0;
        int pasos = 0;

        Program.ForLoop(
            () => d = 0.0, 
            () => d < 1.0, 
            () => d += 0.25, 
            () => pasos++
        );

        Assert.AreEqual(4, pasos);
    }
}
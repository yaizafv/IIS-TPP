namespace TestPila;
using Pila;


[TestClass]
public class PilaTest
{
    private Pila stack;

    [ClassInitialize]
    public static void ClassInitialize(TestContext context) { }

    [TestInitialize]
    public void TestInitialize()
    {
        stack = new Pila(capacidad: 3);
    }

    [TestCleanup]
    public void TestCleanup() { }

    [ClassCleanup]
    public static void ClassCleanup() { }

    [TestMethod]
    public void Capacidad_InicializadaEnConstructor()
    {
        Assert.AreEqual(3, stack.Capacidad);
    }

    [TestMethod]
    public void PushPop_DevuelveUltimoItem()
    {
        stack.Push(1);
        stack.Push(2);

        Assert.AreEqual(2, stack.Pop());
        Assert.AreEqual(1, stack.Count);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Pop_PilaVacia_ThrowsInvalidOperationException()
    {
        stack.Pop();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Push_PilaLlena_ThrowsInvalidOperationException()
    {
        stack.Push(1);
        stack.Push(2);
        stack.Push(3);
        stack.Push(4);
    }

    [TestMethod]
    [Timeout(1000)] // Timeout
    public void Contains()
    {
        for (int i = 0; i < 3; i++)
        {
            stack.Push(i);
        }
        Assert.IsTrue(stack.Contains(0));
        Assert.IsTrue(stack.Contains(1));
        Assert.IsTrue(stack.Contains(2));
        Assert.IsFalse(stack.Contains(999));
    }
}
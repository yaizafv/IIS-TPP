namespace testing;
using Library;

/*
	- Crea un proyecto de biblioteca de clases.
	- Crea una clase Pila, vacía.
	- Crea un proyecto de MsTest Test Proyect / Proyecto de prueba unitaria (MSTest)
	- Copia esta clase dentro (elimina la que crea por defecto).
	- Añade una referencia al proyecto de biblioteca de la pila.
	- Implementa la pila apoyándote en los tests y las acciones rápidas.

    Tipo examen: nos dan un test de una supuesta funcionalidad y a partir de eso hacer la clase
*/

[TestClass]
public class PilaTest
{
    private Pila pila;


    // Se ejecuta una única vez antes de comenzar los test
    [ClassInitialize]
    public static void ClassInitialize(TestContext contextp)
    {
        // Incialización de recursos compartidos para todos los tesst
    }


    // Se ejecuta antes de cada test.
    [TestInitialize]
    public void TestInitialize()
    {
        pila = new Pila(capacidad: 3);
    }

    
    // Se ejecuta después de cada test
    [TestCleanup]
    public void TestCleanup()
    {
        // Se suele usar para limpiar recursos creados durante la inicialización del test
    }

    
    // Se ejecuta una vez han finalizado todos los tests de la clase
    [ClassCleanup]
    public static void ClassCleanup()
    {
        // Liberar recursos.
    }

    // Test
    [TestMethod]
    public void Capacidad_SeInicializaEnElConstructor()
    {
        Assert.AreEqual(3, pila.Capacidad);
        // Como Capacidad es de solo lectura, la siguiente línea no compilaría, así que no puedes probarla.
        // Pero tampoco hace falta probarlo, porque el compilador ya lo impide.
        // pila.Capacidad = 5;
    }

    [TestMethod]
    public void PushYPop_DevuelveElUltimoElementoApilado()
    {
        pila.Push(1);
        pila.Push(2);

        Assert.AreEqual(2, pila.Pop());
        Assert.AreEqual(1, pila.Count);
    }

    
    // Se espera una excepción (versiones MsTest 3.8.0+)
/*    
    [TestMethod]
    public void Pop_EnPilaVacia_LanzaInvalidOperationException()
    {
        Assert.Throws<InvalidOperationException>(PopEmptyStack);        //le estas diciendo que ejecute ese metodo, por eso no lleva los ()
    }
*/

    //Se espera una excepción (versiones anteriores a MSTest v4)
    [TestMethod]
    public void Pop_EnPilaVacia_LanzaInvalidOperationException()
    {
        Assert.ThrowsException<InvalidOperationException>(PopEmptyStack);
    }

    private void PopEmptyStack()
    {
        pila.Pop();
    }


    // Se espera una excepción (versiones anteriores a MSTest v4)
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Push_EnPilaLlena_LanzaInvalidOperationException()
    {
        pila.Push(1);
        pila.Push(2);
        pila.Push(3);

        pila.Push(4); // Debería lanzar InvalidOperationException
    }


    // Test con timeout (bucles infinitos)
    [TestMethod]
    [Timeout(1000)] // 1 segundo
    public void Contains()
    {
        for (int i = 0; i < 3; i++)
        {
            pila.Push(i);
        }
        Assert.IsTrue(pila.Contains(0));
        Assert.IsTrue(pila.Contains(1));
        Assert.IsTrue(pila.Contains(2));
        Assert.IsFalse(pila.Contains(999));
    }
}

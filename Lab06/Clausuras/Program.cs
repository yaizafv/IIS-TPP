using System.Collections;

namespace lab06;

class Program
{
    static void Main()
    {
        EjemploClausurasEncapsulacionEstado();
        PruebaIteradorClausuras();
    }

    private static void EjemploClausurasEncapsulacionEstado()
    {
        Func<decimal, decimal> depositar = Cuenta(1000m);
        Console.WriteLine($"Depositar 100: {depositar(100m)}");
    }

    //clausura - funcion que almacena su estado o contexto 
    static Func<decimal, decimal> Cuenta(decimal inicial)       //no es una clausura
    {
        decimal balance = inicial; //variable local que será capturada ¿Por qué?
        string hola = "asd";
        // decimal depositar(decimal cantidad)     //clausura definida por balance. para que depositar exista necesita balance
        // {
        //     Console.WriteLine(hola);        //ahora hola tambien es una clausura. si no se usa dentro de depositar no es clausura
        //     if (cantidad <= 0)
        //     {
        //         throw new ArgumentException("La cantidad a depositar debe ser positiva");
        //     }
        //     balance += cantidad;
        //     return balance;
        // }
        Func<decimal, decimal> depositar = cantidad =>
        {
            if (cantidad <= 0)
            {
                throw new ArgumentException("La cantidad a depositar debe ser positiva");
            }
            balance += cantidad;
            return balance;
        };
        // Al devolver el delegado, el estado capturado sigue vivo mientras exista esta referencia.
        // En este caso, el estado capturado lo define exclusivamente la variable 'balance'.
        return depositar;
    }

    // EJERCICIO:
    // Imagina que, dentro de Cuenta, quieres devolver dos clausuras para una misma cuenta:
    //  - Una clausura para depositar.
    //  - Otra clausura para extraer.
    // Ambas deben trabajar sobre el mismo estado capturado. Impleméntalo

    static (Func<decimal, decimal>, Func<decimal, decimal>) Cuenta2(decimal inicial)
    {
        decimal balance = inicial;
        Func<decimal, decimal> depositar = (cantidad) =>
        {
            if (cantidad <= 0) throw new ArgumentException("Cantidad no válida");
            balance += cantidad; // Modifica el balance compartido
            return balance;
        };
        Func<decimal, decimal> extraer = (cantidad) =>
        {
            if (cantidad <= 0) throw new ArgumentException("Cantidad no válida");
            if (cantidad > balance) throw new InvalidOperationException("Saldo insuficiente");

            balance -= cantidad; // Modifica EL MISMO balance
            return balance;
        };
        return (depositar, extraer);
    }

    private static void PruebaIteradorClausuras()
    {
        LinkedList<string> miLista = new LinkedList<string>();
        miLista.Add("Primero");
        miLista.Add("Segundo");
        miLista.Add("Tercero");

        Console.WriteLine($"Lista creada con {miLista.Count} elementos.");

        IEnumeratorClausura<string> enumerator = miLista.GetEnumeratorClausura();

        Console.WriteLine("\n--- Primer Recorrido ---");
        while (enumerator.MoveNext())
        {
            Console.WriteLine($"Elemento actual: {enumerator.GetCurrent()}");
        }

        Console.WriteLine("\n--- Ejecutando Reset ---");
        enumerator.Reset();

        Console.WriteLine("--- Segundo Recorrido (post-reset) ---");
        if (enumerator.MoveNext())
        {
            Console.WriteLine($"Volvemos a empezar: {enumerator.GetCurrent()}");
        }

        // Demostración de independencia de clausuras
        // Creamos un segundo enumerador mientras el primero está a medias
        IEnumeratorClausura<string> enumerator2 = miLista.GetEnumeratorClausura();
        enumerator2.MoveNext(); // Apunta al "Primero"

        Console.WriteLine($"\nIndependencia: Enum1 está al final, Enum2 está en: {enumerator2.GetCurrent()}");
    }
}

//Examen: interfaz ienumerator generica empleando el concepto de clausuras. excepto el dispose. todo dentro de una funcion que devuelve una tupla de 3 (moveNext, current, reset)
//movenext : () => pos = -1;
public interface IEnumeratorClausura<T>
{
    // Una función que devuelve bool (como MoveNext)
    Func<bool> MoveNext { get; }

    // Una función que devuelve el valor T (como Current)
    Func<T> GetCurrent { get; }

    // Una acción que no devuelve nada (como Reset)
    Action Reset { get; }
}

public interface IEnumerableClausura<T>
{
    IEnumeratorClausura<T> GetEnumeratorClausura();
}

public class LinkedList<T> : IEnumerableClausura<T>
{
    private class Node
    {
        public T data;
        public Node next;

        public Node(T data)
        {
            this.data = data;
        }
    }

    private Node head;
    public int Count { private set; get; }

    public IEnumeratorClausura<T> GetEnumeratorClausura()
    {
        // --- ESTADO DE LA CLAUSURA ---
        // Estas variables son LOCALES del método. 
        // Normalmente morirían al llegar al final de esta función.
        Node actual = null;
        bool hasStarted = false;

        // --- DEFINICIÓN DE LAS LAMBDAS ---
        // Estas funciones "atrapan" las variables de arriba.
        Func<bool> moveNext = () =>
        {
            if (!hasStarted)
            {
                actual = this.head;
                hasStarted = true;
            }
            else if (actual != null)
            {
                actual = actual.next;
            }
            return actual != null;
        };

        Func<T> getCurrent = () =>
        {
            if (!hasStarted || actual == null) throw new InvalidOperationException();
            return actual.data;
        };

        Action reset = () =>
        {
            actual = null;
            hasStarted = false;
        };

        // --- DEVOLVEMOS EL OBJETO ---
        // Usamos una implementación simple que guarde estas 3 funciones
        return new ImplementacionClausura<T>(moveNext, getCurrent, reset);
    }
    public void Add(T item)
    {
        Node nuevo = new Node(item);
        if (head == null)
            head = nuevo;
        else
        {
            Node actual = head;
            while (actual.next != null)
            {
                actual = actual.next;
            }
            actual.next = nuevo;
        }
        Count++;
    }
}

// Una clase simple para transportar las 3 funciones
public class ImplementacionClausura<T> : IEnumeratorClausura<T>
{
    public Func<bool> MoveNext { get; }
    public Func<T> GetCurrent { get; }
    public Action Reset { get; }

    public ImplementacionClausura(Func<bool> moveNext, Func<T> getCurrent, Action reset)
    {
        MoveNext = moveNext;
        GetCurrent = getCurrent;
        Reset = reset;
    }
}


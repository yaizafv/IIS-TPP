using System.Diagnostics;

namespace Pila;

/// <summary>
/// Precondiciones. Públicas y privadas.
/// Postcondiciones. 
/// Inviariantes:
/// Completa el código con las precondiciones, postcondiciones e invariantes que identifiques.
/// </summary>  
// programacion por contrato. las precondiciones se sobreentienden
public class Pila
{
    private object[] items;

    public int Count { get; private set; }

    public int Capacidad
    {
        get { return items.Length; }
    }

    public Pila(int capacidad)
    {
        //Precondición.
        if (capacidad <= 0)
        {
            throw new ArgumentException("La capacidad debe ser superior a 0.", nameof(capacidad));      //excepcion de operacion invalida
        }
        //aún no existe estado, por lo que no comprobamos invariante.

        items = new object[capacidad];
        Count = 0;

        //Postcondición/es
        Debug.Assert(Capacidad == capacidad, "La capacidad debe coincidir con el argumento recibido.");
        Debug.Assert(Count == 0, "El tamaño actual debería comenzar en 0.");

        //Comprobación de invariante
        CheckInvariante();

    }

    
    public void Push(object item)
    {
        if (Count >= Capacidad)
        {
            throw new InvalidOperationException("La pila está llena.");
        }

        items[Count] = item;
        Count++;
    }

    public object Pop()
    {
        if (Count == 0)
        {
            throw new InvalidOperationException("La pila está vacía.");
        }

// Compilación condicional. El siguiente código solamente se compila en DEBUG.
#if DEBUG
        int tamPrevio = Count;
#endif
        Count--;
        return items[Count];
    }

    public bool Contains(object item)
    {
        int idx = 0;

        while (idx < Count)
        {
            if (Equals(items[idx], item))
            {
                return true;
            }

            idx++;
        }

        return false;
    }

    /// <summary>
    /// Solamente en Debug.
    /// </summary>
    [Conditional("DEBUG")]
    private void CheckInvariante()
    {
        Debug.Assert(items != null, "Items no puede ser null.");
        //Completa con más invariantes.
    }
}

using System.Collections;

namespace EjemploIEnumerable;

class Program
{
    static void Main(string[] args)
    {
        EjemploSimple();
        EjemploRango();
    }

    private static void EjemploSimple()
    {

        string texto = "TPP";

        /* Uso de IEnumerable de forma implícita */
        Console.WriteLine($"(EjemploSimple) Antes del 1er FOREACH");
        foreach (char c in texto) { Console.WriteLine($"Char: {c}"); }

        Console.WriteLine($"(EjemploSimple) Antes del 2o FOREACH");
        foreach (char c in texto) { Console.WriteLine($"Char: {c}"); }


        /* Uso de IEnumerable explícitamente */
        IEnumerable<char> enumerable = texto;
        IEnumerator<char> enumerator = enumerable.GetEnumerator();

        Console.WriteLine($"(EjemploSimple) Antes del 1er WHILE");
        while (enumerator.MoveNext())
        {
            Console.WriteLine($"Char: {enumerator.Current}");
        }

        Console.WriteLine($"(EjemploSimple) Antes del 2o WHILE");

        //Reinicio del iterador
        enumerator.Reset();

        while (enumerator.MoveNext())
        {
            Console.WriteLine($"Char: {enumerator.Current}");
        }
    }

    private static void EjemploRango()
    {
        /* 
            EJERCICIO
            Ejecutando el presente código se pueden observar dos problemas.
            El segundo será más sencillo de localizar una vez se arregle el obvio.
            Corrígelos.
        */
        var rango = new Rango();
        Console.WriteLine($"(EjemploRango) Antes del 1er FOREACH");
        foreach (int n in rango)
        {
            Console.WriteLine($"Número: {n}");
        }
        Console.WriteLine($"(EjemploRango) Antes del 2o FOREACH");

        foreach (int n in rango)
        {
            Console.WriteLine($"Número: {n}");
        }
    }
}

/// <summary>
/// Secuencia de elementos, proporciona un iterador
/// </summary>
class Rango : IEnumerable<int>
{
    // Devuelve un iterador de elementos de tipo int (se implementa IEnumerable<int>)
    public IEnumerator<int> GetEnumerator()
    {
        return new RangoEnumerator();
    }

    // Devuelve un iterador de elementos como object (retrocompatibilidad)
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}


/// <summary>
/// IEnumerator es un iterador con estado: elemento actual y posibilidad de: avanzar, reiniciar, liberar.
/// </summary>
class RangoEnumerator : IEnumerator<int>
{
    private int start = -1;
    private int end = 20;

    // Current elemento actual de tipo T (en este caso int).
    public int Current { get; private set; } = -1;

    object IEnumerator.Current { get { return Current; } }

    public void Dispose() { /* No hay nada que liberar */ }

    // Avanzamos por la secuencia e indicamos si hay aún quedan elementos
    public bool MoveNext()
    {
        if (Current < end)
        {
            Current++;
            return true;
        }
        return false;
    }

    // Se reinicia la iteración. ¿Siempre?
    public void Reset()
    {
        Current = start;
    }
}

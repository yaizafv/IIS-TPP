using System;
using System.Collections.Generic;
using System.Numerics; // Necesario para INumber<T>

public class MemoizedCalculator<T> where T : INumber<T>
{
    // Diccionarios para cachear cada operación por separado
    private readonly Dictionary<(T, T), T> _addCache = new();
    private readonly Dictionary<(T, T), T> _subCache = new();
    private readonly Dictionary<(T, T), T> _mulCache = new();
    private readonly Dictionary<(T, T), T> _divCache = new();

    public T Result { get; private set; } = T.Zero;

    public void Add(T a, T b) => ExecuteOperation(a, b, _addCache, (x, y) => x + y, "Suma");

    public void Subtract(T a, T b) => ExecuteOperation(a, b, _subCache, (x, y) => x - y, "Resta");

    public void Multiply(T a, T b) => ExecuteOperation(a, b, _mulCache, (x, y) => x * y, "Multiplicación");

    public void Divide(T a, T b)
    {
        if (T.IsZero(b)) throw new DivideByZeroException("No se puede dividir por cero.");
        ExecuteOperation(a, b, _divCache, (x, y) => x / y, "División");
    }

    // Método privado para centralizar la lógica de memoización
    private void ExecuteOperation(T a, T b, Dictionary<(T, T), T> cache, Func<T, T, T> operation, string opName)
    {
        var key = (a, b);

        if (cache.TryGetValue(key, out T cachedResult))
        {
            Console.WriteLine($"[Caché] {opName} de {a} y {b} recuperada.");
            Result = cachedResult;
        }
        else
        {
            Console.WriteLine($"[Cálculo] Realizando nueva {opName} de {a} y {b}.");
            Result = operation(a, b);
            cache[key] = Result;
        }
    }
}
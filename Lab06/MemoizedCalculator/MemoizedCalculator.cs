using System;
using System.Collections.Generic;
using System.Numerics;

public class MemoizedCalculator<T> where T : INumber<T>
{
    // Usamos una tupla (NombreOperacion, OperandoA, OperandoB) como clave única
    private Dictionary<(string, T, T), T> cache;

    public T Result { get; private set; } = T.Zero;
    public void Add(T a, T b) => ExecuteOperation("Add", a, b, (x, y) => x + y);
    public void Subtract(T a, T b) => ExecuteOperation("Sub", a, b, (x, y) => x - y);
    public void Multiply(T a, T b) => ExecuteOperation("Mul", a, b, (x, y) => x * y);
    public void Divide(T a, T b)
    {
        if (T.IsZero(b)) throw new DivideByZeroException();
        ExecuteOperation("Div", a, b, (x, y) => x / y);
    }

    public void Clear()
    {
        cache.Clear();
        Result = T.Zero;
        Console.WriteLine("Calculadora reiniciada (Caché vacía).");
    }

    private void ExecuteOperation(string opName, T a, T b, Func<T, T, T> operation)
    {
        var key = (opName, a, b);

        if (cache.TryGetValue(key, out T cachedResult))
        {
            Result = cachedResult;
        }
        else
        {
            Result = operation(a, b);
            cache[key] = Result;
        }
    }
}
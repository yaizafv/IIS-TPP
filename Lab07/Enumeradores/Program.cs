using System;
using System.Collections;
using System.Collections.Generic;

public static class ExtensionesFuncionales
{
    // --- 1. IMPLEMENTACIÓN CON ENUMERADORES 

    public static IEnumerable<TResult> MapManual<T, TResult>(this IEnumerable<T> source, Func<T, TResult> selector)
        => new MapEnumerable<T, TResult>(source, selector);

    public static IEnumerable<T> FilterManual<T>(this IEnumerable<T> source, Predicate<T> predicate)
        => new FilterEnumerable<T>(source, predicate);

    public static IEnumerable<TResult> ZipManual<T1, T2, TResult>(this IEnumerable<T1> first, IEnumerable<T2> second, Func<T1, T2, TResult> resultSelector)
        => new ZipEnumerable<T1, T2, TResult>(first, second, resultSelector);

    // Clases anidadas para la lógica de los Enumeradores
    private class MapEnumerable<T, TResult>(IEnumerable<T> source, Func<T, TResult> selector) : IEnumerable<TResult>
    {
        public IEnumerator<TResult> GetEnumerator() => new MapEnumerator(source.GetEnumerator(), selector);
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private class MapEnumerator(IEnumerator<T> src, Func<T, TResult> map) : IEnumerator<TResult>
        {
            public bool MoveNext() => src.MoveNext();
            public TResult Current => map(src.Current);
            object IEnumerator.Current => Current!;
            public void Reset() => src.Reset();
            public void Dispose() => src.Dispose();
        }
    }

    private class FilterEnumerable<T>(IEnumerable<T> source, Predicate<T> predicate) : IEnumerable<T>
    {
        public IEnumerator<T> GetEnumerator() => new FilterEnumerator(source.GetEnumerator(), predicate);
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private class FilterEnumerator(IEnumerator<T> src, Predicate<T> filter) : IEnumerator<T>
        {
            public bool MoveNext()
            {
                while (src.MoveNext())
                {
                    if (filter(src.Current)) return true;
                }
                return false;
            }
            public T Current => src.Current;
            object IEnumerator.Current => Current!;
            public void Reset() => src.Reset();
            public void Dispose() => src.Dispose();
        }
    }

    private class ZipEnumerable<T1, T2, TResult>(IEnumerable<T1> first, IEnumerable<T2> second, Func<T1, T2, TResult> zip) : IEnumerable<TResult>
    {
        public IEnumerator<TResult> GetEnumerator() => new ZipEnumerator(first.GetEnumerator(), second.GetEnumerator(), zip);
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private class ZipEnumerator(IEnumerator<T1> e1, IEnumerator<T2> e2, Func<T1, T2, TResult> zip) : IEnumerator<TResult>
        {
            public bool MoveNext() => e1.MoveNext() && e2.MoveNext();
            public TResult Current => zip(e1.Current, e2.Current);
            object IEnumerator.Current => Current!;
            public void Reset() { e1.Reset(); e2.Reset(); }
            public void Dispose() { e1.Dispose(); e2.Dispose(); }
        }
    }

    // --- 2. IMPLEMENTACIÓN CON GENERADORES (yield return)  ---

    public static IEnumerable<TResult> Map<T, TResult>(this IEnumerable<T> source, Func<T, TResult> map)
    {
        foreach (var item in source) yield return map(item);
    }

    public static IEnumerable<T> Filter<T>(this IEnumerable<T> source, Predicate<T> filter)
    {
        foreach (var item in source) if (filter(item)) yield return item;
    }

    public static IEnumerable<TResult> Zip<T1, T2, TResult>(this IEnumerable<T1> first, IEnumerable<T2> second, Func<T1, T2, TResult> resultSelector)
    {
        using var e1 = first.GetEnumerator();
        using var e2 = second.GetEnumerator();
        while (e1.MoveNext() && e2.MoveNext()) yield return resultSelector(e1.Current, e2.Current);
    }

    // Nota: Reduce no se puede implementar perezoso porque requiere procesar toda la colección.

    // --- 3. OPERADORES DE PARTICIÓN (Generadores)

    public static IEnumerable<T> Take<T>(this IEnumerable<T> source, int n)
    {
        int count = 0;
        foreach (var item in source)
        {
            if (count++ < n) yield return item;
            else yield break;
        }
    }

    public static IEnumerable<T> TakeWhile<T>(this IEnumerable<T> source, Func<T, bool> p)
    {
        foreach (var item in source)
        {
            if (p(item)) yield return item;
            else yield break;
        }
    }

    public static IEnumerable<T> Skip<T>(this IEnumerable<T> source, int n)
    {
        int count = 0;
        foreach (var item in source)
        {
            if (count++ >= n) yield return item;
        }
    }

    public static IEnumerable<T> SkipWhile<T>(this IEnumerable<T> source, Func<T, bool> p)
    { // 
        bool yielding = false;
        foreach (var item in source)
        {
            if (!yielding && !p(item)) yielding = true;
            if (yielding) yield return item;
        }
    }
}
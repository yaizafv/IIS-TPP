using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text;

namespace Tarea;

class Program
{
    static void Main(string[] args)
    {
        String text = ProcesadorTextos.LeerFicheroTexto(@"..\..\..\..\clarin.txt");
        string[] palabras = ProcesadorTextos.DividirEnPalabras(text);

        if (args.Length > 0 && args[0] == "local")
            ContarPalabrasLocal(palabras);
        else if (args.Length > 0 && args[0] == "plinq")
            ContarPalabrasPLinq(palabras);
        else if (args.Length > 0 && args[0] == "for")
            ContarPalabrasFor(palabras);
        else if (args.Length > 0 && args[0] == "foreach")
            ContarPalabrasForEach(palabras);
        else
            ContarPalabrasSecuencial(palabras);
    }

    public static void ContarPalabrasSecuencial(string[] palabras)
    {
        Stopwatch sw = Stopwatch.StartNew();
        Dictionary<string, int> palabrasContadas = palabras
        .GroupBy(word => word.ToLower())
        .ToDictionary(
            group => group.Key,
            group => group.Count()
        );
        sw.Stop();
        Console.WriteLine($"[Secuencial] Tiempo: {sw.ElapsedMilliseconds} ms.");
        foreach (var palabra in palabrasContadas)
        {
            Console.WriteLine($"{palabra.Key}: {palabra.Value} times");
        }
    }

    public static void ContarPalabrasPLinq(string[] words)
    {
        Stopwatch sw = Stopwatch.StartNew();
        Dictionary<string, int> palabrasContadas = words.AsParallel()
            .GroupBy(word => word.ToLower())
            .ToDictionary(
                group => group.Key,
                group => group.Count()
            );
        sw.Stop();
        Console.WriteLine($"[PLinq] Tiempo: {sw.ElapsedMilliseconds} ms.");
        foreach (var palabra in palabrasContadas)
        {
            Console.WriteLine($"{palabra.Key}: {palabra.Value} times");
        }
    }

    public static void ContarPalabrasForEach(string[] palabras)
    {
        Stopwatch sw = Stopwatch.StartNew();
        Dictionary<string, int> palabrasContadas = new Dictionary<string, int>();
        object obj = new object();
        Parallel.ForEach(palabras, palabra =>
        {
            string word = palabra.ToLower();
            lock (obj)
            {
                if (palabrasContadas.ContainsKey(word))
                    palabrasContadas[word]++;
                else
                    palabrasContadas[word] = 1;
            }
        });
        sw.Stop();
        Console.WriteLine($"[ForEach] Tiempo: {sw.ElapsedMilliseconds} ms.");
        foreach (var palabra in palabrasContadas)
        {
            Console.WriteLine($"{palabra.Key}: {palabra.Value} times");
        }
    }

    public static void ContarPalabrasFor(string[] palabras)
    {
        Stopwatch sw = Stopwatch.StartNew();
        Dictionary<string, int> palabrasContadas = new Dictionary<string, int>();
        object obj = new object();

        Parallel.For(0, palabras.Length, i =>
        {
            string word = palabras[i].ToLower();
            lock (obj)
            {
                if (palabrasContadas.ContainsKey(word))
                    palabrasContadas[word]++;
                else
                    palabrasContadas[word] = 1;
            }
        });
        sw.Stop();
        Console.WriteLine($"[For] Tiempo: {sw.ElapsedMilliseconds} ms.");
        foreach (var palabra in palabrasContadas)
        {
            Console.WriteLine($"{palabra.Key}: {palabra.Value} times");
        }
    }

    public static void ContarPalabrasLocal(string[] palabras)
    {
        Stopwatch sw = Stopwatch.StartNew();
        Dictionary<string, int> palabrasContadas = new Dictionary<string, int>();
        object obj = new object();

        Parallel.ForEach(palabras,
            // Inicializador de variable local (cada hilo crea su propio dic vacio)
            () => new Dictionary<string, int>(),

            // El hilo trabaja con su 'localDict' privado
            (palabra, state, localDict) =>
            {
                string word = palabra.ToLower();
                if (localDict.ContainsKey(word)) localDict[word]++;
                else localDict[word] = 1;
                return localDict; // Pasa el diccionario al siguiente paso
            },

            // Cuando termina, suma su resultado al global
            (localDict) =>
            {
                lock (obj)
                {
                    foreach (var palabra in localDict)
                    {
                        if (palabrasContadas.ContainsKey(palabra.Key)) palabrasContadas[palabra.Key] += palabra.Value;
                        else palabrasContadas[palabra.Key] = palabra.Value;
                    }
                }
            });

        sw.Stop();
        Console.WriteLine($"[Local] Tiempo: {sw.ElapsedMilliseconds} ms.");
        foreach (var palabra in palabrasContadas)
        {
            Console.WriteLine($"{palabra.Key}: {palabra.Value} times");
        }
    }
}

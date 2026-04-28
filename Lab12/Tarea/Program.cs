using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text;

namespace Tarea;

class Program
{
    static void Main(string[] args)
    {
        string texto = ProcesadorTextos.LeerFicheroTexto("../../../../clarin.txt");

        string[] lineas = texto.Split('\n', System.StringSplitOptions.RemoveEmptyEntries);

        Console.WriteLine($"Fichero leído: {lineas.Length} líneas.\n");
        Console.WriteLine("=== CONTEO DE APARICIONES DE PALABRAS ===\n");

        var resultadoSecuencial = ContarSecuencial(lineas);
        var resultadoFor = ContarParaleloFor(lineas);
        var resultadoForEach = ContarParaleloForEach(lineas);
        var resultadoForEachLocales = ContarParaleloForEachLocales(lineas);

        // Verificamos que todas las versiones producen el mismo resultado
        Console.WriteLine("\n=== VERIFICACIÓN DE CONSISTENCIA ===");
        Console.WriteLine($"Secuencial == For            : {DiccionariosIguales(resultadoSecuencial, resultadoFor)}");
        Console.WriteLine($"Secuencial == ForEach        : {DiccionariosIguales(resultadoSecuencial, resultadoForEach)}");
        Console.WriteLine($"Secuencial == ForEach Locales: {DiccionariosIguales(resultadoSecuencial, resultadoForEachLocales)}");

        // Top 10 palabras más frecuentes
        Console.WriteLine("\n=== TOP 10 PALABRAS MÁS FRECUENTES ===");
        foreach (var kv in resultadoSecuencial.OrderByDescending(kv => kv.Value).Take(10))
            Console.WriteLine($"  \"{kv.Key}\" -> {kv.Value} veces");
    }


    // -------------------------------------------------------------------------
    // 1. VERSIÓN SECUENCIAL
    //    foreach normal sobre las líneas.
    //    Usamos DividirEnPalabras de ProcesadorTextos en cada línea.
    //    Dictionary ordinario (no thread-safe), válido al ser un único hilo.
    // -------------------------------------------------------------------------
    static Dictionary<string, int> ContarSecuencial(string[] lineas)
    {
        Dictionary<string, int> conteo = new Dictionary<string, int>(System.StringComparer.OrdinalIgnoreCase);

        Stopwatch sw = Stopwatch.StartNew();

        foreach (string linea in lineas)
        {
            foreach (string palabra in ProcesadorTextos.DividirEnPalabras(linea))
            {
                if (conteo.TryGetValue(palabra, out int count))
                    conteo[palabra] = count + 1;
                else
                    conteo[palabra] = 1;
            }
        }

        sw.Stop();
        MostrarResumen("Secuencial (foreach)", conteo, sw.ElapsedMilliseconds);
        return conteo;
    }


    // -------------------------------------------------------------------------
    // 2. VERSIÓN PARALELA CON Parallel.For
    //    Paralelizamos por índice de línea.
    //    ConcurrentDictionary es thread-safe: AddOrUpdate es atómica.
    //    Problema: contención alta en palabras frecuentes (muchos hilos tocan las mismas claves).
    // -------------------------------------------------------------------------
    static Dictionary<string, int> ContarParaleloFor(string[] lineas)
    {
        ConcurrentDictionary<string, int> conteo =
            new ConcurrentDictionary<string, int>(System.StringComparer.OrdinalIgnoreCase);

        Stopwatch sw = Stopwatch.StartNew();

        Parallel.For(0, lineas.Length, i =>
        {
            foreach (string palabra in ProcesadorTextos.DividirEnPalabras(lineas[i]))
            {
                // AddOrUpdate: inserta 1 si no existe, o suma 1 si ya existe. Operación atómica.
                conteo.AddOrUpdate(palabra, 1, (_, valorActual) => valorActual + 1);
            }
        });

        sw.Stop();
        MostrarResumen("Parallel.For + ConcurrentDictionary", conteo, sw.ElapsedMilliseconds);
        return new Dictionary<string, int>(conteo, System.StringComparer.OrdinalIgnoreCase);
    }


    // -------------------------------------------------------------------------
    // 3. VERSIÓN PARALELA CON Parallel.ForEach
    //    Igual que la anterior pero más natural: itera directamente sobre las líneas
    //    sin necesitar el índice. Misma contención que la versión con For.
    // -------------------------------------------------------------------------
    static Dictionary<string, int> ContarParaleloForEach(string[] lineas)
    {
        ConcurrentDictionary<string, int> conteo =
            new ConcurrentDictionary<string, int>(System.StringComparer.OrdinalIgnoreCase);

        Stopwatch sw = Stopwatch.StartNew();

        Parallel.ForEach(lineas, linea =>
        {
            foreach (string palabra in ProcesadorTextos.DividirEnPalabras(linea))
            {
                conteo.AddOrUpdate(palabra, 1, (_, valorActual) => valorActual + 1);
            }
        });

        sw.Stop();
        MostrarResumen("Parallel.ForEach + ConcurrentDictionary", conteo, sw.ElapsedMilliseconds);
        return new Dictionary<string, int>(conteo, System.StringComparer.OrdinalIgnoreCase);
    }


    // -------------------------------------------------------------------------
    // 4. VERSIÓN PARALELA CON Parallel.ForEach Y DATOS LOCALES  ← MÁS ÓPTIMA
    //    Cada partición/hilo acumula su propio Dictionary local sin ninguna contención.
    //    El lock solo ocurre una vez por partición en la fase de fusión final,
    //    igual que en el ejemplo de ForLocales con primos de clase.
    // -------------------------------------------------------------------------
    static Dictionary<string, int> ContarParaleloForEachLocales(string[] lineas)
    {
        Dictionary<string, int> conteoGlobal =
            new Dictionary<string, int>(System.StringComparer.OrdinalIgnoreCase);
        object bloqueo = new object();

        Stopwatch sw = Stopwatch.StartNew();

        Parallel.ForEach<string, Dictionary<string, int>>(
            lineas,

            // Inicialización: cada partición arranca con su propio diccionario vacío
            () => new Dictionary<string, int>(System.StringComparer.OrdinalIgnoreCase),

            // Cuerpo: actualiza el diccionario LOCAL de la partición (sin lock, sin contención)
            (linea, loopState, conteoLocal) =>
            {
                foreach (string palabra in ProcesadorTextos.DividirEnPalabras(linea))
                {
                    if (conteoLocal.TryGetValue(palabra, out int count))
                        conteoLocal[palabra] = count + 1;
                    else
                        conteoLocal[palabra] = 1;
                }
                return conteoLocal;
            },

            // Agregación final: fusiona el diccionario local en el global (con lock, una sola vez por partición)
            conteoLocalFinal =>
            {
                lock (bloqueo)
                {
                    foreach (var kv in conteoLocalFinal)
                    {
                        if (conteoGlobal.TryGetValue(kv.Key, out int count))
                            conteoGlobal[kv.Key] = count + kv.Value;
                        else
                            conteoGlobal[kv.Key] = kv.Value;
                    }
                }
            }
        );

        sw.Stop();
        MostrarResumen("Parallel.ForEach con datos locales", conteoGlobal, sw.ElapsedMilliseconds);
        return conteoGlobal;
    }


    // -------------------------------------------------------------------------
    // UTILIDADES
    // -------------------------------------------------------------------------

    static void MostrarResumen(string nombre, IEnumerable<KeyValuePair<string, int>> conteo, long ms)
    {
        Console.WriteLine($"[{nombre}]");
        Console.WriteLine($"  Tiempo           : {ms} ms");
        Console.WriteLine($"  Palabras únicas  : {conteo.Count()}");
        Console.WriteLine($"  Total apariciones: {conteo.Sum(kv => kv.Value)}\n");
    }

    static bool DiccionariosIguales(Dictionary<string, int> a, Dictionary<string, int> b)
    {
        if (a.Count != b.Count) return false;
        foreach (var kv in a)
        {
            if (!b.TryGetValue(kv.Key, out int val) || val != kv.Value)
                return false;
        }
        return true;
    }
}

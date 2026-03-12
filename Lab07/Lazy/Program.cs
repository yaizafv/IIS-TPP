namespace Lazy;

class Program
{
    static void Main(string[] args)
    {
        var FICHERO_CSV = "../../../primes.csv";
        var INICIO = 200_000u;
        var FIN = 200_005u;
        DateTime start;

        // WARNING: Para que este ejemplo funcione es importante que el archivo
        // CSV_FILE NO exista.
        File.Delete(FICHERO_CSV); // NO BORRES ESTA LÍNEA!!!

        /***********************************************************************
         * Ejemplo estricto
         ***/
        start = DateTime.UtcNow;
        for (uint n = INICIO; n <= FIN; n++)
        {
            Console.WriteLine($"  ({DateTime.UtcNow - start}): Buscando y registrando el {n}º número primo.");
            // Aquí se calcula el primo antes de intentar registrarlo,
            // lo que puede ser costoso si el número es grande.
            //
            // Si el registro falla porque no se puede abrir o escribir en el archivo,
            // el tiempo invertido en calcular el primo se habrá desperdiciado.

            var exito = TryRegNesimoPrimoEager(FICHERO_CSV, CalcNesimoPrimo(n));
            if (!exito) { Console.WriteLine($"  ({DateTime.UtcNow - start}): Fallo al registrar el {n}º número primo."); }
            else { Console.WriteLine($"  ({DateTime.UtcNow - start}): Registrado con éxito el {n}º número primo."); }
        }
        Console.WriteLine($"Tiempo total de la ejecución estricta (eager): {DateTime.UtcNow - start}");

        // Ejemplo perezoso (lazy)

        start = DateTime.UtcNow;
        for (uint n = INICIO; n <= FIN; n++)
        {
            Console.WriteLine($"  ({DateTime.UtcNow - start}): Buscando y registrando el {n}º número primo.");

            var exito = TryRegNesimoPrimoLazy(FICHERO_CSV, () => CalcNesimoPrimo(n));
            if (!exito) { Console.WriteLine($"  ({DateTime.UtcNow - start}): Fallo al registrar el {n}º número primo."); }
            else { Console.WriteLine($"  ({DateTime.UtcNow - start}): Registrado con éxito el {n}º número primo."); }
        }
        Console.WriteLine($"Tiempo total de la ejecución perezosa (lazy): {DateTime.UtcNow - start}");


    }

    static uint CalcNesimoPrimo(uint n)
    {
        uint contador = 0;
        uint actual = 1;

        while (contador < n)
        {
            actual++;
            if (EsPrimo(actual)) contador++;
        }
        return actual;
    }

    static bool EsPrimo(uint x)
    {
        if (x < 2) return false;
        if (x == 2) return true;
        if (x % 2 == 0) return false;

        for (int d = 3; d * d <= x; d += 2)
            if (x % d == 0) return false;

        return true;
    }

    static bool TryRegNesimoPrimoEager(string ficheroCsv, uint primo)
    {
        try
        {
            using (var stream = new FileStream(ficheroCsv, FileMode.Open, FileAccess.Write))
            {
                stream.Seek(0, SeekOrigin.End);         //se pone en la última posición del fichero
                using (var writer = new StreamWriter(stream))
                {
                    writer.WriteLine("," + primo);
                }
            }
            return true;
        }
        catch
        {
            return false;
        }
    }

    static bool TryRegNesimoPrimoLazy(string ficheroCsv, Func<uint> thunkCalcPrimo)
    {
        using (var stream = new FileStream(ficheroCsv, FileMode.Open, FileAccess.Write))
        {
            stream.Seek(0, SeekOrigin.End);        
            using (var writer = new StreamWriter(stream))
            {
                writer.WriteLine("," + thunkCalcPrimo());
            }
        }
        return true;
    }
}

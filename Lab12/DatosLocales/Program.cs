using System.Diagnostics;

namespace DatosLocales;

class Program
{
    static void Main(string[] args)
    {
        int[] vector = GenerarVectorAleatorio(1, 1000000, 100000000);

        if (args.Length > 0 && args[0] == "p")
            ForLocales(vector);
        else if (args.Length > 0 && args[0] == "pp")
            ForParalelo(vector);
        else if (args.Length > 0 && args[0] == "ppp")
            ForEachParalelo(vector);
        else
            ForSecuencial(vector);

        // EJERCICIO: Implementa un ejemplo utilizando ForEach con resultados locales.
        // Por ejemplo, puedes basarte en alguno de los Master Workers anteriores.
        // o adaptando este mismo ejercicio.
    }

    static void ForSecuencial(int[] vector)
    {
        List<int> posicionesGlobal = new List<int>();
        Stopwatch sw = Stopwatch.StartNew();

        for (int i = 0; i < vector.Length; i++)
        {
            if (EsPrimo(vector[i]))
                posicionesGlobal.Add(i);
        }

        sw.Stop();
        Console.WriteLine($"[Secuencial] {sw.ElapsedMilliseconds} ms.");
    }

    static void ForParalelo(int[] vector)
    {
        List<int> posicionesGlobal = new List<int>();
        Stopwatch sw = Stopwatch.StartNew();
        object obj = new object();
        Parallel.For(0, vector.Length, i =>
        {
            if (EsPrimo(vector[i]))
            {
                lock (obj)
                {
                    posicionesGlobal.Add(i);
                }
            }
        });
        sw.Stop();
        Console.WriteLine($"[For Paralelo] {sw.ElapsedMilliseconds} ms.");
    }

    static void ForEachParalelo(int[] vector)
    {
        List<int> posicionesGlobal = new List<int>();
        Stopwatch sw = Stopwatch.StartNew();
        object obj = new object();
        Parallel.ForEach(vector, (element) =>
        {
            if (EsPrimo(element))
            {
                lock (obj)
                {
                    posicionesGlobal.Add(element);
                }
            }
        });
        sw.Stop();
        Console.WriteLine($"[ForEach Paralelo] {sw.ElapsedMilliseconds} ms.");
    }

    static void ForLocales(int[] vector)
    {
        //CON DATOS LOCALES SIEMPRE QUE HAYA RECURSOS COMPARTIDOS

        List<int> posicionesGlobal = new List<int>();
        object bloqueo = new object();

        Stopwatch sw = Stopwatch.StartNew();

        Parallel.For<List<int>>(
            0, //Inicio
            vector.Length, // Recuerda, no inclusivo

            () => new List<int>(), // Inicialización del resultado local de cada partición      //CADA PARTICION VA A TENER SU PROPIA LISTA

            (i, loopState, posicionesLocal) => // Para cada iteración.
            {
                if (EsPrimo(vector[i]))
                    posicionesLocal.Add(i);

                return posicionesLocal;
            },

            posicionesLocalFinal =>  // Agregación final del resultado local de cada partición
            {
                lock (bloqueo)
                {
                    posicionesGlobal.AddRange(posicionesLocalFinal);
                }
            }
        );

        sw.Stop();
        Console.WriteLine($"[TPL For con locales] {sw.ElapsedMilliseconds} ms.");
    }

    static bool EsPrimo(int n)
    {
        if (n < 2)
            return false;

        if (n == 2)
            return true;

        if (n % 2 == 0)
            return false;

        for (int i = 3; i * i <= n; i += 2)
        {
            if (n % i == 0)
                return false;
        }

        return true;
    }

    static int[] GenerarVectorAleatorio(int min, int max, int tam)
    {
        Random random = new Random();
        int[] vector = new int[tam];

        for (int i = 0; i < tam; i++)
            vector[i] = random.Next(min, max + 1);

        return vector;
    }
}

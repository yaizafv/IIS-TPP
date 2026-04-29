using System.Diagnostics;

namespace PLINQ;

class Program
{
    static void Main(string[] args)
    {
        short[] vector = GenerarVectorAleatorio(1, 1000, 20_000_000);

        switch (args[0])
        {
            case "p":
                ModuloPlinqSimple(vector);
                break;

            case "pp":
                ModuloPlinqSelectAggregate(vector);
                break;

            case "ppp":
                ModuloPlinqLocales(vector);
                break;

            default:
                ModuloLinqSecuencial(vector);
                break;
        }
    }

    static void ModuloLinqSecuencial(short[] vector)
    {
        Stopwatch sw = Stopwatch.StartNew();

        double modulo = Math.Sqrt(
            vector.Aggregate<short, long>(
                0L,
                (acc, item) => acc + (long)item * item
            )
        );

        sw.Stop();
        Console.WriteLine($"LINQ secuencial: {sw.ElapsedMilliseconds} ms. Módulo = {modulo}");
    }

    static void ModuloPlinqSimple(short[] vector)
    {
        Stopwatch sw = Stopwatch.StartNew();

        double modulo = Math.Sqrt(
            vector.AsParallel().Aggregate<short, long>(
                0L,
                (acc, item) => acc + (long)item * item
            )
        );

        sw.Stop();
        Console.WriteLine($"PLINQ con un Aggregate: {sw.ElapsedMilliseconds} ms. Módulo = {modulo}");
    }

    static void ModuloPlinqSelectAggregate(short[] vector)
    {
        Stopwatch sw = Stopwatch.StartNew();

        double modulo = Math.Sqrt(
            vector.AsParallel()
                    .Select(item => (long)item * item)      //tarda mucho mas
                    .Aggregate(
                        0L,
                        (acc, item) => acc + item
                    )
        );

        sw.Stop();
        Console.WriteLine($"PLINQ con Select + Aggregate: {sw.ElapsedMilliseconds} ms. Módulo = {modulo}");
    }

    static void ModuloPlinqLocales(short[] vector)
    {
        Stopwatch sw = Stopwatch.StartNew();
        //la mas rapida
        double modulo = Math.Sqrt(
            vector.AsParallel().Aggregate<short, long, long>(
                () => 0L,                                // acumulador local inicial
                (acc, item) => acc + (long)item * item, // actualiza el acumulador local
                (acc1, acc2) => acc1 + acc2,            // combina parciales
                finalResult => finalResult              // resultado final, lo devuelvo para almacenarlo
            )
        );
        sw.Stop();
        Console.WriteLine($"PLINQ con locales: {sw.ElapsedMilliseconds} ms. Módulo = {modulo}");
    }


    static short[] GenerarVectorAleatorio(short min, short max, int tam)
    {
        Random random = new Random();
        short[] vector = new short[tam];

        for (int i = 0; i < tam; i++)
            vector[i] = (short)random.Next(min, max + 1);

        return vector;
    }

}

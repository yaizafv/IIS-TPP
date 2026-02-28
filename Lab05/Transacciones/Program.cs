namespace Transacciones;


//estado poo, comportamiento pf



/// <summary>
/// Funciones de orden superior: Map, Filter, Reduce y Zip.
/// class vs record y uso de with
/// </summary>
public class Program
{

    //examen: implementa funcion de orden supeior que dado un ienu

    /// <summary>
    /// Map es una función de orden superior que transforma uno a uno los elementos de una secuencia
    /// </summary>
    /// <typeparam name="T1">Tipo de la secuencia de entrada</typeparam>
    /// <typeparam name="T2">Tipo de la secuencia de salida</typeparam>
    /// <param name="secuencia">Secuencia de entrada</param>
    /// <param name="funcion">La función que transforma un elemento de T1 en un elemento de T2</param>
    /// <returns></returns>
    public static IEnumerable<T2> Map<T1, T2>(IEnumerable<T1> secuencia, Func<T1, T2> funcion)
    {
        IList<T2> secuenciaResultante = new List<T2>();

        foreach (T1 elemento in secuencia)
        {
            T2 transformado = funcion(elemento);
            secuenciaResultante.Add(transformado);
        }
        return secuenciaResultante;
    }

    public static IEnumerable<T1> Filter<T1>(IEnumerable<T1> secuencia, Predicate<T1> condition)
    {
        IList<T1> secuenciaResultante = new List<T1>();
        foreach (T1 elemento in secuencia)
        {
            if (condition(elemento))
                secuenciaResultante.Add(elemento);
        }
        return secuencia;
    }

    public static T2 Reduccion<T1,T2>(IEnumerable<T1> secuencia, T2 seed, Func<T1, T2, T2> funcion) 
    {
        T2 r = seed;
        foreach (T1 elemento in secuencia)
        {
            r = funcion(elemento, r);         
        }      
        return r;    
    }

    public static void Main()
    {

        EjemplosImperativos();
        // EjercicioTransacciones();
        // EjercicioZip();
    }

    public static double EntreDos(int a)
    {
        return a / 2.0;
    }

    public static bool EsPar(double a)
    {
        return a % 2 == 0;
    }

    public static void EjemplosImperativos()
    {
        int[] valores = { 1, 2, 3, 4, 5, 6 };

        // Ejemplo 1.

        IList<double> resultante = new List<double>();
        // Iteración del origen
        foreach (var v in valores)
        {
            // Operación de transformación de cada elemento
            double mitad = v / 2.0;

            // Almacenamiento o emisión del elemento resultante
            resultante.Add(mitad);
        }

        Console.WriteLine("La mitad de cada elemento:");
        foreach (var v in resultante)
            Console.WriteLine(v);

        // Haciendolo funcional
        IEnumerable<double> resultado = Map<int, double>(valores, n => n / 2.0);    //asi te ahorras crear el método EntreDos. No afecta al examen
        IEnumerable<double> resultado2 = Map<int, double>(valores, n => n * 2.0);

        // Ejemplo 2.

        IList<double> resultante2 = new List<double>();
        foreach (var v in valores)
        {
            if (v % 2 == 0)
                resultante2.Add(v);
        }
        Console.WriteLine("Los pares:");
        foreach (var v in resultante2)
            Console.WriteLine(v);

        IEnumerable<double> resultado3 = Filter<double>(resultante, n => n % 2 == 0);

        // Ejemplo 3.

        int suma = 0;
        foreach (var v in valores)
            suma += v;
        Console.WriteLine($"La suma: {suma}");

        //Reduccion(valores, (actual, acumulado) => actual + acumulado);
    }

    public static int Suma(int a, int b)
    {
        return a + b;
    }


    public static void EjercicioTransacciones()
    {
        var historicoVentas = new List<Venta>
            {
                new Venta { Region = "Europa",      Estado = Estado.Cancelada, Cantidad = 100m },
                new Venta { Region = "Asia", Estado = Estado.Confirmada,  Cantidad = 500m },
                new Venta { Region = "Europa",      Estado = Estado.Cancelada, Cantidad = 200m },
                new Venta { Region = "Asia", Estado = Estado.Cancelada, Cantidad = 300m },
                new Venta { Region = "Europa",      Estado = Estado.Cancelada, Cantidad = 50m },
                new Venta { Region = "Asia", Estado = Estado.Cancelada, Cantidad = 150m },
                new Venta { Region = "Europa",      Estado = Estado.Confirmada,  Cantidad = 120m },
                new Venta { Region = "Asia", Estado = Estado.Cancelada, Cantidad = 800m },
            };

        // Cálculo del beneficio neto en Europa

        // EJERCICIO: Parte a transformar 1.
        decimal totalBeneficioEuropa = 0;
        foreach (var v in historicoVentas)
        {
            if (v.Region.ToLower() == "europa")                         //Filter
            {
                if (v.Estado == Estado.Confirmada)                      //Filter
                {
                    decimal beneficioNeto = v.Cantidad * 0.80m;         //Map
                    totalBeneficioEuropa += beneficioNeto;
                }
            }
        }

        IEnumerable<Venta> filtradas = Filter<Venta>(historicoVentas, venta => venta.Region.ToLower() == "europa");
        IEnumerable<Venta> filtradasConf = Filter(filtradas, venta => venta.Estado == Estado.Confirmada);
        IEnumerable<decimal> netos = Map(filtradasConf, v => v.Cantidad * 0.80m);
        decimal total1 = Reduccion(netos, 0.0m, (actual, acumulado) => actual + acumulado);

        Console.WriteLine($"Beneficio neto en Europa: {totalBeneficioEuropa}");


        // Cálculo del beneficio medio.
        decimal total = 0;
        uint recuento = 0;
        foreach (var s in historicoVentas)
        {
            if (s.Estado == Estado.Cancelada)
            {
                continue;
            }
            decimal margen = 0;
            switch (s.Region.ToLower())
            {
                case "europa":
                    margen = 0.80m;
                    break;
                case "asia":
                    margen = 0.60m;
                    break;
                default:
                    throw new Exception("Region desconocida");
            }
            total += s.Cantidad * margen;
            recuento++;
        }
        decimal mediaBeneficio = total / recuento;
        Console.WriteLine($"Beneficio medio: {mediaBeneficio}");
    }


    /// <summary>
    /// Implementa una función de orden superior Zip que reciba dos secuencias IEnumerable<T> y una función Func<T1, T2, TResult>. 
    /// La función debe recorrer ambas secuencias en paralelo y devolver una nueva secuencia con el resultado de aplicar
    /// la función a cada par de elementos (uno de cada secuencia -> Tupla). 
    /// La iteración termina cuando se agote cualquiera de las dos secuencias.
    /// </summary>
    static void EjercicioZip()
    {
        //Imprímase por pantalla las tuplas resultantes de aplicar Zip a estas dos secuencias.
        var regiones = new List<string> { "Europa", "África", "Asia" };
        var margenes = new List<decimal> { 0.80m, 0.60m, 0.70m };

    }
}
public enum Estado
{
    Cancelada,
    Confirmada
}

public class Venta
{
    public required string Region { get; set; }
    public Estado Estado { get; set; }
    public decimal Cantidad { get; set; }
}

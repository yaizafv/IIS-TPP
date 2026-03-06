namespace lab06;

static class Program
{
    static void Main()
    {
        var texto = "In a village of La Mancha, the name of which I have no desire to call to mind, there lived not long since one of those gentlemen that keep a lance in the lance-rack, an old buckler, a lean hack, and a greyhound for coursing.";
        
        // Encadenar llamadas mediante el uso de métodos extensores, típico en C#.  
        // Recuento por palabras con longitud menor que 3.
        IDictionary<string,int> resultadoMetodos = texto.Split(' ')
            .Map(palabra => palabra.Trim(['.', ',', ';', ':']))
            .Filter(palabra => palabra.Length < 3) 
            .Map(palabra => palabra.ToLower())
            .Reduce(new Dictionary<string, int>(), (dicc, palabra) =>
            {
                if (dicc.ContainsKey(palabra))
                    dicc[palabra]++;
                else dicc[palabra] = 1;
                return dicc;
            });

        foreach (var kvp in resultadoMetodos)
            Console.WriteLine($"(Extensores) {kvp.Key}: {kvp.Value}");


        /*******************************************************
        * Una aproximación más funcional se basa en aprovechar:
        * - La currificación: Map, Filter y Reduce devuelven funciones unarias (clausuras) al capturar su configuración.
        * - La aplicación parcial: al pasar esa configuración, obtenemos una función procesar ya preparada.
        * - La composición de funciones: construimos el pipeline sin encadenar métodos.
        * Finalmente, invocamos a procesar.
        *
        * EJERCICIO: Implementa versiones del Map, Filter y Reduce currificadas que permitan ejecutar el siguiente código:
        */


        Func<IEnumerable<string>, Dictionary<string,int>> procesar = 
            Combine(
                Map((string palabra) => palabra.Trim(['.', ',', ';', ':'])),
                Filter((string palabra) => palabra.Length < 3),
                Map((string word) => word.ToLower()),
                Reduce<string, Dictionary<string, int>>(new Dictionary<string, int>(), (dicc, palabra) =>
                {
                    if (dicc.ContainsKey(palabra))
                        dicc[palabra]++;
                    else dicc[palabra] = 1;
                    return dicc;
                })
        );

        var resultadoCombine = procesar(texto.Split(' '));
        foreach (var kvp in resultadoCombine)
            Console.WriteLine($"(Combine) {kvp.Key}: {kvp.Value}");
        
    }


    /**************************************************************
     *   IMPLEMENTACIÓN DE MÉTODOS EXTENSORES: Map, Filter, Reduce.
     */
    public static IEnumerable<T2> Map<T1, T2>(this IEnumerable<T1> xs, Func<T1, T2> f)
    {
        var ys = new List<T2>();
        foreach (var x in xs)
            ys.Add(f(x));
        return ys;
    }

    public static IEnumerable<T> Filter<T>(this IEnumerable<T> xs, Predicate<T> p)
    {
        var ys = new List<T>();
        foreach (var x in xs)
            if (p(x))
                ys.Add(x);
        return ys;
    }

    public static T2 Reduce<T1, T2>(this IEnumerable<T1> xs, T2 seed, Func<T2, T1, T2> f)
    {
        var acc = seed;
        foreach (var x in xs)
            acc = f(acc, x);
        return acc;
    }


    /**************************************************************
     *  IMPLEMENTACIÓN DE Combine: 2, 3 y 4 parámetros.
     *  Fíjate que es posible gracias a cómo encajan los tipos.
     */
    public static Func<T1, T3> Combine<T1, T2, T3>(Func<T1, T2> f1, Func<T2, T3> f2)
    {
        return x => f2(f1(x));
    }

    public static Func<T1, T4> Combine<T1, T2, T3, T4>(Func<T1, T2> f1, Func<T2, T3> f2, Func<T3, T4> f3)
    {
        return x => f3(f2(f1(x)));
    }

    public static Func<T1, T5> Combine<T1, T2, T3, T4, T5>(Func<T1, T2> f1, Func<T2, T3> f2, Func<T3, T4> f3, Func<T4, T5> f4)
    {
        return x => f4(f3(f2(f1(x))));
    }

}
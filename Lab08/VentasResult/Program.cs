namespace tpp.lab08;

public interface Identificable
{
    string Id { get; }
}

public record Vendedor(string Id, string Nombre, string Email) : Identificable;
public record Producto(string Id, string Descripcion, uint Stock) : Identificable;
public record Venta(string VendedorId, string ProductoId, uint Cantidad);

// Convertir el proyecto de VentasExcepciones
// para seguir un enfoque basado en Result
// Orden recomendado:
// ParseUint: Aquí no queda otra que capturar las excepciones para que no se propaguen más.
// Cargar vendedor, producto y la configuración de la venta.
// CargarLista y Cargar Diccionario: Acumula éxitos y errores.

static class Program
{
    static void Main()
    {
        //TODO: llamar a CargarDiccionario con "../../../../data/sellers.bad.csv"
        //TODO: llamar a CargarDiccionario con "../../../../data/products.bad.csv"
        //TODO: llamar a CargarLista con "../../../../data/sales.bad.csv"

        // TODO: Cuando termines de implementar, acuérdate de probar también con los CSV buenos:
        // - ../../../../data/sellers.csv
        // - ../../../../data/products.csv
        // - ../../../../data/sales.csv


        // var vendedores = CargarDiccionario
        // var productos = CargarDiccionario
        // var ventas = CargarLista
        // Los diccionaros O diccionarios vacíos


        Console.WriteLine("Vendedores:");
        // TODO: Uso de Match. Imprimir los vendedores, manejando el caso de error.
        Console.WriteLine("\nProductos:");
        // TODO: Uso de Match. Imprimir los productos, manejando el caso de error.
        Console.WriteLine("\nVentas:");
        // TODO:Uso de Match. Imprimir las ventas, manejando el caso de error.
    }


    private static Result<Vendedor, string> CargarVendedor(int num, List<string> campos)
    {
        if (campos.Count != 3)
            return new Failure<Vendedor, string>($"Línea {num} inválida");
        Vendedor vendedor = new Vendedor(Id: campos[0], Nombre: campos[1], Email: campos[2]);
        return new Success<Vendedor, string>(vendedor);
    }

    private static Result<Producto, string> CargarProducto(int num, List<string> campos)
    {
        // TODO: Implementar igual que en VentasExcepciones, pero devolviendo Result.
        // Aquí, además de tener en cuenta que tengas 3 campos...
        // Posteriormente, deberás  manejar el éxito o fallo de parsear el campo a unit.
        if (campos.Count != 3)
        {
            return new Failure<Producto, string>($"Línea {num} inválida");
        }
        Result<uint, string> result = ParseUint(campos[2]);
        Result<Producto, string> resultado = result.Match<Result<Producto, string>>(
            valorExito =>
        {
            Producto producto = new Producto(Id: campos[0], Descripcion: campos[1], Stock: valorExito);
            return new Success<Producto, string>(producto);
        },
        valorError =>
        {
            return new Failure<Producto, string>($"{valorError} inválido");
        }
        );
        return resultado;
    }

    private static Func<int, List<string>, Result<Venta, string>> ConfigCargarVenta(Dictionary<string, Vendedor> vendedores, Dictionary<string, Producto> productos)
    {
        // TODO: Implementar igual que en VentasExcepciones, pero devolviendo Result.
        return (num, campos) =>
        {
            if (campos.Count != 3)
                return new Failure<Venta, string>($"Línea {num} inválida en sales.csv: se esperaban 3 campos, se encontraron {campos.Count}");
            var vendedorId = campos[0];
            var productoId = campos[1];
            if (!vendedores.ContainsKey(vendedorId))
                return new Failure<Venta, string>($"Vendedor desconocido en línea {num} de sales.csv: '{vendedorId}'");
            if (!productos.ContainsKey(productoId))
                return new Failure<Venta, string>($"Producto desconocido en línea {num} de sales.csv: '{productoId}'");

            Result<uint, string> result = ParseUint(campos[2]);
            Result<Venta, string> resultado = result.Match<Result<Venta, string>>(
                valorExito =>
            {
                if (valorExito == 0)
                {
                    return new Failure<Venta, string>($"Cantidad inválida en la línea {num} de sales.csv: no puede ser cero");
                }
                Venta venta = new Venta(VendedorId: vendedorId, ProductoId: productoId, Cantidad: uint.Parse(campos[2]));
                return new Success<Venta, string>(venta);
            },
                valorError =>
                {
                    return new Failure<Venta, string>($"Cantidad inválida en la línea {num} de sales.csv");
                });
        };
    }

    private static Result<uint, string> ParseUint(string entrada)
    {
        // En este caso capturamos la excepción.
        try
        {
            //Devolvemos un Success con el Parse.
            uint valor = uint.Parse(entrada);
            return new Success<uint, string>(valor);
        }
        catch (FormatException)
        {
            // Devolvemos un Failure con el texto.
            return new Failure<uint, string>($"'{entrada}' no es un entero.");
        }
    }

    private static Result<Dictionary<string, T>, string[]> CargarDiccionario<T>(string ruta, Func<int, List<string>, Result<T, string>> f) where T : Identificable
    {
        // TODO: Implementar acumulando éxitos y errores.
        // Sugerencia: Reduce con la semilla de tipo (Result<Dictionary<string, T>, string[]>).
        throw new NotImplementedException();

        // if (errores.Count > 0)
        //     return new Failure<List<T>, string[]>(errores.ToArray());
        // return new Success<List<T>, string[]>(lista);        
    }

    private static Result<List<T>, string[]> CargarLista<T>(string ruta, Func<int, List<string>, Result<T, string>> f)
    {
        // TODO: Implementar acumulando éxitos y errores.
        // Sugerencia: Reduce con la semilla de tipo (Result<List<T>, string[]>).
        throw new NotImplementedException();

        // if (errores.Count > 0)
        //     return new Failure<List<T>, string[]>(errores.ToArray());
        // return new Success<List<T>, string[]>(lista);

    }

    private static IEnumerable<Result<T, string>> CargarFilas<T>(string ruta, Func<int, List<string>, Result<T, string>> f)
    {
        // Fíjate que esto no cambia respecto a la versión con excepciones, porque el manejo de errores lo delegamos a las funciones f.
        // Lo unco que cambia es que ahora f devuelve un Result, y por tanto el tipo de retorno de LoadRows es IEnumerable<Result<T, string>> en lugar de IEnumerable<T>.
        return File.ReadLines(ruta)
            .Zip(Enumerable.Range(1, int.MaxValue)) // Obtenemos la línea y el número de línea.
            .Map(t => (num: t.Item2, linea: t.Item1))
            .Filter(t => !string.IsNullOrWhiteSpace(t.linea)) // Filtramos líneas vacías.
            .Filter(t => !t.linea.Trim().StartsWith('#')) // Filtramos líneas que empiecen por '#'
            .Map(t => (num: t.num, campos: new List<string>(t.linea.Split(';').Map(s => s.Trim()))))
            .Map(t => f(t.num, t.campos)); // f convierte cada fila preprocesada en un Result.
    }

    // Funciones de orden superior
    public static IEnumerable<T2> Map<T1, T2>(this IEnumerable<T1> xs, Func<T1, T2> f)
    {
        foreach (var x in xs)
            yield return f(x);
    }

    public static IEnumerable<T> Filter<T>(this IEnumerable<T> xs, Predicate<T> p)
    {
        foreach (var x in xs)
            if (p(x))
                yield return x;
    }

    public static T2 Reduce<T1, T2>(this IEnumerable<T1> xs, T2 semilla, Func<T2, T1, T2> f)
    {
        var acc = semilla;
        foreach (var x in xs)
            acc = f(acc, x);
        return acc;
    }

    public static IEnumerable<(T1, T2)> Zip<T1, T2>(this IEnumerable<T1> xs, IEnumerable<T2> ys)
    {
        using (var enumX = xs.GetEnumerator())
        using (var enumY = ys.GetEnumerator())
        {
            while (enumX.MoveNext() && enumY.MoveNext())
                yield return (enumX.Current, enumY.Current);
        }
    }
}



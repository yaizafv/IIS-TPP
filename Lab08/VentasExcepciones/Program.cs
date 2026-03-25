namespace VentasExcepciones;

public interface Identificable
{
    string Id { get; }
}

public record Vendedor(string Id, string Nombre, string Email) : Identificable;
public record Producto(string Id, string Descripcion, uint Stock) : Identificable;
public record Venta(string VendedorId, string ProductoId, uint Cantidad);


// Analiza y comprende en el siguiente orden:
// Main
// CargarFilas
// CargarLista y CargarDiccionario ¿Qué representan?
// CargarVendedor, CargarProducto -> Validaciones
// ConfigCargarVenta
// Compila y ejecuta. 
// ¿Qué ocurre con las excepciones?

static class Program
{
    static void Main()
    {
        // Lectura de ficheros CSV, aplicación de transformaciones y validaciones.
        // Usamos un pipeline funcional con gestión de errores mediante excepciones.

        Dictionary<string, Vendedor> vendedores = new Dictionary<string, Vendedor>();
        Dictionary<string, Producto> productos = new Dictionary<string, Producto>();
        List<Venta> ventas = new List<Venta>();

        // Prueba también con los CSV buenos:
        // - ../../../../data/sellers.csv
        // - ../../../../data/products.csv
        // - ../../../../data/sales.csv


        try
        {
            vendedores = CargarDiccionario("../../../../data/sellers.bad.csv", CargarVendedor);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error procesando los vendedores: {ex.Message}");
        }

        try
        {
            productos = CargarDiccionario("../../../../data/products.bad.csv", CargarProducto);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error procesando los productos: {ex.Message}");
        }

        try
        {
            ventas = CargarLista("../../../../data/sales.bad.csv", ConfigCargarVenta(vendedores, productos));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error procesando las ventas: {ex.Message}");
        }



        Console.WriteLine("Vendedores:");
        foreach (var vendedor in vendedores.Values)
            Console.WriteLine($"- {vendedor.Id}: {vendedor.Nombre} ({vendedor.Email})");
        Console.WriteLine("\nProductos:");
        foreach (var producto in productos.Values)
            Console.WriteLine($"- {producto.Id}: {producto.Descripcion} (Stock: {producto.Stock})");
        Console.WriteLine("\nVentas:");
        foreach (var venta in ventas)
            Console.WriteLine($"- Vendedor: {venta.VendedorId}, Producto: {venta.ProductoId}, Cantidad: {venta.Cantidad}");
    }

    private static Vendedor CargarVendedor(int num, List<string> campos)
    {
        if (campos.Count != 3)
            throw new FormatException($"Línea {num} inválida en sellers.csv: se esperaban 3 campos, se encontraron {campos.Count}");
        return new Vendedor(Id: campos[0], Nombre: campos[1], Email: campos[2]);
    }

    private static Producto CargarProducto(int num, List<string> campos)
    {
        if (campos.Count != 3)
            throw new FormatException($"Línea {num} inválida en products.csv: se esperaban 3 campos, se encontraron {campos.Count}");
        try
        {
            return new Producto(Id: campos[0], Descripcion: campos[1], Stock: uint.Parse(campos[2]));
        }
        catch (FormatException)
        {
            throw new ArgumentException($"Línea {num} en products.csv contiene un valor de stock erróneo: '{campos[2]}'");
        }
    }

    private static Func<int, List<string>, Venta> ConfigCargarVenta(Dictionary<string, Vendedor> vendedores, Dictionary<string, Producto> productos)
    {
        // ¿Qué idea se está aplicando aquí?        aplicación parcial
        return (num, campos) =>
        {
            if (campos.Count != 3)
                throw new FormatException($"Línea {num} inválida en sales.csv: se esperaban 3 campos, se encontraron {campos.Count}");
            var vendedorId = campos[0];
            var productoId = campos[1];
            if (!vendedores.ContainsKey(vendedorId))        //vendedorId es una clausura
                throw new ArgumentException($"Vendedor desconocido en línea {num} de sales.csv: '{vendedorId}'");
            if (!productos.ContainsKey(productoId))         //productoId es una clausura
                throw new ArgumentException($"Producto desconocido en línea {num} de sales.csv: '{productoId}'");
            try
            {
                var venta = new Venta(VendedorId: vendedorId, ProductoId: productoId, Cantidad: uint.Parse(campos[2]));
                if (venta.Cantidad == 0)
                    throw new ArgumentException($"Cantidad inválida en la línea {num} de sales.csv: no puede ser cero");
                return venta;
            }
            catch (FormatException)
            {
                throw new ArgumentException($"Cantidad inválida en la línea {num} de sales.csv: '{campos[2]}' no es un entero válido.");
            }
        };
    }

    private static Dictionary<string, T> CargarDiccionario<T>(string ruta, Func<int, List<string>, T> f) where T : Identificable
    {
        // Fíjate. Aquí se produce la reducción ¿qué implicaciones tiene?
        return CargarFilas(ruta, f)
            .Reduce(new Dictionary<string, T>(), (dicc, item) =>
            {
                if (dicc.ContainsKey(item.Id))
                    throw new ArgumentException($"ID duplicada '{item.Id}' en fichero '{ruta}'");
                dicc[item.Id] = item;       //genericidad acotada
                return dicc;
            });
    }

    private static List<T> CargarLista<T>(string ruta, Func<int, List<string>, T> f)
    {
        // Fíjate. Aquí se produce la reducción ¿qué implicaciones tiene?
        return CargarFilas(ruta, f)
            .Reduce(new List<T>(), (list, item) =>
            {
                list.Add(item);
                return list;
            });
    }

    private static IEnumerable<T> CargarFilas<T>(string ruta, Func<int, List<string>, T> f)
    {
        // Fíjate en que este proceso es perezoso.
        // Las funciones de orden superior están implementadas más abajo.

        return File.ReadLines(ruta)
            .Zip(Enumerable.Range(1, int.MaxValue)) // Obtenemos la línea y el número de línea.
            .Map(t => (num: t.Item2, linea: t.Item1))
            .Filter(t => !string.IsNullOrWhiteSpace(t.linea)) // Filtramos líneas vacías.
            .Filter(t => !t.linea.Trim().StartsWith('#')) // Filtramos líneas que empiecen por '#'
            .Map(t => (num: t.num, campos: new List<string>(t.linea.Split(';').Map(s => s.Trim()))))
            .Map(t => f(t.num, t.campos)); // f convierte cada fila preprocesada en un objeto concreto.
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

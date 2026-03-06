namespace Records;

class Program
{
    static void Main()
    {
        Venta v1 = new Venta { Region = "Europa", Cantidad = 10.20m, Estado = Estado.Confirmada };
        Console.WriteLine($"v1: {v1}");

        // Creamos una copia con los mismos valores excepto Cantidad
        Venta v2 = v1 with { Cantidad = 102.20m };
        Console.WriteLine($"v2: {v2}");

        // La igualdad entre records se comprueba por valores de sus miembros
        Venta v3 = new Venta { Region = "Europa", Cantidad = 10.20m, Estado = Estado.Confirmada };
        Console.WriteLine($"¿Son iguales v1 y v3? {v1 == v3}");
    }


    // Los records se utilizan para modelar datos, primando el contenido frente a la identidad.
    // Suelen usarse en un estilo inmutable (por ejemplo con propiedades init).
    public record Venta
    {
        // init solo se puede establecer durante la inicialización (o en un with, que crea una copia nueva).
        public required string Region { get; init; }
        public required decimal Cantidad { get; init; }
        public required Estado Estado { get; init; }
    }

    public enum Estado { Confirmada, Cancelada }
}

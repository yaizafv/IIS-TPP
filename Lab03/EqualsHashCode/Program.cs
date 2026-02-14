namespace EqualsHashCode;

/// <summary>
/// Equals ¿qué comprueba?
/// HashCode ¿para qué sirve? Dos opciones:
///     (valor1, valor2, valor3).GetHashCode();
///     HashCode.Combine(valor1, valor2, valor3)
/// </summary>
class Program
{
    static void Main(string[] args)
    {
        Song cancion1 = new Song { Title ="Pearl Jam - Blood"};
        Song cancion2 = cancion1;
        Song cancion3 = new Song { Title ="Pearl Jam - Blood"};
        Console.WriteLine($"¿Son iguales las canciones 1 y 2? {Equals(cancion1, cancion2)}");
        Console.WriteLine($"¿Son iguales las canciones 1 y 3? {Equals(cancion1, cancion3)}");
    }

}

/// <summary>
/// Añade una propiedad Artist, modifica el código del Main. 
/// Modifica la clase para que tenga los métodos Equals y GetHashCode
/// ¿Es aconsejable que sean de sólo lectura?
/// </summary>
class Song()
{
    public required string Title {get; set;}
}

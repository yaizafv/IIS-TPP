namespace anulables;

/// <summary>
/// Tipos anulabes
/// </summary>
class Program
{
    static Random r = new Random();
    static void Main(string[] args)
    {

        int? nota = null;   // puede no tener valor
        Console.WriteLine(nota); // imprime vacío
        nota = 7;
        if (nota != null)
            Console.WriteLine(nota); // 7

        //operador de coalescencia nula o fusión de nulos
        int notaFinal = nota ?? 0;   // si es null, usa 0
        Console.WriteLine(notaFinal);      

        //¿Qué devuelve LeerNombre?
        string? nombre = LeerNombre();

        //Fíjate en el ?. Se denomina operador de acceso condicional. ¿Cómo funciona?            
        int longitud = nombre?.Length ?? 0;     //nombre? hace que el .lenght no se ejecute

        Console.WriteLine("$Nombre: {nombre}");
        Console.WriteLine($"Longitud del nombre: {longitud}");

        //¿ Por qué es mejor utilizar Equals(nombre,"Pepe") ?       porque si ponermos nombre?.Equals("Pepe") ?? false devolveria siempre false
        if (nombre.Equals("Pepe")) 
            Console.WriteLine("¡Bienvenido Pepe!");

        /*
            EJERCICIO: Haz las correcciones necesarias al código
        */
        
    }
    
    /// <summary>
    /// Fíjate en el warning
    /// </summary>
    /// <returns></returns>
    static string? LeerNombre()
    {
        if (r.Next(0,100) >  50)
            return null; //¿Qué ocurre?

        return "Pepe";
    }
}

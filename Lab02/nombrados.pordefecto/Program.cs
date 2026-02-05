namespace nombrados.pordefecto;


/// <summary>
/// Parámetros opcionales (con valores por defecto) y parámetros o argumentos nombrados.
/// Tipado implícito con var
/// </summary>
class Program
{

    /// <summary>
    /// Devuelve un saludo adaptado al tono e idioma
    /// </summary>
    /// <param name="nombre"></param>
    /// <param name="idioma">opcional</param>
    /// <param name="formal">opcional</param>
    /// <returns></returns>
    static string Saludar(string nombre, string idioma = "es", bool formal = false)
    {
        if (idioma == "en")
            return formal ? $"Good morning, {nombre}." : $"Hi, {nombre}!";

        return formal ? $"Buenos días, {nombre}." : $"Hola, {nombre}!";
    }

    static void Main(string[] args)
    {

        //¿Qué es var?
        var saludo = Saludar("Pepe", "es", false); 
        Console.WriteLine(saludo);

        var saludo2 = Saludar("Pepe"); 
        Console.WriteLine(saludo2);
        
        //nombrados
        var saludo3 = Saludar(nombre: "Pepe", idioma: "en");
        Console.WriteLine(saludo3);

        //Y si quiero saludar solamente utilizando "nombre" y "formal"?
        //var saludo4 = Saludar(,,,);
        //Console.WriteLine(saludo4);




        var personas = new[]
        {
            new Persona { Nombre = "Alice Keys"},
            new Persona { Nombre = "Bob"},
            new Persona { Nombre = "Charlie Brown"},
            new Persona { Nombre = "Alice"},
        };

        /*
            EJERCICIO A : Implementa el código del método estático FindFirstByNombre de más abajo
            Prueba en el main a buscar:
                La primera persona cuyo nombre sea exactamente "Alice".
                La primera persona cuyo nombre sea exactamente "alice", sin distinguir mayúsculas y minúsculas.
                La primera persona cuyo nombre contenga "Alice", distinguiendo mayúsculas y minúsculas.
                La primera persona cuyo nombre contenga "alice" como subcadena, sin distinguir mayúsculas y minúsculas

            EJERCICIO B: Añade un new Persona { },  (es decir, sin nombre) al array de personas.
                Adapta todo el código que consideres necesario para que trabaje con tipos anulables.
        */
    }


    /// <summary>
    /// Busca la primera persona del array cuyo nombre coincida con el nombre indicado.
    /// </summary>
    /// <param name="personas"></param>
    /// <param name="nombre"></param>
    /// <param name="coincidenciaParcial">Establécelo a true para permitir coincidencias parciales. Por defecto: false</param>
    /// <param name="sensibleMayus">Establécelo a true para que la búsqueda distinga entre mayúsculas y minúsculas. Por defecto: true</param>
    static Persona FindFirstByNombre(Persona[] personas, string nombre,
        bool coincidenciaParcial, bool sensibleMayus)
    {
        return null;
    }

}

internal class Persona
{
    public string Nombre { get; set; }
}



using System.Reflection;

namespace Efectos;

class Program
{
    static void Main()
    {
        string ruta = "../../../logs.txt";
        string? resultado1 = FindFirstCritical(ruta);
        Console.WriteLine($"Línea crítica: {resultado1}");

        // Rediseñar FindFirstCritical para desacoplar los efectos secundarios de la lógica principal, 
        // para poder reutilizar la lógica en diferentes contextos (producción, testing, etc.) sin tener que modificarla.
        // Probar la función en producción, es decir, con efectos secundarios reales
        //       (lectura de archivos, escritura en consola, etc.).

        // Producción
        IEnumerable<string> lineas = File.ReadLines(ruta);
        string? resultado2 = FindFirstCriticalV2(lineas, Console.WriteLine);
        Console.WriteLine($"Línea Crítica (V2): {resultado2}");

        IEnumerable<string> lineasTest = ["Hola", "CRITICAL", "No"];
        List<string> listaLog = new List<string>();
        string? resultado3 = FindFirstCriticalV2(lineas, listaLog.Add);
        
        // Probar la función en testing, es decir, con efectos secundarios simulados (mocking),
        // para poder verificar su comportamiento sin depender de recursos externos (archivos, consola, etc.).
        // Por ejemplo: Una lista de cadenas de texto y una lista de mensajes de log y haciendo uso de asertos con el resultado


    }
        //los efectos secundarios pueden ser tanto de entrada como de salida o ambos
    static string? FindFirstCritical(string ruta)
    {
        Console.WriteLine($"Abriendo...");     // Efecto secundario
        foreach (var linea in File.ReadLines(ruta))   // Efecto secundario      operacion e/s
        {
            if (linea.Contains("CRITICAL", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Encontrado.");         // Efecto secundario
                return linea;           //si devuelve linea tuvo que imprimir encontrado
            }
        }
        Console.WriteLine("No encontrado.");             // Efecto secundario
        return null;            //si devuelve null tuvo que imprimir no encontrado
    }

    static string? FindFirstCriticalV2(IEnumerable<string> lineas, Action<string> log)
    {
        log("$Abriendo...");
        foreach(var linea in lineas)
        {
            if (linea.Contains("CRITICAL", StringComparison.OrdinalIgnoreCase))
            {
                log("Encontrado");      
                return linea;           
            }
        }
        log("No encontrado.");
        return null;        
    }
}

using System;

namespace TareasInvoke;

using System.Text;

public static class ProcesadorTextos
{

    /// <summary>
    /// Lee un fichero
    /// </summary>
    /// <param name="fileName">Nombre del fichero de entrada</param>
    /// <returns>String con el texto del fichero</returns>
    public static String LeerFicheroTexto(string nombreFichero)
    {
        using (StreamReader stream = File.OpenText(nombreFichero))
        {
            StringBuilder text = new StringBuilder();
            string? linea;
            while ((linea = stream.ReadLine()) != null)
            {
                text.AppendLine(linea);
            }
            return text.ToString();
        }
    }

    /// <summary>
    /// Devuelve el número de signos de puntuación ( . , ; : ) en el texto.
    /// </summary>
    public static int ContarSignosPuntuacion(string text)
    {
        return text.Count(c => c == '.' || c == ',' || c == ';' || c == ':');
    }


    /// <summary>
    /// Divide un texto en palabras
    /// </summary>
    public static string[] DividirEnPalabras(String texto)
    {
        return texto.Split(new char[] { ' ', '\r', '\n', ',', '.', ';', ':', '-', '!', '¡', '¿', '?', '/', '«',
                                        '»', '_', '(', ')', '\"', '*', '\'', 'º', '[', ']', '#' },
            StringSplitOptions.RemoveEmptyEntries);
    }

    /// <summary>
    /// Devuelve la lista de palabras con la mayor longitud
    /// </summary>
    public static string[] PalabrasMasLargas(string[] palabras)
    {
        // Usando funciones de orden superiror de Linq
        return palabras
            .GroupBy(palabra => palabra.Length)  // Agrupamos las palabras por misma longitud
            .OrderByDescending(grupo => grupo.Key)  // ordenamos los grupos por longitud (de mayor a menor)
            .Select(grupo => grupo.Distinct()) // Eliminamos las palabras repetidas de cada grupo
            .First() // Seleccionamos el primer grupo (palabras con la mayor longitud)
            .ToArray(); // Y lo convertimos en un array.
    }

    /// <summary>
    /// Devuelve la lista de palabras con la menor longitud
    /// </summary>
    public static string[] PalabrasMasCortas(string[] palabras)
    {
        return palabras
            .GroupBy(palabra => palabra.Length)  // Agrupamos las palabras por igual longitud
            .OrderBy(grupo => grupo.Key)  // ordenamos los grupos por longitud (de menor a mayor)
            .Select(grupo => grupo.Distinct()) // Eliminamos las palabras repetidas de cada grupo
            .First() // Seleccionamos el primer grupo (palabras con la mayor longitud)
            .ToArray(); // Y lo convertimos en un array.

    }

    /// <summary>
    /// Devuelve una lista de las palabras que aparece menos veces en el texto.
    /// <param name="palabras">Lista de palabras</param>
    /// <param name="repeticiones">Out: el número de repeticiones de las palabras encontradas</param>
    /// <returns>Una lista con las palabras que aparecen el menor número de veces en el texto.</returns>
    /// </summary>
    public static string[] PalabrasAparecenMenosVeces(string[] palabras, out int repeticiones)
    {
        var palabrasYrepeticiones = palabras
            .GroupBy(palabra => palabra.ToLower()) // agrupamos por palabras (todas en minúsculas)
            .Select(grupo => new { Palabra = grupo.Key, Repeticiones = grupo.Count() }) // Convertimos en lista de parejas (Palabra, Repeticiones)
            .OrderBy(pareja => pareja.Repeticiones); // ordenamos las parejas de menor a mayor número de repeticiones

        // Obtenemos el número de veces que se repite la primera pareja (que será la que menos veces se repita)
        int menorRepeticiones = repeticiones = palabrasYrepeticiones.First().Repeticiones;

        // Devolvemos todas las palabras cuyas repeticiones coinciden con el menor número registrado.
        return palabrasYrepeticiones
            .Where(pareja => pareja.Repeticiones == menorRepeticiones) // las que coincidan con el menor número de repeticines.
            .Select(pareja => pareja.Palabra)  // seleccionamos la palabra (que estará ya en minúsculas)
            .ToArray(); // pasamos a array
    }

    /// <summary>
    /// Devuelve una lista con aquellas palabras que se repiten más veces en un texto.
    /// <param name="palabras">Lista de palabras</param>
    /// <param name="repeticiones">Out: el número de repeticiones de las palabras encontradas</param>
    /// <returns>Una lista con las palabras que aparecen el mayor número de veces en el texto.</returns>
    /// </summary>
    public static string[] PalabrasAparecenMasVeces(string[] palabras, out int repeticiones)
    {
        var palabrasYrepeticiones = palabras
            .GroupBy(palabra => palabra.ToLower()) // agrupamos por palabras (todas en minúsculas)
            .Select(grupo => new { Palabra = grupo.Key, Repeticiones = grupo.Count() }) // Convertimos en lista de aprejas (Palabra, Repeticiones)
            .OrderByDescending(pareja => pareja.Repeticiones); // ordenamos las parejas de mayor a menos número de repeticiones

        // Obtenemos el número de veces que se repite la primera pareja (que será la que más veces se repita)
        int mayorRepeticiones = repeticiones = palabrasYrepeticiones.First().Repeticiones;

        // Devolvemos todas las palabras cuyas repeticiones coinciden con el mayor número registrado.
        return palabrasYrepeticiones
            .Where(pareja => pareja.Repeticiones == mayorRepeticiones) // las que coincidan con el mayor número de repeticines.
            .Select(pareja => pareja.Palabra)  // seleccionamos la palabra (que estará ya en minúsculas)
            .ToArray(); // pasamos a array
    }


    /// <summary>
    /// Método que muestra los resultados de procesar el texto
    /// </summary>
    public static void MostrarResultados(int signosPuntuacion, string[]? palabrasMasCortas, string[]? palabrasMasLargas,
                                    string[]? PalabrasAparecenMenosVeces, int menosRepeticiones,
                                    string[]? PalabrasAparecenMasVeces, int masRepeticiones)
    {
        const int MaxElementosMostrar = 20;

        Console.WriteLine("> Encontrados {0} signos de puntuación. ", signosPuntuacion);

        Console.Write("> {0} palabras más cortas: ", palabrasMasCortas?.Count());
        Mostrar(palabrasMasCortas, Console.Out, MaxElementosMostrar);
        Console.WriteLine();

        Console.Write("> {0} palabras más largas: ", palabrasMasLargas?.Count());
        Mostrar(palabrasMasLargas, Console.Out, MaxElementosMostrar);
        Console.WriteLine();

        Console.Write("> {0} palabras que aparecen menos veces ({1}): ", PalabrasAparecenMenosVeces?.Count(), menosRepeticiones);
        Mostrar(PalabrasAparecenMenosVeces, Console.Out, MaxElementosMostrar);
        Console.WriteLine();

        Console.Write("> {0} palabras que aparecen más veces ({1}): ", PalabrasAparecenMasVeces?.Count(), masRepeticiones);
        Mostrar(PalabrasAparecenMasVeces, Console.Out, MaxElementosMostrar);
        Console.WriteLine();
    }

    /// <summary>
    /// Método genérico que muestra los elemenos de una colección.
    /// Recibe el máximo número de elementos a mostrar.
    /// </summary>
    private static void Mostrar<T>(IEnumerable<T>? coleccion, TextWriter stream, int maxNumeroElementos)
    {
        if (coleccion == null)
        {
            Console.WriteLine("La colección es null.");
            return;
        }

        int i = 0;
        foreach (T elemento in coleccion)
        {
            stream.Write(elemento);
            i = i + 1;
            if (i == maxNumeroElementos)
            {
                stream.Write("...");
                break;
            }
            if (i < coleccion.Count())
                stream.Write(", ");
        }
    }

}

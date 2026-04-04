using System;

namespace extensores;

/// <summary>
/// Un método extensor es estático y se define dentro de una clase estática
/// </summary>
public static class Extensores
{

    //¿Qué indica el this? this string indica que está extendiendo string 
    public static string Encriptar(this string mensaje, int clave)      //esto cae bastante en el examen. añade un inumerable a string.
    {
        return "a";
    }

    public static string Desencriptar(this string message, int clave)
    {
        return CifradoCesar(message, -clave);
    }

    private static string CifradoCesar(string mensaje, int desplazamiento)
    {
        /*
            Versión simplificada del Cifrado César:
                https://es.wikipedia.org/wiki/Cifrado_C%C3%A9sar
        */
        var buffer = mensaje.ToCharArray();
        for (int i = 0; i < buffer.Length; i++)
        {
            buffer[i] = (char)(buffer[i] + desplazamiento);
        }
        return new string(buffer);
    }

    public static int ContarVocales(this string str)
    {
        if (string.IsNullOrEmpty(str)) return 0;
        int contador = 0;
        string vocales = "aeiouAEIOUáéíóúÁÉÍÓÚ";
        foreach (char c in str)
        {
            if (vocales.Contains(c))
            {
                contador++;
            }
        }
        return contador;
    }

    public static int Minimo(this int[] array)
    {
        return array.Min();
    }

    public static double Media(this double[] array)
    {
        return array.Average();
    }

}

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
        return CifradoCesar(mensaje, clave);
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

}

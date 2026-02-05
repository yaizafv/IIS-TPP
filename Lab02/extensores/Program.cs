namespace extensores;

class Program
{
    static void Main(string[] args)
    {
        int clave = 3;
        string mensaje = "Ave Caesar!";
        Console.WriteLine($"Original: {mensaje}");
        
        // ¿Encriptar?
        string encriptado = mensaje.Encriptar(clave);
        Console.WriteLine($"Encriptado: {encriptado}");

        string desencriptado = encriptado.Desencriptar(clave);
        Console.WriteLine($"Desencriptado: {desencriptado}");


        /* EJERCICIOS:
            - Añádase al tipo string la posibilidad de contar cuántas vocales contiene una cadena, mediante ContarVocales.
            - Añádase al tipo int[] la capacidad de devolver el valor mínimo de sus elementos, mediante Minimo.
            - Añádase al tipo double[] la capacidad de devolver la media aritmética de sus elementos, mediante Media.
        */
    }
}

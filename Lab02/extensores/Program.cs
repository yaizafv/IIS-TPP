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

        string frase = "Hola Mundo";
        Console.WriteLine($"Vocales en '{frase}': {frase.ContarVocales()}");

        int[] numeros = { 10, 5, 8, 2, 20 };
        Console.WriteLine($"Mínimo del array: {numeros.Minimo()}");

        double[] valores = { 10.5, 20.5, 30.0 };
        Console.WriteLine($"Media del array: {valores.Media()}");
    }
}

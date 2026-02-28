using System.Diagnostics;

namespace Lambda;

class Program
{
    static void Main(string[] args)
    {

        /*
        * Las expresiones lambda permiten expresar una función “inline”, sin declarar un método aparte.
        * Si partimos de este método:
        *
        *   public static string Concatenar(char a, char b)
        *   {
        *       return string.Format("{0}{1}", a, b);
        *   }
        *
        * Podemos obtener una lambda equivalente así:
        *
        * 1) Quitamos el nombre del método y el tipo de retorno, nos quedamos con parámetros y cuerpo:
        *    (char a, char b) { return string.Format("{0}{1}", a, b); }
        *
        * 2) Insertamos => (operador lambda) entre los parámetros y el cuerpo:
        *    (char a, char b) => { return string.Format("{0}{1}", a, b); }
        *
        * 3) Asignamos la lambda a un delegado compatible según parámetros y retorno:
        */

        Func<char, char, string> operacion = (char a, char b) =>
        {
            return string.Format("{0}{1}", a, b);
        };

        // Si el cuerpo es una única expresión, podemos omitir { } y el return (pero la asignación sigue terminando en ;).
        Func<char, char, string> operacionLimpia = (char a, char b) => string.Format("{0}{1}", a, b);

        // Omitiendo tipos (inferidos por el delegado):
        operacionLimpia = (a, b) => string.Format("{0}{1}", a, b);

        Console.WriteLine(operacionLimpia('l', 'a'));

        // Si no tuviéramos parámetros, usaríamos ()
        // Con un único parámetro, se pueden omitir los paréntesis si no se indica el tipo.
        Predicate<int> esPar = n => n % 2 == 0;


        Action<string> consolaRojo =
            cadena =>
                {
                    ConsoleColor color = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(cadena);
                    Console.ForegroundColor = color;
                }; // Ojo: aquí sí va ;

        int numero = 4; // Par para que se vea el ejemplo en rojo
        string texto = $"Si este texto es rojo, {numero} es par";
        
        Action<string> mostrar;

        if (esPar(numero))
            mostrar = consolaRojo;
        else
            mostrar = Console.WriteLine;


        mostrar(texto);
    }

}

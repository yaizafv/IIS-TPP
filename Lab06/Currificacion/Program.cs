using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.Marshalling;

namespace lab06;

class Program
{
    static void Main()
    {

        // No currificada vs currificada
        bool res1 = StartsWith("pe", "perezoso");
        Console.WriteLine($"\"StartsWith(pe, perezoso)\": {res1}");

        //Aplicación parcial: funciones que almacenan valores aplicados (funciones especializadas)
        Func<string, bool> comienzaPorPe = CurriedStartsWith("pe");
        bool resX = comienzaPorPe("hola");

        bool res2 = CurriedStartsWith("pe")("perezoso");
        Console.WriteLine($"CurriedStartsWith(\"pe\")(\"perezoso\"): {res2}");

        // ¿Qué estámos usando aquí?
        var EsUrlSegura = CurriedStartsWith("https://");
        Console.WriteLine($"¿Es segura https://uniovi.es ? {EsUrlSegura("https://uniovi.es")}");
        Console.WriteLine($"¿Es segura http://uniovi.es ? {EsUrlSegura("http://uniovi.es")}");

        Console.WriteLine($"EstaEnRango(0, 10, 5): {EstaEnRango(0, 10, 5)}");
        // Implementa la versión currificada de EstaEnRango
        // Empleando la anterior, crea "EstaEnEdadLaboral" [16, 67]
        // Currifica la función Suma. En cada devolución, la función solamente recibirá un único valor
        
        // Fijamos el Dividendo en 20
        var validarVeinte = ComprobarDivision(20);
        // Fijamos el divisor en 6 (Ahora tenemos una función que solo espera q y r)
        var validarDivisionEntreSeis = validarVeinte(6);
        // Probamos con diferentes resultados de alumnos
        bool resultadoAlumnoA = validarDivisionEntreSeis(3)(2); // True: (6*3)+2 = 20
        bool resultadoAlumnoB = validarDivisionEntreSeis(2)(8); // False: (6*2)+8 = 20 pero r(8) > d(6)

        Console.WriteLine($"¿Alumno A ok? {resultadoAlumnoA}");
        Console.WriteLine($"¿Alumno B ok? {resultadoAlumnoB}");

        bool todoJunto = ComprobarDivision(20)(6)(3)(2);
    }

    static bool StartsWith(string prefijo, string texto)
    {
        return texto.StartsWith(prefijo, StringComparison.OrdinalIgnoreCase);
    }

    static Func<string, bool> CurriedStartsWith(string prefijo)
    {
        return texto => texto.StartsWith(prefijo, StringComparison.OrdinalIgnoreCase);
    }

    static bool EstaEnRango(int min, int max, int x)
    {
        return x >= min && x <= max;
    }

    static Func<int, Func<int, bool>> EstaEnRango(int min)
    {
        return max => x => x >= min && x <= max;
    }

    static int Suma(int a, int b)
    {
        return a + b;
    }

    static Func<int, int> Suma(int a)
    {
        return b => a + b;
    }

    //currificar una funcion que compruebe que una division este bien (con la formula esa)
    static bool DivisionCorrecta(int dividendo, int divisor)
    {
        int cociente = dividendo / divisor;
        int resto = dividendo % divisor;
        int result = (divisor * cociente) + resto;
        if (dividendo == result) return true;
        return false;
    }

    // Definición Currificada: 
    // Recibe D -> devuelve función que recibe d -> devuelve función que recibe q -> devuelve función que recibe r -> devuelve bool
    static Func<int, Func<int, Func<int, Func<int, bool>>>> ComprobarDivision =
        D => d => q => r => (D == (d * q) + r) && (r < d);

    static Func<int, Func<int, Func<int, bool>>> ComprobarDivisionMasLargo(int D)
    {
        return d => q => r => (D == (d * q) + r) && (r < d);
    }
}

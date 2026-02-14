using System.Diagnostics;

namespace Asertos;

class Program
{
    static void Main()
    {       
       // Paleta de comandos: .NET: Select a Configuration > Release > Any Configuration
       // o dentro la carpeta de un proyecto: dotnet run --configuration Release
       
        EjemploSimple(); // ¿Debug vs Release?
        EjemploProgramacionDefensiva();
    }

    private static void EjemploSimple()
    {
        Console.WriteLine("ANTES DE LA ASERCIÓN");
        Debug.Assert(true, "ESTA ASERCIÓN DE DEPURACIÓN HA TENIDO ÉXITO. NO SE MOSTRARÁ.");
        Debug.Assert(false, "ESTA ASERCIÓN DE DEPURACIÓN FALLA. SOLO SE MOSTRARÁ EN MODO DEBUG.");
        Console.WriteLine("DESPUÉS DE LA ASERCIÓN");
    }

    private static void EjemploProgramacionDefensiva()
    {
        //Release vs Debug.
        var array = new int[] { 1,2,3,4 };
        bool resultado = TryEmptyArray(array);
        Debug.Assert(resultado, "El array era null o estaba vacío, no se puede vaciar.");
        Console.WriteLine($"Array: {string.Join(", ", array)}");
    }

    private static bool TryEmptyArray(int[] array)
    {
        if (array == null || array.Length == 0)
            return false;

        for (int i = 0; i < array.Length; i++)
        {
            array[i] = 0;
        }
        return true;
    }
}

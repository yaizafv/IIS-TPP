using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TareasInvoke;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length > 0 && args[0] == "p")
            IndependientesParalelo();
        else
            IndependientesSecuencial();
    }


    static void IndependientesParalelo()
    {
        string texto = ProcesadorTextos.LeerFicheroTexto("../../../../clarin.txt");
        string[] palabras = ProcesadorTextos.DividirEnPalabras(texto);

        string[]? palabrasMasLargas = null, palabrasMasCortas = null, palabrasAparecenMasVeces = null, palabrasAparecenMenosVeces = null;

        int masRepeticiones = 0, menosRepeticiones = 0, signosPuntuacion = 0;

        // El método Invoke de la clase Parallel se emplea en la paralelización de tareas independientes:
        // Recibe Actions - Haciendo uso de la palabra clave params : 
        // Esto nos permite ir pasando N parámetros separados por comas. En este caso Actions y, cada uno, es una tarea.

        // Las tareas pueden ejecutarse en paralelo sobre varios hilos, gestionados por TPL.
        // Espera a que acaben todas las tareas.

        Stopwatch sw = Stopwatch.StartNew();
        int contador = 0;
        Parallel.Invoke(            //lanza un hilo para cada tarea (expresion lambda)

            () =>
            {
                contador++; //si solo esta aqui no es un recurso compartido porque solo accede a él un hilo (no hace falta lock)
                signosPuntuacion = ProcesadorTextos.ContarSignosPuntuacion(texto);
            }
                ,
            () =>
            {
                contador++;     //al estar tambien aqui si que haria falta
                palabrasMasLargas = ProcesadorTextos.PalabrasMasLargas(palabras);
            }
            ,
            () => palabrasMasCortas = ProcesadorTextos.PalabrasMasCortas(palabras),
            () => palabrasAparecenMasVeces = ProcesadorTextos.PalabrasAparecenMasVeces(palabras, out masRepeticiones),
            () => palabrasAparecenMenosVeces = ProcesadorTextos.PalabrasAparecenMenosVeces(palabras, out menosRepeticiones)

        );

        sw.Stop();

        ProcesadorTextos.MostrarResultados(signosPuntuacion, palabrasMasCortas, palabrasMasLargas, palabrasAparecenMenosVeces, menosRepeticiones,
            palabrasAparecenMasVeces, masRepeticiones);

        Console.WriteLine($"[TPL Invoke] Tiempo: {sw.ElapsedMilliseconds} ms.");
    }

    static void IndependientesSecuencial()
    {
        String texto = ProcesadorTextos.LeerFicheroTexto("../../../../clarin.txt");
        string[] palabras = ProcesadorTextos.DividirEnPalabras(texto);

        Stopwatch sw = Stopwatch.StartNew();

        int signosPuntuacion = ProcesadorTextos.ContarSignosPuntuacion(texto);
        var palabrasMasLargas = ProcesadorTextos.PalabrasMasLargas(palabras);
        var palabrasMasCortas = ProcesadorTextos.PalabrasMasCortas(palabras);
        int masRepeticiones, menosRepeticiones;
        var palabrasAparecenMasVeces = ProcesadorTextos.PalabrasAparecenMasVeces(palabras, out masRepeticiones);    //out -> paso por referencia
        var palabrasAparecenMenosVeces = ProcesadorTextos.PalabrasAparecenMenosVeces(palabras, out menosRepeticiones);
        sw.Stop();

        ProcesadorTextos.MostrarResultados(signosPuntuacion, palabrasMasCortas, palabrasMasLargas, palabrasAparecenMenosVeces, menosRepeticiones,
            palabrasAparecenMasVeces, masRepeticiones);

        Console.WriteLine($"[Secuencial] Tiempo: {sw.ElapsedMilliseconds} ms.");

    }
}

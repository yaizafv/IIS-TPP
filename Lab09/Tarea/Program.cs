namespace Tarea;

using System.Linq;
using tpp.lab09;

class Program
{
    private static Modelo modelo = new Modelo();

    static void Main()
    {
        Ejercicio1();
        Ejercicio2();
        Ejercicio3();
        Ejercicio4();
        Ejercicio5();
        Ejercicio6();
    }

    // 1. Obtener el nombre de las plataformas que tengan al menos una película dirigida por "Greta Gerwig".
    static void Ejercicio1()
    {
        Console.WriteLine("Ejercicio 1");
        var consulta1 = modelo.Plataformas
            .Where(plat => plat.Peliculas.Any(p => p.Director == "Greta Gerwig"))
            .Select(plat => plat.Nombre);

        Show(consulta1);
    }

    // 2. Obtener el nombre de los directores que tengan al menos una película con más de un género.
    static void Ejercicio2()
    {
        Console.WriteLine("Ejercicio 2");
        var consulta2 = modelo.Peliculas
            .Where(p => p.Generos.Count() > 1)
            .Select(p => p.Director)
            .Distinct();

        Show(consulta2);
    }

    // 3. Obtener el título de la película, la fuente y la puntuación de aquellas valoraciones 
    // cuya fuente coincida con la plataforma en la que está disponible la película, 
    // ordenadas alfabéticamente por título.
    static void Ejercicio3()
    {
        Console.WriteLine("Ejercicio 3");
        var consulta3 = modelo.Valoraciones
            .Join(
                modelo.Peliculas,
                v => v.IdImdb,
                p => p.IdImdb,
                (v, p) => new { v, p }
            )
            .Where(t => t.v.Fuente == t.p.Plataforma.Nombre)
            .Select(t => new
            {
                Titulo = t.p.Titulo,
                Fuente = t.v.Fuente,
                Puntuacion = t.v.Puntuacion
            })
            .OrderBy(a => a.Titulo);

        Show(consulta3);
    }

    // 4. Obtener, para cada plataforma, la duración mínima, máxima y media de sus películas, 
    // ordenando el resultado alfabéticamente por nombre de plataforma.
    static void Ejercicio4()
    {
        Console.WriteLine("Ejercicio 4 (Plataforma; Min; Max; Media)");
        var consulta4 = modelo.Plataformas
            .OrderBy(plat => plat.Nombre)
            .Select(plat => new
            {
                Formato = $"{plat.Nombre}; " +
                          $"{(plat.Peliculas.Any() ? plat.Peliculas.Min(p => p.Duracion) : 0)}; " +
                          $"{(plat.Peliculas.Any() ? plat.Peliculas.Max(p => p.Duracion) : 0)}; " +
                          $"{(plat.Peliculas.Any() ? plat.Peliculas.Average(p => p.Duracion) : 0)}"
            });

        Show(consulta4.Select(c => c.Formato));
    }

    // 5. Obtener, para cada director que tenga películas estrenadas a partir de 2010 con valoraciones, 
    // el nombre del director y la suma de votos de las valoraciones de dichas películas.
    static void Ejercicio5()
    {
        Console.WriteLine("Ejercicio 5");
        var consulta5 = modelo.Peliculas
            .Where(p => p.FechaEstreno.Year >= 2010)
            .SelectMany(p => modelo.Valoraciones
                .Where(v => v.IdImdb == p.IdImdb)
                .Select(v => new { p.Director, v.Votos }))
            .GroupBy(x => x.Director)
            .Select(g => new
            {
                Director = g.Key,
                SumaVotos = g.Sum(x => x.Votos)
            });

        Show(consulta5);
    }

    // 6. Obtener el título y la duración de las películas cuya duración sea superior 
    // a la duración media de las películas del modelo.
    static void Ejercicio6()
    {
        Console.WriteLine("Ejercicio 6");
        double mediaGlobal = modelo.Peliculas.Average(p => p.Duracion);

        var consulta6 = modelo.Peliculas
            .Where(p => p.Duracion > mediaGlobal)
            .Select(p => new { p.Titulo, p.Duracion });

        Show(consulta6);
    }

    static void Show<T>(IEnumerable<T> secuencia)
    {
        Console.WriteLine("-----------------------------------------------");
        foreach (var elemento in secuencia)
            Console.WriteLine(elemento);
        Console.WriteLine();
    }

    static void Show<TClave, TElemento>(IEnumerable<IGrouping<TClave, TElemento>> grupos)
    {
        Console.WriteLine("-----------------------------------------------");
        foreach (var grupo in grupos)
        {
            Console.WriteLine(grupo.Key); // Un grupo consta de una clave (.Key)
            foreach (var elemento in grupo) // Y almacena los elementos asociados a la clave.
                Console.WriteLine($"\t{elemento}");
            Console.WriteLine();
        }
    }
}

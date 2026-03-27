namespace tpp.lab09;

using System.Linq;
public static class Program
{
    private static Modelo modelo = new Modelo();

    static void Main()
    {
        // Map, Filter y Reduce en LINQ y objetos anónimos.
        MetodosEquivalentes();

        // FlatMap -> SelectMany
        MetodoSelectMany();

        // Join
        MetodoJoin();

        // GroupBy
        MetodoGroupBy();

        EjerciciosConsultas();
    }

    static void MetodosEquivalentes()
    {
        Console.WriteLine("Uso del Where (Filter):");
        // Filter -> Where (NOTA: Recibe Func<T,bool> y no Predicate<T>)
        IEnumerable<Pelicula> consultaWhere = modelo.Peliculas
            .Where(p => p.Duracion > 110);
        Show(consultaWhere);

        Console.WriteLine("Uso del Select (Map):");
        // Map -> Select
        IEnumerable<string> consultaSelect = modelo.Peliculas
            .Select(p => p.Titulo);
        Show(consultaSelect);

        Console.WriteLine("Consultas con objetos anónimos:");
        // Uso de objetos anónimos.
        // Pueden utilizarse también tuplas (ToString menos expresivo)
        var consultaAnonimos = modelo.Peliculas
            .Select(p => new // Sintaxis para crear un objeto anónimo new { ... } y tipo 'var'.
            {
                Encabezado = $"{p.Titulo} por {p.Director} ({p.FechaEstreno.Year})",
                Duracion = p.Duracion
            });

        Show(consultaAnonimos);

        Console.WriteLine("Uso del Aggregate (Reduce):");
        // Reduce -> Aggregate
        int totalMinutos = modelo.Peliculas.Aggregate(0, (acc, p) => acc + p.Duracion);
        Console.WriteLine($"Sumatorio de minutos: {totalMinutos}\n");
    }

    static void MetodoSelectMany()
    {
        // Obtener todos los géneros distintos presentes en las películas del modelo.

        // Con Select estamos obteniendo una colección de colecciones.
        Console.WriteLine("Problema del Select (Map). Géneros:");
        IEnumerable<IEnumerable<string>> generosSelect = modelo.Peliculas.Select(p => p.Generos);
        Show(generosSelect);

        // SelectMany (FlatMap). A partir de cada elemento, obtenemos una secuencia de elementos Y
        // y queremos aplanar todas esas secuencias en una.
        Console.WriteLine("SelectMany (FlatMap). Géneros:");
        IEnumerable<string> generos = modelo.Peliculas.SelectMany(p => p.Generos);
        Show(generos.Distinct()); // Eliminamos repetidos con Distinct()
    }

    static void MetodoJoin()
    {
        Console.WriteLine("Uso del Join:");
        // Obtener, para cada valoración, el título de la película a la que corresponde, la fuente de la valoración y su puntuación.

        // Join. Une dos colecciones distintas a partir de una clave común.
        // Para cada elemento de la primera colección, busca en la segunda los elementos cuya clave coincida.
        // Por cada coincidencia (pareja) encontrada, genera un resultado

        var join = modelo.Valoraciones           // Valoraciones es la primera colección
            .Join(
                modelo.Peliculas,                // Peliculas es la segunda colección
                valoracion => valoracion.IdImdb, // La clave de cada Valoración será IdImdb
                pelicula => pelicula.IdImdb,     // La clave de cada Película será IdImdb
                (valoracion, pelicula) =>        // Si ambas claven coinciden, tratamos la pareja (Valoración, Película)
                    new
                    {
                        Titulo = pelicula.Titulo,
                        Fuente = valoracion.Fuente,
                        Puntuacion = valoracion.Puntuacion
                    }
            );

        Show(join);
    }

    static void MetodoGroupBy()
    {
        Console.WriteLine("Uso del GroupBy:");
        // Obtener cuántas películas hay de cada director.

        // GroupBy. Agrupa en base a una clave y devuelve una secuencia de grupos (IGrouping).
        // Cada grupo se compone de:
        // - Una clave. En este caso, Director <string>
        // - Los elementos que pertenecen a esa clave. En este caso, las películas <Pelicula>

        IEnumerable<IGrouping<string, Pelicula>> porDirector = modelo.Peliculas
            .GroupBy(p => p.Director); // Clave de agrupación.

        Show(porDirector); // Analiza este Show.

        // Con el Select (Map) transformamos los grupos en lo que nos convenga.
        var numPeliculasPorDirector = porDirector.Select(grupo => new
        {
            Director = grupo.Key,
            NumPeliculas = grupo.Count()
        });
        Show(numPeliculasPorDirector);
    }

    static void EjerciciosConsultas()
    {
        // Obtener los títulos de las películas cuyo director empiece por "C".
        Consulta1();

        // Título y fecha de estreno de las películas disponibles en HBO Max
        Consulta2();

        // A partir de aquí, necesitarás emplear otros métodos de LINQ: 
        // https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable?view=net-9.0

        // Obtener el nombre de las plataformas que tengan al menos una película con una duración superior a 120 minutos.
        Consulta3a();

        // Obtener el nombre de las plataformas que tengan más de una película con una duración superior a 120 minutos.
        Consulta3b();

        // Obtener el nombre de las plataformas en las que ninguna película pertenezca al género Fantasy.
        Consulta4();

        // Obtener las valoraciones procedentes de la fuente IMDb para aquellas películas cuya duración sea superior a 120 minutos,
        // mostrando el título de la película y su puntuación, ordenadas de mayor a menor puntuación.
        Consulta5();

        // Obtener, para cada plataforma, su nombre y el título de su película más corta ("-" si no tiene películas),
        // ordenando los resultados alfabéticamente por nombre de plataforma.
        Consulta6();

        // Obtener, para cada película que tenga valoraciones, la mayor puntuación recibida entre las distintas fuentes de valoración.
        Consulta7();
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

    static void Consulta1()
    {
        // Obtener los títulos de las películas cuyo director empiece por "C".
        Console.WriteLine("Consulta 1");
        var consulta1 = modelo.Peliculas.Where(p => p.Director.StartsWith('C')).Select(p => p.Titulo);
        Show(consulta1);
    }

    static void Consulta2()
    {
        // Título y fecha de estreno de las películas disponibles en HBO Max
        Console.WriteLine("Consulta 2");
        var consulta2 = modelo.Peliculas.Where(p => p.Plataforma.Nombre.Equals("HBO Max")).Select(p => new
        {
            Titulo = p.Titulo,
            Fecha = p.FechaEstreno
        });
        Show(consulta2);
    }

    // A partir de aquí, necesitarás emplear otros métodos de LINQ: 
    // https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable?view=net-9.0
    static void Consulta3a()
    {
        // Obtener el nombre de las plataformas que tengan al menos una película con una duración superior a 120 minutos.
        Console.WriteLine("Consulta 3a");
        var consulta3a = modelo.Plataformas.Where(p => p.Peliculas.Any(p => p.Duracion > 120)).Select(p => p.Nombre);
        Show(consulta3a.Distinct());
    }

    static void Consulta3b()
    {
        // Obtener el nombre de las plataformas que tengan más de una película con una duración superior a 120 minutos.
        var consulta3b = modelo.Plataformas.Where(p => p.Peliculas.Count(p => p.Duracion > 120) > 1).Select(p => p.Nombre);
        //var consulta3b = modelo.Plataformas.Where(p => p.Duracion > 120).Skip(1).Any().Select(p => p.Nombre);    // forma perezosa (mejor rendimiento)
        Console.WriteLine("Consulta 3b");
        Show(consulta3b.Distinct());
    }

    static void Consulta4()
    {
        // Obtener el nombre de las plataformas en las que ninguna película pertenezca al género Fantasy.
        var consulta4 = modelo.Plataformas.Where(p => !p.Peliculas.Any(p => p.Generos.Contains("Fantasy"))).Select(p => p.Nombre);
        //var consulta4 = modelo.Plataformas.Where(p => p.Peliculas.All(p => !p.Generos.Contains("Fantasy"))).Select(p => p.Nombre);
        Console.WriteLine("Consulta 4");
        Show(consulta4.Distinct());
    }

    static void Consulta5()
    {
        // Obtener las valoraciones procedentes de la fuente IMDb para aquellas películas cuya duración sea superior a 120 minutos,
        // mostrando el título de la película y su puntuación, ordenadas de mayor a menor puntuación.
        var consulta5 = modelo.Valoraciones.Where(v => v.Fuente == "IMDb")
        .Join(
                modelo.Peliculas.Where(p => p.Duracion > 120),
                valoracion => valoracion.IdImdb,
                pelicula => pelicula.IdImdb,
                (valoracion, pelicula) => new
                {
                    Titulo = pelicula.Titulo,
                    Puntuacion = valoracion.Puntuacion
                }).OrderByDescending(a => a.Puntuacion);
        Show(consulta5);
        Console.WriteLine("Consulta 5");
    }

    static void Consulta6()
    {
        // Obtener, para cada plataforma, su nombre y el título de su película más corta ("-" si no tiene películas),
        // ordenando los resultados alfabéticamente por nombre de plataforma.
        Console.WriteLine("Consulta 6");
    }

    static void Consulta7()
    {
        // Obtener, para cada película que tenga valoraciones, la mayor puntuación recibida entre las distintas fuentes de valoración.
        Console.WriteLine("Consulta 7");
    }
}

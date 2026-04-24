namespace tpp.lab09;

public class Modelo
{
    private readonly List<Plataforma> _plataformas = new List<Plataforma>();
    private readonly List<Pelicula> _peliculas = new List<Pelicula>();
    private readonly List<Valoracion> _valoraciones = new List<Valoracion>();

    public IReadOnlyList<Plataforma> Plataformas { get {return _plataformas; } }
    public IReadOnlyList<Pelicula> Peliculas { get {return _peliculas; } }
    public IReadOnlyList<Valoracion> Valoraciones { get {return _valoraciones; } }

    public Modelo()
    {
        ResetModel();
    }

    public void ResetModel()
    {
        _plataformas.Clear();
        _peliculas.Clear();
        _valoraciones.Clear();

        var netflix = new Plataforma { Nombre = "Netflix" };
        var hboMax = new Plataforma { Nombre = "HBO Max" };
        var primeVideo = new Plataforma { Nombre = "Prime Video" };

        _plataformas.AddRange(new[]
        {
            netflix,
            hboMax,
            primeVideo
        });

        _peliculas.AddRange(new[]
        {
            new Pelicula
            {
                IdImdb = "tt1375666",
                Titulo = "Inception",
                Director = "Christopher Nolan",
                FechaEstreno = new DateTime(2010, 7, 16),
                Duracion = 148,
                Generos = new List<string> { "Sci-Fi", "Thriller" },
                Plataforma = netflix
            },
            new Pelicula
            {
                IdImdb = "tt0816692",
                Titulo = "Interstellar",
                Director = "Christopher Nolan",
                FechaEstreno = new DateTime(2014, 11, 5),
                Duracion = 169,
                Generos = new List<string> { "Sci-Fi", "Drama", "Adventure" },
                Plataforma = hboMax
            },
            new Pelicula
            {
                IdImdb = "tt2543164",
                Titulo = "Arrival",
                Director = "Denis Villeneuve",
                FechaEstreno = new DateTime(2016, 11, 11),
                Duracion = 116,
                Generos = new List<string> { "Sci-Fi", "Drama" },
                Plataforma = hboMax
            },
            new Pelicula
            {
                IdImdb = "tt3397884",
                Titulo = "Sicario",
                Director = "Denis Villeneuve",
                FechaEstreno = new DateTime(2015, 9, 18),
                Duracion = 121,
                Generos = new List<string> { "Thriller", "Crime", "Drama" },
                Plataforma = primeVideo
            },
            new Pelicula
            {
                IdImdb = "tt6751668",
                Titulo = "Parasite",
                Director = "Bong Joon Ho",
                FechaEstreno = new DateTime(2019, 5, 30),
                Duracion = 132,
                Generos = new List<string> { "Thriller", "Drama" },
                Plataforma = netflix
            },
            new Pelicula
            {
                IdImdb = "tt0353969",
                Titulo = "Memories of Murder",
                Director = "Bong Joon Ho",
                FechaEstreno = new DateTime(2003, 5, 2),
                Duracion = 131,
                Generos = new List<string> { "Crime", "Drama", "Thriller" },
                Plataforma = primeVideo
            },
            new Pelicula
            {
                IdImdb = "tt4925292",
                Titulo = "Lady Bird",
                Director = "Greta Gerwig",
                FechaEstreno = new DateTime(2017, 11, 3),
                Duracion = 94,
                Generos = new List<string> { "Drama", "Comedy" },
                Plataforma = netflix
            },
            new Pelicula
            {
                IdImdb = "tt1517268",
                Titulo = "Barbie",
                Director = "Greta Gerwig",
                FechaEstreno = new DateTime(2023, 7, 21),
                Duracion = 114,
                Generos = new List<string> { "Comedy", "Fantasy", "Adventure" },
                Plataforma = primeVideo
            },
            new Pelicula
            {
                IdImdb = "tt0335266",
                Titulo = "Lost in Translation",
                Director = "Sofia Coppola",
                FechaEstreno = new DateTime(2003, 10, 3),
                Duracion = 102,
                Generos = new List<string> { "Drama", "Comedy" },
                Plataforma = hboMax
            },
            new Pelicula
            {
                IdImdb = "tt9770150",
                Titulo = "Nomadland",
                Director = "Chloé Zhao",
                FechaEstreno = new DateTime(2021, 2, 19),
                Duracion = 108,
                Generos = new List<string> { "Drama" },
                Plataforma = netflix
            }
        });

        foreach (var pelicula in _peliculas)
        {
            pelicula.Plataforma.AddPelicula(pelicula);
        }

        _valoraciones.AddRange(new[]
        {
            new Valoracion { IdImdb = "tt1375666", Fuente = "IMDb",           Puntuacion = 8.8m, Votos = 2470000 },
            new Valoracion { IdImdb = "tt1375666", Fuente = "FilmAffinity",   Puntuacion = 8.2m, Votos = 185000  },
            new Valoracion { IdImdb = "tt1375666", Fuente = "Netflix",        Puntuacion = 8.5m, Votos = 95000   },

            new Valoracion { IdImdb = "tt0816692", Fuente = "IMDb",           Puntuacion = 8.7m, Votos = 2180000 },
            new Valoracion { IdImdb = "tt0816692", Fuente = "HBO Max",        Puntuacion = 8.6m, Votos = 110000  },

            new Valoracion { IdImdb = "tt2543164", Fuente = "IMDb",           Puntuacion = 8.1m, Votos = 820000  },
            new Valoracion { IdImdb = "tt2543164", Fuente = "HBO Max",        Puntuacion = 8.0m, Votos = 54000   },

            new Valoracion { IdImdb = "tt6751668", Fuente = "IMDb",           Puntuacion = 8.5m, Votos = 1050000 },
            new Valoracion { IdImdb = "tt6751668", Fuente = "RottenTomatoes", Puntuacion = 9.1m, Votos = 280000  },
            new Valoracion { IdImdb = "tt6751668", Fuente = "Netflix",        Puntuacion = 8.7m, Votos = 78000   },

            new Valoracion { IdImdb = "tt0353969", Fuente = "FilmAffinity",   Puntuacion = 8.0m, Votos = 41000   },
            new Valoracion { IdImdb = "tt0353969", Fuente = "Prime Video",    Puntuacion = 8.2m, Votos = 26000   },

            new Valoracion { IdImdb = "tt4925292", Fuente = "IMDb",           Puntuacion = 7.4m, Votos = 410000  },
            new Valoracion { IdImdb = "tt4925292", Fuente = "Netflix",        Puntuacion = 7.6m, Votos = 52000   },

            new Valoracion { IdImdb = "tt1517268", Fuente = "IMDb",           Puntuacion = 6.8m, Votos = 710000  },
            new Valoracion { IdImdb = "tt1517268", Fuente = "FilmAffinity",   Puntuacion = 6.1m, Votos = 87000   },
            new Valoracion { IdImdb = "tt1517268", Fuente = "Prime Video",    Puntuacion = 6.9m, Votos = 61000   },

            new Valoracion { IdImdb = "tt0335266", Fuente = "HBO Max",        Puntuacion = 7.8m, Votos = 34000   },

            new Valoracion { IdImdb = "tt9770150", Fuente = "IMDb",           Puntuacion = 7.3m, Votos = 235000  },
            new Valoracion { IdImdb = "tt9770150", Fuente = "FilmAffinity",   Puntuacion = 7.0m, Votos = 33000   }
        });
    }
}
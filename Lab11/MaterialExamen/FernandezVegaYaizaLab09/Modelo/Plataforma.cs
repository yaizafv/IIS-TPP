namespace tpp.lab09;

public class Plataforma
{
    private readonly List<Pelicula> _peliculas = new List<Pelicula>();

    public required string Nombre { get; init; }
    public IReadOnlyList<Pelicula> Peliculas { get { return _peliculas; } }  

    internal void AddPelicula(Pelicula pelicula)
    {
        _peliculas.Add(pelicula);
    }

    public override string ToString()
    {
        return $"Plataforma: {Nombre}";
    }
}

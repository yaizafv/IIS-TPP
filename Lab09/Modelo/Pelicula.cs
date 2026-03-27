namespace tpp.lab09;

public class Pelicula
{
    public required string IdImdb { get; init; }
    public required string Titulo { get; init; }
    public required string Director { get; init; }
    public DateTime FechaEstreno { get; init; }
    public int Duracion { get; init; }
    public required IReadOnlyList<string> Generos { get; init; }
    public required Plataforma Plataforma { get; init; }

    public override string ToString()
    {
        return $"{Titulo} ({FechaEstreno:d}). Dirección: {Director}. {Duracion} min.";
    }
}
namespace tpp.lab09;

public class Valoracion
{
    public required string IdImdb { get; init; }
    public required string Fuente { get; init; }
    public decimal Puntuacion { get; init; }
    public int Votos { get; init; }

    public override string ToString()
    {
        return $"Id: {IdImdb} tiene una puntuación de {Puntuacion} ({Votos} votos) en {Fuente}.";
    }
}

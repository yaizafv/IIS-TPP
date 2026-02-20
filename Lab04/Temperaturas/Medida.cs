namespace Temperaturas;

public class Medida
{
    public required object Valor { get; set; }
    public DateTime Fecha { get; set; }
}


/// <summary>
/// Mediante el uso de <T, S> indicamos que se emplean tipos genéricos.
/// En la línea anterior se declaran dos parámetros de tipo (marcadores de tipo).
/// Por convención, se usan letras como T ("Type") para nombrarlos.
/// En la clase MedidaGen usamos un único parámetro.
/// </summary>
/// <typeparam name="T">
/// El parámetro de tipo se reemplaza por un tipo concreto
///  cuando se usa la clase,veáse: MedidaGen<int> o MedidaGen<string>)
/// </typeparam>
public class MedidaGen<T>
{
    public required T Valor { get; set; }
    public DateTime Fecha { get; set; }
}

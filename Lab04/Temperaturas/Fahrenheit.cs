using System;

namespace Temperaturas;


/// <summary>
/// Tanto la clase Celsius como la clase Fahrenheit representan un diseño muy discutible
/// de comparación cruzada.
/// IComparable<T> normalmente se usa para comparar T con T.
/// Celsius no solamente se compara con Celsius, también con Fahrenhit (y al revés)
/// Permitir mezclas de tipos -> potencialmente casts -> errores en ejecucion.
/// </summary>
public class Fahrenheit : IComparable<Fahrenheit>, IComparable<Celsius>, IComparable
{
    public double Grados { get; }

    public Fahrenheit(double grados)
    {
        Grados = grados;
    }

    public override string ToString()
    {
        return $"{Grados} °F";
    }

    public int CompareTo(Fahrenheit? other)
    {
        if (other == null) return 1;
        return Grados.CompareTo(other.Grados);
    }

    public int CompareTo(Celsius? other)
    {
        if (other == null) return 1;
        double otherFahrenheit = (other.Grados * 9.0 / 5.0) + 32;
        return Grados.CompareTo(otherFahrenheit);
    }

    public int CompareTo(object? obj)
    {
        var fahrenheit = obj as Fahrenheit;
        if (fahrenheit != null) return CompareTo(fahrenheit);
        var celsius = obj as Celsius;
        if (celsius != null) return CompareTo(celsius);
        throw new ArgumentException("No se ha recibido un object de tipo Celsius o Fahrenheit.", nameof(obj));
    }
}

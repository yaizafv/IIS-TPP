using System;

namespace Temperaturas;


/// <summary>
/// Tanto la clase Celsius como la clase Fahrenheit representan un diseño muy discutible
/// de comparación cruzada.
/// IComparable<T> normalmente se usa para comparar T con T.
/// Celsius no solamente se compara con Celsius, también con Fahrenhit (y al revés)
/// Permitir mezclas de tipos -> potencialmente casts -> errores en ejecucion.
/// </summary>
public class Celsius : IComparable<Celsius>, IComparable<Fahrenheit>, IComparable
{
    public double Grados { get; }

    public Celsius(double grados)
    {
        Grados = grados;
    }

    public override string ToString()
    {
        return $"{Grados} °C";
    }

    public int CompareTo(Celsius? other)
    {
        if (other == null) return 1;
        return Grados.CompareTo(other.Grados);
    }

    public int CompareTo(Fahrenheit? other)
    {
        if (other == null) return 1;
        double otherCelsius = (other.Grados - 32) * 5.0 / 9.0;
        return Grados.CompareTo(otherCelsius);
    }

    public int CompareTo(object? obj)
    {
        var celsius = obj as Celsius;
        if (celsius != null) return CompareTo(celsius);
        var fahrenheit = obj as Fahrenheit;
        if (fahrenheit != null) return CompareTo(fahrenheit);
        throw new ArgumentException("No se ha recibido un object de tipo Celsius o Fahrenheit.", nameof(obj));
    }
}


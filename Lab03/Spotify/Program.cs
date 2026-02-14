namespace Spotify;

/// <summary>
/// Sobre la jerarquía y la clase Radio (¿es "descargable" la emisión de una Radio?)
/// Opción 1: fallo silencioso
/// Opción 2: excepción inesperada
/// Opción 3: cambio contrato
/// Opción 4: Parche compatibilidad 
/// Opción 5: explosión jerarquía
/// </summary>
class Program
{
    static void Main(string[] args)
    {
        //Ejecuta el código. Habilita la clase Radio y añádela a la biblioteca.
        var spotify = new Spotify();
        Audio[] biblioteca = new Audio[]
        {
            new Song("Canción 1"),
            new Podcast("Podcast 1"),
            new Radio("Radio 1"),
            new Song("Canción 2"),
            new Song("Canción 2"),
        };

        Console.WriteLine("Reproducir todos los audios:");
        spotify.PlayAll(biblioteca);
        Console.WriteLine("Me gustan, vamos a descargarlos todos:");
        spotify.DownloadAll(biblioteca);
        Console.WriteLine("Serializar todos los metadatos:");
        spotify.SerializeAll(biblioteca);
    }
}

class Spotify
{
    public void PlayAll(Audio[] library)
    {
        foreach (var audio in library)
        {
            audio.Play();
        }
    }

    public void DownloadAll(Audio[] library)
    {
        foreach (var audio in library)
        {
            audio.Download();
        }
    }

    public void SerializeAll(Audio[] library)
    {
        foreach (var audio in library)
        {
            Console.WriteLine(audio.Serialize());
        }
    }
}

abstract class Audio
{
    public abstract string Name { get; }

    public abstract void Play();
    public abstract void Download();

    public string Serialize()
    {
        return $"Metadatos serialziados: {Name}";
    }
}

class Song : Audio
{
    public Song(string name)
    {
        Name = name;
    }

    public override string Name { get; }

    public override void Play()
    {
        Console.WriteLine($"Reproduciendo canción '{Name}'...");
    }

    public override void Download()
    {
        Console.WriteLine($"Descargando canción'{Name}'...");
    }
}

class Podcast : Audio
{
    public Podcast(string name)
    {
        Name = name;
    }

    public override string Name { get; }

    public override void Play()
    {
        Console.WriteLine($"Reproduciendo podcast '{Name}'...");
    }

    public override void Download()
    {
        Console.WriteLine($"Descargando Podcast  podcast '{Name}'...");
    }
}


class Radio : Audio
{
    public Radio(string name)
    {
        Name = name;
    }

    public override string Name { get; }

    public override void Play()
    {
        Console.WriteLine($"Reproduciendo radio '{Name}'...");
    }

    public override void Download()
    {
        throw new NotSupportedException("La emisión de la radio no se puede descargar.");       // no se puede descargar porque es en directo
    }
}


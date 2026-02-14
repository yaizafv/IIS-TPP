namespace SpotifyEjercicio;

/// <summary>
/// Replantea código del proyecto Spotify empleando ISP.
/// </summary>
class Program
{
    static void Main()
    {

        IDownloadable[] downloadables = new IDownloadable();
        var spotify = new Spotify();
        var song1 = new Song("Canción 1");
        var song2 = new Song("Canción 2");
        var podcast1 = new Podcast("Podcast 1");
        var radio1 = new Radio("Radio 1");

        // Combinaciones de las anteriores en distintos arrays con distintos tipos...

    }

    class Spotify
    {
        public void PlayAll(IPlayable playables)
        {
            playables.Play();
        }

        public void DownloadAll(IDownloadable downloadables)
        {
            downloadables.Download();
        }

        public void SerializeAll(ISerializable serializables)
        {

            Console.WriteLine(serializables.Serialize());

        }
    }

    interface IDownloadable { void Download(); }
    interface IPlayable { void Play(); }
    interface ISerializable { string Serialize(); }

    class Song : IDownloadable, IPlayable, ISerializable
    {
        private string Name { get; }
        public void Download()
        {
            Console.WriteLine($"Descargando canción'{Name}'...");
        }

        public void Play()
        {
            Console.WriteLine($"Reproduciendo canción '{Name}'...");
        }

        public string Serialize()
        {
            return Name;
        }
    }

    class Audio : IDownloadable, IPlayable, ISerializable
    {
        private string Name { get; }

        public void Download()
        {
            Console.WriteLine($"Descargando canción'{Name}'...");
        }

        public void Play()
        {
            Console.WriteLine($"Reproduciendo canción '{Name}'...");
        }

        public string Serialize()
        {
            return Name;
        }
    }

    class Podcast : IDownloadable, IPlayable, ISerializable
    {
        private string Name { get; }
        public void Download()
        {
            Console.WriteLine($"Descargando canción'{Name}'...");
        }

        public void Play()
        {
            Console.WriteLine($"Reproduciendo canción '{Name}'...");
        }

        public string Serialize()
        {
            return Name;
        }
    }

    class Radio : IPlayable, ISerializable
    {
        private string Name { get; }

        public void Play()
        {
            Console.WriteLine($"Reproduciendo canción '{Name}'...");
        }

        public string Serialize()
        {
            return Name;
        }
    }

}


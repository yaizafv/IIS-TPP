namespace SpotifyEjercicio;

/// <summary>
/// Replantea código del proyecto Spotify empleando ISP.
/// </summary>
class Program
{
    static void Main()
    {
        var spotify = new Spotify();
        var song1 = new Song("Canción 1");
        var song2 = new Song("Canción 2");
        var podcast1 = new Podcast("Podcast 1");
        var radio1 = new Radio("Radio 1");

        // Combinaciones de las anteriores en distintos arrays con distintos tipos...
        IPlayable[] playables = { song1, song2, podcast1, radio1 };
        IDownloadable[] downloadables = { song1, song2, podcast1 };
        ISerializable[] serializables = { song1, song2, podcast1, radio1 };

        spotify.PlayAll(playables);
        spotify.DownloadAll(downloadables);
        spotify.SerializeAll(serializables);
    }

    class Spotify
    {
        public void PlayAll(IEnumerable<IPlayable> playables)
        {
            foreach (var item in playables)
            {
                item.Play();
            }
        }

        public void DownloadAll(IEnumerable<IDownloadable> downloadables)
        {
            foreach (var item in downloadables)
            {
                item.Download();
            }
        }

        public void SerializeAll(IEnumerable<ISerializable> serializables)
        {
            foreach (var item in serializables)
            {
                Console.WriteLine(item.Serialize());
            }
        }
    }

    interface IDownloadable { void Download(); }
    interface IPlayable { void Play(); }
    interface ISerializable { string Serialize(); }

    class Song : IDownloadable, IPlayable, ISerializable
    {
        private string Name { get; }

        public Song(string name)
        {
            Name = name;
        }
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
            Console.WriteLine($"Metadatos serializados: {Name}");
            return Name;
        }
    }

    class Podcast : IDownloadable, IPlayable, ISerializable
    {
        private string Name { get; }

        public Podcast(string name)
        {
            Name = name;
        }
        public void Download()
        {
            Console.WriteLine($"Descargando podcast '{Name}'...");
        }

        public void Play()
        {
            Console.WriteLine($"Reproduciendo podcast '{Name}'...");
        }

        public string Serialize()
        {
            Console.WriteLine($"Metadatos serializados: {Name}");
            return Name;
        }
    }

    class Radio : IPlayable, ISerializable
    {
        private string Name { get; }

        public Radio(string name)
        {
            Name = name;
        }

        public void Play()
        {
            Console.WriteLine($"Reproduciendo radio '{Name}'...");
        }

        public string Serialize()
        {
            Console.WriteLine($"Metadatos serializados: {Name}");
            return Name;
        }
    }

}


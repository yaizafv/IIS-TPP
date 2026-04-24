using System.Diagnostics;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.ColorSpaces;
using SixLabors.ImageSharp.PixelFormats;

namespace DatosFor;

class Program
{
    static void Main(string[] args)
    {
        string dirImagenes = Path.GetFullPath("../../../../pics");
        string dirDestino = Path.GetFullPath("../../../../pics/efectos");

        Directory.CreateDirectory(dirDestino);

        // Prueba combinaciones de "tejon" y "zorro" con efectos PixelSepia y PixelAplicarGamma
        // La ganancia depende del tamaño de entrada (de la imagen) y de la complejidad de las operaciones (sepia mas simple que gamma)

        string nombreFichero = "tejon";
        Func<Rgba32, Rgba32> efecto = PixelSepia;
        string nombreEfecto = efecto == PixelAplicarGamma ? "sepia" : "gamma";
        string fichero = Path.Combine(dirImagenes, $"{nombreFichero}.jpg");


        if (args.Length > 0 && args[0] == "p")
            ForParalelo(fichero, Path.Combine(dirDestino, $"{nombreFichero}_{nombreEfecto}.jpg"), efecto);
        else
            ForSecuencial(fichero, Path.Combine(dirDestino, $"{nombreFichero}_{nombreEfecto}.jpg"), efecto);

    }


    static void ForSecuencial(string ficheroOri, string ficheroDest, Func<Rgba32, Rgba32> transformar)
    {
        Stopwatch sw = Stopwatch.StartNew();

        using Image<Rgba32> imgOri = Image.Load<Rgba32>(ficheroOri);
        using Image<Rgba32> imgDest = new Image<Rgba32>(imgOri.Width, imgOri.Height);

        for (int y = 0; y < imgOri.Height; y++)
        {
            for (int x = 0; x < imgOri.Width; x++)
            {
                Rgba32 pixel = imgOri[x, y];

                // Aplicamos al píxel la transformación correspondiente.
                imgDest[x, y] = transformar(pixel);
            }
        }
        imgDest.Save(ficheroDest);
        sw.Stop();
        Console.WriteLine($"[For Secuencial] Tiempo: {sw.ElapsedMilliseconds} ms -> {ficheroDest} .");
    }


    // Task Parallel Library (TPL)
    // Uso del For
    static void ForParalelo(string ficheroOrigen, string ficheroDest, Func<Rgba32, Rgba32> transformar)
    {

        Stopwatch sw = Stopwatch.StartNew();
        using Image<Rgba32> imgOri = Image.Load<Rgba32>(ficheroOrigen);
        using Image<Rgba32> imgDest = new Image<Rgba32>(imgOri.Width, imgOri.Height);

        Parallel.For(0, imgOri.Height, y =>         //solo se paraleliza un bucle. se paraleliza el for externo CASI siempre (osea siempre xd)
        {
            //AQUI SI XD
            for (int x = 0; x < imgOri.Width; x++)
            {
                Rgba32 p = imgOri[x, y];
                // Aplicamos al pixel la transformación correspondiente.
                imgDest[x, y] = transformar(p);
                //NO CONTAR LOS HILOS AQUI DENTRO
            }
            //AQUI TAMBIEN SE PUEDE
        });
        imgDest.Save(ficheroDest);
        sw.Stop();
        Console.WriteLine($"[For TPL] Tiempo: {sw.ElapsedMilliseconds} ms -> {ficheroDest}.");
        // Ejercicio: Implementa el código necesario para saber cuantos hilos utiliza TPL en esta operación.

    }




    static Rgba32 PixelSepia(Rgba32 pixel)
    {
        // https://www.w3.org/TR/filter-effects-1/#sepiaEquivalent
        byte r = (byte)Math.Clamp(0.393 * pixel.R + 0.769 * pixel.G + 0.189 * pixel.B, 0, 255);
        byte g = (byte)Math.Clamp(0.349 * pixel.R + 0.686 * pixel.G + 0.168 * pixel.B, 0, 255);
        byte b = (byte)Math.Clamp(0.272 * pixel.R + 0.534 * pixel.G + 0.131 * pixel.B, 0, 255);
        return new Rgba32(r, g, b, pixel.A);
    }

    static Rgba32 PixelAplicarGamma(Rgba32 pixel)
    {
        // Aclaramos (en mayor medida) los medios tonos y las sombras.
        double gamma = 2.2;
        double rojoNormalizado = pixel.R / 255.0;
        double verdeNormalizado = pixel.G / 255.0;
        double azulNormalizado = pixel.B / 255.0;

        byte rojo = (byte)Math.Round(Math.Pow(rojoNormalizado, 1.0 / gamma) * 255);
        byte verde = (byte)Math.Round(Math.Pow(verdeNormalizado, 1.0 / gamma) * 255);
        byte azul = (byte)Math.Round(Math.Pow(azulNormalizado, 1.0 / gamma) * 255);

        return new Rgba32(rojo, verde, azul, pixel.A);
    }

    [Conditional("DEBUG")]
    static void Imprimir(string texto)
    {
        Console.WriteLine(texto);
    }

    //Elegir mecanismo mas optimo en el examen. Datos -> ForEach For, Tareas -> Invoke

}

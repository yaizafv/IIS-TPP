using System.Diagnostics;

namespace Thunks;

class Program
{
    static void Main()
    {
        var carpeta = "../../../tmp";
        var extensionVieja = ".txt";
        var extensionNueva = ".bak";


        // Renombrar ficheros V1
        try
        {
            List<string> ficherosCreados = CrearTemporales(carpeta, 10);
            RenombrarFicherosV1(carpeta, extensionVieja, extensionNueva);
            Console.WriteLine("RenombrarFicherosV1 completado con éxito.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"RenombrarFicherosV1 falló: {ex.Message}");
            // Si aquí ocurre un error, esto queda en un estado inconsistente (la operación queda a medias), con algunos archivos renombrados y otros no.
        }


        // Renombrar ficheros V2 (Testing)
        try
        {
            List<string> ficherosCreados = CrearTemporales(carpeta, 10);
            List<(string src, string dst)> ficherosMovidos = new List<(string src, string dst)>();
            RenombrarFicherosV2(carpeta, extensionVieja, extensionNueva,
                (src, dst) =>
                {
                    ficherosMovidos.Add((src, dst));
                },
                fichero => DateTime.UtcNow,
                fichero => DateTime.UtcNow,
                (fichero, fecha) => { },
                (fichero, fecha) => { }
            );
            Debug.Assert(ficherosMovidos.Count == ficherosCreados.Count);
            Debug.Assert(ficherosMovidos.All(m => ficherosCreados.Contains(m.src)));
            Debug.Assert(ficherosMovidos.All(m => Path.GetExtension(m.dst) == extensionNueva));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"RenombrarFicherosV2 (Testing) falló: {ex.Message}");
            // En este caso, si ocurre un error, no se ha modificado el estado del sistema, por lo que no queda en un estado inconsistente.
        }

        // Renombrar ficheros V2 (Producción)
        try
        {
            List<string> ficherosCreados = CrearTemporales(carpeta, 10);
            RenombrarFicherosV2(carpeta, extensionVieja, extensionNueva,
                File.Move,
                File.GetCreationTimeUtc,
                File.GetLastWriteTimeUtc,
                File.SetCreationTimeUtc,
                File.SetLastWriteTimeUtc);
            Console.WriteLine("RenombrarFicherosV2 (Producción) completado con éxito.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"RenombrarFicherosV2 (Producción) falló: {ex.Message}");
            // Si aquí ocurre un error, esto queda en un estado inconsistente, con algunos archivos renombrados y otros no.
        }



        // PlanRenombrarFicherosV3

        IEnumerable<Action>? actions = null;
        try
        {
            actions = PlanRenombrarFicherosV3(carpeta, extensionVieja, extensionNueva);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"PlanRenombrarFicherosV3 falló en la planificación: {ex.Message}");
            // En este caso, si ocurre un error, no se ha modificado el estado del sistema, por lo que no queda en un estado inconsistente.
        }

        // Ahora que ya sabemos que todas las acciones son válidas, las ejecutamos.
        try
        {
            foreach (var action in actions ?? Enumerable.Empty<Action>())
            {
                action();
            }
            Console.WriteLine("PlanRenombrarFicherosV3 completado con éxito.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"PlanRenombrarFicherosV3 falló en la ejecución: {ex.Message}");
        }


        Directory.Delete(carpeta, recursive: true);
    }

    // ¿Qué ocurre si hay un error en mitad del proceso?
    static void RenombrarFicherosV1(string carpeta, string extensionVieja, string extensionNueva)
    {
        foreach (var fichero in Directory.GetFiles(carpeta, $"*{extensionVieja}"))
        {
            //Comprobación
            var ficheroNuevo = Path.ChangeExtension(fichero, extensionNueva);           //devuelve el string de la ruta nueva (no hace el cambio)
            if (ficheroNuevo == null) { throw new InvalidOperationException($"Fichero {fichero} no tiene extensión."); }
            if (ficheroNuevo == fichero) { throw new InvalidOperationException($"Fichero {fichero} ya tiene la extensión {extensionNueva}."); }
            if (File.Exists(ficheroNuevo)) { throw new InvalidOperationException($"Fichero {ficheroNuevo} ya existe."); }

            // Captura metadatos
            var creado = File.GetCreationTimeUtc(fichero);
            var modificado = File.GetLastWriteTimeUtc(fichero);

            // Mueve el archivo
            File.Move(fichero, ficheroNuevo);       //hace el cambio

            // Restaura metadatos
            File.SetCreationTimeUtc(ficheroNuevo, creado);
            File.SetLastWriteTimeUtc(ficheroNuevo, modificado);
        }
    }

    // ¿Qué enfoque se está siguiendo aquí?
    // ¿Qué ocurre con el código?
    // ¿Soluciona el problema de la versión anterior?
    static void RenombrarFicherosV2(string carpeta, string extensionVieja, string extensionNueva,
        Action<string, string> moverFichero,
        Func<string, DateTime> getCreationTimeUtc,
        Func<string, DateTime> getLastWriteTimeUtc,
        Action<string, DateTime> setCreationTimeUtc,
        Action<string, DateTime> setLastWriteTimeUtc)
    {
        foreach (var fichero in Directory.GetFiles(carpeta, $"*{extensionVieja}"))
        {
            var ficheroNuevo = Path.ChangeExtension(fichero, extensionNueva);
            if (ficheroNuevo == null) { throw new InvalidOperationException($"Fichero {fichero} no tiene extensión."); }
            if (ficheroNuevo == fichero) { throw new InvalidOperationException($"Fichero {fichero} ya tiene la extensión {extensionNueva}."); }
            if (File.Exists(ficheroNuevo)) { throw new InvalidOperationException($"Fichero {ficheroNuevo} ya existe."); }

            // Captura metadatos
            var creado = getCreationTimeUtc(fichero);
            var modificado = getLastWriteTimeUtc(fichero);

            // Mueve el archivo
            moverFichero(fichero, ficheroNuevo);

            // Restaura metadatos
            setCreationTimeUtc(ficheroNuevo, creado);
            setLastWriteTimeUtc(ficheroNuevo, modificado);
        }
    }

    // Planificación vs Ejecución. Acciones diferidas.
    static IEnumerable<Action> PlanRenombrarFicherosV3(string carpeta, string extensionVieja, string extensionNueva)
    {
        List<Action> actions = new List<Action>();
        foreach (var fichero in Directory.GetFiles(carpeta, $"*{extensionVieja}"))
        {
            var ficheroActual = fichero; // Captura la variable local para evitar problemas con clausuras en versiones anteriores a C# 5.
            // Comprobaciones y ChangeExtension
            var ficheroNuevo = Path.ChangeExtension(fichero, extensionNueva);
            if (ficheroActual == null) { throw new InvalidOperationException($"Fichero {ficheroActual} no tiene extensión."); }
            if (ficheroActual == fichero) { throw new InvalidOperationException($"Fichero {ficheroActual} ya tiene la extensión {extensionNueva}."); }
            if (File.Exists(ficheroActual)) { throw new InvalidOperationException($"Fichero {ficheroActual} ya existe."); }

            // En cada iteración añadimos un action que: capture metadatos, mueva el fichero y restaure metadatos
            Action renombrar = () =>
            {
                // Captura metadatos
                var creado = File.GetCreationTimeUtc(ficheroActual);
                var modificado = File.GetLastWriteTimeUtc(ficheroActual);

                // Mueve el archivo
                File.Move(ficheroActual, ficheroNuevo);       //hace el cambio

                // Restaura metadatos
                File.SetCreationTimeUtc(ficheroNuevo, creado);
                File.SetLastWriteTimeUtc(ficheroNuevo, modificado);
            };
            actions.Add(renombrar);
        }
        return actions;
    }

    static List<string> CrearTemporales(string carpeta, uint numFicheros = 5)
    {
        if (Directory.Exists(carpeta))
        {
            Directory.Delete(carpeta, recursive: true);
        }
        Directory.CreateDirectory(carpeta);

        var createdFiles = new List<string>();
        for (int i = 0; i < numFicheros; i++)
        {
            var filePath = Path.Combine(carpeta, $"fichero{i}.txt");
            File.WriteAllText(filePath, $"Este es el fichero {i}");
            createdFiles.Add(filePath);
        }
        return createdFiles;
    }
}

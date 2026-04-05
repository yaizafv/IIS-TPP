namespace For;

public class Program
{

    static void Main()
    {
        int i = -10;
        Action initialize = () => i = 0;
        Func<bool> condition = () => i < 5;
        Action iteration = () => i = i + 1;
        Action body = () => Console.WriteLine($"Iteración ForLoop: {i}");

        Console.WriteLine("Ejecutando ForLoop:");
        ForLoopV2(initialize, condition, iteration, body);
    }
    public static void ForLoop(Action initialize, Func<bool> condition, Action iteration, Action body)
    {
        initialize();
        RecursiveStep(condition, iteration, body);
    }

    private static void RecursiveStep(Func<bool> condition, Action iteration, Action body)
    {
        if (condition())
        {
            body();
            iteration();
            RecursiveStep(condition, iteration, body);
        }
    }

    public static void ForLoopV2(Action initialize, Func<bool> condition, Action iteration, Action body)    //la forma buena es esta
    {
        initialize();
        Action For = null;
        For = () =>
        {
            if (!condition())
                return;
            body();
            iteration();
            For();
        };
        For();
    }

    public static void WhileLoop(Func<bool> condition, Action body)
    {
        Action While = null;
        While = () =>
        {
            if (!condition()) return;
            body();
            While();
        };
        While();
    }

    public static void DoWhileLoop(Func<bool> condition, Action body)
    {
        Action DoWhile = null;
        DoWhile = () =>
        {
            body();
            if (!condition()) return;
            DoWhile();
        };
        DoWhile();
    }

    public static void SwitchLoop<T>(T valor, Dictionary<T, Action> casos, Action defaultCase = null)
    {
        // 1. Buscamos si el valor existe en nuestro "diccionario de casos"
        if (casos.ContainsKey(valor))
        {
            // 2. Si existe, ejecutamos la acción asociada
            casos[valor]();
        }
        // 3. Si no existe y tenemos un caso por defecto, lo ejecutamos
        else if (defaultCase != null)
        {
            defaultCase();
        }
    }
}


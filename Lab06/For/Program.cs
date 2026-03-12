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
        ForLoop(initialize, condition, iteration, body);
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
}


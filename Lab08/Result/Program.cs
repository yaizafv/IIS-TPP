namespace tpp.lab08;


// Una operación puede resultar en un éxito o en un fallo.
public interface Result<TSuccess, TFailure>
{
    // Ejecuta un Action distinto según haya éxito o fallo.
    void Match(Action<TSuccess> onSuccess, Action<TFailure> onFailure);

    // Con Func
    TResult Match<TResult>(Func<TSuccess, TResult> onSuccess, Func<TFailure, TResult> onFailure);

    // En este ejemplo, trataremos AndThen como un map: transforma TSuccess en TResult.
    // En algunos lenguajes, AndThen se asocia con bind o flatMap, donde la función devuelve un Result.
    // En ese caso, al devolver un Result en lugar de un TResult, podríamos encadenar operaciones que también devuelvan Result.
    Result<TResult, TFailure> AndThen<TResult>(Func<TSuccess, TResult> f);


    // Devuelve el valor de éxito o, si hubo fallo, un valor por defecto.
    TSuccess OrElse(TSuccess defaultValue);
}

// Gsetión del éxito
public record Success<TSuccess, TFailure>(TSuccess Value) : Result<TSuccess, TFailure>
{
    // Ejecuta la acción de éxito.
    public void Match(Action<TSuccess> onSuccess, Action<TFailure> onFailure) => onSuccess(Value);

    // Aplica la función de éxito y devuelve su resultado.
    public TResult Match<TResult>(Func<TSuccess, TResult> onSuccess, Func<TFailure, TResult> onFailure) => onSuccess(Value);

    // Traansforma el valor y lo vuelve a envolver en Success.
    public Result<TResult, TFailure> AndThen<TResult>(Func<TSuccess, TResult> f) => new Success<TResult, TFailure>(f(Value));

    // Ignora el valor por defecto y devuelve el valor real.
    public TSuccess OrElse(TSuccess defaultValue) => Value;
}

// Exactamente igual pero adaptado al error.
public record Failure<TSuccess, TFailure>(TFailure Error) : Result<TSuccess, TFailure>
{
    public void Match(Action<TSuccess> onSuccess, Action<TFailure> onFailure) => onFailure(Error);
    public TResult Match<TResult>(Func<TSuccess, TResult> onSuccess, Func<TFailure, TResult> onFailure) => onFailure(Error);
    public Result<TResult, TFailure> AndThen<TResult>(Func<TSuccess, TResult> f) => new Failure<TResult, TFailure>(Error);
    public TSuccess OrElse(TSuccess defaultValue) => defaultValue;
}


// Ejemplos de ueso
static class Program
{
    static void Main(string[] args)
    {
        EjemploDivisionSegura();
        EjemploParsingSeguro();
    }


    // División segura
    private static void EjemploDivisionSegura()
    {
        Result<int, string> resultado1 = DivisionSeguraDouble(10, 2)
            .AndThen(quotient => (int) Math.Round(quotient, MidpointRounding.AwayFromZero));        //andThen funciona como un map
        string resultado = resultado1.Match(success => $"Success: {success}",
            error => $"Error: {error}");
        Console.WriteLine($"Resultado de la división segura: {resultado}");

        Result<int, string> resultado2 = DivisionSeguraDouble(10, 0)
            .AndThen(cociente => (int) Math.Round(cociente, MidpointRounding.AwayFromZero));
        Console.WriteLine($"Resultado de la división segura: {resultado2.Match(
            success => $"Success: {success}",
            error => $"Error: {error}"
        )}");

        double resultado3 = DivisionSeguraDouble(10, 0)
            .OrElse(double.NaN);
        Console.WriteLine($"Resultado de la división segura con OrElse: {resultado3}");
    }

    private static Result<double, string> DivisionSeguraDouble(double dividendo, double divisor)
    {
        if (divisor == 0)
            return new Failure<double, string>("División por cero.");
        else
            return new Success<double, string>(dividendo / divisor);
    }

    private static void EjemploParsingSeguro()
    {
        Result<int, string> resultado = SecureParseInt("123")
            .AndThen(value => value * 2);

        Console.WriteLine($"Resultado del parsing: {resultado.Match(
            success => $"Success: {success}",
            error => $"Error: {error}"
        )}");

        Result<int, string> result2 = SecureParseInt("abc")
            .AndThen(v => v * 2);

        Console.WriteLine($"Resultado del parsing: {result2.Match(
            success => $"Success: {success}",
            error => $"Error: {error}"
        )}");
    }

    private static Result<int, string> SecureParseInt(string entrada)
    {
        try
        {
            int valor = int.Parse(entrada);
            return new Success<int, string>(valor);
        }
        catch (FormatException)
        {
            return new Failure<int, string>($"'{entrada}' no es un entero.");
        }
    }
}

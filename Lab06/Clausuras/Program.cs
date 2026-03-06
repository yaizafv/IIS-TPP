namespace lab06;

class Program
{
    static void Main()
    {
        EjemploClausurasEncapsulacionEstado();
    }

    private static void EjemploClausurasEncapsulacionEstado()
    {
        Func<decimal,decimal> depositar = Cuenta(1000m);
        Console.WriteLine($"Depositar 100: {depositar(100m)}");
    }

    //clausura - funcion que almacena su estado o contexto 
    static Func<decimal, decimal> Cuenta(decimal inicial)       //no es una clausura
    {
        decimal balance = inicial; //variable local que será capturada ¿Por qué?
        string hola = "asd";
        decimal depositar(decimal cantidad)     //clausura definida por balance. para que depositar exista necesita balance
        {
            Console.WriteLine(hola);        //ahora hola tambien es una clausura. si no se usa dentro de depositar no es clausura
            if (cantidad <= 0)
            {
                throw new ArgumentException("La cantidad a depositar debe ser positiva");
            }
            balance += cantidad; 
            return balance;
        }
        // Al devolver el delegado, el estado capturado sigue vivo mientras exista esta referencia.
        // En este caso, el estado capturado lo define exclusivamente la variable 'balance'.
        return depositar;
    }

    // EJERCICIO:
    // Imagina que, dentro de Cuenta, quieres devolver dos clausuras para una misma cuenta:
    //  - Una clausura para depositar.
    //  - Otra clausura para extraer.
    // Ambas deben trabajar sobre el mismo estado capturado. Impleméntalo

    //Examen: interfaz ienumerator generica empleando el concepto de clausuras. excepto el dispose. todo dentro de una funcion que devuelve una tupla de 3 (moveNext, current, reset)
    //movenext : () => pos = -1;
}

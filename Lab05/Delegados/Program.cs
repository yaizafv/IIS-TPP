namespace Delegados;

class Program
{
    
    // Definimos un tipo delegado (delegate) que representa cualquier método compatible
    // con la firma: recibe 1 int y devuelve un int.
    // Lo tipos delegados son nominales.
    // ¿Qué posibilitan los tipos delegados?
    public delegate int Function(int n1); 

    //¿Cómo se definiría el delegado para el método Concatenar, Multiplica y Media?
    public delegate string OpString(string a1, string a2);
    public delegate int OpEntero(int a1, int a2);
    
    public delegate T opBinaria<T> (T a1, T a2);    //mierda de solucion

    static void Main()
    {
        // El método Cuadrado es compatible con el delegado Function
        Function cuadrado = Cuadrado;   //funcion que recibe un entero y devuelve un entero
        Func<int, int> cuadrado2 = Cuadrado;    //devuelve un int y no recibe nada
        Action<string> toLog = Console.WriteLine;
        Console.WriteLine(cuadrado(5));     
        toLog("Hola");
        // Func admite de 0 a 16 parámetros de entrada y siempre devuelve un valor (TResult).
        // Predicate admite único parámetro y siempre devuelve bool.
        // Action admite de 0 a 16 parámetros de entrada y el retorno es void.
    }
    static int Cuadrado(int a)
    {
        return a * a;
    }

    static string Concatenar(string a, string b)
    {
        return a+b;
    }

    static int Multiplicar(int n1, int n2)
    {
        return n1*n2;
    }

    static double Media(int a, int b)
    {
        return (a + b) / 2.0;
    }



}

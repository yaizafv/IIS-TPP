namespace MasterWorkerEj;

class Program
{
    // A través de 2 arrays de enteros (el tamaño del 2º es <= al del 1º)
    // Calcular el número de ocurrencias del 2º array en el primero.
    // Suponer que tendrá un máximo de 30 hilos
    // Ejemplo:
    // { 2, 2, 1, 3, 2, 2, 1, 2, 1, 2, 2, 1 } y { 2, 2, 1}
    // Resultado: 3

    static void Main()
    {
        //short[] v1 = new short[] { 2, 2, 1, 3, 2, 2, 1, 2, 1, 2, 2, 1 };
        //short[] v2 = new short[] { 2, 2, 1 };

        //Probarlo posteriormente con dos aleatorios.
        //short[] v1 = CrearVectorAleatorio(1000, 0, 4);
        //short[] v2 = CrearVectorAleatorio(2, 0, 4);

    }

    public static short[] CrearVectorAleatorio(int numElementos, short menor, short mayor)
    {
        short[] vector = new short[numElementos];
        Random random = new Random();
        for (int i = 0; i < numElementos; i++)
            vector[i] = (short)random.Next(menor, mayor + 1);
        return vector;
    }
}

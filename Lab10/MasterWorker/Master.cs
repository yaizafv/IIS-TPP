using System;

namespace MasterWorker;

/// <summary>
/// Calcula el módulo de un vector de manera concurrente.
///     RaizCuadrada(x1^2 + x2^2 + x3^2 + ... + xn^2);
/// Genera y coordina un conjunto de hilos trabajadores (Workers)
/// que realizarán el cálculo.
/// 
/// </summary>
public class Master
{
    /// <summary>
    /// Vector del que se obtendrá el módulo
    /// </summary>
    private short[] vector;

    /// <summary>
    /// Número de trabajadores que se van a emplear en el cálculo.
    /// </summary>
    private int numeroHilos;


    public Master(short[] vector, int numeroHilos)
    {
        if (numeroHilos < 1 || numeroHilos > vector.Length)
            throw new ArgumentException("El número de hilos debe ser menor o igual al tamaño del vector");
        this.vector = vector;
        this.numeroHilos = numeroHilos;
    }

    /// <summary>
    /// Este método crea y coordina el cálculo
    /// </summary>
    public double CalcularModulo()
    {
        // Creamos los workers
        Worker[] workers = new Worker[this.numeroHilos];
        int numElementosPorHilo = this.vector.Length / numeroHilos;
        for (int i = 0; i < this.numeroHilos; i++)
        {
            int indiceDesde = i * numElementosPorHilo;
            int indiceHasta = (i + 1) * numElementosPorHilo - 1;        //cada worker trabaja con una parte del vector
            if (i == this.numeroHilos - 1) //el último hilo, llega hasta el final del vector.
            {
                indiceHasta = this.vector.Length - 1;
            }
            workers[i] = new Worker(this.vector, indiceDesde, indiceHasta);
        }
        // Iniciamos hilos.
        Thread[] hilos = new Thread[workers.Length];
        for (int i = 0; i < workers.Length; i++)
        {
            hilos[i] = new Thread(workers[i].Calcular);
            hilos[i].Name = "Worker número: " + (i + 1);
            hilos[i].Priority = ThreadPriority.Normal;
            hilos[i].Start();
        }

        foreach (Thread thread in hilos)
            thread.Join();          //condicion de carrera

        // Recolectamos de los trabajadores.
        long resultado = 0;
        foreach (Worker worker in workers)
            resultado += worker.Resultado;
        return Math.Sqrt(resultado);
    }
}
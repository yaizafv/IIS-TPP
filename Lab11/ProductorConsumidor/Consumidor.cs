using System;

namespace ProductorConsumidor;

public class Consumidor
{
    private Queue<TareaImpresion> _cola;

    public Consumidor(Queue<TareaImpresion> cola)
    {
        this._cola = cola;
    }

    public void Run()
    {
        Random random = new Random();

        while (true)
        {
            TareaImpresion tarea;

            lock (_cola)
            {
                if (_cola.Count == 0)
                    Thread.Sleep(100);

                Console.WriteLine("- Sacando tarea...");
                tarea = _cola.Dequeue();
                Console.WriteLine($"- Tarea obtenida: {tarea.TareaId}. En cola: {_cola.Count}");
                int hojasImpresas = tarea.Imprimir();
                Console.WriteLine($"Se han impreso {hojasImpresas} hojas.");
            }

            Thread.Sleep(random.Next(300, 700));
        }
    }
}



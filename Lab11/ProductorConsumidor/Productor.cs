using System;

namespace ProductorConsumidor;

class Productor
{
    private Queue<TareaImpresion> cola;
    private int capacidadMaxima;
    private int numeroTareasCreadas;

    public Productor(Queue<TareaImpresion> cola, int capacidadMaxima)
    {
        this.cola = cola;
        this.capacidadMaxima = capacidadMaxima;
    }

    public void Run()
    {
        Random random = new Random();

        while (true)
        {
            int id = ++numeroTareasCreadas;
            string extension = random.Next(2) == 0 ? ".docx" : ".pdf";

            TareaImpresion tarea = new TareaImpresion(
                tareaId: id,
                nombreFichero: $"fichero{id}{extension}",
                numPaginas: random.Next(1, 21),
                numCopias: random.Next(1, 4),
                dobleCara: random.Next(2) == 0
            );

            lock (cola)
            {
                if (cola.Count == capacidadMaxima)
                    Thread.Sleep(100);

                Console.WriteLine($"+ Insertando {tarea.TareaId}...");
                cola.Enqueue(tarea);
                Console.WriteLine($"+ {tarea} insertada. En cola: {cola.Count}");
            }

            Thread.Sleep(random.Next(500, 1000));
        }
    }
}


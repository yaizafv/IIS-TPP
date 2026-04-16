namespace DeadlockFix;

public class Program
{

    public static void Main()
    {
        decimal cantidadInicial = 30000;
        Cuenta cuentaA = new Cuenta("A", cantidadInicial), cuentaB = new Cuenta("B", cantidadInicial);

        Random random = new Random();
        int iteraciones = 10;
        Thread[] hilos = new Thread[iteraciones];

        //Mitad de transferencias (una por hilo) serán A->B y la otra mitad B->A con cantidades aleatorias.
        for (int i = 0; i < iteraciones; i++)
        {
            decimal cantidad = Math.Round((decimal)(random.NextDouble() * random.Next(1, 600)), 2);
            if (i % 2 == 0)
                hilos[i] = new Thread(() => Transferir(cuentaA, cuentaB, cantidad));
            else
                hilos[i] = new Thread(() => Transferir(cuentaB, cuentaA, cantidad));
            hilos[i].Name = "Num. transferencia: " + i;
        }

        foreach (Thread hilo in hilos)
            hilo.Start();

        //EJERCICIO:
        // En este proyecto se resuelve el problema del interbloqueo.
        // Sin embargo, la transferencia ya no es atómica:
        //      Hemos protegido Extraer e Ingresar por separado.
        //      Entre ambas operaciones, sigue existiendo un estado intermedio.
        //
        // Propuesta:
        //      Bloquear las cuentas de la transferencia siguiendo un criterio global fijo.
        //
        // Antes:
        //      A -> B y B -> A. (primero bloqueábamos origen y luego destino).
        //      Pero origen podría ser A o B y destino, la alternativa.
        //
        // Ahora:
        //      Bien reciba A -> B o B -> A, siempre se bloquearía primero la misma cuenta.
        //      lock (primera) { lock (segunda) { .... }}  
        //
        // ¿Qué tienen en común todas las cuentas que nos permita 
        // decidir siempre cuál debe bloquearse antes?
    }

    private static void Transferir(Cuenta cuentaA, Cuenta cuentaB, decimal cantidad)
    {
        Console.WriteLine($"Transfiriendo {cantidad} euros de la cuenta {cuentaA.NumCuenta} a la cuenta {cuentaB.NumCuenta}...");
        if (cuentaA.Transferir(cuentaB, cantidad))
            Console.WriteLine($"\tTrasferencia con éxito, hilo: {Thread.CurrentThread.Name}.");
        else
            Console.WriteLine($"\tTransferencia errónea, hilo: {Thread.CurrentThread.Name}.");
    }
}

//EXAMEN: codigo roto y arreglarlo


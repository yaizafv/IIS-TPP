using System;

namespace Deadlock;

public class Cuenta
{
    private decimal _balance;
    private string _numCuenta;

    public Cuenta(string numCuenta, decimal balance = 0)
    {
        _balance = balance;
        _numCuenta = numCuenta;
    }

    public string NumCuenta { get { return _numCuenta; } }

    /// <summary>
    /// Extraer dinero de la cuenta
    /// <param name="cantidad">Cantidad de dinero a extraer</param>
    /// <returns>Si se ha extraído la cantidad de dinero o no.</returns>
    /// </summary>
    public bool Extraer(decimal cantidad)
    {
        if (_balance < cantidad)
            return false;
        _balance -= cantidad;
        return true;
    }

    /// <summary>
    /// Ingresa dinero en la cuenta
    /// <param name="cantidad">Cantidad de dinero a ingresar</param>
    /// </summary>
    public void Ingresar(decimal cantidad)
    {
        _balance += cantidad;
    }


    /// <summary>
    /// Transfiere dinero de la cuenta actual (this) a la cuenta pasada como parámetro.
    /// <param name="destino">Cuenta a la que se va a realizar la transferencia</param>
    /// <param name="cantidad">Cantidad de dinero a transferir</param>
    /// <returns>Si la transferencia se ha realizado con éxito o no.</returns>
    /// </summary>
    public bool Transferir(Cuenta destino, decimal cantidad)
    {
        lock (this) // this no suele ser una buena idea porque se puede bloquear desde fuera de la clase.        this será A o B
        {
            lock (destino)      // A o B
            {
                Thread.Sleep(100); // Simulamos procesamiento.
                if (this.Extraer(cantidad))
                {
                    destino.Ingresar(cantidad);
                    return true;
                }
                else
                    return false;
            }
        }
    }
}

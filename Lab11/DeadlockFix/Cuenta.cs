using System;

namespace DeadlockFix;

 public class Cuenta
    {
        private readonly object _object = new object();
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
            lock (_object)      // al estar bloqueando con el mismo objeto no se puede extraer e ingresar a la vez
            {
                if (_balance < cantidad)    // el if tiene que estar dentro del lock porque balance es un recurso que se modifica concurrentemente
                    return false;
                _balance -= cantidad;
                return true;
            }
        }

        /// <summary>
        /// Ingresa dinero en la cuenta
        /// <param name="cantidad">Cantidad de dinero a ingresar</param>
        /// </summary>
        public void Ingresar(decimal cantidad)
        {
            lock (_object)
                this._balance += cantidad;      //balance es el recurso compartido
        }


        /// <summary>
        /// Transfiere dinero de la cuenta actual (this) a la cuenta pasada como parámetro.
        /// <param name="destino">Cuenta a la que se va a realizar la transferencia</param>
        /// <param name="cantidad">Cantidad de dinero a transferir</param>
        /// <returns>Si la transferencia se ha realizado con éxito o no.</returns>
        /// </summary>
        public bool Transferir(Cuenta destino, decimal cantidad)
        {
            Thread.Sleep(100); // Simulamos procesamiento.
            // problema: no todos los hilos se ejecutan a la vez. Solucion: coger los locks en un orden predeterminado
            if (this.Extraer(cantidad))
            {
                destino.Ingresar(cantidad);
                return true;
            }
            else
                return false;
        }
    }


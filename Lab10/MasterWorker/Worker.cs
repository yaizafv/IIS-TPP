namespace MasterWorker;

    public class Worker
    {
        /// <summary>
        /// Vector del que vamos a obtener el módulo
        /// </summary>
        private short[] _vector;

        /// <summary>
        /// Índices que indican el rango de elementos del vector 
        /// con el que vamos a trabajar.
        /// En el intervalo se incluyen ambos índices.
        /// </summary>
        private int _indiceDesde, _indiceHasta;

        /// <summary>
        /// Resultado del cálculo
        /// </summary>
        private long _resultado;

        internal long Resultado { get { return _resultado; } }

        internal Worker(short[] vector, int indiceDesde, int indiceHasta)
        {
            _vector = vector;
            _indiceDesde = indiceDesde;
            _indiceHasta = indiceHasta;
        }

        /// <summary>
        /// Método que realiza el cálculo
        /// </summary>
        internal void Calcular()
        {
            _resultado = 0;
            for (int i = _indiceDesde; i <= _indiceHasta; i++)
                _resultado += _vector[i] * _vector[i];
        }

    }

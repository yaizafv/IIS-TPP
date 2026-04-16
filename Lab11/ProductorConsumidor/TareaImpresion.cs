using System;

namespace ProductorConsumidor;

public class TareaImpresion
{
    public int TareaId { get;}
    public string NombreFichero { get;}
    public int NumPaginas { get;}
    public int NumCopias { get;}
    public bool DobleCara { get; }

    public TareaImpresion(int tareaId, string nombreFichero, int numPaginas, int numCopias, bool dobleCara)
    {
        NombreFichero = nombreFichero;
        TareaId = tareaId;
        NumPaginas = numPaginas;
        NumCopias = numCopias;
        DobleCara = dobleCara;
    }

    public int Imprimir()
    {
        int hojasPorCopia = DobleCara? (NumPaginas + 1) / 2 : NumPaginas;
        return hojasPorCopia * NumCopias;
    }

    public override string ToString()
    {
        string impDobleCara = DobleCara ? "doble cara" : "una cara";
        return $"{NombreFichero}, {NumPaginas} páginas, {NumCopias} copias, {impDobleCara}";
    }
}

using System;

namespace activity10;

public class BitcoinWorker
{
    private BitcoinValueData[] data;
    private int inicio;
    private int fin;
    private double threshold;
    private int result;

    internal int Result => this.result;

    internal BitcoinWorker(BitcoinValueData[] data, int inicio, int fin, double threshold)
    {
        this.data = data;
        this.inicio = inicio;
        this.fin = fin;
        this.threshold = threshold;
    }

    public void Compute()
    {
        this.result = 0;
        for (int i = inicio; i <= fin; i++)
        {
            if (data[i].Value >= threshold)
            {
                result++;
            }
        }
    }

}

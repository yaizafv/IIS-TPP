using System;

namespace MasterWorkerEj;

public class Worker
{
    private short[] vector1;
    private short[] vector2;
    private int fromIndex;
    private int toIndex;
    private int result;

    internal int Result
    {
        get { return this.result; }
    }

    public Worker(short[] vector1, short[] vector2, int fromIndex, int toIndex)
    {
        this.vector1 = vector1;
        this.vector2 = vector2;
        this.fromIndex = fromIndex;
        this.toIndex = toIndex;
    }

    internal void Coincidencias()
    {
        for (int i = fromIndex; i <= toIndex; i++)
        {
            bool match = true;
            for (int j = 0; j < vector2.Length; j++)
            {
                if (vector1[i + j] != vector2[j])
                {
                    match = false;
                    break;
                }
            }
            if (match)
                result++;
        }
    }



}

using System;

namespace activity10;
internal class BitcoinValueData
{
    public DateTime Timestamp { get; set; }
    public double Value { get; set; }

    public override string ToString()
    {
        return Timestamp + ": " + Value;
    }
}

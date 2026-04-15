namespace activity10;

class Program
{
    static void Main(string[] args)
    {
        var data = activity10.Utils.GetBitcoinData();
        foreach (var d in data)
            Console.WriteLine(d);
    }
}   


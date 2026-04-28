namespace Tarea;
class Program
{
    static void Main(string[] args)
    {
        string[] productNames = { "Laptop", "Mouse", "Keyboard", "Sticker" };
        decimal[] productPrices = { 800m, 25m, 50m, 0m };

        decimal budget = 1000m;

        RunCase(productNames, productPrices, budget, "Laptop");
        RunCase(productNames, productPrices, budget, "Sticker");
        RunCase(productNames, productPrices, budget, "Monitor");
    }

    private static void RunCase(string[] names, decimal[] prices, decimal budget, string product)
    {
        Console.WriteLine($"Caso: producto = {product}");

        try
        {
            decimal price = Store.GetPrice(names, prices, product);
            decimal units = Store.UnitsYouCanBuy(budget, price);

            Console.WriteLine($"  Precio: {price}");
            Console.WriteLine($"  Unidades que puedes comprar con {budget}: {units}");
        }
        catch (InvalidOperationException)
        {
            Console.WriteLine("  Error: el producto no existe.");
        }
        catch (DivideByZeroException)
        {
            Console.WriteLine("  Error: el precio es 0, no se puede dividir.");
        }

        Console.WriteLine();
    }
}

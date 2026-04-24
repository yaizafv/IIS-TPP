using System;

namespace Tarea;

public static class Store
{
    public static decimal GetPrice(string[] names, decimal[] prices, string product)
    {
        if (names == null || prices == null)
            throw new ArgumentNullException("Names and prices cannot be null.");

        if (names.Length != prices.Length)
            throw new ArgumentException("Names and prices must have the same length.");

        for (int i = 0; i < names.Length; i++)
        {
            if (names[i] == product)
                return prices[i];
        }

        throw new InvalidOperationException("Product not found.");
    }

    public static decimal UnitsYouCanBuy(decimal budget, decimal price)
    {
        if (price == 0)
            throw new DivideByZeroException();

        return budget / price;
    }
}

using System;

namespace homework;

public static class Store
{
    public static Maybe<decimal> GetPrice(string[] names, decimal[] prices, string product)
    {
        for (int i = 0; i < names.Length; i++)
        {
            if (names[i] == product)
                return new Some<decimal>(prices[i]);
        }
        return new None<decimal>();
    }

    public static Result<decimal> UnitsYouCanBuy(decimal budget, decimal price)
    {
        if (price == 0)
            return Result<decimal>.Failure("el precio es 0, no se puede dividir.");
        return Result<decimal>.Success(budget / price);
    }
}

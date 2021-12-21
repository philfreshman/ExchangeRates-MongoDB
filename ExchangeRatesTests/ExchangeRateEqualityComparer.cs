using ExchangeRates.Models;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ExchangeRatesTests;
public class ExchangeRateEqualityComparer : IEqualityComparer<ExchangeRate>
{
    public bool Equals(ExchangeRate? x, ExchangeRate? y)
    {
        if (x == null || y == null)
            return false;

        if (x.Date == y.Date && x.Rate == y.Rate)
            return true;
        
        return false;
    }

    public int GetHashCode([DisallowNull] ExchangeRate obj)
    {
        return base.GetHashCode();
    }
}
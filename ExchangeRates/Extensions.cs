namespace ExchangeRates;

public static class Extensions
{
    public static DateTime Truncate(this DateTime dateTime)
    {
        return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
    }
}
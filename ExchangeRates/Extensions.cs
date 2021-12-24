namespace ExchangeRates;

public static class Extensions
{
    public static DateTime Truncate(this DateTime dateTime)
    {
        return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
    }
    
    
    public static void Deconstruct<T>(this T[] srcArray, out T a0, out T a1)
    {
        if (srcArray == null || srcArray.Length < 2)
            throw new ArgumentException(nameof(srcArray));

        a0 = srcArray[0];
        a1 = srcArray[1];
    }
}
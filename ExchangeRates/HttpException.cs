using System.Net;

namespace ExchangeRates;
public class HttpException : Exception
{
    public HttpStatusCode StatusCode { get; private set; }

    public HttpException(HttpStatusCode statusCode)
    {
        StatusCode = statusCode;
    }

    public HttpException(HttpStatusCode statusCode, string message) : base(message)
    {
        StatusCode = statusCode;
    }

    public HttpException(HttpStatusCode statusCode, string message, Exception innerException) : base(message, innerException)
    {
        StatusCode = statusCode;
    }
}

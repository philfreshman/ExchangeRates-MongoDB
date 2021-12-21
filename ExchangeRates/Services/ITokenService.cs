namespace ExchangeRates.Services;
public interface ITokenService
{
    /// <summary>
    /// List of active access tokens that should be allowed to make requests
    /// </summary>
    IList<string> Tokens { get; }

    /// <summary>
    /// Checks user credentials and generate a random access token
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    string? GenerateToken(string? username, string? password);
}


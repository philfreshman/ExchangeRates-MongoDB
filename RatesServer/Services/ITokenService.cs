namespace ExchangeRates.Services;
public interface ITokenService
{
    /// <summary>
    /// List of active access tokens that should be allowed to make requests.
    /// </summary>
    IList<string> Tokens { get; }

    /// <summary>
    /// Generates new token.
    /// </summary>
    string GenerateNewToken();
    
    /// <summary>
    /// Checks if provided user credentials are correct.
    /// </summary>
    /// <param Username="username"></param>
    /// <param Password="password"></param>
    /// <returns></returns>
    bool CheckCredentials(string username, string password);
    
    /// <summary>
    /// Checks if any Basic-Auth input was provided.
    /// </summary>
    bool IsRequestEmptyOrNull (string? AuthInput);

}
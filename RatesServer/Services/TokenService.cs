namespace ExchangeRates.Services;

public class TokenService : ITokenService
{
    
    private readonly IDictionary<string, string> Users = new Dictionary<string, string>
    {
        {"philip", "password"}
    };
        
    /// <inheritdoc/>
    public IList<string> Tokens { get; set; } = new List<string>
    {
        "T4ykNM7MvgxluJRMcFniawOiuhOUDd2fniFs5VPwHLemjxHA1sPJUnMREifYy9BL"
    };

    ///<inheritdoc/>
    public string GenerateNewToken()
    {
        const string allowedCharacters = "abcdefghijklmnopqrstuwvxyzABCDEFGHIJKLMNOPQRSTUWVXYZ0123456789";
        var random = new Random();
        var token = new string(Enumerable.Repeat(allowedCharacters, 64).Select(s => s[random.Next(s.Length)]).ToArray());
        Tokens.Add(token);
        return token;
    }

    public bool CheckCredentials(string username, string password)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            return true;

        var storedPassword = Users[username];
        if (storedPassword == null || storedPassword != password)
            return true;

        return false;
    }
    
    
    public bool IsRequestEmptyOrNull(string? AuthInput)
    {
        if (AuthInput is "Basic Og==" or null)
        {
            return true;
        }
        return false;
    }
}
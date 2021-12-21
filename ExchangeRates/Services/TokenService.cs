namespace ExchangeRates.Services;
public class TokenService : ITokenService
{
    private readonly IDictionary<string, string> _users = new Dictionary<string, string>
    {
        { "philip", "password" }
    };

    /// <inheritdoc/>
    public IList<string> Tokens { get; } = new List<string>();

    public TokenService(IConfiguration configuration)
    {
        Tokens.Add(configuration.GetValue<string>("DefaultApiKey"));
    }

    /// <inheritdoc/>
    public string? GenerateToken(string? username, string? password)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            return null;

        var storedPassword = _users[username];
        if (storedPassword == null || storedPassword != password)
            return null;

        const string allowedCharacters = "abcdefghijklmnopqrstuwvxyzABCDEFGHIJKLMNOPQRSTUWVXYZ0123456789";
        var random = new Random();
        var token = new string(Enumerable.Repeat(allowedCharacters, 64).Select(s => s[random.Next(s.Length)]).ToArray());
        Tokens.Add(token);
        return token;
    }
}


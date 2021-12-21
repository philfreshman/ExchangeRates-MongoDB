using ExchangeRates.Models;
using ExchangeRates.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Local;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient<IRatesService>();
builder.Services.AddSingleton<ICacheService, CacheService>();
builder.Services.AddSingleton<ITokenService, TokenService>();
builder.Services.AddScoped<IRatesService, RatesService>();
builder.Services.Configure<ExchangeRatesDatabaseSettings>(builder.Configuration.GetSection("ExchangeRatesDatabase"));


var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

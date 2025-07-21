using System.Text.Json;

namespace Predictor.Web.Services;

public interface ICurrencyService
{
    Task<decimal> GetExchangeRateAsync(string fromCurrency, string toCurrency);
}

public class CurrencyService(HttpClient httpClient) : ICurrencyService
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<decimal> GetExchangeRateAsync(string fromCurrency, string toCurrency)
    {
        if (fromCurrency.Equals(toCurrency, StringComparison.OrdinalIgnoreCase))
        {
            return 1.0M;
        }

        string url = $"https://open.er-api.com/v6/latest/{fromCurrency}"; // todo: get from config

        var response = await this._httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            return 1.0M;
        }

        using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        if (doc.RootElement.TryGetProperty("rates", out var rates) &&
            rates.TryGetProperty(toCurrency.Trim().ToUpper(), out var rateEl) &&
            rateEl.TryGetDecimal(out var rate))
        {
            return rate;
        }

        return 1.0M;
    }
}

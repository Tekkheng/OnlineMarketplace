using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace CartService.Services;

public class MidtransService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _config;

    public MidtransService(IHttpClientFactory httpClientFactory, IConfiguration config)
    {
        _httpClientFactory = httpClientFactory;
        _config = config;
    }

    public async Task<string?> CreateTransactionAsync(string orderId, decimal amount, string customerEmail)
    {
        var client = _httpClientFactory.CreateClient();
        var serverKey = _config["Midtrans:ServerKey"];
        var apiUrl = _config["Midtrans:ApiUrl"];

        var authToken = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{serverKey}:"));
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);

        long grossAmount = (long)Math.Round(amount, MidpointRounding.AwayFromZero);

        var payload = new
        {
            transaction_details = new
            {
                order_id = orderId,
                gross_amount = grossAmount,
            },
            customer_details = new
            {
                email = customerEmail
            }
        };

        var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
        var response = await client.PostAsync(apiUrl, content);

        if (!response.IsSuccessStatusCode) return null;

        var body = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(body);

        return doc.RootElement.GetProperty("redirect_url").GetString();
    }
}

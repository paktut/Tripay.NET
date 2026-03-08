namespace Tripay.NET.Configuration;

public class TripayConfig
{
    public string ApiKey { get; set; } = string.Empty;
    public string PrivateKey { get; set; } = string.Empty;
    public string MerchantCode { get; set; } = string.Empty;
    public bool IsProduction { get; set; } = false;

    public string BaseUrl => IsProduction 
        ? "https://tripay.co.id/api/" 
        : "https://tripay.co.id/api-sandbox/";
}

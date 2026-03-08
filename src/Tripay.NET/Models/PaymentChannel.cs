using System.Text.Json.Serialization;

namespace Tripay.NET.Models;

public class PaymentChannel
{
    [JsonPropertyName("code")]
    public string Code { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("group")]
    public string Group { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("fee_merchant")]
    public Fee FeeMerchant { get; set; } = new();

    [JsonPropertyName("fee_customer")]
    public Fee FeeCustomer { get; set; } = new();

    [JsonPropertyName("total_fee")]
    public Fee TotalFee { get; set; } = new();

    [JsonPropertyName("minimum_fee")]
    public decimal? MinimumFee { get; set; }

    [JsonPropertyName("maximum_fee")]
    public decimal? MaximumFee { get; set; }

    [JsonPropertyName("icon_url")]
    public string IconUrl { get; set; } = string.Empty;

    [JsonPropertyName("active")]
    public bool Active { get; set; }
}

public class Fee
{
    [JsonPropertyName("flat")]
    public decimal Flat { get; set; }

    [JsonPropertyName("percent")]
    public decimal Percent { get; set; }
}

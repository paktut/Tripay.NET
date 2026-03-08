using System.Text.Json.Serialization;

namespace Tripay.NET.Models;

public class TransactionRequest
{
    [JsonPropertyName("method")]
    public string Method { get; set; } = string.Empty;

    [JsonPropertyName("merchant_ref")]
    public string MerchantRef { get; set; } = string.Empty;

    [JsonPropertyName("amount")]
    public long Amount { get; set; }

    [JsonPropertyName("customer_name")]
    public string CustomerName { get; set; } = string.Empty;

    [JsonPropertyName("customer_email")]
    public string CustomerEmail { get; set; } = string.Empty;

    [JsonPropertyName("customer_phone")]
    public string? CustomerPhone { get; set; }

    [JsonPropertyName("order_items")]
    public List<OrderItem> OrderItems { get; set; } = new();

    [JsonPropertyName("callback_url")]
    public string? CallbackUrl { get; set; }

    [JsonPropertyName("return_url")]
    public string? ReturnUrl { get; set; }

    [JsonPropertyName("expired_time")]
    public long? ExpiredTime { get; set; } // Unix timestamp

    [JsonPropertyName("signature")]
    public string Signature { get; set; } = string.Empty;
}

public class OrderItem
{
    [JsonPropertyName("sku")]
    public string? Sku { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("price")]
    public long Price { get; set; }

    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }

    [JsonPropertyName("subtotal")]
    public long? Subtotal { get; set; }
}

public class TransactionDetail
{
    [JsonPropertyName("reference")]
    public string Reference { get; set; } = string.Empty;

    [JsonPropertyName("merchant_ref")]
    public string MerchantRef { get; set; } = string.Empty;

    [JsonPropertyName("payment_selection_type")]
    public string PaymentSelectionType { get; set; } = string.Empty;

    [JsonPropertyName("payment_method")]
    public string PaymentMethod { get; set; } = string.Empty;

    [JsonPropertyName("payment_name")]
    public string PaymentName { get; set; } = string.Empty;

    [JsonPropertyName("customer_name")]
    public string CustomerName { get; set; } = string.Empty;

    [JsonPropertyName("customer_email")]
    public string CustomerEmail { get; set; } = string.Empty;

    [JsonPropertyName("customer_phone")]
    public string? CustomerPhone { get; set; }

    [JsonPropertyName("callback_url")]
    public string? CallbackUrl { get; set; }

    [JsonPropertyName("return_url")]
    public string? ReturnUrl { get; set; }

    [JsonPropertyName("amount")]
    public long Amount { get; set; }

    [JsonPropertyName("fee_merchant")]
    public long FeeMerchant { get; set; }

    [JsonPropertyName("fee_customer")]
    public long FeeCustomer { get; set; }

    [JsonPropertyName("total_fee")]
    public long TotalFee { get; set; }

    [JsonPropertyName("amount_received")]
    public long AmountReceived { get; set; }

    [JsonPropertyName("pay_code")]
    public string? PayCode { get; set; }

    [JsonPropertyName("pay_url")]
    public string? PayUrl { get; set; }

    [JsonPropertyName("checkout_url")]
    public string? CheckoutUrl { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("expired_time")]
    public long ExpiredTime { get; set; }

    [JsonPropertyName("order_items")]
    public List<OrderItem> OrderItems { get; set; } = new();

    [JsonPropertyName("instructions")]
    public List<Instruction>? Instructions { get; set; }
    
    [JsonPropertyName("qr_string")]
    public string? QrString { get; set; }
    
    [JsonPropertyName("qr_url")]
    public string? QrUrl { get; set; }
}

public class Instruction
{
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("steps")]
    public List<string> Steps { get; set; } = new();
}

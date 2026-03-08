using System.Net.Http.Json;
using System.Text.Json;
using Tripay.NET.Configuration;
using Tripay.NET.Models;

namespace Tripay.NET.Services;

public interface ITripayClient
{
    Task<TripayResponse<List<PaymentChannel>>> GetPaymentChannelsAsync(string? code = null);
    Task<TripayResponse<TransactionDetail>> CreateTransactionAsync(TransactionRequest request);
    Task<TripayResponse<TransactionDetail>> CreateOpenTransactionAsync(TransactionRequest request);
    Task<TripayResponse<TransactionDetail>> GetTransactionDetailAsync(string reference);
    Task<TripayResponse<TransactionDetail>> GetOpenTransactionDetailAsync(string uuid);
}

public class TripayClient : ITripayClient
{
    private readonly HttpClient _httpClient;
    private readonly TripayConfig _config;

    public TripayClient(HttpClient httpClient, TripayConfig config)
    {
        _httpClient = httpClient;
        _config = config;

        _httpClient.BaseAddress = new Uri(_config.BaseUrl);
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _config.ApiKey);
    }

    public async Task<TripayResponse<List<PaymentChannel>>> GetPaymentChannelsAsync(string? code = null)
    {
        var url = "merchant/payment-channel";
        if (!string.IsNullOrEmpty(code))
        {
            url += $"?code={code}";
        }

        var response = await _httpClient.GetAsync(url);
        return await HandleResponse<List<PaymentChannel>>(response);
    }

    public async Task<TripayResponse<TransactionDetail>> CreateTransactionAsync(TransactionRequest request)
    {
        if (string.IsNullOrEmpty(request.Signature))
        {
            request.Signature = TripaySignature.Generate(
                _config.PrivateKey, 
                _config.MerchantCode, 
                request.MerchantRef, 
                request.Amount
            );
        }

        var response = await _httpClient.PostAsJsonAsync("transaction/create", request);
        return await HandleResponse<TransactionDetail>(response);
    }

    public async Task<TripayResponse<TransactionDetail>> CreateOpenTransactionAsync(TransactionRequest request)
    {
        if (string.IsNullOrEmpty(request.Signature))
        {
            request.Signature = TripaySignature.Generate(
                _config.PrivateKey, 
                _config.MerchantCode, 
                request.Method,
                request.MerchantRef
            );
        }

        var response = await _httpClient.PostAsJsonAsync("transaction/open-payment/create", request);
        return await HandleResponse<TransactionDetail>(response);
    }

    public async Task<TripayResponse<TransactionDetail>> GetTransactionDetailAsync(string reference)
    {
        var url = $"transaction/detail?reference={reference}";
        var response = await _httpClient.GetAsync(url);
        return await HandleResponse<TransactionDetail>(response);
    }

    public async Task<TripayResponse<TransactionDetail>> GetOpenTransactionDetailAsync(string uuid)
    {
        var url = $"open-payment/{uuid}/detail";
        var response = await _httpClient.GetAsync(url);
        return await HandleResponse<TransactionDetail>(response);
    }

    private async Task<TripayResponse<T>> HandleResponse<T>(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            try
            {
                var errorResponse = JsonSerializer.Deserialize<TripayErrorResponse>(errorContent);
                return new TripayResponse<T>
                {
                    Success = false,
                    Message = errorResponse?.Message ?? "Unknown error occurred.",
                    Data = default
                };
            }
            catch
            {
                return new TripayResponse<T>
                {
                    Success = false,
                    Message = $"HTTP Error {response.StatusCode}: {errorContent}",
                    Data = default
                };
            }
        }

        var content = await response.Content.ReadFromJsonAsync<TripayResponse<T>>();
        if (content == null)
        {
             return new TripayResponse<T>
            {
                Success = false,
                Message = "Empty response from server",
                Data = default
            };
        }

        return content;
    }
}

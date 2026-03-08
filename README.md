# Tripay.NET

An unofficial, modern, and easy-to-use .NET SDK for integrating with the [Tripay Payment Gateway](https://tripay.co.id/). This library is designed for .NET developers, providing a strongly-typed client to interact with the Tripay API, including support for dependency injection, automatic signature generation, and robust error handling.

## Features

-   **.NET 9 Support**: Built on the latest .NET platform.
-   **Strongly-Typed**: Provides clear and predictable models for all API requests and responses.
-   **Dependency Injection Ready**: Easily integrate into your ASP.NET Core application with a single line of code.
-   **Automatic Signature Generation**: Automatically handles HMAC-SHA256 signature creation for secure API calls.
-   **Full API Coverage**: Supports Closed and Open Payment transactions, payment channel inquiries, and transaction detail retrieval.
-   **Async-First**: All API calls are asynchronous, following modern .NET best practices.

## Installation

Install the package via NuGet:

```bash
dotnet add package Tripay.NET
```

## Getting Started

### 1. Configuration

First, register the `TripayClient` in your application's service container. In an ASP.NET Core application, you would do this in your `Program.cs` file.

```csharp
// In Program.cs
using Tripay.NET;

var builder = WebApplication.CreateBuilder(args);

// Add Tripay SDK services
builder.Services.AddTripay(config =>
{
    config.ApiKey = builder.Configuration["Tripay:ApiKey"];
    config.PrivateKey = builder.Configuration["Tripay:PrivateKey"];
    config.MerchantCode = builder.Configuration["Tripay:MerchantCode"];
    config.IsProduction = false; // Set to true for production environment
});

// ... other services
```

It's recommended to store your credentials in `appsettings.json` or another secure configuration source:

```json
// In appsettings.json
{
  "Tripay": {
    "ApiKey": "YOUR_API_KEY",
    "PrivateKey": "YOUR_PRIVATE_KEY",
    "MerchantCode": "YOUR_MERCHANT_CODE"
  }
}
```

### 2. Usage

Inject the `ITripayClient` interface into your controllers or services to start making API calls.

#### Get Payment Channels

Retrieve a list of all available payment channels.

```csharp
public class PaymentController : ControllerBase
{
    private readonly ITripayClient _tripayClient;

    public PaymentController(ITripayClient tripayClient)
    {
        _tripayClient = tripayClient;
    }

    [HttpGet("channels")]
    public async Task<IActionResult> GetChannels()
    {
        var response = await _tripayClient.GetPaymentChannelsAsync();
        if (response.Success)
        {
            return Ok(response.Data);
        }
        return BadRequest(response.Message);
    }
}
```

#### Create a Closed Payment Transaction

Create a standard transaction where the amount is fixed.

```csharp
[HttpPost("create-transaction")]
public async Task<IActionResult> CreateTransaction()
{
    var request = new TransactionRequest
    {
        Method = "BRIVA", // Example payment channel
        MerchantRef = "INV-" + Guid.NewGuid().ToString("N"),
        Amount = 50000,
        CustomerName = "John Doe",
        CustomerEmail = "john.doe@example.com",
        CustomerPhone = "081234567890",
        OrderItems = new List<OrderItem>
        {
            new OrderItem { Sku = "PRODUCT-01", Name = "My Awesome Product", Price = 50000, Quantity = 1 }
        },
        ReturnUrl = "https://your-domain.com/payment/success",
        ExpiredTime = new DateTimeOffset(DateTime.Now.AddHours(24)).ToUnixTimeSeconds()
    };

    // The signature will be generated automatically by the client.
    var response = await _tripayClient.CreateTransactionAsync(request);

    if (response.Success)
    {
        // Return the checkout URL or payment code to the user
        return Ok(response.Data);
    }

    return BadRequest(response.Message);
}
```

#### Create an Open Payment Transaction

Create a transaction where the customer can input the payment amount (e.g., for donations or top-ups).

```csharp
[HttpPost("create-open-transaction")]
public async Task<IActionResult> CreateOpenTransaction()
{
    var request = new TransactionRequest
    {
        Method = "BCAVA", // An Open Payment channel
        MerchantRef = "TOPUP-" + Guid.NewGuid().ToString("N"),
        CustomerName = "Jane Doe"
        // Amount is not specified for open payments
    };

    // The signature is generated automatically using the correct formula for open payments.
    var response = await _tripayClient.CreateOpenTransactionAsync(request);

    if (response.Success)
    {
        // Return the payment code (VA number) to the user
        return Ok(response.Data);
    }

    return BadRequest(response.Message);
}
```

#### Get Transaction Details

Check the status of a transaction using its reference.

```csharp
[HttpGet("transaction/{reference}")]
public async Task<IActionResult> GetTransactionDetail(string reference)
{
    var response = await _tripayClient.GetTransactionDetailAsync(reference);

    if (response.Success)
    {
        // response.Data contains the full transaction details, including status.
        return Ok(response.Data);
    }

    return NotFound(response.Message);
}
```

## License

This project is licensed under the MIT License.

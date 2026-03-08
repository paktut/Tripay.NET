using Tripay.NET.Services;

namespace Tripay.NET.Tests;

public class TripaySignatureTests
{
    [Fact]
    public void Generate_ClosedPayment_ReturnsCorrectSignature()
    {
        // Arrange
        // Example from Tripay docs/snippets
        var privateKey = "ytf6ooi2gmlNPfpchd94jDOk8hRWOu";
        var merchantCode = "T0001";
        var merchantRef = "INV55567";
        var amount = 1500000;
        
        // Expected: 9f167eba844d1fcb369404e2bda53702e2f78f7aa12e91da6715414e65b8c86a
        var expected = "9f167eba844d1fcb369404e2bda53702e2f78f7aa12e91da6715414e65b8c86a";

        // Act
        var result = TripaySignature.Generate(privateKey, merchantCode, merchantRef, amount);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Generate_OpenPayment_ReturnsCorrectSignature()
    {
        // Arrange
        var privateKey = "private_key_anda";
        var merchantCode = "T0001";
        var merchantRef = "INV55567";
        var channel = "BCAVA";
        
        // We don't have the exact expected hash for this generic example without calculating it manually or trusting a tool.
        // But we can verify the logic matches standard HMAC-SHA256
        
        var payload = merchantCode + channel + merchantRef;
        using var hmac = new System.Security.Cryptography.HMACSHA256(System.Text.Encoding.UTF8.GetBytes(privateKey));
        var hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(payload));
        var expected = BitConverter.ToString(hash).Replace("-", "").ToLower();

        // Act
        var result = TripaySignature.Generate(privateKey, merchantCode, channel, merchantRef);

        // Assert
        Assert.Equal(expected, result);
    }
}

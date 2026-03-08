using System.Security.Cryptography;
using System.Text;

namespace Tripay.NET.Services;

public static class TripaySignature
{
    public static string Generate(string privateKey, string merchantCode, string merchantRef, long amount)
    {
        var payload = $"{merchantCode}{merchantRef}{amount}";
        return ComputeHmacSha256(privateKey, payload);
    }

    public static string Generate(string privateKey, string merchantCode, string method, string merchantRef)
    {
        var payload = $"{merchantCode}{method}{merchantRef}";
        return ComputeHmacSha256(privateKey, payload);
    }

    private static string ComputeHmacSha256(string key, string data)
    {
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
        return BitConverter.ToString(hash).Replace("-", "").ToLower();
    }
}

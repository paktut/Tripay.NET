using Microsoft.Extensions.DependencyInjection;
using Tripay.NET.Configuration;
using Tripay.NET.Services;

namespace Tripay.NET;

public static class TripayServiceExtensions
{
    public static IServiceCollection AddTripay(this IServiceCollection services, Action<TripayConfig> configure)
    {
        var config = new TripayConfig();
        configure(config);

        services.AddSingleton(config);
        services.AddHttpClient<ITripayClient, TripayClient>();

        return services;
    }
}

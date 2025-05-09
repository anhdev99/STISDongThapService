using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Shared.Extensions;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddConfigSection<TInterface, TImplementation>(
        this IServiceCollection services,
        IConfiguration configuration,
        string sectionName)
        where TImplementation : class, TInterface
        where TInterface : class
    {
        var section = configuration.GetSection(sectionName);

        services.Configure<TImplementation>(section);
        services.AddSingleton(sp => sp.GetRequiredService<IOptions<TImplementation>>().Value);
        services.AddSingleton<TInterface>(sp => sp.GetRequiredService<TImplementation>());
        return services;
    }
}
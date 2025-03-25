using Microsoft.Extensions.DependencyInjection;

namespace ImageManager.Services.ImageManagement;

/// <summary>
/// Provides extension methods for registering image management services in the dependency injection container.
/// </summary>
public static class Bootstrapper
{
    /// <summary>
    /// Registers the image management service implementation in the service collection.
    /// </summary>
    /// <param name="services">The service collection to add the image management service to.</param>
    /// <returns>The modified service collection with image management services registered.</returns>
    public static IServiceCollection AddImageManagement(this IServiceCollection services)
    {
        return services.AddSingleton<IImageManagement, ImageManagement>();
    }
}
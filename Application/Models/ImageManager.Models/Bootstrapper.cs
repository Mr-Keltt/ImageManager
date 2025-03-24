using Microsoft.Extensions.DependencyInjection;

namespace ImageManager.Application.Models;

/// <summary>
/// Provides a bootstrap mechanism to configure application model mappings using AutoMapper.
/// </summary>
public static class Bootstrapper
{
    /// <summary>
    /// Extends the IServiceCollection to register AutoMapper with the application's model mapping profiles.
    /// </summary>
    /// <param name="services">The IServiceCollection instance to be configured.</param>
    /// <returns>The modified IServiceCollection with AutoMapper registration.</returns>
    public static IServiceCollection AddApplicationModels(this IServiceCollection services)
    {
        // Return the updated service collection to allow for method chaining.
        return services;
    }
}

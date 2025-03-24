using ImageManager.Services.Settings;
using ImageManager.Services.Logger;
using ImageManager.API.Configuration;
using ImageManager.Application.Models;
using ImageManager.Services.ImageManagement;

namespace ImageManager.API;



public static class Bootstrapper
{
    /// <summary>
    /// Registers application settings, logging, repository, game service, AutoMapper profiles, and other related services
    /// with the dependency injection container.
    /// </summary>
    /// <param name="service">The service collection to which the services and models will be added.</param>
    /// <returns>
    /// The updated service collection with all required services, models, and configurations registered.
    /// </returns>
    public static IServiceCollection RegisterServicesAndModels(this IServiceCollection service)
    {
        service
                .AddMainSettings()
                .AddSwaggerSettings()
                .AddLogSettings()
                .AddAppLogger()
                .AddAppAutoMappers()
                .AddApplicationModels()
                .AddImageManagement();

        return service;
    }
}

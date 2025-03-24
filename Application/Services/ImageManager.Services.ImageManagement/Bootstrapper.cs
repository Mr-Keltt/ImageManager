using Microsoft.Extensions.DependencyInjection;

namespace ImageManager.Services.ImageManagement;

public static class Bootstrapper
{

    public static IServiceCollection AddImageManagement(this IServiceCollection services)
    {

        return services.AddSingleton<IImageManagement, ImageManagement>();
    }
}

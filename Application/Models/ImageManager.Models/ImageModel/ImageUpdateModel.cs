using AutoMapper;
using ImageManager.Entities;

namespace ImageManager.Models.ImageModel;

/// <summary>
/// Represents the data transfer object for updating an existing image entry.
/// </summary>
public class ImageUpdateModel
{
    /// <summary>
    /// Gets or sets the updated name identifier for the image.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the updated binary data representing the image content.
    /// </summary>
    public byte[] Data { get; set; }
}

/// <summary>
/// Provides AutoMapper configuration for mapping between <see cref="ImageUpdateModel"/> and <see cref="ImageEntity"/>.
/// </summary>
public class ImageUpdateModelProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ImageUpdateModelProfile"/> class and configures the update mapping rules.
    /// </summary>
    public ImageUpdateModelProfile()
    {
        CreateMap<ImageUpdateModel, ImageEntity>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data));
    }
}
using AutoMapper;
using ImageManager.Entities;

namespace ImageManager.Models.ImageModel;

/// <summary>
/// Represents the data transfer object for creating a new image entry.
/// </summary>
public class ImageCreateModel
{
    /// <summary>
    /// Gets or sets the name identifier for the image.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the binary data representing the image content.
    /// </summary>
    public byte[] Data { get; set; }
}

/// <summary>
/// Provides AutoMapper configuration for mapping between <see cref="ImageCreateModel"/> and <see cref="ImageEntity"/>.
/// </summary>
public class ImageCreateModelProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ImageCreateModelProfile"/> class and configures the mapping rules.
    /// </summary>
    public ImageCreateModelProfile()
    {
        CreateMap<ImageCreateModel, ImageEntity>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data));
    }
}
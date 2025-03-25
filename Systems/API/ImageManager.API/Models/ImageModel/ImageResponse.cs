using AutoMapper;
using ImageManager.Models.ImageModel;

namespace ImageManager.API.Models;

/// <summary>
/// Represents the response model for image resource operations.
/// </summary>
public class ImageResponse
{
    /// <summary>
    /// Gets or sets the unique identifier of the image.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the display name of the image.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the binary data containing the image content.
    /// </summary>
    /// <remarks>
    /// This property contains the raw byte data of the stored image.
    /// </remarks>
    public byte[] Data { get; set; }
}

/// <summary>
/// Configures AutoMapper mappings between <see cref="ImageModel"/> and <see cref="ImageResponse"/>.
/// </summary>
public class ImageResponseProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the mapping profile and configures bidirectional mapping.
    /// </summary>
    public ImageResponseProfile()
    {
        CreateMap<ImageModel, ImageResponse>().ReverseMap();
    }
}
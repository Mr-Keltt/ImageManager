using AutoMapper;
using ImageManager.Models.ImageModel;
using System.ComponentModel.DataAnnotations;

namespace ImageManager.API.Models;

/// <summary>
/// Represents the request model for creating a new image resource.
/// </summary>
public class ImageCreateRequest
{
    /// <summary>
    /// Gets or sets the display name for the image (1-100 characters).
    /// </summary>
    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the binary content of the image file.
    /// </summary>
    /// <remarks>
    /// This should be the raw byte data of the image being uploaded.
    /// </remarks>
    [Required]
    public byte[] FileContent { get; set; }
}

/// <summary>
/// Configures AutoMapper mappings between <see cref="ImageCreateRequest"/> and <see cref="ImageCreateModel"/>.
/// </summary>
public class ImageCreateRequestProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the mapping profile and configures the transformation rules.
    /// </summary>
    public ImageCreateRequestProfile()
    {
        CreateMap<ImageCreateRequest, ImageCreateModel>()
            .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.FileContent));
    }
}
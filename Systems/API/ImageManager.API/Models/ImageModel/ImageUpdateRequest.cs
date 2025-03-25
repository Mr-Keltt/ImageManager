using AutoMapper;
using ImageManager.Models.ImageModel;
using System.ComponentModel.DataAnnotations;

namespace ImageManager.API.Models;

/// <summary>
/// Represents the request model for updating an existing image resource.
/// </summary>
public class ImageUpdateRequest
{
    /// <summary>
    /// Gets or sets the updated display name for the image (1-100 characters).
    /// </summary>
    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the optional new binary content for the image file.
    /// </summary>
    /// <remarks>
    /// When provided, replaces the existing image content. Null value preserves current content.
    /// </remarks>
    public byte[]? FileContent { get; set; }
}

/// <summary>
/// Configures AutoMapper mappings between <see cref="ImageUpdateRequest"/> and <see cref="ImageUpdateModel"/>.
/// </summary>
public class ImageUpdateRequestProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the mapping profile and configures the update transformation rules.
    /// </summary>
    public ImageUpdateRequestProfile()
    {
        CreateMap<ImageUpdateRequest, ImageUpdateModel>()
            .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.FileContent));
    }
}
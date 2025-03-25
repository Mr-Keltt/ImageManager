using AutoMapper;
using ImageManager.Entities;

namespace ImageManager.Models.ImageModel;

/// <summary>
/// Represents the complete data transfer object for image entity interactions.
/// </summary>
public class ImageModel
{
    /// <summary>
    /// Gets or sets the unique identifier for the image.
    /// </summary>
    public Guid Id { get; set; }

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
/// Provides AutoMapper configuration for bidirectional mapping between <see cref="ImageModel"/> and <see cref="ImageEntity"/>.
/// </summary>
public class ImageModelProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ImageModelProfile"/> class and configures bidirectional mapping rules.
    /// </summary>
    public ImageModelProfile()
    {
        // Entity -> Model
        CreateMap<ImageEntity, ImageModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Uid))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data));

        // Model -> Entity
        CreateMap<ImageModel, ImageEntity>()
            .ForMember(dest => dest.Uid, opt => opt.Ignore())
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data));
    }
}
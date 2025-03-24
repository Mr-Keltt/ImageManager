using AutoMapper;
using ImageManager.Entities;

namespace ImageManager.Models.ImageModel;

public class ImageModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public byte[] Data { get; set; }
}


public class ImageModelProfile : Profile
{
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
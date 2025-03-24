using AutoMapper;
using ImageManager.Entities;

namespace ImageManager.Models.ImageModel;

public class ImageUpdateModel
{
    public string Name { get; set; }
    public byte[] Data { get; set; }
}


public class ImageUpdateModelProfile : Profile
{
    public ImageUpdateModelProfile()
    {
        CreateMap<ImageUpdateModel, ImageEntity>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data));
    }
}
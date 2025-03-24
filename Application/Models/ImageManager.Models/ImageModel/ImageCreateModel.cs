using AutoMapper;
using ImageManager.Entities;

namespace ImageManager.Models.ImageModel;

public class ImageCreateModel
{
    public string Name { get; set; }
    public byte[] Data { get; set; }
}


public class ImageCreateModelProfile : Profile
{
    public ImageCreateModelProfile()
    {
        CreateMap<ImageCreateModel, ImageEntity>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data));
    }
}
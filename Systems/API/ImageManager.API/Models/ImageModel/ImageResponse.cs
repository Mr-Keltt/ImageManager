using AutoMapper;
using ImageManager.Models.ImageModel;

namespace ImageManager.API.Models;

public class ImageResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public byte[] Data { get; set; }
}

public class ImageResponseProfile : Profile
{
    public ImageResponseProfile()
    {
        CreateMap<ImageModel, ImageResponse>().ReverseMap();
    }
}
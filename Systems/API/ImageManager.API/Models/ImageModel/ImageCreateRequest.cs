using AutoMapper;
using ImageManager.Models.ImageModel;
using System.ComponentModel.DataAnnotations;

namespace ImageManager.API.Models;

public class ImageCreateRequest
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Required]
    public byte[] FileContent { get; set; }
}

public class ImageCreateRequestProfile : Profile
{
    public ImageCreateRequestProfile()
    {
        CreateMap<ImageCreateRequest, ImageCreateModel>()
            .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.FileContent));
    }
}
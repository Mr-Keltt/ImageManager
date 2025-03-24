using AutoMapper;
using ImageManager.Models.ImageModel;
using System.ComponentModel.DataAnnotations;

namespace ImageManager.API.Models;

public class ImageUpdateRequest
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    public byte[]? FileContent { get; set; }
}

public class ImageUpdateRequestProfile : Profile
{
    public ImageUpdateRequestProfile()
    {
        CreateMap<ImageUpdateRequest, ImageUpdateModel>()
            .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.FileContent));
    }
}
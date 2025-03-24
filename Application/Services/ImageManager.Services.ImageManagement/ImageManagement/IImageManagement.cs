using ImageManager.Models.ImageModel;

namespace ImageManager.Services.ImageManagement;

public interface IImageManagement
{
    Task<IEnumerable<ImageModel>> GetAllImagesAsync();
    Task<ImageModel> CreateImageAsync(ImageCreateModel createModel);
    Task<ImageModel> UpdateImageAsync(Guid id, ImageUpdateModel updateModel);
    Task<bool> DeleteImageAsync(Guid id);
}

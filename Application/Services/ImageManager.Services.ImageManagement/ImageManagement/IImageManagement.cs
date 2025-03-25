using ImageManager.Models.ImageModel;

namespace ImageManager.Services.ImageManagement;

/// <summary>
/// Defines operations for managing image entities including CRUD operations.
/// </summary>
public interface IImageManagement
{
    /// <summary>
    /// Retrieves all available images from the storage.
    /// </summary>
    /// <returns>A collection of image models representing all stored images.</returns>
    Task<IEnumerable<ImageModel>> GetAllImagesAsync();

    /// <summary>
    /// Creates a new image entry from the provided data transfer object.
    /// </summary>
    /// <param name="createModel">The data transfer object containing image creation details.</param>
    /// <returns>The newly created image model with generated identifiers.</returns>
    Task<ImageModel> CreateImageAsync(ImageCreateModel createModel);

    /// <summary>
    /// Updates an existing image identified by the specified unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the image to update.</param>
    /// <param name="updateModel">The data transfer object containing updated image details.</param>
    /// <returns>The updated image model reflecting the changes.</returns>
    Task<ImageModel> UpdateImageAsync(Guid id, ImageUpdateModel updateModel);

    /// <summary>
    /// Deletes an image identified by the specified unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the image to delete.</param>
    /// <returns>True if the image was successfully deleted, false otherwise.</returns>
    Task<bool> DeleteImageAsync(Guid id);
}
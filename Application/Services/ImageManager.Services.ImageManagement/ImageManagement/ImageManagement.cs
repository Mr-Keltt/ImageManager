using AutoMapper;
using ImageManager.Context;
using ImageManager.Entities;
using ImageManager.Models.ImageModel;
using ImageManager.Services.Logger;
using Microsoft.EntityFrameworkCore;

namespace ImageManager.Services.ImageManagement;

/// <summary>
/// Provides implementation for image management operations including CRUD functionality.
/// </summary>
public class ImageManagement : IImageManagement
{
    private readonly IDbContextFactory<MainDbContext> _dbContextFactory;
    private readonly IMapper _mapper;
    private readonly IAppLogger _logger;
    private readonly object _logModule = typeof(ImageManagement);

    /// <summary>
    /// Initializes a new instance of the ImageManagement service with required dependencies.
    /// </summary>
    /// <param name="dbContextFactory">Factory for creating database contexts.</param>
    /// <param name="mapper">AutoMapper instance for object mapping.</param>
    /// <param name="logger">Application logger for tracking operations.</param>
    public ImageManagement(
        IDbContextFactory<MainDbContext> dbContextFactory,
        IMapper mapper,
        IAppLogger logger)
    {
        _dbContextFactory = dbContextFactory;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves all image entries from the database.
    /// </summary>
    /// <returns>A collection of image models representing all stored images.</returns>
    /// <exception cref="ApplicationException">Thrown when database operation fails.</exception>
    public async Task<IEnumerable<ImageModel>> GetAllImagesAsync()
    {
        using var context = await _dbContextFactory.CreateDbContextAsync();

        try
        {
            _logger.Information(_logModule, "Starting to get all images");

            var images = await context.Images.ToListAsync();
            _logger.Debug(_logModule, "Retrieved {ImageCount} images from database", images.Count);

            return _mapper.Map<List<ImageModel>>(images);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, _logModule, "Failed to get images");
            throw new ApplicationException("Error retrieving images", ex);
        }
    }

    /// <summary>
    /// Creates a new image entry in the database.
    /// </summary>
    /// <param name="createModel">Data transfer object containing image details.</param>
    /// <returns>The newly created image model with generated identifiers.</returns>
    /// <exception cref="ArgumentNullException">Thrown when createModel is null.</exception>
    /// <exception cref="ApplicationException">Thrown when creation operation fails.</exception>
    public async Task<ImageModel> CreateImageAsync(ImageCreateModel createModel)
    {
        if (createModel == null)
        {
            _logger.Warning(_logModule, "Attempt to create image with null model");
            throw new ArgumentNullException(nameof(createModel));
        }

        using var context = await _dbContextFactory.CreateDbContextAsync();

        try
        {
            _logger.Verbose(_logModule, "Mapping create model to entity");
            var entity = _mapper.Map<ImageEntity>(createModel);

            context.Images.Add(entity);
            await context.SaveChangesAsync();

            _logger.Information(_logModule, "Created new image with ID {ImageId}", entity.Uid);
            return _mapper.Map<ImageModel>(entity);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, _logModule, "Error creating image");
            throw new ApplicationException("Image creation failed", ex);
        }
    }

    /// <summary>
    /// Updates an existing image entry in the database.
    /// </summary>
    /// <param name="id">Unique identifier of the image to update.</param>
    /// <param name="updateModel">Data transfer object containing updated image details.</param>
    /// <returns>The updated image model reflecting changes.</returns>
    /// <exception cref="ArgumentNullException">Thrown when updateModel is null.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when specified image ID is not found.</exception>
    /// <exception cref="ApplicationException">Thrown when update operation fails.</exception>
    public async Task<ImageModel> UpdateImageAsync(Guid id, ImageUpdateModel updateModel)
    {
        if (updateModel == null)
        {
            _logger.Warning(_logModule, "Attempt to update image with null model for ID {ImageId}", id);
            throw new ArgumentNullException(nameof(updateModel));
        }

        using var context = await _dbContextFactory.CreateDbContextAsync();

        try
        {
            _logger.Debug(_logModule, "Looking for image with ID {ImageId}", id);
            var existingEntity = await context.Images.FindAsync(id);

            if (existingEntity == null)
            {
                _logger.Warning(_logModule, "Image not found for ID {ImageId}", id);
                throw new KeyNotFoundException($"Image with ID {id} not found");
            }

            _mapper.Map(updateModel, existingEntity);
            await context.SaveChangesAsync();

            _logger.Information(_logModule, "Successfully updated image with ID {ImageId}", id);
            return _mapper.Map<ImageModel>(existingEntity);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, _logModule, "Failed to update image with ID {ImageId}", id);
            throw new ApplicationException($"Error updating image {id}", ex);
        }
    }

    /// <summary>
    /// Deletes an image entry from the database.
    /// </summary>
    /// <param name="id">Unique identifier of the image to delete.</param>
    /// <returns>True if deletion succeeded, false if image was not found.</returns>
    /// <exception cref="ApplicationException">Thrown when deletion operation fails.</exception>
    public async Task<bool> DeleteImageAsync(Guid id)
    {
        using var context = await _dbContextFactory.CreateDbContextAsync();

        try
        {
            _logger.Debug(_logModule, "Attempting to delete image with ID {ImageId}", id);
            var entity = await context.Images.FindAsync(id);

            if (entity == null)
            {
                _logger.Warning(_logModule, "Delete failed - image not found for ID {ImageId}", id);
                return false;
            }

            context.Images.Remove(entity);
            await context.SaveChangesAsync();

            _logger.Information(_logModule, "Deleted image with ID {ImageId}", id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, _logModule, "Error deleting image with ID {ImageId}", id);
            throw new ApplicationException($"Error deleting image {id}", ex);
        }
    }
}
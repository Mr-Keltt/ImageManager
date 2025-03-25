using AutoMapper;
using ImageManager.API.Models;
using ImageManager.Models.ImageModel;
using ImageManager.Services.ImageManagement;
using ImageManager.Services.Logger;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
/// <summary>
/// Provides API endpoints for managing image resources including CRUD operations.
/// </summary>
public class ImagesController : ControllerBase
{
    private readonly IImageManagement _imageService;
    private readonly IMapper _mapper;
    private readonly IAppLogger _logger;

    /// <summary>
    /// Initializes a new instance of the ImagesController with required services.
    /// </summary>
    /// <param name="imageService">Image management service implementation.</param>
    /// <param name="mapper">AutoMapper instance for object conversion.</param>
    /// <param name="logger">Application logger for operation tracking.</param>
    public ImagesController(
        IImageManagement imageService,
        IMapper mapper,
        IAppLogger logger)
    {
        _imageService = imageService;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves all stored images from the system.
    /// </summary>
    /// <returns>A collection of image responses representing all available images.</returns>
    /// <response code="200">Successfully retrieved list of images</response>
    /// <response code="500">Internal server error occurred</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ImageResponse>), 200)]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var images = await _imageService.GetAllImagesAsync();
            return Ok(_mapper.Map<IEnumerable<ImageResponse>>(images));
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error getting all images");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Creates a new image entry in the system.
    /// </summary>
    /// <param name="request">Image creation request containing image data.</param>
    /// <returns>Newly created image resource with generated identifiers.</returns>
    /// <response code="201">Successfully created new image</response>
    /// <response code="400">Invalid request format or data</response>
    /// <response code="500">Image creation process failed</response>
    [HttpPost]
    [ProducesResponseType(typeof(ImageResponse), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create([FromBody] ImageCreateRequest request)
    {
        if (!ModelState.IsValid)
        {
            _logger.Warning("Invalid create model: {@Errors}", ModelState.Values.SelectMany(v => v.Errors));
            return BadRequest(ModelState);
        }

        try
        {
            var createModel = _mapper.Map<ImageCreateModel>(request);
            var result = await _imageService.CreateImageAsync(createModel);
            return StatusCode(StatusCodes.Status201Created, _mapper.Map<ImageResponse>(result));
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error creating image");
            return StatusCode(500, "Image creation failed");
        }
    }

    /// <summary>
    /// Updates an existing image resource with new data.
    /// </summary>
    /// <param name="id">Unique identifier of the image to update.</param>
    /// <param name="request">Image update request containing new data.</param>
    /// <returns>Updated image resource with modified values.</returns>
    /// <response code="200">Successfully updated image</response>
    /// <response code="400">Invalid request format or data</response>
    /// <response code="404">Specified image ID not found</response>
    /// <response code="500">Image update process failed</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ImageResponse), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update(Guid id, [FromBody] ImageUpdateRequest request)
    {
        if (!ModelState.IsValid)
        {
            _logger.Warning("Invalid update model for {ImageId}: {@Errors}", id, ModelState.Values.SelectMany(v => v.Errors));
            return BadRequest(ModelState);
        }

        try
        {
            var updateModel = _mapper.Map<ImageUpdateModel>(request);
            var result = await _imageService.UpdateImageAsync(id, updateModel);
            return Ok(_mapper.Map<ImageResponse>(result));
        }
        catch (KeyNotFoundException)
        {
            _logger.Warning("Update failed: image {ImageId} not found", id);
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error updating image {ImageId}", id);
            return StatusCode(500, "Image update failed");
        }
    }

    /// <summary>
    /// Deletes a specific image resource from the system.
    /// </summary>
    /// <param name="id">Unique identifier of the image to remove.</param>
    /// <returns>Empty response for successful deletion or not found status.</returns>
    /// <response code="204">Image successfully deleted</response>
    /// <response code="404">Specified image ID not found</response>
    /// <response code="500">Image deletion process failed</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var result = await _imageService.DeleteImageAsync(id);
            return result ? NoContent() : NotFound();
        }
        catch (KeyNotFoundException)
        {
            _logger.Warning("Delete failed: image {ImageId} not found", id);
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error deleting image {ImageId}", id);
            return StatusCode(500, "Image deletion failed");
        }
    }
}
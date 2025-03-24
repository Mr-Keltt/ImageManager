using AutoMapper;
using ImageManager.API.Models;
using ImageManager.Models.ImageModel;
using ImageManager.Services.ImageManagement;
using ImageManager.Services.Logger;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ImagesController : ControllerBase
{
    private readonly IImageManagement _imageService;
    private readonly IMapper _mapper;
    private readonly IAppLogger _logger;

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
    /// Get all images
    /// </summary>
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
    /// Create new image
    /// </summary>
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

            // Возвращаем созданный объект напрямую
            return StatusCode(StatusCodes.Status201Created, _mapper.Map<ImageResponse>(result));
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error creating image");
            return StatusCode(500, "Image creation failed");
        }
    }

    /// <summary>
    /// Update existing image
    /// </summary>
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
    /// Delete image by ID
    /// </summary>
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
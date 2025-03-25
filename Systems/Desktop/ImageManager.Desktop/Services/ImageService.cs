using ImageManager.Desktop.Models;
using System.Net.Http;
using System.Net.Http.Json;

namespace ImageManager.Desktop.Services;

/// <summary>
/// Provides client-side service for interacting with image management API endpoints
/// </summary>
/// <remarks>
/// Handles HTTP communication and data conversion between desktop models and API models
/// </remarks>
public class ImageService
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "http://localhost:10000/api/Images";

    /// <summary>
    /// Initializes a new instance of the ImageService with default HTTP client
    /// </summary>
    public ImageService() => _httpClient = new HttpClient();

    /// <summary>
    /// Uploads a new image to the image management service
    /// </summary>
    /// <param name="request">Image creation request containing file data</param>
    /// <returns>Base model of the newly created image resource</returns>
    /// <remarks>
    /// Converts the response data from Base64 string to byte array for desktop usage
    /// </remarks>
    public async Task<ImageBaseModel> UploadImageAsync(ImageCreateRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync(BaseUrl, request);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadFromJsonAsync<ImageResponse>();
        return new ImageBaseModel
        {
            Id = content.Id,
            Name = content.Name,
            Data = Convert.FromBase64String(content.Data)
        };
    }

    /// <summary>
    /// Retrieves all available images from the image management service
    /// </summary>
    /// <returns>List of base image models with converted binary data</returns>
    /// <remarks>
    /// Performs Base64 to byte array conversion for each received image resource
    /// </remarks>
    public async Task<List<ImageBaseModel>> GetAllImagesAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<List<ImageResponse>>(BaseUrl);
        return response.Select(r => new ImageBaseModel
        {
            Id = r.Id,
            Name = r.Name,
            Data = Convert.FromBase64String(r.Data)
        }).ToList();
    }

    /// <summary>
    /// Updates an existing image resource in the image management service
    /// </summary>
    /// <param name="id">Unique identifier of the image to update</param>
    /// <param name="request">Image update request containing new data</param>
    /// <returns>Updated base image model with converted binary data</returns>
    /// <remarks>
    /// Uses PUT operation and converts response data from Base64 string
    /// </remarks>
    public async Task<ImageBaseModel> UpdateImageAsync(Guid id, ImageCreateRequest request)
    {
        var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/{id}", request);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadFromJsonAsync<ImageResponse>();
        return new ImageBaseModel
        {
            Id = content.Id,
            Name = content.Name,
            Data = Convert.FromBase64String(content.Data)
        };
    }

    /// <summary>
    /// Deletes a specific image resource from the image management service
    /// </summary>
    /// <param name="id">Unique identifier of the image to remove</param>
    /// <remarks>
    /// Performs permanent deletion of the specified image resource
    /// </remarks>
    public async Task DeleteImageAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}");
        response.EnsureSuccessStatusCode();
    }
}
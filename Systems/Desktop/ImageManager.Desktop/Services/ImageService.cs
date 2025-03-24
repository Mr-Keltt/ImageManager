using ImageManager.Desktop.Models;
using System.Net.Http;
using System.Net.Http.Json;

namespace ImageManager.Desktop.Services;

public class ImageService
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "http://localhost:10000/api/Images";

    public ImageService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<ImageItem> UploadImageAsync(ImageCreateDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync(BaseUrl, dto);
        response.EnsureSuccessStatusCode();

        var createdImage = await response.Content.ReadFromJsonAsync<ImageItem>();
        return createdImage;
    }
}
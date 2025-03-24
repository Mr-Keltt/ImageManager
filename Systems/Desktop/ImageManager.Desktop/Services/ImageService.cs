using ImageManager.Desktop.Models;
using System.Diagnostics;
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

        var content = await response.Content.ReadFromJsonAsync<ImageApiResponse>();
        return new ImageItem
        {
            Id = content.Id,
            Name = content.Name,
            ImageData = Convert.FromBase64String(content.Data)
        };
    }

    public async Task<List<ImageItem>> GetAllImagesAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<List<ImageApiResponse>>(BaseUrl);
        return response.Select(ConvertToImageItem).ToList();
    }

    private ImageItem ConvertToImageItem(ImageApiResponse apiResponse)
    {
        try
        {
            var imageData = Convert.FromBase64String(apiResponse.Data);
            return new ImageItem
            {
                Id = apiResponse.Id,
                Name = apiResponse.Name,
                ImageData = imageData
            };
        }
        catch (FormatException ex)
        {
            Debug.WriteLine($"Ошибка конвертации Base64: {ex.Message}");
            return new ImageItem
            {
                Id = apiResponse.Id,
                Name = apiResponse.Name,
                ImageData = Array.Empty<byte>()
            };
        }
    }
}
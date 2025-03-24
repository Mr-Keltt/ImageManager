using ImageManager.Desktop.Models;
using System.Net.Http;
using System.Net.Http.Json;

namespace ImageManager.Desktop.Services;

public class ImageService
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "http://localhost:10000/api/Images";

    public ImageService() => _httpClient = new HttpClient();

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
}
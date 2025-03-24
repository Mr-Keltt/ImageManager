// ImageBaseModel.cs
namespace ImageManager.Desktop.Models;

public class ImageBaseModel
{
    public Guid Id { get; init; }
    public string Name { get; set; } = null!;
    public byte[] Data { get; set; } = null!;
    public string? LocalPath { get; set; }
}
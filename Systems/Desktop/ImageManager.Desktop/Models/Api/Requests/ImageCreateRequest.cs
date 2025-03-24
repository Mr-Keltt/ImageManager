namespace ImageManager.Desktop.Models;

public class ImageCreateRequest
{
    public string Name { get; set; }
    public byte[] FileContent { get; set; }
}
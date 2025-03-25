namespace ImageManager.Desktop.Models;

/// <summary>
/// Represents the request model for creating a new image entry in the desktop application
/// </summary>
public class ImageCreateRequest
{
    /// <summary>
    /// Gets or sets the display name for the image
    /// </summary>
    /// <remarks>
    /// Required field containing the name identifier for the image
    /// </remarks>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the binary data of the image file
    /// </summary>
    /// <remarks>
    /// Required field containing the raw bytes of the image content
    /// </remarks>
    public byte[] FileContent { get; set; }
}
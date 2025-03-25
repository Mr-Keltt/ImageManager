namespace ImageManager.Desktop.Models;

/// <summary>
/// Represents the response model for image data in desktop application interactions
/// </summary>
public class ImageResponse
{
    /// <summary>
    /// Gets or sets the unique identifier of the image
    /// </summary>
    /// <remarks>
    /// System-generated GUID that uniquely identifies the image resource
    /// </remarks>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the display name of the image
    /// </summary>
    /// <remarks>
    /// Human-readable identifier for the image resource
    /// </remarks>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the string representation of image data
    /// </summary>
    /// <remarks>
    /// Contains encoded image data suitable for desktop application rendering.
    /// Typically represents base64-encoded byte array or file path reference
    /// </remarks>
    public string Data { get; set; }
}
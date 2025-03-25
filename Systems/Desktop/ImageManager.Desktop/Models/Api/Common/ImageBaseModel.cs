// ImageBaseModel.cs
namespace ImageManager.Desktop.Models;

/// <summary>
/// Represents the base model for image data in desktop application context
/// </summary>
public class ImageBaseModel
{
    /// <summary>
    /// Gets or sets the unique identifier for the image
    /// </summary>
    /// <remarks>
    /// Initialized during object creation and remains immutable
    /// </remarks>
    public Guid Id { get; init; }

    /// <summary>
    /// Gets or sets the display name of the image
    /// </summary>
    /// <remarks>
    /// Required field with non-null default value
    /// </remarks>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets the binary data containing the image content
    /// </summary>
    /// <remarks>
    /// Represents the actual image bytes in binary format
    /// </remarks>
    public byte[] Data { get; set; } = null!;

    /// <summary>
    /// Gets or sets the local filesystem path to the image file
    /// </summary>
    /// <remarks>
    /// Optional property used for desktop-specific storage tracking
    /// </remarks>
    public string? LocalPath { get; set; }
}
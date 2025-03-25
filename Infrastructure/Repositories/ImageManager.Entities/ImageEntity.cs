using ImageManager.Context.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImageManager.Entities;

/// <summary>
/// Represents an image entity in the database storage system.
/// </summary>
public class ImageEntity : BaseEntity
{
    /// <summary>
    /// Gets or sets the name identifier for the image.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the binary data containing the image content.
    /// </summary>
    /// <remarks>
    /// Mapped to PostgreSQL bytea type for binary storage.
    /// </remarks>
    [Column(TypeName = "bytea")]
    public byte[] Data { get; set; }
}
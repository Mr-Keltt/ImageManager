using ImageManager.Context.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImageManager.Entities;

public class ImageEntity : BaseEntity
{
    public string Name { get; set; }

    [Column(TypeName = "bytea")]
    public byte[] Data { get; set; }
}
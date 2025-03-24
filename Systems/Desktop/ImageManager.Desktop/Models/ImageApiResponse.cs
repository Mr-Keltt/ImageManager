using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageManager.Desktop.Models;

public class ImageApiResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Data { get; set; }
}
// ImageViewModel.cs
using System.ComponentModel;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ImageManager.Desktop.Models;

/// <summary>
/// Represents a view model for image display and manipulation in the desktop application
/// </summary>
/// <remarks>
/// Implements INotifyPropertyChanged to support data binding and automatic UI updates.
/// Handles image rotation based on EXIF orientation metadata.
/// </remarks>
public class ImageViewModel : INotifyPropertyChanged
{
    /// <summary>
    /// Gets the unique identifier of the image
    /// </summary>
    /// <remarks>
    /// Immutable identifier assigned during initialization from base model
    /// </remarks>
    public Guid Id { get; private set; }

    private string _name;

    /// <summary>
    /// Gets or sets the display name of the image
    /// </summary>
    /// <remarks>
    /// Triggers property change notifications when updated
    /// </remarks>
    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged();
        }
    }

    private ImageSource _imageSource;

    /// <summary>
    /// Gets the image source for display in the UI
    /// </summary>
    /// <remarks>
    /// Automatically updated when image data changes. Contains rotated version
    /// of the image based on EXIF orientation metadata.
    /// </remarks>
    public ImageSource ImageSource
    {
        get => _imageSource;
        private set
        {
            _imageSource = value;
            OnPropertyChanged();
        }
    }

    private string _localPath;

    /// <summary>
    /// Gets or sets the local filesystem path to the image file
    /// </summary>
    /// <remarks>
    /// Triggers property change notifications when updated. Can be null
    /// for images not stored locally.
    /// </remarks>
    public string LocalPath
    {
        get => _localPath;
        set
        {
            _localPath = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Updates view model properties from a base image model
    /// </summary>
    /// <param name="baseModel">The base model containing updated image data</param>
    /// <remarks>
    /// Propagates changes from the base model to the view model properties,
    /// including image data conversion and EXIF orientation handling
    /// </remarks>
    public void UpdateFromBaseModel(ImageBaseModel baseModel)
    {
        Id = baseModel.Id;
        Name = baseModel.Name;
        LocalPath = baseModel.LocalPath;
        UpdateImageSource(baseModel.Data);
    }

    /// <summary>
    /// Updates the image source from raw byte data
    /// </summary>
    /// <param name="imageData">The binary image data to display</param>
    /// <remarks>
    /// Handles image decoding and automatic rotation correction based on
    /// EXIF orientation metadata. Supports common rotation angles:
    /// 180°, 270° (clockwise), and 90° (counter-clockwise)
    /// </remarks>
    private void UpdateImageSource(byte[] imageData)
    {
        if (imageData == null || imageData.Length == 0) return;

        using var stream = new MemoryStream(imageData);
        var image = new BitmapImage();
        image.BeginInit();
        image.CacheOption = BitmapCacheOption.OnLoad;
        image.StreamSource = stream;
        image.EndInit();

        var exifOrientation = GetExifOrientation(imageData);
        if (exifOrientation != 0)
        {
            int angle = exifOrientation switch
            {
                3 => 180,
                6 => 270,
                8 => 90,
                _ => 0
            };

            var transform = new RotateTransform(-angle);
            var transformedBitmap = new TransformedBitmap(image, transform);
            ImageSource = transformedBitmap;
        }
        else
        {
            ImageSource = image;
        }
    }

    /// <summary>
    /// Occurs when a property value changes
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Raises the PropertyChanged event
    /// </summary>
    /// <param name="propertyName">The name of the changed property</param>
    /// <remarks>
    /// Uses CallerMemberName attribute to automatically detect property name
    /// when called from property setters
    /// </remarks>
    protected virtual void OnPropertyChanged(string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Extracts EXIF orientation metadata from image data
    /// </summary>
    /// <param name="imageData">Binary image data to analyze</param>
    /// <returns>
    /// EXIF orientation value (0 if not found or error occurs)
    /// </returns>
    /// <remarks>
    /// Common orientation values:
    /// 1 = Normal, 3 = 180° rotation, 6 = 270° rotation, 8 = 90° rotation
    /// Silently handles exceptions during metadata parsing
    /// </remarks>
    private static ushort GetExifOrientation(byte[] imageData)
    {
        try
        {
            using (var ms = new MemoryStream(imageData))
            {
                var decoder = BitmapDecoder.Create(ms, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
                var frame = decoder.Frames[0];
                var metadata = frame.Metadata as BitmapMetadata;
                if (metadata != null && metadata.ContainsQuery("/app1/ifd/{ushort=274}"))
                {
                    return (ushort)metadata.GetQuery("/app1/ifd/{ushort=274}");
                }
            }
        }
        catch
        {
            // Intentionally suppressed error handling
        }
        return 0;
    }
}
// ImageViewModel.cs
using System.ComponentModel;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ImageManager.Desktop.Models;

public class ImageViewModel : INotifyPropertyChanged
{
    public Guid Id { get; private set; }

    private string _name;
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
    public string LocalPath
    {
        get => _localPath;
        set
        {
            _localPath = value;
            OnPropertyChanged();
        }
    }

    public void UpdateFromBaseModel(ImageBaseModel baseModel)
    {
        Id = baseModel.Id;
        Name = baseModel.Name;
        LocalPath = baseModel.LocalPath;
        UpdateImageSource(baseModel.Data);
    }

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

    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged(string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

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

        }
        return 0;
    }
}
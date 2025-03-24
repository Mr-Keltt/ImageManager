// ImageViewModel.cs
using System.ComponentModel;
using System.IO;
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

    private BitmapImage _imageSource;
    public BitmapImage ImageSource
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
        ImageSource = image;
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged(string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
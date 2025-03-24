using ImageManager.Desktop.Models;
using ImageManager.Desktop.Services;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace ImageManager.Desktop;

public partial class MainWindow : Window
{
    private readonly ImageService _imageService = new();
    public ObservableCollection<ImageItem> Images { get; } = new();

    public MainWindow()
    {
        InitializeComponent();
        Loaded += MainWindow_Loaded;
        ImagesGrid.ItemsSource = Images;
    }

    private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        await LoadImagesFromApi();
    }

    private async Task LoadImagesFromApi()
    {
        try
        {
            var apiImages = await _imageService.GetAllImagesAsync();
            foreach (var image in apiImages)
            {
                Images.Add(image); // ImageData уже содержит данные, ImageSource обновится автоматически
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка загрузки: {ex.Message}");
        }
    }

    private async void AddButton_Click(object sender, RoutedEventArgs e)
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = "Image files (*.png;*.jpg)|*.png;*.jpg",
            Title = "Выберите изображение"
        };

        if (openFileDialog.ShowDialog() == true)
        {
            try
            {
                var filePath = openFileDialog.FileName;
                var fileName = Path.GetFileName(filePath);
                var fileBytes = File.ReadAllBytes(filePath);

                var newImage = await _imageService.UploadImageAsync(new ImageCreateDto
                {
                    Name = fileName,
                    FileContent = fileBytes
                });

                var imageItem = new ImageItem
                {
                    Id = newImage.Id,
                    Name = newImage.Name,
                    ImageData = fileBytes,
                    ImagePath = filePath
                };

                imageItem.UpdateImageSource();
                Images.Add(imageItem);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
    }

    private void EditButton_Click(object sender, RoutedEventArgs e)
    {
        // Логика изменения
    }

    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        // Логика удаления
    }

    private void ImagesGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // Обработка выделения строки
        var selectedItem = ImagesGrid.SelectedItem as ImageItem;
        // Ваша логика...
    }
}
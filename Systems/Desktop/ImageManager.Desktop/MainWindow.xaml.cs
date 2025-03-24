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

    public ObservableCollection<ImageItem> Images { get; set; } = new ObservableCollection<ImageItem>();

    public MainWindow()
    {
        InitializeComponent();
        
        // Пример с абсолютными путями для теста
        Images.Add(new ImageItem 
        { 
            Name = "Привет.png", 
            ImagePath = @"C:\Users\dmitr\Desktop\Programming\ImageManager\Systems\Desktop\ImageManager.Desktop\bin\Debug\net8.0-windows\placeholder.png"
        });
        Images.Add(new ImageItem
        {
            Name = "Привет.png",
            ImagePath = @"C:\Users\dmitr\Desktop\Programming\ImageManager\Systems\Desktop\ImageManager.Desktop\bin\Debug\net8.0-windows\placeholder.png"
        });
        

        ImagesGrid.ItemsSource = Images;
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

                var dto = new ImageCreateDto
                {
                    Name = fileName,
                    FileContent = fileBytes
                };

                var newImage = await _imageService.UploadImageAsync(dto);

                Images.Add(new ImageItem
                {
                    Id = newImage.Id,
                    Name = newImage.Name,
                    ImagePath = filePath
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка загрузки",
                    MessageBoxButton.OK, MessageBoxImage.Error);
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
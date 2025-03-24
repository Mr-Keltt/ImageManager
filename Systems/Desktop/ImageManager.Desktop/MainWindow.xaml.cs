using ImageManager.Desktop.Models;
using ImageManager.Desktop.Services;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Net;
using System.Windows;
using System.Windows.Controls;

namespace ImageManager.Desktop;

public partial class MainWindow : Window
{
    private readonly ImageService _imageService = new();
    public ObservableCollection<ImageViewModel> Images { get; } = new();

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
                var viewModel = new ImageViewModel();
                viewModel.UpdateFromBaseModel(image);
                Images.Add(viewModel);
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
            Title = "Добавить изображение"
        };

        if (openFileDialog.ShowDialog() == true)
        {
            try
            {
                var filePath = openFileDialog.FileName;
                var fileName = Path.GetFileName(filePath);
                var fileBytes = File.ReadAllBytes(filePath);

                var newImage = await _imageService.UploadImageAsync(new ImageCreateRequest
                {
                    Name = fileName,
                    FileContent = fileBytes
                });

                var viewModel = new ImageViewModel();
                viewModel.UpdateFromBaseModel(new ImageBaseModel
                {
                    Id = newImage.Id,
                    Name = newImage.Name,
                    Data = newImage.Data,
                    LocalPath = filePath
                });

                Images.Add(viewModel);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
    }

    private async void EditButton_Click(object sender, RoutedEventArgs e)
    {
        if (ImagesGrid.SelectedItem is not ImageViewModel selectedImage)
        {
            MessageBox.Show("Выберите изображение для изменения.", "Предупреждение",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var openFileDialog = new OpenFileDialog
        {
            Filter = "Image files (*.png;*.jpg)|*.png;*.jpg",
            Title = "Изменить изображение"
        };

        if (openFileDialog.ShowDialog() == true)
        {
            try
            {
                var filePath = openFileDialog.FileName;
                var fileName = Path.GetFileName(filePath);
                var fileBytes = File.ReadAllBytes(filePath);

                var updatedImage = await _imageService.UpdateImageAsync(
                    selectedImage.Id,
                    new ImageCreateRequest
                    {
                        Name = fileName,
                        FileContent = fileBytes
                    });

                selectedImage.UpdateFromBaseModel(new ImageBaseModel
                {
                    Id = updatedImage.Id,
                    Name = updatedImage.Name,
                    Data = updatedImage.Data,
                    LocalPath = filePath
                });

                var index = Images.IndexOf(selectedImage);
                Images[index] = selectedImage;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
    }

    private async void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        if (ImagesGrid.SelectedItem is not ImageViewModel selectedImage)
        {
            MessageBox.Show("Выберите изображение для удаления.",
                           "Предупреждение",
                           MessageBoxButton.OK,
                           MessageBoxImage.Warning);
            return;
        }

        var confirmation = MessageBox.Show("Удалить выбранное изображение?",
                                          "Подтверждение",
                                          MessageBoxButton.YesNo,
                                          MessageBoxImage.Question);

        if (confirmation != MessageBoxResult.Yes) return;

        try
        {
            await _imageService.DeleteImageAsync(selectedImage.Id);

            Images.Remove(selectedImage);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            MessageBox.Show("Изображение не найдено на сервере.");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка удаления: {ex.Message}");
        }
    }
}
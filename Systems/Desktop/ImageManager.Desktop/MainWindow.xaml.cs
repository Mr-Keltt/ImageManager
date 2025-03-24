using ImageManager.Desktop.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace ImageManager.Desktop;

public partial class MainWindow : Window
{
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

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        // Логика добавления
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
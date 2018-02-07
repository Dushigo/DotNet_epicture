using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Imgur.API.Authentication.Impl;
using Imgur.API.Endpoints.Impl;
using Imgur.API.Models;
using System.IO;
using System.Threading.Tasks;
using Imgur.API;
using System.Diagnostics;

namespace epitecture
{
    public sealed partial class MainPage : Page
    {
        public async void AddImage(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");
            picker.FileTypeFilter.Add(".gif");
            picker.FileTypeFilter.Add(".svg");
            picker.FileTypeFilter.Add(".bmp");

            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                imgurFct.UploadImage(file.Path);
                //ImageList.Items.Add(new ImageClass { ImageSource = file.Path, Title = file.Path, IsFav = false});
            }
        }

        public void Search(object sender, RoutedEventArgs e)
        {
            imgurFct.Search();
        }

        public async void LoadImage(object sender, RoutedEventArgs e)
        {
            var image = await imgurFct.LoadImage();
            ImageList.Items.Add(new ImageClass { ImageSource = image.Link, Title = image.Title, IsFav = (bool)image.Favorite});
        }

        public void SortByType(object sender, RoutedEventArgs e)
        {
            imgurFct.SortByType();
        }

        public void SortBySize(object sender, RoutedEventArgs e)
        {
            imgurFct.SortBySize();
        }

        public void FavOrUnfav(object sender, ItemClickEventArgs e)
        {
            imgurFct.FavOrUnfav();
        }

        public MainPage()
        {
            imgurFct = new ImgurFct();
            this.InitializeComponent();
            LoadImage(new object(), new RoutedEventArgs());
        }

        private ImgurFct imgurFct;
    }
}

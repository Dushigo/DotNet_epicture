using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Collections.Generic;

namespace epitecture
{
    public sealed partial class MainPage : Page
    {
        public void ToogleChange(object sender, RoutedEventArgs e)
        {
            if (StateApp.Label == "Imgur")
                StateApp.Label = "Flickr";
            else
                StateApp.Label = "Imgur";
        }
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
                var result = imgurFct.UploadImage(file);
                ImageList.Items.Clear();
                foreach (var element in result)
                {
                    var image = await imgurFct.LoadImage(element);
                    ImageList.Items.Add(
                        new ImageClass
                        {
                            ImageSource = image.Link,
                            Title = image.Title,
                            Type = image.Type,
                            Size = image.Size
                        });
                }
            }
        }
        public async void Search(object sender, RoutedEventArgs e)
        {
            ImageList.Items.Clear();
            var imageList = await imgurFct.Search(TextSearch.Text);
            prevSearch = TextSearch.Text;
            foreach (var element in imageList)
            {
                var image = await imgurFct.LoadImage(element);
                ImageList.Items.Add(
                    new ImageClass
                    {
                        ImageSource = image.Link,
                        Title = image.Title,
                        Type = image.Type,
                        Size = image.Size,
                        Id = image.Id
                    });
            }
        }

        public async void LoadImage(object sender, RoutedEventArgs e)
        {
            ImageList.Items.Clear();
            if (TopList.Count == 0)
            {
                var imageList = await imgurFct.Search("");
                foreach (var element in imageList)
                {
                    var image = await imgurFct.LoadImage(element);
                    ImageClass tmp = new ImageClass
                    {
                        ImageSource = image.Link,
                        Title = image.Title,
                        Type = image.Type,
                        Size = image.Size,
                        Id = image.Id
                    };
                    TopList.Add(tmp);
                    ImageList.Items.Add(tmp);
                }
            }
            else
                foreach (var element in TopList)
                    ImageList.Items.Add(element);
        }
        public void SortByType(object sender, RoutedEventArgs e)
        {
            TopList.Sort(delegate (ImageClass x, ImageClass y)
            {
                if (x.Type == "image/png") return 0;
                else if (y.Type == "image/png") return 1;
                else if (x.Type == "image/jpeg" && y.Type == "image/gif") return 0;
                else if (y.Type == "image/jpeg" && x.Type == "image/gif") return 1;
                else return 1;
            });
            LoadImage(new object(), new RoutedEventArgs());
        }

        public void SortBySize(object sender, RoutedEventArgs e)
        {
            TopList.Sort(delegate (ImageClass x, ImageClass y)
            {
                if (x.Size < y.Size) return 0;
                else return 1;
            });
            LoadImage(new object(), new RoutedEventArgs());
        }

        public void FavOrUnfav(object sender, ItemClickEventArgs e)
        {
            if (FavList.Contains((ImageClass)e.ClickedItem))
                FavList.Remove((ImageClass)e.ClickedItem);
            else
                FavList.Add((ImageClass)e.ClickedItem);
        }
        public void DisplayFav(object sender, RoutedEventArgs e)
        {
            if (FavDisplay == false)
            {
                ImageList.Items.Clear();
                if (FavList.Count != 0)
                {
                    foreach (var element in FavList)
                        ImageList.Items.Add(element);
                }
                FavDisplay = true;
            }
            else
            {
                LoadImage(new object(), new RoutedEventArgs());
                FavDisplay = false;
            }
        }
        public MainPage()
        {
            imgurFct = new ImgurFct();
            FavList = new List<ImageClass>();
            TopList = new List<ImageClass>();
            FavDisplay = false;
            this.InitializeComponent();
            prevSearch = "";
            LoadImage(new object(), new RoutedEventArgs());
        }

        private ImgurFct            imgurFct;
        private string              prevSearch;
        private List<ImageClass>    FavList;
        private List<ImageClass>    TopList;
        private bool                FavDisplay;
    }
}

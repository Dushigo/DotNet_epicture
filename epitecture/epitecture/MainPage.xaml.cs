using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;


namespace epitecture
{
    public sealed partial class MainPage : Page
    {
        public void AddImage(object sender, RoutedEventArgs e)
        {
            Image tmp = new Image();
            tmp.Source = new BitmapImage(new Uri("https://www.w3schools.com/w3css/img_fjords.jpg", UriKind.Absolute));
            ImageList.Items.Add(tmp);
        }

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

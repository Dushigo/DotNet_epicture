using System;
using Imgur.API.Authentication.Impl;
using Imgur.API.Endpoints.Impl;
using Imgur.API.Models;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;
using Imgur.API;

namespace epitecture
{
    class ImgurFct
    {
        public async Task UploadImage(string path)
        {
            try
            {
                var client = new ImgurClient("28f3f874fb3b452", "36d67daa2ca2215d576137cf71ac673faab8907e");
                var endpoint = new ImageEndpoint(client);
                IImage image;
                var fs = System.IO.File.ReadAllBytes(path);
                image = await endpoint.UploadImageBinaryAsync(fs);
                Debug.Write("Image uploaded. Image Url: " + image.Link);
            }
            catch (ImgurException imgurEx)
            {
                Debug.Write("An error occurred uploading an image to Imgur.");
                Debug.Write(imgurEx.Message);
            }
        }
        public void Search()
        {

        }
        public async Task<IImage> LoadImage()
        {
            var client = new ImgurClient("28f3f874fb3b452", "36d67daa2ca2215d576137cf71ac673faab8907e");
            var endpoint = new ImageEndpoint(client);
            var image = await endpoint.GetImageAsync("VmdDV");
            return (image);
        }

        public void SortByType()
        {

        }

        public void SortBySize()
        {

        }

        public void FavOrUnfav()
        {
            //ImageClass imageClass = (ImageClass)e.ClickedItem;

        }
    }
}

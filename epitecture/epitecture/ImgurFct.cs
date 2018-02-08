using Imgur.API.Authentication.Impl;
using Imgur.API.Endpoints.Impl;
using Imgur.API.Models;
using System.Threading.Tasks;
using System.Diagnostics;
using Imgur.API;
using Flurl;
using Flurl.Http;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Imgur.API.Models.Impl;
using System;
using System.IO;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Media;
using Windows.Storage.Streams;

namespace epitecture
{
    class ImgurFct
    {
        public async Task UploadImage(Windows.Storage.StorageFile path)
        {

            MemoryStream memoryStream = new MemoryStream();
            Stream stream = await path.OpenStreamForReadAsync();
            stream.CopyTo(memoryStream);
            byte[] result = memoryStream.ToArray();

            string base64img = System.Convert.ToBase64String(result);
            var response = await ("https://api.imgur.com/3/image")
                    .WithHeader("Authorization", "Client-ID fe43cee3f8ac2d5")
                    .PostMultipartAsync(mp => mp
                    .AddFile("image", base64img)
                    ).ReceiveString();
        }
        public string CheckImage(dynamic ressources)
        {
            try
            {
                int width = -1, height = -1;

                Int32.TryParse((string)ressources.width, out width);
                Int32.TryParse((string)ressources.height, out height);
                return (ressources.id);
            }
            catch
            {
                return null;
            }
        }
        public async Task<List<string>> Search(string ToSearch)
        {
            imageList = new List<string>();
            var fctTry = new Action<dynamic>((ressource) =>
            {
                string tmp = CheckImage(ressource);
                if (tmp != null)
                    imageList.Add(tmp);
                });
            string response;
            if (ToSearch != "")
                response = await ("https://api.imgur.com/3" + "/gallery/search/top/1")
                    .WithHeader("Authorization", "Client-ID fe43cee3f8ac2d5")
                    .SetQueryParams(new {
                        q = ToSearch,
                    })
                    .GetStringAsync();
            else
                response = await ("https://api.imgur.com/3" + "/gallery/top/1")
                    .WithHeader("Authorization", "Client-ID fe43cee3f8ac2d5")
                    .GetStringAsync();
            dynamic dataResponse = JObject.Parse(response);
            foreach (var image in dataResponse.data)
            {
                try
                {
                    foreach (var element in image.images)
                        fctTry(element);
                }
                catch
                {
                    fctTry(image);
                }
            }
            return (imageList);
        }
        public async Task<IImage> LoadImage(string ToLoad)
        {
            var client = new ImgurClient("fe43cee3f8ac2d5", "9260ff63f41a4cf0ed2168802a60973cfa1bb0b7");
            var endpoint = new ImageEndpoint(client);
            var image = await endpoint.GetImageAsync(ToLoad);
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
            /*var client = new ImgurClient("714c61ea1ce9e75", "d72ef8c7061c71e816294e2dd08c7f1d4ac48964");
            var endpoint = new OAuth2Endpoint(client);
            var token = client.OAuth2Token;//await endpoint.GetTokenByCodeAsync("access_token");
            Debug.WriteLine(token);*/
            //ImageClass imageClass = (ImageClass)e.ClickedItem;

        }
        private List<string> imageList;
    }
}

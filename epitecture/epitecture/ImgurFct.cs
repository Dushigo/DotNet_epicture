using Imgur.API.Authentication.Impl;
using Imgur.API.Endpoints.Impl;
using Imgur.API.Models;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System;
using System.IO;

namespace epitecture
{
    class ImgurFct
    {
        public async Task<IImage> Upload(ImageEndpoint endpoint, Windows.Storage.StorageFile file)
        {
            var randomAccessStream = await file.OpenReadAsync();
            Stream stream = randomAccessStream.AsStreamForRead();
            return await endpoint.UploadImageStreamAsync(stream);
        }
        public List<string> UploadImage(Windows.Storage.StorageFile file)
        {
            imageListUp = new List<string>();
            var client = new ImgurClient("fe43cee3f8ac2d5", "9260ff63f41a4cf0ed2168802a60973cfa1bb0b7");
            var endpoint = new ImageEndpoint(client);
            var image = (Task.Run<IImage>(() => Upload(endpoint, file)));
            imageListUp.Add(image.Result.Id);
            return (imageListUp);
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

        private List<string> imageList;
        private List<string> imageListUp;
    }
}

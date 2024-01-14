using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ExternalApi.ImageServer
{
    public interface IImageUploadService
    {
        UploadDto Upload(List<IFormFile> files);
    }
    public class ImageUploadService : IImageUploadService
    {
        public  UploadDto Upload(List<IFormFile> files)
        {
            var options = new RestClientOptions("https://localhost:7234")
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest("/api/Images?apiKey=mysecretkey", Method.Post);
            request.AlwaysMultipartFormData = true;
            foreach (var item in files)
            {
                byte[] bytes;
                using (var ms = new MemoryStream())
                {
                    item.CopyToAsync(ms);
                    bytes = ms.ToArray();
                }
                request.AddFile(item.FileName, bytes, item.FileName, item.ContentType);
            }
            RestResponse response =  client.Execute(request);
            Console.WriteLine(response.Content);
            UploadDto  upload = JsonConvert.DeserializeObject<UploadDto>(response.Content);
            return upload;

            //var client = new RestClient("https://localhost:7234//api/Images?apikey=mysecretkey");
            //client.Timeout = -1;
            //var request = new RestRequest(Method.POST);
            //foreach (var item in files)
            //{
            //    byte[] bytes;
            //    using (var ms = new MemoryStream())
            //    {
            //        item.CopyToAsync(ms);
            //        bytes = ms.ToArray();
            //    }
            //    request.AddFile(item.FileName, bytes, item.FileName, item.ContentType);
            //}


            //IRestResponse response = client.Execute(request);
            //UploadDto  upload = JsonConvert.DeserializeObject<UploadDto>(response.Content);
            //return upload.FileNameAddress;

        }
    }


    public class UploadDto
    {
        public bool Status { get; set; }
        public List<string> FileNameAddress { get; set; }
    }
}

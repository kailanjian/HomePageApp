using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace BingImageAPI
{
    public class BingImageClient
    {
        public readonly string ApiUrl = "http://www.bing.com/HPImageArchive.aspx?format=js&idx=0&n=1&mkt=en-US";

        public BingImageClient()
        {
            
        }

        public ImageResponse GetImageResponse()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ApiUrl);
            //Console.WriteLine($"Request: {request.RequestUri.ToString()}");
            WebResponse response = request.GetResponseAsync().Result;

            string json;

            using (Stream responseStream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(responseStream))
            {
                json = reader.ReadToEnd();
            }

            ImageResponse ir = JsonConvert.DeserializeObject<ImageResponse>(json);

            return ir;
        }
    }
}

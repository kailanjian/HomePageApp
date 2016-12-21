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
    /// <summary>
    /// Client to access data and return a CLR object
    /// </summary>
    public class BingImageClient
    {
        // URL to get data from
        public readonly string ApiUrl = 
            "http://www.bing.com/HPImageArchive.aspx"
            +"?format=js&idx=0&n=1&mkt=en-US";

        public BingImageClient()
        {
        }

        /// <summary>
        /// Request the JSON data and returns it as an ImageResponse object
        /// </summary>
        /// <returns>ImageResponse object with the data</returns>
        public ImageResponse GetImageResponse()
        {
            // Submit a request for the web page
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ApiUrl);
            WebResponse response = request.GetResponseAsync().Result;

            string json;

            // Read and store JSON
            using (Stream responseStream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(responseStream))
            {
                json = reader.ReadToEnd();
            }

            // Deserialize and return
            ImageResponse ir = 
                JsonConvert.DeserializeObject<ImageResponse>(json);

            return ir;
        }
    }
}

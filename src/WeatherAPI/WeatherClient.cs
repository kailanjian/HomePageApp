using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace WeatherAPI
{
    public class WeatherClient
    {
        private string key;
        public WeatherClient()
        {
            key = Resource.ResourceManager.GetString("APIKey");
        }

        public ConditionsResponse GetConditionsResponse()
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"http://api.wunderground.com/api/{key}/conditions/q/CA/San_Diego.json");
            Console.WriteLine($"Request: {request.RequestUri.ToString()}");
            WebResponse response = request.GetResponseAsync().Result;

            string json;

            using (Stream responseStream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(responseStream))
            {
                json = reader.ReadToEnd();
            }

            ConditionsResponse cr = JsonConvert.DeserializeObject<ConditionsResponse>(json);

            Console.WriteLine(cr.current_observation.feelslike_f);

            return cr;
        }
    }
}

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

        /// <summary>
        /// Gets data about the weather conditions (currently limited to San Diego)
        /// </summary>
        /// <returns>Response with data about the weather</returns>
        public ConditionsResponse GetConditionsResponse()
        {
            // Request URL based on Wunderground API documentation for getting 
            // city data based on API key 
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"http://api.wunderground.com/api/{key}/conditions/q/CA/San_Diego.json");
            WebResponse response = request.GetResponseAsync().Result;

            // Get JSON data from response
            string json;
            using (Stream responseStream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(responseStream))
            {
                json = reader.ReadToEnd();
            }

            // Convert JSON
            ConditionsResponse cr = JsonConvert.DeserializeObject<ConditionsResponse>(json);

            // Return the CLR data based on the request
            return cr;
        }

        /// <summary>
        /// Gets the Autocomplete API data from Wunderground
        /// </summary>
        /// <param name="query">query to search in the API</param>
        /// <returns>Response with the appropriate data (list of matching data)</returns>
        public AutocompleteResponse GetAutocompleteResponse(string query)
        {
            // Based on this page:
            // https://www.wunderground.com/weather/api/d/docs?d=autocomplete-api&MR=1
            string url = "autocomplete.wunderground.com/aq?query=" + query;

            // Get request
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            WebResponse response = request.GetResponseAsync().Result;

            // Get JSON Data
            string json;
            using (Stream responseStream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(responseStream))
            {
                json = reader.ReadToEnd();
            }

            // Convert JSON
            AutocompleteResponse ar = JsonConvert.DeserializeObject<AutocompleteResponse>(json);

            // return CLR object
            return ar;
        }
    }
}

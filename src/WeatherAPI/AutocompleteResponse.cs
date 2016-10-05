using System.Collections.Generic;

namespace WeatherAPI
{
    // Names and data about this API can be found at: 
    //https://www.wunderground.com/weather/api/d/docs?d=autocomplete-api&MR=1

    public class RESULT
    {
        public string name { get; set; }
        public string type { get; set; }
        public string c { get; set; }
        public string zmw { get; set; }
        public string tz { get; set; }
        public string tzs { get; set; }
        public string l { get; set; }
        public string ll { get; set; }
        public string lat { get; set; }
        public string lon { get; set; }
    }

    public class AutocompleteResponse
    {
        public List<RESULT> RESULTS { get; set; }
    }
}
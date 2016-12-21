using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using WeatherAPI;
using System.Text;

namespace HomePageApp.Controllers
{
    public class HomeController : Controller
    {
        private const string NAME_COOKIE = "name";
        private const string LOCATION_COOKIE = "location_link";

        // Number of results to show in the location page
        private const int LOCATION_RESULTS = 5;

        // Hours which describe the begginning of afternoon and evening
        private const int AFTERNOON_LOWER_BOUND = 12;
        private const int EVENING_LOWER_BOUND = 18;

        // How long to store cookies (in days)
        // TODO: pick reasonable value for published version
        private const int COOKIE_DAYS = 1;

        public IActionResult Index()
        {
            // Check for needed cookies
            string nameCookie = Request.Cookies[NAME_COOKIE];
            if (nameCookie == null)
                return RedirectToAction("GetName");

            string locationCookie = Request.Cookies[LOCATION_COOKIE];
            if (locationCookie == null)
                return RedirectToAction("GetLocation");


            // Request a background image
            ViewData["ShowBackground"] = true;


            // Copy nameCookie into name
            // nameCookie may be changed into a different data source
            // in the future
            string name = nameCookie;

            ViewData["Name"] = name;



            // date information to be used within page
            ViewData["Date"] = 
                string.Format("{0:dddd, MMMM d, yyyy}", DateTime.Today);


            // Get weather data based on user location

            // May change source in the future
            string locationLink = locationCookie;

            // Get data with API request
            WeatherClient wc = new WeatherClient();
            ConditionsResponse cr = wc.GetConditionsResponse(locationCookie);

            // temperature in fahrenheit, TODO: option for celsius
            ViewData["TemperatureF"] = cr.current_observation.temp_f;
            ViewData["TemperatureC"] = cr.current_observation.temp_c;

            // icon mapping based on 
            // https://erikflowers.github.io/weather-icons/api-list.html
            ViewData["WeatherIconName"] = 
                "wi-wu-" + cr.current_observation.icon;

            // precipitation data possibly needed in the future
            // ViewData["Precipitation"] = 
            //  cr.current_observation.precip_today_in;

            return View();
        }

        public IActionResult About()
        {
            // Message will be displayed on about page
            ViewData["Message"] = "About this site";

            return View();
        }

        public IActionResult GetName()
        {
            // Display the background
            ViewData["ShowBackground"] = true;

            return View();
        }

        public IActionResult GetLocation(string locationValue)
        {
            // Display the background
            ViewData["ShowBackground"] = true;

            // Limit number of options for location (user location is usually
            // in the first three options
            ViewData["NumResults"] = LOCATION_RESULTS;
            
            // Check that we posted back a location to display options for
            if (Request.Method == "POST")
            {
                // Call API
                WeatherClient client = new WeatherClient();
                AutocompleteResponse response 
                    = client.GetAutocompleteResponse(locationValue);

                // Go through the results and build a cities string to pass
                // in a serialized format
                StringBuilder cities = new StringBuilder();
                foreach (var item in response.RESULTS)
                {
                    // Filter out cities
                    if (item.type == "city")
                    {
                        // Append the data in serialized form
                        // cityname1@link1;cityname2@link2;cityname3@link3
                        // and so on
                        cities.Append(item.name);
                        cities.Append("@");
                        cities.Append(item.l);
                        cities.Append(";");
                    }
                }

                // Remove the last semicolon at the end 
                if (cities.Length > 0)
                    cities.Remove(cities.Length - 1, 1);

                ViewData["cities"] = cities.ToString();
            }


            return View();
        }

        public IActionResult SubmitLocation(string cityLink)
        {
            // Create cookie to store the location link for later use
            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.Now.AddDays(COOKIE_DAYS);
            Response.Cookies.Append(LOCATION_COOKIE, cityLink, options);

            // Now that we have the data, go to main page
            return RedirectToAction("Index");
        }

        public IActionResult SubmitName(string nameValue)
        {
            // Create cookie to store the name for later use
            CookieOptions options = new CookieOptions();

            options.Expires = DateTime.Now.AddDays(COOKIE_DAYS);
            Response.Cookies.Append(NAME_COOKIE, nameValue, options);

            // The data is stored, go to main page
            return RedirectToAction("Index");
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}

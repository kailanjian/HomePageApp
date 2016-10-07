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

        public IActionResult Index()
        {
            /*
             * Check for needed cookies
             */
            string nameCookie = Request.Cookies[NAME_COOKIE];
            if (nameCookie == null)
                return RedirectToAction("GetName");

            string locationCookie = Request.Cookies[LOCATION_COOKIE];
            if (locationCookie == null)
                return RedirectToAction("GetLocation");

            /*
             * Background Image Setup
             */

            // Once this is toggled, the cshtml layout will handle
            // the styling.
            ViewData["ShowBackground"] = true;

            /* 
             * Create the Greeting
             */

            // Copy nameCookie into name
            // nameCookie may be changed into a different data source
            // in the future
            string name = nameCookie;

            // time based greeting
            string greeting = "Good Morning";
            if (DateTime.Now.Hour >= AFTERNOON_LOWER_BOUND)
                greeting = "Good Afternoon";
            if (DateTime.Now.Hour >= EVENING_LOWER_BOUND)
                greeting = "Good Evening";

            // create the greeting
            ViewData["Greeting"] = greeting + ", " + name;


            /*
             * Store date
             */

            // date information to be used within page
            ViewData["Date"] = String.Format("{0:dddd, MMMM d, yyyy}", DateTime.Today);

            /*
             * Weather Information
             */

            // May change source in the future
            string locationLink = locationCookie;

            // Get data with API request
            WeatherClient wc = new WeatherClient();
            ConditionsResponse cr = wc.GetConditionsResponse(locationCookie);

            // temperature in fahrenheit, TODO: option for celsius
            ViewData["TemperatureF"] = cr.current_observation.feelslike_f;

            // icon mapping based on https://erikflowers.github.io/weather-icons/api-list.html
            ViewData["WeatherIconName"] = "wi-wu-" + cr.current_observation.icon;

            // precipitation data possibly needed in the future
            //ViewData["Precipitation"] = cr.current_observation.precip_today_in;

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Welcome to the Future";

            return View();
        }

        public IActionResult GetName()
        {
            ViewData["ShowBackground"] = true;

            return View();
        }

        public IActionResult GetLocation(string locationValue)
        {
            ViewData["ShowBackground"] = true;
            // TODO: put 5 as a constant somewhere
            ViewData["NumResults"] = 5;
            
            if (Request.Method == "POST")
            {
                WeatherClient client = new WeatherClient();
                AutocompleteResponse response = client.GetAutocompleteResponse(locationValue);

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
                cities.Remove(cities.Length - 1, 1);
                ViewData["cities"] = cities.ToString();
            }


            return View();
        }

        public IActionResult SubmitLocation(string cityLink)
        {
            // TODO: save link as a cookie
            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Append("location_link", cityLink, options);

            return RedirectToAction("Index");
        }

        public IActionResult SubmitName(string nameValue)
        {
            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Append("name", nameValue, options);

            return RedirectToAction("Index");
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}

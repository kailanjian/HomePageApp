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
        public IActionResult Index()
        {
            /*
             * cookies
             */
            string nameCookie = Request.Cookies["name"];
            if (nameCookie == null)
                return RedirectToAction("GetName");

            string locationCookie = Request.Cookies["location"];
            if (locationCookie == null)
                return RedirectToAction("GetLocation");

            /*
             * background image setup
             */

            ViewData["ShowBackground"] = true;
/*
            // create the image url using the api
            BingImageAPI.BingImageClient client = new BingImageAPI.BingImageClient();
            string imageUrlPart = client.GetImageResponse().images[0].url;
            string imageUrl = "http://www.bing.com" + imageUrlPart;

            // integrate the background image into the style
            ViewData["ExtraStyle"] =
                @"body 
                    { 
                        background-image: linear-gradient( rgba(0, 0, 0, 0.5), rgba(0, 0, 0, 0.5) ), url(""" + imageUrl + @""");
                        background-repeat: no-repeat;
                        background-size: inherit;
                        height: 100%;
                        text-shadow: 2px 2px 5px #000000;
                    }";

        */
            /* 
             * create the greeting
             */

            // TODO: name should be loaded via cookie
            string name = nameCookie;

            // time based greeting
            string greeting = "Good Morning";
            if (DateTime.Now.Hour >= 12)
                greeting = "Good Afternoon";
            if (DateTime.Now.Hour >= 18)
                greeting = "Good Evening";

            // create the greeting
            ViewData["Greeting"] = greeting + ", " + name;


            /*
             * date
             */

            // date information to be used within page
            ViewData["Date"] = String.Format("{0:dddd, MMMM d, yyyy}", DateTime.Today);

            /*
             * weather information
             */

            // get data from the api
            WeatherClient wc = new WeatherClient();
            ConditionsResponse cr = wc.GetConditionsResponse();

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
                        cities.Append(item.zmw);
                        cities.Append(";");
                    }
                }

                // Remove the last semicolon at the end 
                cities.Remove(cities.Length - 1, 1);
                ViewData["cities"] = cities.ToString();
            }


            return View();
        }

        public IActionResult SubmitLocationQuery(string locationValue)
        {

            return RedirectToAction("");
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

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
    public class WidgetsController : Controller
    {
        public IActionResult Search(string query)
        {
            string searchURL = "";

            // TODO: logic for search engine options
            searchURL += Constants.GOOGLE_SEARCH_BASE;

            // append the query
            searchURL += query;

            return Redirect(searchURL);
        }
    }
}

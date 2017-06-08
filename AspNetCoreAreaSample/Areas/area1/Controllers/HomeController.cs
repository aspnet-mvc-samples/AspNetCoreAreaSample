using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreAreaSample.Area1.Controllers
{
    [Area("area1")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "AREA1 Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "AREA1 Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}

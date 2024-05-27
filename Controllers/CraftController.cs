
using KhumaloCraft_Part2.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace KhumaloCraft_Part2.Controllers
{
    public class CraftController : Controller
    {
        private readonly ILogger<CraftController> _logger;

        public CraftController(ILogger<CraftController> logger)
        {
            _logger = logger;
        }


        public IActionResult AboutUs()
        {
            return View();
        }

     
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
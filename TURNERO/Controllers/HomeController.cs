using TURNERO.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TURNERO.Auth;
using Newtonsoft.Json;
using NuGet.Common;

namespace TURNERO.Controllers
{
    [ValidateSession]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        private void SesionData()
        {
            string userJson = HttpContext.Session.GetString("user");

            if (userJson == null)
            {
                RedirectToAction("Login", "Access");
            }
            else
            {
                UserModel user = JsonConvert.DeserializeObject<UserModel>(userJson);
                ViewBag.Id = user.id;
                ViewBag.Type = user.type;
            }
        }
        [HttpGet]
        public IActionResult Index()
        {
            SesionData();
            return View();
        }

        public IActionResult Privacy()
        {
            SesionData();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

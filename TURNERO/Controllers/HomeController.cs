using TURNERO.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TURNERO.Auth;
using Newtonsoft.Json;
using NuGet.Common;
using TURNERO.Data;

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
        private int SesionData()
        {
            int id = 0;
            string userJson = HttpContext.Session.GetString("user");

            if (userJson == null)
            {
                RedirectToAction("Login", "Access");
            }
            else
            {
                UserModel user = JsonConvert.DeserializeObject<UserModel>(userJson);
                id = user.id;
                ViewBag.Id = user.id;
                ViewBag.Type = user.type;
            }

            return id;
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

        //PROVIDER
        ProviderData _ProviderData = new ProviderData();
        [HttpGet]
        public IActionResult ProviderList()
        {
            SesionData();
            var ol = _ProviderData.List();
            return View(ol);
        }
        public IActionResult ProviderCreate()
        {
            SesionData();
            return View();
        }
        [HttpPost]
        public IActionResult ProviderCreate(ProviderModel oc)
        {
            int id = SesionData();
            var res = _ProviderData.Create(oc, id);
            if (res)
            {
                return RedirectToAction("ProviderList");
            }
            else
            {
                return View();
            }
        }
        [HttpGet]
        public IActionResult ProviderUpdate(int id)
        {
            SesionData();
            var oc = _ProviderData.Read(id);
            return View(oc);
        }
        [HttpPost]
        public IActionResult ProviderUpdate(ProviderModel oc)
        {
            int id = SesionData();
            
            /*if (!ModelState.IsValid)
            {
                return View();
            }*/

            var res = _ProviderData.Update(oc,id);

            if (res)
            {
                return RedirectToAction("ProviderList");
            }
            else
            {
                return View();
            }
        }
        [HttpGet]
        public IActionResult ProviderDelete(int id)
        {
            SesionData();
            var oc = _ProviderData.Read(id);
            return View(oc);
        }
        [HttpPost]
        public IActionResult ProviderDelete(ProviderModel oc)
        {
            SesionData();
            var res = _ProviderData.Delete(oc.id);

            if (res)
            {
                return RedirectToAction("ProviderList");
            }
            else
            {
                return View();
            }
        }
    }
}

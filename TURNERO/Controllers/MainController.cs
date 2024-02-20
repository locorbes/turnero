using Microsoft.AspNetCore.Mvc;
using TURNERO.Data;
using TURNERO.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace TURNERO.Controllers
{
    public class MainController : Controller
    {
        ContactData _ContactData = new ContactData();
        [HttpGet]
        public IActionResult List()
        {
            var ol = _ContactData.List();
            return View(ol);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(ContactModel oc)
        {
            var res = _ContactData.Create(oc);
            if (res)
            {
                return RedirectToAction("List");
            }
            else
            {
                return View();
            }
        }
        [HttpGet]
        public IActionResult Update(int id)
        {
            var oc = _ContactData.Read(id);
            return View(oc);
        }
        [HttpPost]
        public IActionResult Update(ContactModel oc)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var res = _ContactData.Update(oc);

            if (res)
            {
                return RedirectToAction("List");
            }
            else
            {
                return View();
            }
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var oc = _ContactData.Read(id);
            return View(oc);
        }
        [HttpPost]
        public IActionResult Delete(ContactModel oc)
        {
            var res = _ContactData.Delete(oc.id);

            if (res)
            {
                return RedirectToAction("List");
            }
            else
            {
                return View();
            }
        }
    }
}

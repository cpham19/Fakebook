using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Fakebook.Models;
using Fakebook.Services;
using System.Collections.Generic;

namespace Fakebook.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Index", "User", new { id = User.Identity.GetPersonId() });
        }

        public IActionResult Friends()
        {
            return RedirectToAction("Friends", "User", new {id=User.Identity.GetPersonId() });
        }

        public IActionResult Groups()
        {

            return RedirectToAction("Groups", "User", new { id = User.Identity.GetPersonId() });
        }

        public IActionResult Blogs()
        {
            return RedirectToAction("Blogs", "User", new { id = User.Identity.GetPersonId() });
        }

        public IActionResult Stores()
        {
            return RedirectToAction("Stores", "User", new { id = User.Identity.GetPersonId() });
        }

        public IActionResult Reviews()
        {
            return RedirectToAction("Reviews", "User", new { id = User.Identity.GetPersonId() });
        }

        public IActionResult Edit()
        {
            return RedirectToAction("Edit", "User", new { id = User.Identity.GetPersonId() });
        }

        public IActionResult Privacy()
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

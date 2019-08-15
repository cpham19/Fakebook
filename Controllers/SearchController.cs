using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Fakebook.Models;
using Fakebook.Services;
using System.Collections.Generic;

namespace Fakebook.Controllers
{
    public class SearchController : Controller
    {
        private readonly UserService userService;
        private readonly TimelineService timelineService;
        public SearchController(UserService userService, TimelineService timelineService)
        {
            this.userService = userService;
            this.timelineService = timelineService;
        }

        // This is the page that when the user clicks Search button for "first time" or when they type something in the url (Search/"Name of Person")
        [HttpGet("/Search", Name = "SearchIndex")]
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public IActionResult Index(string name)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Search", new { name = name });
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // This is the page that when the user uses the input box to search people
        [HttpGet("/Search/{**name}", Name = "SearchResult")]
        public IActionResult Search(string name)
        {
            ViewBag.name = name;
            ViewBag.Persons = userService.GetPersonsBasedOnName(User.Identity.GetPersonId(), name);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Fakebook.Models;
using Fakebook.Services;

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
        [HttpGet]
        public IActionResult Index(string name)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (name == null || name == "")
                {
                    // Search all people if no parameter
                    ViewBag.Persons = userService.GetPersons(User.Identity.GetPersonId());
                    return View();
                }
                else
                {
                    ViewBag.name = name;
                    // Search for people based on given parameter
                    ViewBag.Persons = userService.GetPersonsBasedOnName(User.Identity.GetPersonId(), name);
                    return View("Index", name);
                }
            }
            else
            {
                return Redirect("/Account/Login");
            }
        }

        // This is the page that when the user uses the input box to search people
        [HttpPost]
        public IActionResult Search(string name)
        {
            ViewBag.name = name;
            ViewBag.Persons = userService.GetPersonsBasedOnName(User.Identity.GetPersonId(), name);
            return RedirectToAction(nameof(Index), new { name = name });
            //return View("Index", name);
        }

        [HttpGet]
        public IActionResult ViewUser(string name)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (name == null || name == "")
                {
                    // Don't View anything because no paramater. Search all people if no parameter
                    ViewBag.Persons = userService.GetPersons(User.Identity.GetPersonId());
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // Search for person based on given parameter
                    var person = userService.GetPerson(name);
                    person.TimelinePosts = timelineService.GetTimelinePosts(person.PersonId);
                    return View(person);
                }
            }
            else
            {
                return Redirect("/Account/Login");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

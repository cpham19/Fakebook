using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Fakebook.Models;
using Fakebook.Services;

namespace Fakebook.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService userService;
        private readonly TimelineService timelineService;
        public UserController(UserService userService, TimelineService timelineService)
        {
            this.userService = userService;
            this.timelineService = timelineService;
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
                    return RedirectToAction("Index", "Search");
                }
                else
                {
                    // Search for person based on given parameter
                    var person = userService.GetPerson(name);
                    person.TimelinePosts = timelineService.GetTimelinePosts(person.PersonId);
                    ViewBag.Person = person;
                    //TempData["myModel"] = person;
                    return View(person);
                }
            }
            else
            {
                return Redirect("/Account/Login");
            }
        }

        // Used for adding timeline post
        [HttpPost]
        public IActionResult AddTimelinePost(string n, TimelinePost tp)
        {
            tp.PosterName = User.Identity.GetName();
            tp.DatePosted = DateTime.Now;
            timelineService.AddTimelinePost(tp);

            return RedirectToAction(nameof(ViewUser), new { name = n });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

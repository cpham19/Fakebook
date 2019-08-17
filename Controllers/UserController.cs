using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Fakebook.Models;
using Fakebook.Services;
using System.Collections.Generic;

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

        [HttpGet("/User/{**name}", Name = "ViewUser")]
        public IActionResult ViewUser(string name)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (name == null || name == "")
                {
                    // Don't View anything because no paramater. Search all people if no parameter
                    ViewBag.Persons = new List<Person>();
                    return RedirectToAction("Index", "Search");
                }
                else
                {
                    // Show the user
                    var person = userService.GetPerson(name);
                    person.TimelinePosts = timelineService.GetTimelinePosts(person.PersonId);
                    return View(person);
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        
        // Used by clicking the "View" Button in Search results
        [HttpPost]
        public IActionResult ViewPerson(string name)
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.name = name;
                return RedirectToAction("ViewUser", new { name = name });
            }
            else
            {
                return RedirectToAction("Login", "Account");
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

        // Used for replying to posts
        [HttpPost]
        public IActionResult AddReplyPost(string n, ReplyPost rp)
        {
            rp.PosterId = User.Identity.GetPersonId();
            rp.PosterName = User.Identity.GetName();
            rp.DatePosted = DateTime.Now;
            timelineService.AddReplyPost(rp);

            return RedirectToAction(nameof(ViewUser), new { name = n });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

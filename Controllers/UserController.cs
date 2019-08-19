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

        [HttpGet("/User/{name}", Name = "ViewUser")]
        public IActionResult Index(string name)
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
                    if (person.PersonId == User.Identity.GetPersonId())
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    person.TimelinePosts = timelineService.GetTimelinePosts(person.PersonId);
                    ViewBag.PersonId = User.Identity.GetPersonId();
                    return View(person);
                }
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
            tp.PosterId = User.Identity.GetPersonId();
            tp.DatePosted = DateTime.Now;
            timelineService.AddTimelinePost(tp);

            return RedirectToAction(nameof(Index), new { name = n });
        }

        // Used for replying to posts
        [HttpPost]
        public IActionResult AddReplyPost(string n, ReplyPost rp)
        {
            rp.PosterId = User.Identity.GetPersonId();
            rp.PosterName = User.Identity.GetName();
            rp.DatePosted = DateTime.Now;
            timelineService.AddReplyPost(rp);

            return RedirectToAction(nameof(Index), new { name = n });
        }

        // USed for editting a timeline post
        [HttpGet("User/{name}/EditPost/{TimelinePostId}", Name = "UserEditTimelinePost")]
        public IActionResult EditTimelinePost(string name, int TimelinePostId)
        {
            ViewBag.Name = name;
            TimelinePost tp = timelineService.GetTimelinePost(TimelinePostId);
            return View(tp);
        }

        // USed for editting a timeline post
        [HttpPost("User/{n}/EditPost/{TimelinePostId}", Name = "UserSubmitEditTimelinePost")]
        public IActionResult EditTimelinePost(string n, int TimelinePostId, TimelinePost tp)
        {
            tp.TimelinePostId = TimelinePostId;
            timelineService.EditTimelinePost(tp);
            return RedirectToAction(nameof(Index), new { name = n });
        }

        // USed for editting a reply post
        [HttpGet("User/{name}/EditReplyPost/{ReplyPostId}", Name = "UserEditReplyPost")]
        public IActionResult EditReplyPost(string name, int ReplyPostId)
        {
            ViewBag.Name = name;
            ReplyPost rp = timelineService.GetReplyPost(ReplyPostId);
            return View(rp);
        }

        // USed for editting a reply post
        [HttpPost("User/{n}/EditReplyPost/{ReplyPostId}", Name = "UserSubmitEditReplyPost")]
        public IActionResult EditReplyPost(string n, int ReplyPostId, ReplyPost rp)
        {
            rp.ReplyPostId = ReplyPostId;
            timelineService.EditReplyPost(rp);
            return RedirectToAction(nameof(Index), new { name = n});
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

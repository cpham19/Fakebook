using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Fakebook.Models;
using Fakebook.Services;

namespace Fakebook.Controllers
{
    public class HomeController : Controller
    {
        private readonly TimelineService timelineService;
        private readonly UserService userService;
        public HomeController(TimelineService timelineService, UserService userService)
        {
            this.timelineService = timelineService;
            this.userService = userService;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                Person person = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
                ViewData["Person"] = person;
                return View(timelineService.GetTimelinePosts(User.Identity.GetPersonId()));
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // Used for adding timeline post
        [HttpPost]
        public IActionResult AddTimelinePost(TimelinePost tp)
        {
            tp.PosterId = User.Identity.GetPersonId();
            tp.UserIdOfProfile = User.Identity.GetPersonId();
            tp.PosterName = User.Identity.GetName();
            tp.DatePosted = DateTime.Now;
            timelineService.AddTimelinePost(tp);
            Debug.WriteLine("HELLO");
            Debug.WriteLine("HELLO");
            Debug.WriteLine("HELLO");
            Debug.WriteLine("HELLO");
            Debug.WriteLine("HELLO");

            return RedirectToAction(nameof(Index));
        }

        // USed for editting a timeline post
        [HttpGet("/EditPost/{TimelinePostId}", Name = "EditTimelinePost")]
        public IActionResult EditTimelinePost(int TimelinePostId)
        {
            TimelinePost tp = timelineService.GetTimelinePost(TimelinePostId);
            return View(tp);
        }

        // USed for editting a timeline post
        [HttpPost("/EditPost/{TimelinePostId}", Name = "SubmitEditTimelinePost")]
        public IActionResult EditTimelinePost(int TimelinePostId, TimelinePost tp)
        {
            tp.TimelinePostId = TimelinePostId;
            timelineService.EditTimelinePost(tp);
            return RedirectToAction(nameof(Index));
        }

        // USed for editting a reply post
        [HttpGet("/EditReplyPost/{ReplyPostId}", Name = "EditReplyPost")]
        public IActionResult EditReplyPost(int ReplyPostId)
        {
            ReplyPost rp = timelineService.GetReplyPost(ReplyPostId);
            return View(rp);
        }

        // USed for editting a reply post
        [HttpPost("/EditReplyPost/{ReplyPostId}", Name = "SubmitEditReplyPost")]
        public IActionResult EditReplyPost(int ReplyPostId, ReplyPost rp)
        {
            rp.ReplyPostId = ReplyPostId;
            timelineService.EditReplyPost(rp);
            return RedirectToAction(nameof(Index));
        }

        // Used for replying to posts
        [HttpPost]
        public IActionResult AddReplyPost(ReplyPost rp)
        {
            rp.PosterId = User.Identity.GetPersonId();
            rp.PosterName = User.Identity.GetName();
            rp.DatePosted = DateTime.Now;
            timelineService.AddReplyPost(rp);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("/Edit", Name = "Edit")]
        public IActionResult Edit()
        {
            Person person = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
            ViewData["Person"] = person;
            return View();
        }

        [HttpPost("/Edit", Name = "SubmitEdit")]
        public IActionResult Edit(Person person)
        {
            person.PersonId = User.Identity.GetPersonId();
            userService.Edit(person);
            return RedirectToAction(nameof(Index));
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

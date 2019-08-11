using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Fakebook.Models;
using Fakebook.Services;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;
using System.Security.Claims;
using System.Threading;

namespace Fakebook.Controllers
{
    public class HomeController : Controller
    {
        private readonly TimelineService timelineService;
        public HomeController(TimelineService timelineService)
        {
            this.timelineService = timelineService;
        }

        // Index Page shows the user's timeline posts
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View(timelineService.GetTimelinePosts(User.Identity.GetPersonId()));
            }
            else
            {
                return Redirect("/Account/Login");
            }
        }

        // Used for adding timeline post
        [HttpPost]
        public IActionResult Index(TimelinePost tp)
        {
            tp.PersonId = User.Identity.GetPersonId();
            tp.PosterName = User.Identity.GetName();
            tp.DatePosted = DateTime.Now;
            timelineService.AddTimelinePost(tp);

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

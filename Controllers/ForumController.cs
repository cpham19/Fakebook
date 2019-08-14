using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Fakebook.Models;
using Fakebook.Services;
using Microsoft.AspNetCore.Routing;

namespace Fakebook.Controllers
{
    public class ForumController : Controller
    {
        private readonly IForumService forumService;

        public ForumController(IForumService forumService)
        {
            this.forumService = forumService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(forumService.GetForums());
        }

        [HttpGet("/Forum/{id}", Name = "ViewForum")]
        public IActionResult ViewForum(int id)
        {
            return View(forumService.GetForum(id));
        }

        [HttpGet("/Forum/{id}/{id2}", Name = "ViewTopic")]
        public IActionResult ViewTopic(int id, int id2)
        {
            return View(forumService.GetTopic(id, id2));
        }

        [HttpGet("/Forum/AddForum", Name = "AddForum")]
        public IActionResult AddForum()
        {
            return View();
        }

        [HttpPost("/Forum/AddForum", Name = "AddForum")]
        public IActionResult AddForum(Forum f)
        {
            f.PosterId = User.Identity.GetPersonId();
            forumService.AddForum(f);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("/Forum/{id}/AddTopic", Name = "AddTopic")]
        public IActionResult AddTopic(int id)
        {
            ViewBag.id = id;
            return View();
        }


        [HttpPost("/Forum/{id}/AddTopic", Name = "AddTopic")]
        public IActionResult AddTopic(int id, Topic t)
        {
            t.ForumId = id;
            t.TopicDate = DateTime.Now;
            t.PosterId = User.Identity.GetPersonId();
            Debug.WriteLine(t.ForumId);
            forumService.AddTopic(t);
            return RedirectToAction("ViewForum", new { id = id });
        }

        [HttpGet("/Forum/{id}/{id2}/AddReply", Name = "AddReply")]
        public IActionResult AddReply(int id, int id2)
        {
            ViewBag.id = id;
            ViewBag.id2 = id2;
            return View();
        }

        [HttpPost("/Forum/{id}/{id2}/AddReply", Name = "AddReply")]
        public IActionResult AddReply(int id, int id2, Reply r)
        {
            r.ReplyDate = DateTime.Now;
            r.TopicId = id2;
            r.PosterId = User.Identity.GetPersonId();
            forumService.AddReply(r);
            return RedirectToAction("ViewTopic", new { id = id , id2 = id2});
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

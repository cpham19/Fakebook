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

        [HttpGet("/Forum", Name = "ForumIndex")]
        public IActionResult Index()
        {
            return View(forumService.GetForums());
        }

        [HttpGet("/Forum/{ForumName}", Name = "ViewForum")]
        public IActionResult ViewForum(string ForumName)
        {
            string[] words = ForumName.Split("_");
            string regularName = string.Join(" ", words).ToLower();
            return View(forumService.GetForumBasedOnName(regularName));
        }

        [HttpGet("/Forum/{ForumName}/{TopicId}/{TopicName}", Name = "ViewTopic")]
        public IActionResult ViewTopic(string ForumName, int TopicId, string TopicName)
        {
            ViewBag.ForumName = ForumName;
            ViewBag.PersonId = User.Identity.GetPersonId();
            return View(forumService.GetTopic(TopicId));
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

        [HttpGet("/Forum/{ForumName}/AddTopic", Name = "AddTopic")]
        public IActionResult AddTopic(string ForumName)
        {
            ViewBag.ForumName = ForumName;
            return View();
        }


        [HttpPost("/Forum/{ForumName}/AddTopic", Name = "AddTopic")]
        public IActionResult AddTopic(string ForumName, Topic t)
        {
            Forum forum = forumService.GetForumBasedOnName(ForumName);
            t.ForumId = forum.ForumId;
            t.TopicDate = DateTime.Now;
            t.PosterId = User.Identity.GetPersonId();
            t.PosterName = User.Identity.GetName();
            forumService.AddTopic(t);
            return RedirectToAction("ViewForum", new { ForumName = ForumName });
        }

        [HttpPost("/Forum/{ForumName}/{TopicId}/{TopicName}", Name = "AddReply")]
        public IActionResult AddReply(string ForumName, int TopicId, string TopicName, Reply r)
        {
            r.ReplyDate = DateTime.Now;
            r.TopicId = TopicId;
            r.PosterId = User.Identity.GetPersonId();
            r.PosterName = User.Identity.GetName();
            forumService.AddReply(r);
            return RedirectToAction("ViewTopic", new { ForumName = ForumName , TopicId = TopicId, TopicName = TopicName});
        }

        [HttpGet("/Forum/{ForumName}/{TopicId}/{TopicName}/Edit", Name = "EditTopic")]
        public IActionResult EditTopic(string ForumName, int TopicId, string TopicName)
        {
            ViewBag.ForumName = ForumName;
            ViewBag.TopicName = TopicName;
            Topic topic = forumService.GetTopic(TopicId);
            return View(topic);
        }

        [HttpPost("/Forum/{ForumName}/{TopicId}/{TopicName}/Edit", Name = "SubmitEditTopic")]
        public IActionResult EditTopic(string ForumName, int TopicId, string TopicName, Topic t)
        {
            t.TopicId = TopicId;
            forumService.EditTopic(t);
            return RedirectToAction("ViewTopic", new { ForumName = ForumName, TopicId = TopicId, TopicName = TopicName });
        }

        [HttpGet("/Forum/{ForumName}/{TopicId}/{TopicName}/{ReplyId}/Edit", Name = "EditReply")]
        public IActionResult EditReply(string ForumName, int TopicId, string TopicName, int ReplyId)
        {
            ViewBag.ForumName = ForumName;
            ViewBag.TopicName = TopicName;
            Reply reply = forumService.GetReply(ReplyId);
            return View(reply);
        }

        [HttpPost("/Forum/{ForumName}/{TopicId}/{TopicName}/{ReplyId}/Edit", Name = "SubmitEditReply")]
        public IActionResult EditReply(string ForumName, int TopicId, string TopicName, int ReplyId, Reply r)
        {
            r.ReplyId = ReplyId;
            forumService.EditReply(r);
            return RedirectToAction("ViewTopic", new { ForumName = ForumName, TopicId = TopicId, TopicName = TopicName });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

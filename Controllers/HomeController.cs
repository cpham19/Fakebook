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
        private readonly WallService wallService;
        private readonly UserService userService;
        private readonly GroupService groupService;
        private readonly BlogService blogService;
        private readonly StoreService storeService;
        public HomeController(WallService wallService, UserService userService, GroupService groupService, BlogService blogService, StoreService storeService)
        {
            this.wallService = wallService;
            this.userService = userService;
            this.groupService = groupService;
            this.blogService = blogService;
            this.storeService = storeService;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                Person person = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
                ViewData["Person"] = person;
                ViewBag.WallPosts = wallService.GetWallPosts(User.Identity.GetPersonId());
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // Used for adding wall post
        [HttpPost]
        public IActionResult AddWallPost(WallPost tp)
        {
            tp.PosterId = User.Identity.GetPersonId();
            tp.UserIdOfProfile = User.Identity.GetPersonId();
            tp.PosterName = User.Identity.GetName();
            tp.DatePosted = DateTime.Now;
            wallService.AddWallPost(tp);

            return RedirectToAction(nameof(Index));
        }

        // USed for editting a wall post
        [HttpGet("/EditPost/{WallPostId}", Name = "EditWallPost")]
        public IActionResult EditWallPost(int WallPostId)
        {
            WallPost tp = wallService.GetWallPost(WallPostId);
            return View(tp);
        }

        // USed for editting a wall post
        [HttpPost("/EditPost/{WallPostId}", Name = "SubmitEditWallPost")]
        public IActionResult EditWallPost(int WallPostId, WallPost tp)
        {
            tp.WallPostId = WallPostId;
            wallService.EditWallPost(tp);
            return RedirectToAction(nameof(Index));
        }

        // USed for editting a reply post
        [HttpGet("/EditReplyPost/{ReplyPostId}", Name = "EditReplyPost")]
        public IActionResult EditReplyPost(int ReplyPostId)
        {
            ReplyPost rp = wallService.GetReplyPost(ReplyPostId);
            return View(rp);
        }

        // USed for editting a reply post
        [HttpPost("/EditReplyPost/{ReplyPostId}", Name = "SubmitEditReplyPost")]
        public IActionResult EditReplyPost(int ReplyPostId, ReplyPost rp)
        {
            rp.ReplyPostId = ReplyPostId;
            wallService.EditReplyPost(rp);
            return RedirectToAction(nameof(Index));
        }

        // Used for replying to posts
        [HttpPost]
        public IActionResult AddReplyPost(ReplyPost rp)
        {
            rp.PosterId = User.Identity.GetPersonId();
            rp.PosterName = User.Identity.GetName();
            rp.DatePosted = DateTime.Now;
            wallService.AddReplyPost(rp);

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

        // Doesn't work if you put a HttpDelete tag on this. Otherwise this works fine
        public IActionResult DeleteWallPost(int WallPostId)
        {
            wallService.DeleteWallPost(WallPostId);
            return RedirectToAction(nameof(Index));
        }

        // Doesn't work if you put a HttpDelete tag on this. Otherwise this works fine
        public IActionResult DeleteReplyPost(int ReplyPostId)
        {
            wallService.DeleteReplyPost(ReplyPostId);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("/MyFriends", Name = "MyFriends")]
        public IActionResult Friends()
        {
            List<Person> friends = userService.GetFriends(User.Identity.GetPersonId());
            ViewBag.Friends = friends;
            return View();
        }

        [HttpGet("/MyGroups", Name = "MyGroups")]
        public IActionResult Groups()
        {
            List<Group> groups = groupService.GetGroupsOfUser(User.Identity.GetPersonId());
            ViewBag.Groups = groups;
            return View();
        }

        [HttpGet("/MyFriends/RemoveFriend/{PersonTwoId}", Name = "RemoveFriend")]
        public IActionResult RemoveFriend(int PersonTwoId)
        {
            int id1 = User.Identity.GetPersonId();
            int id2 = PersonTwoId;
            userService.RemoveFriend(id1, id2);
            return RedirectToAction(nameof(Friends));
        }

        public IActionResult LeaveGroup(int GroupId)
        {
            groupService.LeaveGroup(User.Identity.GetPersonId(), GroupId);
            return RedirectToAction(nameof(Index));
        }


        [HttpGet("/MyBlogs", Name = "MyBlogs")]
        public IActionResult Blogs()
        {
            List<Blog> blogs = blogService.GetBlogsOfUser(User.Identity.GetPersonId());
            ViewBag.Blogs = blogs;
            return View();
        }

        [HttpGet("/MyStores", Name = "MyStores")]
        public IActionResult Stores()
        {
            List<Store> stores = storeService.GetStoresOfUser(User.Identity.GetPersonId());
            ViewBag.Stores = stores;
            return View();
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

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
                ViewBag.Me = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
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
        public IActionResult AddWallPost(WallPost wp)
        {
            ViewBag.Me = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
            wp.PosterId = User.Identity.GetPersonId();
            wp.UserIdOfProfile = User.Identity.GetPersonId();
            wp.PosterName = User.Identity.GetName();
            wp.DatePosted = DateTime.Now;
            wallService.AddWallPost(wp);

            return RedirectToAction(nameof(Index));
        }

        // USed for editting a wall post
        [HttpGet("/EditPost/{WallPostId}", Name = "EditWallPost")]
        public IActionResult EditWallPost(int WallPostId)
        {
            ViewBag.Me = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
            WallPost wp = wallService.GetWallPost(WallPostId);
            return View("_EditWallPostPartial", wp);
        }

        // USed for editting a wall post
        [HttpPost("/EditPost/{WallPostId}", Name = "SubmitEditWallPost")]
        public IActionResult EditWallPost(int WallPostId, WallPost wp)
        {
            ViewBag.Me = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
            if (ModelState.IsValid)
            {
                wp.WallPostId = WallPostId;
                wallService.EditWallPost(wp);
                return RedirectToAction(nameof(Index));
            }

            return View("_EditWallPostPartial", wp);
        }

        // USed for editting a reply post
        [HttpGet("/EditReplyPost/{ReplyPostId}", Name = "EditReplyPost")]
        public IActionResult EditReplyPost(int ReplyPostId)
        {
            ViewBag.Me = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
            ReplyPost rp = wallService.GetReplyPost(ReplyPostId);
            return View("_EditReplyPostPartial", rp);
        }

        // USed for editting a reply post
        [HttpPost("/EditReplyPost/{ReplyPostId}", Name = "SubmitEditReplyPost")]
        public IActionResult EditReplyPost(int ReplyPostId, ReplyPost rp)
        {
            ViewBag.Me = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
            if (ModelState.IsValid)
            {
                rp.ReplyPostId = ReplyPostId;
                wallService.EditReplyPost(rp);
                return RedirectToAction(nameof(Index));
            }

            return View("_EditReplyPostPartial", rp);
        }

        // Used for replying to posts
        [HttpPost]
        public IActionResult AddReplyPost(ReplyPost rp)
        {
            ViewBag.Me = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
            rp.PosterId = User.Identity.GetPersonId();
            rp.PosterName = User.Identity.GetName();
            rp.DatePosted = DateTime.Now;
            wallService.AddReplyPost(rp);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("/Edit", Name = "Edit")]
        public IActionResult Edit()
        {
            ViewBag.Me = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
            Person person = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
            ViewData["Person"] = person;
            return View();
        }

        [HttpPost("/Edit", Name = "SubmitEdit")]
        public IActionResult Edit(Person person)
        {
            ViewBag.Me = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
            person.PersonId = User.Identity.GetPersonId();
            userService.Edit(person);
            return RedirectToAction(nameof(Index));
        }

        // Doesn't work if you put a HttpDelete tag on this. Otherwise this works fine
        public IActionResult DeleteWallPost(int WallPostId)
        {
            ViewBag.Me = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
            wallService.DeleteWallPost(WallPostId);
            return RedirectToAction(nameof(Index));
        }

        // Doesn't work if you put a HttpDelete tag on this. Otherwise this works fine
        public IActionResult DeleteReplyPost(int ReplyPostId)
        {
            ViewBag.Me = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
            wallService.DeleteReplyPost(ReplyPostId);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("/MyFriends", Name = "MyFriends")]
        public IActionResult Friends()
        {
            ViewBag.Me = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
            ViewBag.Controller = "Home";
            List<Person> friends = userService.GetFriends(User.Identity.GetPersonId());
            ViewBag.Friends = friends;
            return View("_FriendsPartial");
        }

        [HttpGet("/MyGroups", Name = "MyGroups")]
        public IActionResult Groups()
        {
            ViewBag.Me = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
            ViewBag.Controller = "Home";
            ViewBag.PersonId = User.Identity.GetPersonId();
            List<Group> groups = groupService.GetGroupsOfUser(User.Identity.GetPersonId());
            ViewBag.Groups = groups;
            return View("_GroupsPartial");
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
            ViewBag.Me = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
            List<Blog> blogs = blogService.GetBlogsOfUser(User.Identity.GetPersonId());
            ViewBag.Blogs = blogs;
            return View("_BlogsPartial");
        }

        [HttpGet("/MyStores", Name = "MyStores")]
        public IActionResult Stores()
        {
            ViewBag.Me = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
            List<Store> stores = storeService.GetStoresOfUser(User.Identity.GetPersonId());
            ViewBag.Stores = stores;
            return View("_StoresPartial");
        }

        [HttpGet("/MyReviews", Name = "MyReviews")]
        public IActionResult Reviews()
        {
            ViewBag.Me = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
            List<StoreItem> itemsReviewed = storeService.GetReviewsOfUser(User.Identity.GetPersonId());
            ViewBag.ItemsReviewed = itemsReviewed;
            return View("_ReviewsPartial");
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

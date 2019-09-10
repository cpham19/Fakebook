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
        private readonly WallService wallService;
        private readonly UserService userService;
        private readonly GroupService groupService;
        private readonly BlogService blogService;
        private readonly StoreService storeService;
        public UserController(WallService wallService, UserService userService, GroupService groupService, BlogService blogService, StoreService storeService)
        {
            this.wallService = wallService;
            this.userService = userService;
            this.groupService = groupService;
            this.blogService = blogService;
            this.storeService = storeService;
        }

        public IActionResult RedirectToIndex()
        {
            return RedirectToAction("Index", new { id = User.Identity.GetPersonId() });
        }

        [HttpGet("/User/{id}", Name = "ViewUser")]
        public IActionResult Index(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                // Show the user
                var person = userService.GetPersonBasedOnId(id);
                ViewBag.Me = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
                person.WallPosts = wallService.GetWallPosts(person.PersonId);
                ViewBag.PersonId = User.Identity.GetPersonId();
                if(person.PersonId != ViewBag.PersonId && userService.CheckFriends(person.PersonId, ViewBag.PersonId))
                {
                    person.IsFriend = true;
                }

                return View(person);

            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // Used for adding wall post
        [HttpPost]
        public IActionResult AddWallPost(int id, WallPost wp)
        {
            wp.PosterName = User.Identity.GetName();
            wp.PosterId = User.Identity.GetPersonId();
            wp.DatePosted = DateTime.Now;
            wallService.AddWallPost(wp);

            return RedirectToAction(nameof(Index), new { id = wp.PosterId });
        }

        // Used for replying to posts
        [HttpPost]
        public IActionResult AddReplyPost(int id, ReplyPost rp)
        {
            rp.PosterId = User.Identity.GetPersonId();
            rp.PosterName = User.Identity.GetName();
            rp.DatePosted = DateTime.Now;
            wallService.AddReplyPost(rp);

            return RedirectToAction(nameof(Index), new { id = rp.PosterId });
        }

        // USed for editting a wall post
        [HttpGet("User/{id}/EditPost/{WallPostId}", Name = "UserEditWallPost")]
        public IActionResult EditWallPost(int id, int WallPostId)
        {
            ViewBag.Me = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
            ViewBag.Id = id;
            WallPost wp = wallService.GetWallPost(WallPostId);
            return View("_EditWallPostPartial", wp);
        }

        // USed for editting a wall post
        [HttpPost("User/{id}/EditPost/{WallPostId}", Name = "UserSubmitEditWallPost")]
        public IActionResult EditWallPost(int id, int WallPostId, WallPost wp)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Me = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
                wp.WallPostId = WallPostId;
                wallService.EditWallPost(wp);
                return RedirectToAction(nameof(Index), new { id = id });
            }
            return View("_EditWallPostPartial", wp);
        }

        // USed for editting a reply post
        [HttpGet("User/{id}/EditReplyPost/{ReplyPostId}", Name = "UserEditReplyPost")]
        public IActionResult EditReplyPost(int id, int ReplyPostId)
        {
            ViewBag.Me = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
            ViewBag.Id = id;
            ReplyPost rp = wallService.GetReplyPost(ReplyPostId);
            return View("_EditReplyPostPartial", rp);
        }

        // USed for editting a reply post
        [HttpPost("User/{id}/EditReplyPost/{ReplyPostId}", Name = "UserSubmitEditReplyPost")]
        public IActionResult EditReplyPost(int id, int ReplyPostId, ReplyPost rp)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Me = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
                rp.ReplyPostId = ReplyPostId;
                wallService.EditReplyPost(rp);
                return RedirectToAction(nameof(Index), new { id = id });
            }

            return View("_EditReplyPostPartial", rp);
        }

        // Doesn't work if you put a HttpDelete tag on this. Otherwise this works fine
        public IActionResult DeleteWallPost(int id, int WallPostId)
        {
            wallService.DeleteWallPost(WallPostId);
            return RedirectToAction(nameof(Index), new { id = id });
        }

        // Doesn't work if you put a HttpDelete tag on this. Otherwise this works fine
        public IActionResult DeleteReplyPost(int id, int ReplyPostId)
        {
            wallService.DeleteReplyPost(ReplyPostId);
            return RedirectToAction(nameof(Index), new { id = id });
        }


        [HttpGet("/User/{id}/Friends", Name = "UserFriends")]
        public IActionResult Friends(int id)
        {
            ViewBag.Me = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
            List<Person> friends = userService.GetFriends(id);
            foreach (var friend in friends)
            {
                // Check if people's friends are friends of the user (You) so that you can remove them on your User page or from their user page
                if (userService.CheckFriends(friend.PersonId, User.Identity.GetPersonId()))
                {
                    friend.IsFriend = true;
                }
            }

            ViewBag.Friends = friends;
            return View("Friends");
        }

        [HttpGet("/User/{id}/Groups", Name = "UserGroups")]
        public IActionResult Groups(int id)
        {
            ViewBag.Me = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
            List<Group> groups = groupService.GetGroupsOfUser(id);
            ViewBag.Groups = groups;
            return View("Groups");
        }

        [HttpGet("/User/{id}/Blogs", Name = "UserBlogs")]
        public IActionResult Blogs(int id)
        {
            ViewBag.Me = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
            List<Blog> blogs = blogService.GetBlogsOfUser(id);
            ViewBag.Blogs = blogs;
            return View("Blogs");
        }

        [HttpGet("/User/{id}/Stores", Name = "UserStores")]
        public IActionResult Stores(int id)
        {
            ViewBag.Me = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
            List<Store> stores = storeService.GetStoresOfUser(id);
            ViewBag.Stores = stores;
            return View("Stores");
        }

        [HttpGet("/User/{id}/Reviews", Name = "UserReviews")]
        public IActionResult Reviews(int id)
        {
            ViewBag.Me = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
            ViewBag.Person = userService.GetPersonBasedOnId(id);
            ViewBag.Reviews = storeService.GetReviewsOfUser(id);
            return View("Reviews");
        }

        [HttpGet("/User/{id}/Edit", Name = "Edit")]
        public IActionResult Edit(int id)
        {
            ViewBag.Me = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
            Person person = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
            ViewData["Person"] = person;
            return View();
        }

        [HttpPost("/User/{id}/Edit", Name = "SubmitEdit")]
        public IActionResult Edit(Person person)
        {
            ViewBag.Me = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
            person.PersonId = User.Identity.GetPersonId();
            userService.Edit(person);
            return RedirectToAction(nameof(Index), new { id = person.PersonId });
        }

        public IActionResult AddFriend(int PersonTwoId)
        {
            int id1 = User.Identity.GetPersonId();
            int id2 = PersonTwoId;
            Friend rel = new Friend();
            rel.PersonOneId = id1;
            rel.PersonTwoId = id2;
            userService.AddFriend(rel);
            return RedirectToAction(nameof(Index), new { id = id2 });
        }

        public IActionResult RemoveFriend(int PersonTwoId)
        {
            int id1 = User.Identity.GetPersonId();
            int id2 = PersonTwoId;
            userService.RemoveFriend(id1, id2);
            return RedirectToAction(nameof(Index), new { id = id2 });
        }

        public IActionResult LeaveGroup(int GroupId)
        {
            groupService.LeaveGroup(User.Identity.GetPersonId(), GroupId);
            return RedirectToAction(nameof(Groups), new { id = User.Identity.GetPersonId() });
        }

        public IActionResult DeleteGroup(int GroupId)
        {
            groupService.LeaveGroup(User.Identity.GetPersonId(), GroupId);
            return RedirectToAction(nameof(Groups), new { id = User.Identity.GetPersonId()});
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Fakebook.Services;
using Fakebook.Models;

namespace Fakebook.Controllers
{
    public class GroupController : Controller
    {
        private readonly GroupService groupService;
        private readonly WallService wallService;
        private readonly UserService userService;

        public GroupController(GroupService groupService, WallService wallService, UserService userService)
        {
            this.groupService = groupService;
            this.wallService = wallService;
            this.userService = userService;
        }

        [HttpGet("/Groups", Name = "Groups")]
        public IActionResult Index()
        {
            ViewBag.Me = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
            ViewBag.PersonId = User.Identity.GetPersonId();
            ViewBag.Groups = groupService.GetGroups(User.Identity.GetPersonId());
            return View();
        }

        [HttpGet("/Groups/AddGroup", Name = "AddGroup")]
        public IActionResult AddGroup()
        {
            ViewBag.Me = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
            return View();
        }

        [HttpPost("/Groups/AddGroup", Name = "SubmitAddGroup")]
        public IActionResult AddGroup(Group g)
        {
            ViewBag.Me = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
            if (ModelState.IsValid)
            {   
                g.GroupCreatorId = User.Identity.GetPersonId();
                g.DateCreated = DateTime.Now;
                groupService.AddGroup(g);
                return RedirectToAction(nameof(Index));
            }

            return View(g);
        }

        [HttpGet("/Group/{GroupId}", Name = "ViewGroup")]
        public IActionResult ViewGroup(int GroupId)
        {
            ViewBag.Me = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
            Group group = groupService.GetGroup(User.Identity.GetPersonId(), GroupId);
            ViewBag.PersonId = User.Identity.GetPersonId();
            return View(group);
        }

        public IActionResult JoinGroup(int GroupId)
        {
            GroupMember gm = new GroupMember
            {
                GroupId = GroupId,
                GroupMemberId = User.Identity.GetPersonId()
            };
            groupService.JoinGroup(gm);
            return RedirectToAction("ViewGroup", new {GroupId = GroupId});
        }

        public IActionResult LeaveGroup(int GroupId)
        {
            groupService.LeaveGroup(User.Identity.GetPersonId(), GroupId);
            return RedirectToAction("ViewGroup", new { GroupId = GroupId });
        }

        // Used for adding wall post
        [HttpPost]
        public IActionResult AddWallPost(int GroupId, WallPost wp)
        {
            wp.PosterName = User.Identity.GetName();
            wp.PosterId = User.Identity.GetPersonId();
            wp.GroupId = GroupId;
            wp.DatePosted = DateTime.Now;
            wallService.AddWallPost(wp);

            return RedirectToAction(nameof(ViewGroup), new { GroupId = GroupId });
        }

        // Used for replying to posts
        [HttpPost]
        public IActionResult AddReplyPost(int GroupId, ReplyPost rp)
        {
            rp.PosterId = User.Identity.GetPersonId();
            rp.PosterName = User.Identity.GetName();
            rp.DatePosted = DateTime.Now;
            wallService.AddReplyPost(rp);

            return RedirectToAction("ViewGroup", new { GroupId = GroupId });
        }

        // USed for editting a wall post
        [HttpGet("Group/{GroupId}/EditPost/{WallPostId}", Name = "GroupEditWallPost")]
        public IActionResult EditWallPost(int GroupId, int WallPostId)
        {
            ViewBag.Me = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
            ViewBag.GroupId = GroupId;
            WallPost wp = wallService.GetWallPost(WallPostId);
            return View("_EditWallPostPartial", wp);
        }

        // USed for editting a wall post
        [HttpPost("Group/{GroupId}/EditPost/{WallPostId}", Name = "GroupSubmitEditWallPost")]
        public IActionResult EditWallPost(int GroupId, int WallPostId, WallPost wp)
        {
            ViewBag.Me = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
            if (ModelState.IsValid)
            {
                wp.WallPostId = WallPostId;
                wallService.EditWallPost(wp);
                return RedirectToAction(nameof(ViewGroup), new { GroupId = GroupId });
            }

            return View("_EditWallPostPartial", wp);
        }

        // USed for editting a reply post
        [HttpGet("Group/{GroupId}/EditReplyPost/{ReplyPostId}", Name = "GroupEditReplyPost")]
        public IActionResult EditReplyPost(int GroupId, int ReplyPostId)
        {
            ViewBag.Me = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
            ViewBag.GroupId = GroupId;
            ReplyPost rp = wallService.GetReplyPost(ReplyPostId);
            return View("_EditReplyPostPartial", rp);
        }

        // USed for editting a reply post
        [HttpPost("Group/{GroupId}/EditReplyPost/{ReplyPostId}", Name = "GroupSubmitEditReplyPost")]
        public IActionResult EditReplyPost(int GroupId, int ReplyPostId, ReplyPost rp)
        {
            ViewBag.Me = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
            if (ModelState.IsValid)
            {
                rp.ReplyPostId = ReplyPostId;
                wallService.EditReplyPost(rp);
                return RedirectToAction(nameof(ViewGroup), new { GroupId = GroupId });
            }

            return View("_EditReplyPostPartial", rp);
        }

        // Doesn't work if you put a HttpDelete tag on this. Otherwise this works fine
        public IActionResult DeleteWallPost(int GroupId, int WallPostId)
        {
            wallService.DeleteWallPost(WallPostId);
            return RedirectToAction(nameof(ViewGroup), new { GroupId = GroupId });
        }

        // Doesn't work if you put a HttpDelete tag on this. Otherwise this works fine
        public IActionResult DeleteReplyPost(int GroupId, int ReplyPostId)
        {
            wallService.DeleteReplyPost(ReplyPostId);
            return RedirectToAction(nameof(ViewGroup), new { GroupId = GroupId });
        }

        // Delete Group
        public IActionResult DeleteGroup(int GroupId)
        {
            groupService.DeleteGroup(GroupId);
            return RedirectToAction(nameof(Index));
        }
    }
}
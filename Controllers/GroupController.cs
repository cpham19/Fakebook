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

        public GroupController(GroupService groupService, WallService wallService)
        {
            this.groupService = groupService;
            this.wallService = wallService;
        }

        [HttpGet("/Groups", Name = "Groups")]
        public IActionResult Index()
        {
            ViewBag.Groups = groupService.GetGroups(User.Identity.GetPersonId());
            return View();
        }

        public IActionResult Group()
        {
            return View();
        }

        [HttpGet("/Group/AddGroup", Name = "AddGroup")]
        public IActionResult AddGroup()
        {
            return View();
        }

        [HttpPost("/Group/AddGroup", Name = "AddGroup")]
        public IActionResult AddGroup(Group g)
        {
            g.GroupCreatorId = User.Identity.GetPersonId();
            g.DateCreated = DateTime.Now;
            groupService.AddGroup(g);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("/Group/{GroupId}", Name = "ViewGroup")]
        public IActionResult ViewGroup(int GroupId)
        {
            Group g = groupService.GetGroup(User.Identity.GetPersonId(), GroupId);
            ViewBag.Group = g;
            ViewBag.PersonId = User.Identity.GetPersonId();
            return View();
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
        public IActionResult AddWallPost(int GroupId, WallPost tp)
        {
            tp.PosterName = User.Identity.GetName();
            tp.PosterId = User.Identity.GetPersonId();
            tp.GroupId = GroupId;
            tp.DatePosted = DateTime.Now;
            wallService.AddWallPost(tp);

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
            ViewBag.GroupId = GroupId;
            WallPost tp = wallService.GetWallPost(WallPostId);
            return View(tp);
        }

        // USed for editting a wall post
        [HttpPost("Group/{GroupId}/EditPost/{WallPostId}", Name = "GroupSubmitEditWallPost")]
        public IActionResult EditWallPost(int GroupId, int WallPostId, WallPost tp)
        {
            tp.WallPostId = WallPostId;
            wallService.EditWallPost(tp);
            return RedirectToAction(nameof(ViewGroup), new { GroupId = GroupId });
        }

        // USed for editting a reply post
        [HttpGet("Group/{GroupId}/EditReplyPost/{ReplyPostId}", Name = "GroupEditReplyPost")]
        public IActionResult EditReplyPost(int GroupId, int ReplyPostId)
        {
            ViewBag.GroupId = GroupId;
            ReplyPost rp = wallService.GetReplyPost(ReplyPostId);
            return View(rp);
        }

        // USed for editting a reply post
        [HttpPost("Group/{GroupId}/EditReplyPost/{ReplyPostId}", Name = "GroupSubmitEditReplyPost")]
        public IActionResult EditReplyPost(int GroupId, int ReplyPostId, ReplyPost rp)
        {
            rp.ReplyPostId = ReplyPostId;
            wallService.EditReplyPost(rp);
            return RedirectToAction(nameof(ViewGroup), new { GroupId = GroupId });
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
    }
}
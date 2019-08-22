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
        public GroupController(GroupService groupService)
        {
            this.groupService = groupService;
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

        public IActionResult JoinGroup(int GroupId)
        {
            GroupMember gm = new GroupMember
            {
                GroupId = GroupId,
                GroupMemberId = User.Identity.GetPersonId()
            };
            groupService.JoinGroup(gm);
            return RedirectToAction(nameof(Index));
        }
    }
}
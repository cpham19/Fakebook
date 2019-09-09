using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Fakebook.Models;
using Fakebook.Services;
using System.Collections.Generic;

namespace Fakebook.Controllers
{
    public class SearchController : Controller
    {
        private readonly UserService userService;
        private readonly WallService wallService;
        public SearchController(UserService userService, WallService wallService)
        {
            this.userService = userService;
            this.wallService = wallService;
        }

        // This is the page that when the user clicks Search button on the navbar
        [HttpGet("/Search", Name = "SearchIndex")]
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.Me = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
                ViewBag.PersonId = User.Identity.GetPersonId();
                ViewBag.Persons = userService.GetPeople(User.Identity.GetPersonId());
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // This is the page that when the user enters a name in the searchbar (in the navbar or in Search page)
        public IActionResult RedirectToSearch(string name)
        {
            return RedirectToAction("Search", new { name = name });
        }


        // This is the page that when the user uses the input box to search people
        [HttpGet("/Search/{**name}", Name = "SearchResult")]
        public IActionResult Search(string name)
        {
            ViewBag.Me = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
            ViewBag.name = name;
            ViewBag.PersonOneId = User.Identity.GetPersonId();
            ViewBag.Persons = userService.GetPersonsBasedOnName(User.Identity.GetPersonId(), name);
            return View();
        }

        public IActionResult RemoveFriend(string name, int PersonTwoId)
        {
            int id1 = User.Identity.GetPersonId();
            int id2 = PersonTwoId;
            userService.RemoveFriend(id1, id2);
            if (name == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Search", new { name = name });
        }

        public IActionResult AddFriend(string name, int PersonTwoId)
        {
            Friend relationship = new Friend();
            relationship.PersonOneId = User.Identity.GetPersonId();
            relationship.PersonTwoId = PersonTwoId;
            relationship.StatusCode = 1;
            userService.AddFriend(relationship);
            if (name == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Search", new { name = name });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

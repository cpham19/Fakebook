using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Fakebook.Models;
using Fakebook.Services;
using System.Collections.Generic;

namespace Fakebook.Controllers
{
    public class BlogController : Controller
    {
        private readonly BlogService blogService;
        private readonly UserService userService;

        public BlogController(BlogService blogService, UserService userService)
        {
            this.blogService = blogService;
            this.userService = userService;
        }

        [HttpGet("/Blogs", Name = "BlogIndex")]
        public IActionResult Index()
        {
            ViewBag.Me = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.PersonId = User.Identity.GetPersonId();
                ViewBag.Blogs = blogService.GetBlogs();
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpGet("/Blogs/AddBlog", Name = "AddBlog")]
        public IActionResult AddBlog()
        {
            ViewBag.Me = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost("/Blogs/AddBlog", Name = "AddBlog")]
        public IActionResult AddBlog(Blog blog)
        {
            ViewBag.Me = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    blog.DatePosted = DateTime.Now;
                    blog.PosterId = User.Identity.GetPersonId();
                    blogService.AddBlog(blog);
                    return RedirectToAction(nameof(Index));
                }

                return View(blog);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // Used for editting blog
        [HttpGet("/Blogs/blog/{BlogId}/EditBlog", Name = "EditBlog")]
        public IActionResult EditBlog(int BlogId)
        {
            ViewBag.Me = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
            Blog blog = blogService.GetBlog(BlogId);
            return View(blog);
        }

        // Used for editting blog
        [HttpPost("/Blogs/blog/{BlogId}/EditBlog", Name = "SubmitEditBlog")]
        public IActionResult EditBlog(int BlogId, Blog blog)
        {
            ViewBag.Me = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
            if (ModelState.IsValid)
            {
                blog.BlogId = BlogId;
                blogService.EditBlog(blog);
                return RedirectToAction("ReadBlog", new { BlogId = BlogId });
            }
            return View(blog);
        }

        [HttpGet("/Blogs/blog/{BlogId}/DeleteBlog", Name = "DeleteBlog")]
        public IActionResult DeleteBlog(int BlogId)
        {
            blogService.DeleteBlog(BlogId);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("/Blogs/blog/{BlogId}", Name = "ReadBlog")]
        public IActionResult ReadBlog(int BlogId)
        {
            ViewBag.Me = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
            ViewBag.PersonId = User.Identity.GetPersonId();
            Blog blog = blogService.GetBlog(BlogId);
            return View(blog);
        }

        [HttpPost]
        public IActionResult AddBlogComment(BlogComment bc)
        {
            bc.PosterId = User.Identity.GetPersonId();
            bc.DatePosted = DateTime.Now;
            blogService.AddBlogComment(bc);
            return RedirectToAction("ReadBlog", new { BlogId = bc.BlogId }); 
        }

        // Used for editting blog comment
        [HttpGet("/Blogs/{BlogId}/EditComment/{BlogCommentId}", Name = "EditBlogComment")]
        public IActionResult EditBlogComment(int BlogId, int BlogCommentId)
        {
            ViewBag.Me = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
            BlogComment blogComment = blogService.GetBlogComment(BlogCommentId);
            return View(blogComment);
        }

        // Used for editting blog comment
        [HttpPost("/Blogs/{BlogId}/EditComment/{BlogCommentId}", Name = "SubmitEditBlogComment")]
        public IActionResult EditBlogComment(int BlogId, int BlogCommentId, BlogComment bc)
        {
            ViewBag.Me = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
            if (ModelState.IsValid)
            {
                bc.BlogCommentId = BlogCommentId;
                bc.BlogId = BlogId;
                blogService.EditBlogComment(bc);
                return RedirectToAction("ReadBlog", new { BlogId = BlogId });
            }

            return View(bc);
        }

        [HttpGet("/Blogs/Blog/{BlogId}/DeleteBlogComment/{BlogCommentId}", Name = "DeleteBlogComment")]
        public IActionResult DeleteBlogComment(int BlogId, int BlogCommentId)
        {
            ViewBag.Me = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
            blogService.DeleteBlogComment(BlogCommentId);
            return RedirectToAction("ReadBlog", new { BlogId = BlogId });
        }

        public IActionResult SearchQuery(string s)
        {
            return RedirectToAction("Search", new { s = s });
        }

        [HttpGet("/Blogs/search/{s}", Name = "SearchBlog")]
        public IActionResult Search(string s)
        {
            ViewBag.Me = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
            ViewBag.Query = s;

            string[] words = s.Split(" ");
            ViewBag.Blogs = null;
            if (words.Length > 1)
            {
                ViewBag.Blogs = blogService.SearchWords(words);
            }
            else
            {
                ViewBag.Blogs = blogService.Search(s);
            }
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

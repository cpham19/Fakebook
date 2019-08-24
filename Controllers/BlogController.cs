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
        public BlogController(BlogService blogService)
        {
            this.blogService = blogService;
        }

        [HttpGet("/Blogs", Name = "BlogIndex")]
        public IActionResult Index()
        {
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
            if (User.Identity.IsAuthenticated)
            {
                blog.DatePosted = DateTime.Now;
                blog.PosterId = User.Identity.GetPersonId();
                blogService.AddBlog(blog);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // Used for editting blog
        [HttpGet("/Blogs/EditBlog/{BlogId}", Name = "EditBlog")]
        public IActionResult EditBlog(int BlogId)
        {
            Blog blog = blogService.GetBlog(BlogId);
            return View(blog);
        }

        // Used for editting blog
        [HttpPost("/Blogs/EditBlog/{BlogId}", Name = "SubmitEditBlog")]
        public IActionResult EditBlog(int BlogId, Blog blog)
        {
            blog.BlogId = BlogId;
            blogService.EditBlog(blog);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("/Blogs/DeleteBlog/{BlogId}", Name = "DeleteBlog")]
        public IActionResult DeleteBlog(int BlogId)
        {
            blogService.DeleteBlog(BlogId);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("/Blogs/blog/{BlogId}", Name = "ReadBlog")]
        public IActionResult ReadBlog(int BlogId)
        {
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
            BlogComment blogComment = blogService.GetBlogComment(BlogCommentId);
            return View(blogComment);
        }

        // Used for editting blog comment
        [HttpPost("/Blogs/{BlogId}/EditComment/{BlogCommentId}", Name = "SubmitEditBlogComment")]
        public IActionResult EditBlogComment(int BlogId, int BlogCommentId, BlogComment bc)
        {
            bc.BlogCommentId = BlogCommentId;
            bc.BlogId = BlogId;
            blogService.EditBlogComment(bc);
            return RedirectToAction("ReadBlog", new { BlogId = BlogId });
        }

        [HttpGet("/Blogs/Blog/{BlogId}/DeleteBlogComment/{BlogCommentId}", Name = "DeleteBlogComment")]
        public IActionResult DeleteBlogComment(int BlogId, int BlogCommentId)
        {
            blogService.DeleteBlogComment(BlogCommentId);
            return RedirectToAction("ReadBlog", new { BlogId = BlogId });
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

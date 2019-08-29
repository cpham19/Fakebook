using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Fakebook.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Diagnostics;

// Used for wall posts and their replies
namespace Fakebook.Services
{
    public static class StringExtensions
    {
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source?.IndexOf(toCheck, comp) >= 0;
        }
    }

    public class BlogService
    {
        private readonly AppDbContext db;

        public BlogService(AppDbContext db)
        {
            this.db = db;
        }

        public Blog GetBlog(int BlogId)
        {
            Blog blog = db.Blogs.Where(b => b.BlogId == BlogId).SingleOrDefault();
            Person person = db.Persons.Where(p => p.PersonId == blog.PosterId).SingleOrDefault();
            blog.PosterName = person.Name;
            List<BlogComment> comments = db.BlogComments.Where(bc => bc.BlogId == BlogId).ToList();
            foreach (var comment in comments)
            {
                Person person2 = db.Persons.Where(p => p.PersonId == comment.PosterId).SingleOrDefault();
                comment.PosterName = person2.Name;
            }
            blog.BlogComments = comments;

            return blog;
        }

        public BlogComment GetBlogComment(int BlogCommentId)
        {
            return db.BlogComments.Where(bc => bc.BlogCommentId == BlogCommentId).SingleOrDefault();
        }

        public List<Blog> GetBlogs()
        {
            List<Blog> blogs = db.Blogs.OrderByDescending(b => b.DatePosted).ToList();
            foreach (var blog in blogs)
            {
                Person person = db.Persons.Where(p => p.PersonId == blog.PosterId).SingleOrDefault();
                blog.PosterName = person.Name;
            }
            return blogs;
        }

        public List<Blog> Search(string s)
        {
            List<Blog> blogs = db.Blogs
                .Where(b => b.Headline.Contains(s, StringComparison.OrdinalIgnoreCase) == true || 
                            b.Description.Contains(s, StringComparison.OrdinalIgnoreCase) == true ||
                            b.Title.Contains(s, StringComparison.OrdinalIgnoreCase) == true)
                .OrderByDescending(b => b.DatePosted).ToList();
            foreach (var blog in blogs)
            {
                Person person = db.Persons.Where(p => p.PersonId == blog.PosterId).SingleOrDefault();
                blog.PosterName = person.Name;
            }
            return blogs;
        }

        public List<Blog> SearchWords(string[] words)
        {
            List<Blog> blogs = new List<Blog>();
                
            foreach(var word in words)
            {
                List<Blog> blogs1 = db.Blogs
                .Where(b => b.Headline.Contains(word, StringComparison.OrdinalIgnoreCase) == true ||
                            b.Description.Contains(word, StringComparison.OrdinalIgnoreCase) == true ||
                            b.Title.Contains(word, StringComparison.OrdinalIgnoreCase) == true)
                            .OrderByDescending(b => b.DatePosted).ToList();
                blogs.AddRange(blogs1);
            }
             
            foreach (var blog in blogs)
            {
                Person person = db.Persons.Where(p => p.PersonId == blog.PosterId).SingleOrDefault();
                blog.PosterName = person.Name;
            }
            return blogs.Distinct().ToList();
        }

        public List<Blog> GetBlogsOfUser(int PersonId)
        {
            return db.Blogs.Where(b => b.PosterId == PersonId).ToList();
        }

        public void AddBlog(Blog b)
        {
            db.Blogs.Add(b);
            db.SaveChanges();
        }

        public void EditBlog(Blog b)
        {
            Blog blog = this.GetBlog(b.BlogId);
            blog.Headline = b.Headline;
            blog.PictureUrl = b.PictureUrl;
            blog.Description = b.Description;
            db.SaveChanges();
        }

        public void DeleteBlog(int BlogId)
        {
            Blog blog = this.GetBlog(BlogId);
            List<BlogComment> blogComments = db.BlogComments.Where(bc => bc.BlogId == BlogId).ToList();
            foreach (var comment in blogComments)
            {
                db.BlogComments.Remove(comment);
            }
            db.Blogs.Remove(blog);
            db.SaveChanges();
        }

        public void AddBlogComment(BlogComment bc)
        {
            db.BlogComments.Add(bc);
            db.SaveChanges();
        }

        public void EditBlogComment(BlogComment bc)
        {
            BlogComment blogComment = this.GetBlogComment(bc.BlogCommentId);
            blogComment.Description = bc.Description;
            db.SaveChanges();
        }

        public void DeleteBlogComment(int BlogCommentId)
        {
            BlogComment blogComment = this.GetBlogComment(BlogCommentId);
            db.BlogComments.Remove(blogComment);
            db.SaveChanges();
        }
    }
}


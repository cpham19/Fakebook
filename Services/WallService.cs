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
    public class WallService
    {
        private readonly AppDbContext db;

        public WallService(AppDbContext db)
        {
            this.db = db;
        }

        public WallPost GetWallPost(int id)
        {
            return db.WallPosts.Where(tp => tp.WallPostId == id).SingleOrDefault();
        }

        public ReplyPost GetReplyPost(int id)
        {
            return db.ReplyPosts.Where(rp => rp.ReplyPostId == id).SingleOrDefault();
        }

        public List<ReplyPost> GetReplyPostsByWallPostId(int id)
        {
            return db.ReplyPosts.Where(rp => rp.WallPostId == id).ToList();
        }

        // Get user's wall posts and their replies
        public List<WallPost> GetWallPosts(int id)
        {
            List<WallPost> wallPosts = db.WallPosts.Where(tp => tp.UserIdOfProfile == id).OrderByDescending(tp => tp.DatePosted).ToList();
            foreach (var tp in wallPosts) {
                tp.Replies = db.ReplyPosts.Where(reply => reply.WallPostId == tp.WallPostId).OrderBy(reply => reply.DatePosted).ToList();
            }
            return wallPosts;
        }

        // Add a new wall post to their page (or other people's page perhaps)
        public void AddWallPost(WallPost tp)
        {
            db.WallPosts.Add(tp);
            db.SaveChanges();
        }

        // Add a new Reply Post
        public void AddReplyPost(ReplyPost rp)
        {
            db.ReplyPosts.Add(rp);
            db.SaveChanges();
        }

        // Editting Wall Post
        public void EditWallPost(WallPost tp)
        {
            WallPost wallPost = this.GetWallPost(tp.WallPostId);
            wallPost.Description = tp.Description;
            db.SaveChanges();
        }

        // Editting Reply Post
        public void EditReplyPost(ReplyPost rp)
        {
            ReplyPost replyPost = this.GetReplyPost(rp.ReplyPostId);
            replyPost.Description = rp.Description;
            db.SaveChanges();
        }

        public void DeleteWallPost(int id)
        {
            List<ReplyPost> replies = this.GetReplyPostsByWallPostId(id);
            foreach (var reply in replies)
            {
                db.ReplyPosts.Remove(reply);
            }

            WallPost tp = this.GetWallPost(id);
            db.WallPosts.Remove(tp);
            db.SaveChanges();
        }

        public void DeleteReplyPost(int id)
        {
            ReplyPost rp = this.GetReplyPost(id);
            db.ReplyPosts.Remove(rp);
            db.SaveChanges();
        }
    }
}


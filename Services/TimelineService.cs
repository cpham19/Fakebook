using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Fakebook.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Diagnostics;

// Used for timeline posts and their replies
namespace Fakebook.Services
{
    public class TimelineService
    {
        private readonly AppDbContext db;

        public TimelineService(AppDbContext db)
        {
            this.db = db;
        }

        public TimelinePost GetTimelinePost(int id)
        {
            return db.TimelinePosts.Where(tp => tp.TimelinePostId == id).SingleOrDefault();
        }

        public ReplyPost GetReplyPost(int id)
        {
            return db.ReplyPosts.Where(rp => rp.ReplyPostId == id).SingleOrDefault();
        }

        // Get user's timeline posts and their replies
        public List<TimelinePost> GetTimelinePosts(int id)
        {
            List<TimelinePost> timelinePosts = db.TimelinePosts.Where(tp => tp.UserIdOfProfile == id).OrderByDescending(tp => tp.DatePosted).ToList();
            foreach (var tp in timelinePosts) {
                tp.Replies = db.ReplyPosts.Where(reply => reply.TimelinePostId == tp.TimelinePostId).OrderBy(reply => reply.DatePosted).ToList();
            }
            return timelinePosts;
        }

        // Add a new timeline post to their page (or other people's page perhaps)
        public void AddTimelinePost(TimelinePost tp)
        {
            db.TimelinePosts.Add(tp);
            db.SaveChanges();
        }

        // Add a new Reply Post
        public void AddReplyPost(ReplyPost rp)
        {
            db.ReplyPosts.Add(rp);
            db.SaveChanges();
        }

        // Editting Timeline Post
        public void EditTimelinePost(TimelinePost tp)
        {
            TimelinePost timelinePost = this.GetTimelinePost(tp.TimelinePostId);
            timelinePost.Description = tp.Description;
            db.SaveChanges();
        }

        // Editting Reply Post
        public void EditReplyPost(ReplyPost rp)
        {
            ReplyPost replyPost = this.GetReplyPost(rp.ReplyPostId);
            replyPost.Description = rp.Description;
            db.SaveChanges();
        }
    }
}


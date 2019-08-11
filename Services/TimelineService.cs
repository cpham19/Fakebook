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

        // Get user's timeline posts and their replies
        public List<TimelinePost> GetTimelinePosts(int id)
        {
            List<TimelinePost> timelinePosts = db.TimelinePosts.Where(tp => tp.PersonId == id).OrderByDescending(tp => tp.DatePosted).ToList();
            foreach (var tp in timelinePosts) {
                tp.Replies = db.ReplyPosts.Where(reply => reply.TimelinePostId == tp.TimelinePostId).OrderByDescending(reply => reply.DatePosted).ToList();
            }
            return timelinePosts;
        }

        // Add a new timeline post to their page (or other people's page perhaps)
        public void AddTimelinePost(TimelinePost tp)
        {
            db.TimelinePosts.Add(tp);
            db.SaveChanges();
        }
    }
}


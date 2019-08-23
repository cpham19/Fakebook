using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fakebook.Models
{
    public class WallPost
    {
        public int WallPostId { get; set; }
        // Default is 0 in Database so any GroupId that is >= 1 means the wallpost belongs to a group
        public int GroupId { get; set; }
        public string PosterName { get; set; }
        public string Description { get; set; }
        public DateTime DatePosted { get; set; }
        public int PosterId { get; set; }
        public int UserIdOfProfile { get; set; }
        public List<ReplyPost> Replies { get; set; }
    }

    public class ReplyPost
    {
        public int ReplyPostId { get; set; }
        public string PosterName { get; set; }
        public string Description { get; set; }
        public DateTime DatePosted { get; set; }
        public int WallPostId { get; set; }
        public int PosterId { get; set; }
    }
}

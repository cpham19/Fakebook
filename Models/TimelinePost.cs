using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fakebook.Models
{
    public class TimelinePost
    {
        public int TimelinePostId { get; set; }
        public string PosterName { get; set; }
        public string Description { get; set; }
        public DateTime DatePosted { get; set; }
        public int PersonId { get; set; }
        public List<ReplyPost> Replies { get; set; }
    }

    public class ReplyPost
    {
        public int ReplyPostId { get; set; }
        public string PosterName { get; set; }
        public string Description { get; set; }
        public DateTime DatePosted { get; set; }
        public int TimelinePostId { get; set; }
        public int PersonId { get; set; }
    }
}

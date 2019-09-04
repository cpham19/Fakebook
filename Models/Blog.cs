using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fakebook.Models
{
    public class Blog
    {
        public int BlogId { get; set; }
        [StringLength(30, MinimumLength = 3)]
        public string Headline { get; set; }
        [ValidUrl]
        public string PictureUrl { get; set; }
        [StringLength(30, MinimumLength = 3)]
        public string Title { get; set; }
        [StringLength(50, MinimumLength = 1)]
        public string Description { get; set; }
        public DateTime DatePosted { get; set; }
        public int PosterId { get; set; }
        [NotMapped]
        public string PosterName { get; set; }
        [NotMapped]
        public List<BlogComment> BlogComments{ get; set; }
    }

    public class BlogComment
    {
        public int BlogCommentId { get; set; }
        public int BlogId { get; set; }
        [StringLength(50, MinimumLength = 3)]
        public string Description { get; set; }
        public DateTime DatePosted { get; set; }
        public int PosterId { get; set; }
        [NotMapped]
        public string PosterName { get; set; }
    }
}

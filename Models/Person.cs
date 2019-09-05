using Fakebook.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fakebook.Models
{
    public class Person
    {
        public int PersonId { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 3)]
        [RegularExpression(@"^[a-zA-Z""'\s-]*$")]
        public string Name { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 3)]
        [RegularExpression(@"^[a-zA-Z""'\s-]*$")]
        public string Username { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 3)]
        [RegularExpression(@"^[a-zA-Z""'\s-]*$")]
        public string Password { get; set; }
        public bool IsAdmin { get; set; } = false;
        [NotMapped]
        public bool IsFriend { get; set; } = false;
        public string ProfileDescription { get; set; } = "Profile Description";
        [NotMapped]
        public List<WallPost> WallPosts { get; set; } = new List<WallPost>();
        [NotMapped]
        public List<Person> Friends { get; set; } = new List<Person>();
        [NotMapped]
        public List<Group> Groups { get; set; } = new List<Group>();
        [NotMapped]
        public List<Blog> Blogs { get; set; } = new List<Blog>();
        [NotMapped]
        public List<Store> Stores { get; set; } = new List<Store>();
        [NotMapped]
        public List<Review> Reviews { get; set; } = new List<Review>();
        [NotMapped]
        public List<StoreItem> ItemsReviewed { get; set; } = new List<StoreItem>();
    }

    public class Friend
    {
        [Key]
        public int PersonOneId { get; set; }
        public int PersonTwoId { get; set; }
        public int StatusCode { get; set; } = 0;
    }
}

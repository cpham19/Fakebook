using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fakebook.Models
{
    public class Person
    {
        public int PersonId { get; set; }
        public string Name { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
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
    }

    public class Friend
    {
        [Key]
        public int PersonOneId { get; set; }
        public int PersonTwoId { get; set; }
        public int StatusCode { get; set; } = 0;
    }
}

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
        public string ProfileDescription { get; set; } = "Profile Description";
        [NotMapped]
        public List<TimelinePost> TimelinePosts { get; set; } = new List<TimelinePost>();
        [NotMapped]
        public List<Person> Friends { get; set; } = new List<Person>();
    }
}

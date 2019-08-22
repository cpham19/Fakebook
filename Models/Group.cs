using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fakebook.Models
{
    public class Group
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public int GroupCreatorId { get; set; }
        [NotMapped]
        public string GroupCreatorName { get; set; }
        public string GroupPictureUrl { get; set; }
        [NotMapped]
        public bool UserJoined { get; set; } = false;
        [NotMapped]
        public List<Person> Members { get; set; } = new List<Person>();
    }

    public class GroupMember
    {
        [Key]
        public int GroupId { get; set; }
        public int GroupMemberId { get; set; }
    }
}

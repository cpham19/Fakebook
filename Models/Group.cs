using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Fakebook.Models
{
    public class Group
    {
        public int GroupId { get; set; }
        [StringLength(20, MinimumLength = 3)]
        public string GroupName { get; set; }
        [StringLength(50, MinimumLength = 3)]
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public int GroupCreatorId { get; set; }
        [NotMapped]
        public string GroupCreatorName { get; set; }
        [ValidUrl]
        public string GroupPictureUrl { get; set; }
        [NotMapped]
        public bool UserJoined { get; set; } = false;
        [NotMapped]
        public List<Person> Members { get; set; } = new List<Person>();
        public List<WallPost> WallPosts { get; set; } = new List<WallPost>();
    }

    public class GroupMember
    {
        [Key]
        [Column(Order = 1)]
        public int GroupId { get; set; }
        [Key]
        [Column(Order = 2)]
        public int GroupMemberId { get; set; }
    }

    // https://stackoverflow.com/questions/46694745/asp-net-core-custom-model-validation
    public class ValidUrlAttribute : ValidationAttribute
    {
        public string GetErrorMessage { get; set; } = "Invalid URL. An example of a proper url to an image is freeimages.com/freeimage.png";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var url = value as string;

            if (url != null && Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(this.GetErrorMessage);
        }
    }
}

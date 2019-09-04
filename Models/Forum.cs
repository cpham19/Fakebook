using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Fakebook
{
    public class Forum
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ForumId { get; set; }

        [StringLength(20, MinimumLength = 3)]
        public string ForumName { get; set; }
        public int PosterId { get; set; }

        public string ModifiedForumName()
        {
            string[] names = ForumName.Split(" ");
            return string.Join("_", names).ToLower();
        }

        //public override string ToString()
        //{
        //    return $"{ForumId}) Forum - {ForumName}";
        //}

        [NotMapped]
        public List<Topic> Topics { get; set; }
    }

    public class Topic
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int TopicId { get; set; }
        [StringLength(20, MinimumLength = 3)]
        public string TopicName { get; set; }
        public DateTime TopicDate { get; set; }
        [StringLength(50, MinimumLength = 3)]
        public string TopicContent { get; set; }
        public virtual int ForumId { get; set; }
        public int PosterId { get; set; }
        [NotMapped]
        public string PosterName { get; set; }

        public string ModifiedTopicName()
        {
            string[] names = TopicName.Split(" ");
            return string.Join("_", names).ToLower();
        }

        //public override string ToString()
        //{
        //    return $"{TopicId}) Topic - {TopicName} (Posted on {TopicDate})";
        //}

        [NotMapped]
        public List<Reply> Replies { get; set; }
    }

    public class Reply
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ReplyId { get; set; }
        //[StringLength(50, MinimumLength = 3)]
        public string ReplyContent { get; set; }
        public DateTime ReplyDate { get; set; }
        public virtual int TopicId { get; set; }
        public int PosterId { get; set; }
        [NotMapped]
        public string PosterName { get; set; }

        //public override string ToString()
        //{
        //    return $"Reply ({ReplyDate}): {ReplyContent}";
        //}
    }

}

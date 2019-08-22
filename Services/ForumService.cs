using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Fakebook.Services
{
    public interface IForumService
    {
        List<Forum> GetForums();
        Forum GetForum(int id);
        Forum GetForumBasedOnName(string name);
        Topic GetTopic(int id);
        Reply GetReply(int id);
        void AddForum(Forum f);
        void AddTopic(Topic t);
        void AddReply(Reply r);
        void EditReply(Reply r);
        void EditTopic(Topic t);
        void DeleteTopic(int id);
        void DeleteReply(int id);
    }

    public class ForumService : IForumService
    {
        private readonly AppDbContext db;

        public ForumService(AppDbContext db)
        {
            this.db = db;
        }

        public List<Forum> GetForums()
        {
            List<Forum> forums = db.Forums.OrderBy(f => f.ForumName).ToList();
            foreach(var forum in forums)
            {
                forum.Topics = db.Topics.Where(t => t.ForumId == forum.ForumId).ToList();
            }
            return forums;
        }

        public Forum GetForumBasedOnName(string name)
        {
            Forum forum = db.Forums.Where(f => f.ForumName == name).SingleOrDefault();
            forum.Topics = db.Topics.Where(t => t.ForumId == forum.ForumId).ToList();
            foreach (var topic in forum.Topics)
            {
                topic.Replies = db.Replies.Where(r => r.TopicId == topic.TopicId).ToList();
            }
            return forum;
        }

        public Forum GetForum(int id)
        {
            Forum forum = db.Forums.Where(f => f.ForumId == id).SingleOrDefault();
            forum.Topics = db.Topics.Where(t => t.ForumId == id).ToList();
            return forum;
        }

        public Topic GetTopic(int id)
        {
            Topic topic = db.Topics.Where(t => t.TopicId == id).SingleOrDefault();
            topic.Replies = db.Replies.Where(r => r.TopicId == id).ToList();

            return topic;
        }

        public void EditTopic(Topic t)
        {
            Topic topic = db.Topics.Where(top => top.TopicId == t.TopicId).SingleOrDefault();
            topic.TopicName = t.TopicName;
            topic.TopicContent = t.TopicContent;
            db.SaveChanges();
        }

        public Reply GetReply(int id)
        {
            Reply reply = db.Replies.Where(r => r.ReplyId == id).SingleOrDefault();
            return reply;
        }

        public void EditReply(Reply r)
        {
            Reply reply = db.Replies.Where(rep => rep.ReplyId == r.ReplyId).SingleOrDefault();
            reply.ReplyContent = r.ReplyContent;
            db.SaveChanges();
        }

        public void AddForum(Forum f)
        {
            db.Forums.Add(f);
            db.SaveChanges();
        }

        public void AddTopic(Topic t)
        {
            db.Topics.Add(t);
            db.SaveChanges();
        }

        public void AddReply(Reply r)
        {
            db.Replies.Add(r);
            db.SaveChanges();
        }

        public void DeleteTopic(int id)
        {
            Topic topic = this.GetTopic(id);
            foreach (var reply in topic.Replies)
            {
                db.Replies.Remove(reply);
            }

            db.Topics.Remove(topic);
            db.SaveChanges();
        }

        public void DeleteReply(int id)
        {
            Reply reply = this.GetReply(id);
            db.Replies.Remove(reply);
            db.SaveChanges();
        }
    }
    
}

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Fakebook.Services
{
    public interface IForumService
    {
        List<Forum> GetForums();
        Forum GetForum(int id);
        Topic GetTopic(int ForumId, int TopicId);
        void AddForum(Forum f);
        void AddTopic(Topic t);
        void AddReply(Reply r);
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
            return db.Forums.OrderBy(f => f.ForumName).ToList();
        }
        public Forum GetForum(int id)
        {
            Forum forum = db.Forums.Where(f => f.ForumId == id).SingleOrDefault();
            forum.Topics = db.Topics.Where(t => t.ForumId == id).ToList();

            return forum;
        }

        public Topic GetTopic(int id, int id2)
        {
            Topic topic = db.Topics.Where(t => t.TopicId == id2).SingleOrDefault();
            topic.Replies = db.Replies.Where(r => r.TopicId == id2).ToList();

            return topic;
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
    }
    
}

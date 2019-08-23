using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Fakebook.Models;

// Used for searching people
namespace Fakebook.Services
{
    public class GroupService
    {
        private readonly AppDbContext db;

        public GroupService(AppDbContext db)
        {
            this.db = db;
        }

        public Group GetGroup(int PersonId, int GroupId)
        {
            Group group = db.Groups.Where(g => g.GroupId == GroupId).SingleOrDefault();
            GroupMember isGroupMember = db.GroupMembers.Where(gm => gm.GroupId == group.GroupId && gm.GroupMemberId == PersonId).SingleOrDefault();
            if (isGroupMember != null)
            {
                group.UserJoined = true;
            }
            group.Members = this.GetGroupMembers(GroupId);
            group.WallPosts = this.GetGroupWallPosts(GroupId);

            return group;
        }

        public List<WallPost> GetGroupWallPosts(int GroupId)
        {
            List<WallPost> wallPosts = db.WallPosts.Where(tp => tp.GroupId == GroupId).OrderByDescending(tp => tp.DatePosted).ToList();
            foreach (var tp in wallPosts)
            {
                tp.Replies = db.ReplyPosts.Where(reply => reply.WallPostId == tp.WallPostId).OrderBy(reply => reply.DatePosted).ToList();
            }

            return wallPosts;
        }

        public List<Person> GetGroupMembers(int GroupId)
        {
            List<GroupMember> gms = db.GroupMembers.Where(gm => gm.GroupId == GroupId).ToList();
            List<Person> members = new List<Person>();
            foreach(var gm in gms)
            {
                Person person = db.Persons.Where(p => p.PersonId == gm.GroupMemberId).SingleOrDefault();
                members.Add(person);
            }
            return members;
        }

        public List<Group> GetGroups(int PersonId)
        {
            List<Group> groups = db.Groups.OrderBy(group => group.GroupId).ToList();
            // Get Creator Names of groups
            foreach (var group in groups)
            {
                Person person = db.Persons.Where(p => p.PersonId == group.GroupCreatorId).SingleOrDefault();
                group.GroupCreatorName = person.Name;
            }

            List<Group> groupsOfUser = this.GetGroupsOfUser(PersonId);
            return groups;
        }

        public List<Group> GetGroupsOfUser(int PersonId)
        {
            List<GroupMember> gms = db.GroupMembers.Where(gm => gm.GroupMemberId == PersonId).ToList();
            List<Group> groups = new List<Group>();
            foreach (var gm in gms)
            {
                Group group = db.Groups.Where(g => g.GroupId == gm.GroupId).SingleOrDefault();
                groups.Add(group);
            }

            foreach (var group in groups)
            {
                foreach (var gm in gms)
                {
                    if (group.GroupId == gm.GroupId)
                    {
                        group.UserJoined = true;
                    }
                }
            }

            return groups;
        }

        public void AddGroup(Group g)
        {
            db.Groups.Add(g);
            db.SaveChanges();
        }

        public void JoinGroup(GroupMember gm)
        {
            db.GroupMembers.Add(gm);
            db.SaveChanges();
        }

        public void LeaveGroup(int PersonId, int GroupId)
        {
            GroupMember gm = db.GroupMembers.Where(g => g.GroupId == GroupId && g.GroupMemberId == PersonId).SingleOrDefault();
            db.GroupMembers.Remove(gm);
            db.SaveChanges();
        }
    }
}


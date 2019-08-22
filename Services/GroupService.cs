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

            foreach (var userGroup in groupsOfUser)
            {
                foreach(var group in groups)
                {
                    if (userGroup.GroupId == group.GroupId)
                    {
                        group.UserJoined = true;
                    }
                }
            }

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
    }
}


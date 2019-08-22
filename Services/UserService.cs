using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Fakebook.Models;

// Used for searching people
namespace Fakebook.Services
{
    public class UserService
    {
        private readonly AppDbContext db;

        public UserService(AppDbContext db)
        {
            this.db = db;
        }

        public Person GetPerson(string name)
        {
            return db.Persons.Where(p => p.Name == name).SingleOrDefault();
        }

        public Person GetPersonBasedOnId(int id)
        {
            Person person = db.Persons.Where(p => p.PersonId == id).SingleOrDefault();
            person.Friends = this.GetFriends(id);
            List<GroupMember> gms = db.GroupMembers.Where(gm => gm.GroupMemberId == id).ToList();

            foreach(var gm in gms)
            {
                Debug.WriteLine(gm.GroupId);
            }

            List<Group> groups = new List<Group>();
            foreach (var gm in gms)
            {
                Group group = db.Groups.Where(g => g.GroupId == gm.GroupId).SingleOrDefault();
                groups.Add(group);
            }
            person.Groups = groups;
            return person;
        }

        public List<Person> GetFriends(int id)
        {
            List<Friend> relationships = this.GetRelationships(id);
            List<Person> friends = new List<Person>();
            foreach(var rel in relationships)
            {
                if (rel.PersonOneId != id)
                {
                    Person friend = db.Persons.Where(p => p.PersonId == rel.PersonOneId).SingleOrDefault();
                    friends.Add(friend);
                }
                else if (rel.PersonTwoId != id)
                {
                    Person friend = db.Persons.Where(p => p.PersonId == rel.PersonTwoId).SingleOrDefault();
                    friends.Add(friend);
                }
            }

            return friends;
        }

        // Getting a list of people excluding user
        public List<Person> GetPersons(int id)
        {
            return db.Persons.Where(p => p.PersonId != id).ToList();
        }

        // Getting a list of people based on the name parameter
        public List<Person> GetPersonsBasedOnName(int PersonOneId, string name)
        {
            List<Person> people = db.Persons.Where(p => p.Name.ToUpper() == name.ToUpper()).ToList();
            foreach(var person in people)
            {
                var check = CheckFriends(PersonOneId, person.PersonId);
                if (check == true)
                {
                    person.IsFriend = true;
                }
            }

            return people;
        }

        public List<Friend> GetRelationships(int id)
        {
            return db.Friends.Where(rel => rel.PersonOneId == id || rel.PersonTwoId == id).ToList();
        }

        public Friend GetRelationshipWithTwoIds(int PersonOneId, int PersonTwoId)
        {
            return db.Friends.Where(rel => (rel.PersonOneId == PersonOneId || rel.PersonTwoId == PersonOneId) && (rel.PersonOneId == PersonTwoId || rel.PersonTwoId == PersonTwoId)).SingleOrDefault(); ;
        }

        public bool CheckFriends(int PersonOneId, int PersonTwoId)
        {
            var check = this.GetRelationshipWithTwoIds(PersonOneId, PersonTwoId);
            if (check == null)
            {
                return false;
            }

            return true;
        }

        public void AddFriend(Friend relationship)
        {
            db.Friends.Add(relationship);
            db.SaveChanges();
        }

        public void RemoveFriend(int PersonOneId, int PersonTwoId)
        {
            Friend relationship = this.GetRelationshipWithTwoIds(PersonOneId, PersonTwoId);
            db.Friends.Remove(relationship);
            db.SaveChanges();
        }

        // Edit stuff for Person
        public void Edit(Person person)
        {
            var p = db.Persons.Where(per => per.PersonId == person.PersonId).SingleOrDefault();
            p.ProfileDescription = person.ProfileDescription;
            db.SaveChanges();
        }
    }
}


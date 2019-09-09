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

            List<Group> groups = new List<Group>();
            foreach (var gm in gms)
            {
                Group group = db.Groups.Where(g => g.GroupId == gm.GroupId).SingleOrDefault();
                groups.Add(group);
            }
            person.Groups = groups;
            person.Blogs = db.Blogs.Where(b => b.PosterId == id).ToList();
            person.Stores = db.Stores.Where(store => store.StoreOwnerId == id).ToList();
            person.Reviews = db.Reviews.Where(review => review.PosterId == id).ToList();

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

        // Getting a list of people
        public List<Person> GetPeople(int id)
        {
            List<Person> people = db.Persons.ToList();
            foreach (var person in people)
            {
                var check = CheckFriends(id, person.PersonId);
                if (check == true)
                {
                    person.IsFriend = true;
                }
            }

            return people;
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
            Friend friend = db.Friends.Where(rel => (rel.PersonOneId == PersonOneId && rel.PersonTwoId == PersonTwoId) || (rel.PersonOneId == PersonTwoId && rel.PersonTwoId == PersonOneId)).SingleOrDefault();
            return friend;
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
            Friend relationship1 = db.Friends.Where(rel => rel.PersonOneId == PersonOneId && rel.PersonTwoId == PersonTwoId).SingleOrDefault();
            Friend relationship2 = db.Friends.Where(rel => rel.PersonOneId == PersonTwoId && rel.PersonTwoId == PersonOneId).SingleOrDefault();
            if (relationship1 == null)
            {
                db.Friends.Remove(relationship2);
            }
            else
            {
                db.Friends.Remove(relationship1);
            }
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


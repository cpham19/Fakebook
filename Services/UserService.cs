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
            return db.Persons.Where(p => p.PersonId == id).SingleOrDefault();
        }

        // Getting a list of people excluding user
        public List<Person> GetPersons(int id)
        {
            return db.Persons.Where(p => p.PersonId != id).ToList();
        }

        // Getting a list of people based on the name parameter
        public List<Person> GetPersonsBasedOnName(int id, string name)
        {
            if (name == null || name == "")
            {
                return db.Persons.Where(p => p.PersonId != id).ToList();
            } 
            else
            {
                return db.Persons.Where(p => p.Name.ToUpper() == name.ToUpper()).ToList();
            }
        }

        // Edit stuff for Person
        public void Edit(Person person)
        {
            var p = db.Persons.Where(per => per.PersonId == person.PersonId).SingleOrDefault();
            p.ProfileDescription = person.ProfileDescription;
            Debug.WriteLine(p.PersonId);
            Debug.WriteLine(p.ProfileDescription);
            db.SaveChanges();
        }
    }
}


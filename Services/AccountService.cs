using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Fakebook.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Principal;

// Used for logging in, creating user, and authenticating credentials
namespace Fakebook.Services
{
    public class AccountService
    {
        private readonly AppDbContext db;

        public AccountService(AppDbContext db)
        {
            this.db = db;
        }

        // Getting one person based on username
        public Person GetPerson(string username)
        {
            return db.Persons.Where(e => e.Username.ToUpper() == username.ToUpper()).SingleOrDefault();
        }

        public bool CheckUsernameExists(string username)
        {
            Person person = db.Persons.Where(per => per.Username == username).SingleOrDefault();
            if (person == null)
            {
                return false;
            }

            return true;
        }

        // Adding a person to DB for registration
        public bool AddPerson(Person p)
        {
            if (!this.CheckUsernameExists(p.Username))
            {
                db.Persons.Add(p);
                db.SaveChanges();
                return true;
            }

            return false;
        }

        // Authenticating user login credentials
        public ClaimsIdentity Authenticate(string username, string password)
        {
            var person = this.GetPerson(username);
            if (person == null || person.Password != password)
            {
                return null;
            }

            var claims = new List<Claim>
            {
                new Claim(PersonClaimTypes.PersonId, person.PersonId.ToString()),
                new Claim(ClaimTypes.Name, person.Name),
                new Claim(ClaimTypes.NameIdentifier, person.Username),
                new Claim(PersonClaimTypes.ProfileDescription, person.ProfileDescription),
                new Claim(PersonClaimTypes.NumberOfFriends, person.Friends.Count().ToString()),
            };

            if (person.IsAdmin)
            {
                claims.Add(new Claim("IsAdmin", "Yes"));
            }

            return new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}


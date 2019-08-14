using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Fakebook.Models;

// Used for custom claim types. Easy to use because of extension methods
namespace Fakebook.Services
{
    public static class PersonClaimTypes
    {
        public const string PersonId = "PersonId";
        public const string Name = "Name";
        public const string ProfileDescription = "ProfileDescription";
        public const string NumberOfFriends = "NumberOfFriends"; 
    }

    public static class IdentityExtensions
    {
        public static int GetPersonId(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst(PersonClaimTypes.PersonId);

            if (claim == null)
                return 0;

            return int.Parse(claim.Value);
        }

        public static string GetName(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst(ClaimTypes.Name) ;

            if (claim == null)
                return "";

            return claim.Value;
        }

        public static string GetProfileDescription(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst(PersonClaimTypes.ProfileDescription);

            if (claim == null)
                return "";

            return claim.Value;
        }

        public static int GetNumberOfFriends(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst(PersonClaimTypes.NumberOfFriends);

            if (claim == null)
                return 0;

            return int.Parse(claim.Value);
        }
    }
}

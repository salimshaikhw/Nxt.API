using Microsoft.AspNetCore.Identity;
using Nxt.Entities.Dtos.Account;
using System.Collections.Generic;

namespace Nxt.Entities.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public List<RefreshToken> RefreshTokens { get; set; }
    }
}

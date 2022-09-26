using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace NZWalks.API.Models.Domain
{
    public class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        [NotMapped]
        public List<string> Roles { get; set; }

        // Navigation Property
        public List<UserRole> UserRoles { get; set; }
    }
}

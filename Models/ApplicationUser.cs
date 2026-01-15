using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Garage_2._0.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required, MaxLength(50)]
        public string FirstName { get; set; } = "";

        [Required, MaxLength(50)]
        public string LastName { get; set; } = "";

        [Required, MaxLength(20)]
        public string PersonalNumber { get; set; } = ""; // must be unique

        [Required]
        public DateTime DateOfBirth { get; set; }

        // Extra: membership
        public string MembershipType { get; set; } = "Pro";
        public DateTime MembershipValidUntil { get; set; }
    }
}

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Bilet11.Models
{
    public class AppUser:IdentityUser
    {
        [Required]
        [MinLength(2),MaxLength(20)]
        public string FirstName { get; set; }
        [Required]
        [MinLength(2), MaxLength(20)]
        public string LastName { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Bilet11.ViewModels
{
    public class UserRegisterVM
    {
        [Required]
        [MinLength(2),MaxLength(20)]
        public string Name { get; set; }
        [Required]
        [MaxLength(25)]
        public string Surname { get; set; }
        [Required]
        [MinLength(5), MaxLength(25)]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}

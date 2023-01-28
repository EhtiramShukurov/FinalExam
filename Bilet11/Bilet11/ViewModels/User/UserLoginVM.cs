using System.ComponentModel.DataAnnotations;

namespace Bilet11.ViewModels
{
    public class UserLoginVM
    {
        [Required]
        public string UsernameOrEmail { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool IsPersistance { get; set; }
    }
}

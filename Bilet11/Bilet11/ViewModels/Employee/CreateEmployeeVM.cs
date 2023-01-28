using System.ComponentModel.DataAnnotations;

namespace Bilet11.ViewModels
{
    public class CreateEmployeeVM
    {
        [Required]
        [MinLength(2), MaxLength(20)]
        public string Name { get; set; }
        [Required]
        [MaxLength(25)]
        public string Surname { get; set; }
        [Required]
        [MinLength(3), MaxLength(100)]
        public string Description { get; set; }
        [Required]
        public IFormFile Image { get; set; }
        [MaxLength(100)]
        public string? FbLink { get; set; }
        [MaxLength(100)]
        public string? InstagramLink { get; set; }
        [MaxLength(100)]
        public string? TwitterLink { get; set; }
        [MaxLength(100)]
        public string? LinkedinLink { get; set; }
        [Required]
        public int PositionId { get; set; }
    }
}

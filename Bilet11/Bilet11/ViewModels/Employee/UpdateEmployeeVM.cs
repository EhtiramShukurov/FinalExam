using System.ComponentModel.DataAnnotations;

namespace Bilet11.ViewModels
{
    public class UpdateEmployeeVM
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
        public string? ImageUrl { get; set; }
        public IFormFile? Image { get; set; }
        [MaxLength(100)]
        public string? FbLink { get; set; }
        [MaxLength(100)]
        public string? InstagramLink { get; set; }
        [MaxLength(100)]
        public string? TwitterLink { get; set; }
        [MaxLength(100)]
        public string? LinkedinLink { get; set; }
        public int PositionId { get; set; }
    }
}

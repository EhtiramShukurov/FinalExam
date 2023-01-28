using Bilet11.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace Bilet11.Models
{
    public class Setting:BaseEntity
    {
        [Required]
        [MaxLength(15)]
        public string Key { get; set; }
        [MaxLength(30)]
        public string? Value { get; set; }
    }
}

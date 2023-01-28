using System.ComponentModel.DataAnnotations;

namespace Bilet11.ViewModels
{
    public class UpdateSettingVM
    {
        public string? Key { get; set; }
        [MaxLength(30)]
        public string? Value { get; set; }
    }
}

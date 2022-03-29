using System.ComponentModel.DataAnnotations;

namespace E_Commerce.MvcWebUI.Models
{
    public class LoginModel
    {
        [Key]
        public int id { get; set; }
        [Required]
        [Display(Name ="Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = String.Empty;
        [Required]
        [Display(Name ="Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = String.Empty;
    }
}

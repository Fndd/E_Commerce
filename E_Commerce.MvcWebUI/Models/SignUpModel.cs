using System.ComponentModel.DataAnnotations;

namespace E_Commerce.MvcWebUI.Models
{
    public class SignUpModel
    {
        public string Id { get; set; }

        [Required]
        [Display(Name ="Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = String.Empty;

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = String.Empty;

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; } = String.Empty;
        [Required]
        [Display(Name = "Surname")]
        public string Surname { get; set; } = String.Empty;
        [Required]
        [Display(Name ="Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; } = String.Empty;

        [Required]
        [Display(Name ="Address")]
        public string Address { get; set; } = String.Empty;
        public string Role { get; set; } = String.Empty;
    }
}

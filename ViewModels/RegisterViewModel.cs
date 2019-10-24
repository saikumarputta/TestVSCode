using System.ComponentModel.DataAnnotations;
namespace TestVSCode.viewModels
{
    public class RegisterViewModel
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
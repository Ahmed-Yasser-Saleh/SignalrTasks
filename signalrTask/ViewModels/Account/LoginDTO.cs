using System.ComponentModel.DataAnnotations;

namespace signalrTask.ViewModels.Account
{
    public class LoginDTO
    {
        [RegularExpression("[a-zA-Z0-9-_]+@gmail.com", ErrorMessage = "Please enter a valid Gmail address.")]
        [Required]
        public string email { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Password must be between 3 and 20 characters.")]
        public string password { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace signalrTask.ViewModels.Account
{
    public class RegisterDTO
    {
        [RegularExpression("[A-Za-z]+", ErrorMessage = "Username does not cantain any number")]
        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; }
        [Required]
        [Compare("password", ErrorMessage = "Password must match ConfirmPassword")]
        public string Confirmpassword { get; set; }
        [RegularExpression("[a-zA-Z0-9-_]+@gmail.com", ErrorMessage = "Please enter a valid Gmail address.")]
        [Required]
        public string email { get; set; }
    }
}

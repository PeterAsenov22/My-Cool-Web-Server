namespace WebServer.GameStoreApplication.ViewModels.Account
{
    using System.ComponentModel.DataAnnotations;

    public class LoginUserViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}

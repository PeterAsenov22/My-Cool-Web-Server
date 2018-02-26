namespace WebServer.GameStoreApplication.ViewModels.Account
{
    using System.ComponentModel.DataAnnotations;
    using Common;
    using Utilities;

    public class RegisterUserViewModel
    {
        [Required]
        [Display(Name = "E-mail")]
        [MaxLength(
            ValidationConstants.Account.EmailMaxLength,
            ErrorMessage = ValidationConstants.InvalidMaxLengthErrorMessage)]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Full Name")]
        [MinLength(
            ValidationConstants.Account.NameMinLength,
            ErrorMessage = ValidationConstants.InvalidMinLengthErrorMessage)]
        [MaxLength(
            ValidationConstants.Account.NameMaxLength,
            ErrorMessage = ValidationConstants.InvalidMaxLengthErrorMessage)]
        public string Name { get; set; }

        [Required]
        [MinLength(
            ValidationConstants.Account.PasswordMinLength,
            ErrorMessage = ValidationConstants.InvalidMinLengthErrorMessage)]
        [MaxLength(
            ValidationConstants.Account.PasswordMaxLength,
            ErrorMessage = ValidationConstants.InvalidMaxLengthErrorMessage)]
        [Password]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}

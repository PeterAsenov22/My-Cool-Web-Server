namespace WebServer.GameStoreApplication.ViewModels.Admin
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Common;

    public class AddGameViewModel
    {
        [Required]
        [MinLength(ValidationConstants.Game.TitleMinLength,
            ErrorMessage = ValidationConstants.InvalidMinLengthErrorMessage)]
        [MaxLength(ValidationConstants.Game.TitleMaxLength,
            ErrorMessage = ValidationConstants.InvalidMaxLengthErrorMessage)]
        public string Title { get; set; }

        [Display(Name = "YouTube Video URL")]
        [Required]
        [MinLength(ValidationConstants.Game.VideoIdLength,
            ErrorMessage = ValidationConstants.ExactLengthErrorMessage)]
        [MaxLength(ValidationConstants.Game.VideoIdLength,
            ErrorMessage = ValidationConstants.ExactLengthErrorMessage)]
        public string TrailerId { get; set; }

        [Required]
        public string Image { get; set; }

        public double Size { get; set; }

        public decimal Price { get; set; }

        [Required]
        [MinLength(ValidationConstants.Game.DescriptionMinLength,
            ErrorMessage = ValidationConstants.InvalidMinLengthErrorMessage)]
        public string Description { get; set; }

        [Display(Name = "Release Date")]
        [Required]
        public DateTime? ReleaseDate { get; set; }
    }
}

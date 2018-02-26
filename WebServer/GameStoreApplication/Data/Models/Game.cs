namespace WebServer.GameStoreApplication.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using WebServer.GameStoreApplication.Common;

    public class Game
    {
        public Game()
        {
            this.Users = new List<UserGame>();
        }

        public int Id { get; set; }

        [Required]
        [MinLength(ValidationConstants.Game.TitleMinLength)]
        [MaxLength(ValidationConstants.Game.TitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [MinLength(ValidationConstants.Game.VideoIdLength)]
        [MaxLength(ValidationConstants.Game.VideoIdLength)]
        public string TrailerId { get; set; }

        [Required]
        public string Image { get; set; }

        public double Size { get; set; }

        public decimal Price { get; set; }

        [Required]
        [MinLength(ValidationConstants.Game.DescriptionMinLength)]
        public string Description { get; set; }

        public DateTime ReleaseDate { get; set; }

        public List<UserGame> Users { get; set; }
    }
}

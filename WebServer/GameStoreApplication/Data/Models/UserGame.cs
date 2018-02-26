namespace WebServer.GameStoreApplication.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class UserGame
    {
        [ForeignKey("User")]
        public int UserId { get; set; }

        public User User { get; set; }

        [ForeignKey("Game")]
        public int GameId { get; set; }

        public Game Game { get; set; }
    }
}

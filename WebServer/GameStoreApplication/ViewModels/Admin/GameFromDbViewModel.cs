namespace WebServer.GameStoreApplication.ViewModels.Admin
{
    using System;

    public class GameFromDbViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string TrailerId { get; set; }

        public string Image { get; set; }

        public double Size { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public DateTime ReleaseDate { get; set; }
    }
}

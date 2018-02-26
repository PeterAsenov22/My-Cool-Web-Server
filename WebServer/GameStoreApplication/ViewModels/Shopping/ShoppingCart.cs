namespace WebServer.GameStoreApplication.ViewModels.Shopping
{
    using System.Collections.Generic;
    using Admin;

    public class ShoppingCart
    {
        private readonly Dictionary<int, GameFromDbViewModel> products;

        public ShoppingCart()
        {
            this.products = new Dictionary<int, GameFromDbViewModel>();
            this.TotalPrice = 0;
        }

        public const string SessionKey = "%^Current_Shopping_Cart^%";

        public decimal TotalPrice { get; private set; }

        public void Add(GameFromDbViewModel game)
        {
            this.products.Add(game.Id,game);
            this.TotalPrice += game.Price;
        }

        public void Remove(int gameId)
        {
            var game = this.products[gameId];
            this.TotalPrice -= game.Price;

            this.products.Remove(gameId);          
        }

        public bool Contains(int gameId)
        {
            return this.products.ContainsKey(gameId);
        }

        public IEnumerable<GameFromDbViewModel> Products()
        {
            return this.products.Values;
        }

        public void Clear()
        {
            this.products.Clear();
            this.TotalPrice = 0;
        }
    }
}
